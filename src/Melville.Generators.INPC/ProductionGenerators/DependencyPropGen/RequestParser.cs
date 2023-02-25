using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;

public class RequestParser
{
    private readonly SemanticModel semanticModel;
    public MemberDeclarationSyntax TargetMemberSyntax { get; }
    public ITypeSymbol ParentSymbol { get; }
    public ITypeSymbol? Type { get; private set; }
    public string PropName { get; private set; } = "";
    public bool Attached { get; private set; }
    public bool Nullable { get; private set; }
    public string XmlDocumentation { get; set; } = "";
    private string? customDefault;
        
    public bool Valid() => PropName.Length > 0 && Type != null;
    public bool FromCustomDpDeclaration => storedDependencyPropertyName != null;
    public string TargetType() => Type?.FullyQualifiedName()??"";
    public string NullableTargetType() => (Type?.FullyQualifiedName()??"")+ (Nullable?"?":"");
    public string ParentType() => ParentSymbol.FullyQualifiedName();
    private string? storedDependencyPropertyName;
    public string DependencyPropName() => storedDependencyPropertyName?? (PropName + "Property");
    public string DefaultExpression() =>  customDefault??$"default({NullableTargetType()})";
        
    public RequestParser(SemanticModel semanticModel, ITypeSymbol parentSymbol, MemberDeclarationSyntax targetMemberSyntax)
    {
        this.semanticModel = semanticModel;
        ParentSymbol = parentSymbol;
        TargetMemberSyntax = targetMemberSyntax;
        ParseAttributeTarget();
    }

    #region Parameter and Attribute Parsing

    public void ParseAllParams(AttributeArgumentListSyntax? arguments)
    {
        if (arguments?.Arguments is not { } args) return;
        var pos = 0;
        foreach (var argument in args)
        {
            ParseParam(pos++, argument);
        }
    }

    private void ParseParam(int position, AttributeArgumentSyntax syntax)
    {
        switch (position, syntax.NameEquals, syntax.NameColon, syntax.Expression) // remember to handle nameequals and colonequals
        {
            case (_, {} ne, _, var exp):
                ParseNamedProperty(ne, exp);
                break;
            case (_, _, {} nc, _):
                ParseColonParameter(nc.Name.ToString(), syntax);
                break;
            case (0, _, _,TypeOfExpressionSyntax tos):
                ParseTargetType(tos);
                break;
            case (1, _, _, LiteralExpressionSyntax):
                ParsePropName(syntax);
                break;
        }
    }

    private void ParseColonParameter(string name, AttributeArgumentSyntax expr)
    {
        switch (name, expr.Expression)
        {
            case ("targetType", TypeOfExpressionSyntax tos):
                ParseTargetType(tos);
                break;
            case ("name", _):
                ParsePropName(expr);
                break;
        }
    }

    private void ParseTargetType(TypeOfExpressionSyntax tos) => 
        Type = tos.ToTypeSymbol(semanticModel);

    private void ParsePropName(AttributeArgumentSyntax syntax) => 
        PropName = syntax.ExtractArgumentFromAttribute();

    private void ParseNamedProperty(NameEqualsSyntax ne, ExpressionSyntax les)
    {
        switch (ne.Name.ToString())
        {
            case "Attached" :
                Attached = ReadBoolLiteral(les);
                break;
            case "Nullable" :
                Nullable = ReadBoolLiteral(les);
                break;
            case "Default":
                customDefault = ParseDefaultExpression(les.ToString());
                break;
            case "XmlDocumentation":
                XmlDocumentation = ParseStringFromExpression(les);
                break;

        }
    }

    private static string ParseStringFromExpression(ExpressionSyntax les)
    {
        return les is LiteralExpressionSyntax ss && (ss.Kind() is SyntaxKind.StringLiteralExpression)
            ? ss.Token.ValueText
            : les.ToString();
    }

    private readonly Regex shiftedDefaultFinder = new Regex(@"^""@(.+)""$");
    private string? ParseDefaultExpression(string input)
    {
        return (shiftedDefaultFinder.Match(input) is { Success: true } match) ? match.Groups[1].Value : input;
    }

    private static bool ReadBoolLiteral(ExpressionSyntax les) => 
        les.ToString().StartsWith("t");
        

    #endregion

    private void ParseAttributeTarget()
    {
        switch (TargetMemberSyntax)
        {
            case MethodDeclarationSyntax mds:
                TryParseModifyMethod(mds);
                break;
            case FieldDeclarationSyntax fds:
                TryParseDpDeclaration(fds);
                break;
        }
    }

    private void TryParseModifyMethod(MethodDeclarationSyntax targetMethod)
    {
        if (!TryParseModifyMethodName(targetMethod.Identifier.ToString(), out var extractedName)) return;
        PropName = extractedName;
        if (ModelExtensions.GetDeclaredSymbol(semanticModel, targetMethod) is IMethodSymbol symbol)
        {
            Attached = ComputeAttached(symbol.IsStatic, symbol.Parameters);
            Type = FilterTypeSymbolForNullability(symbol.Parameters.LastOrDefault()?.Type);
            if (Type != null) // we haave a type so we must have found a valid parameter
            {
                customDefault = DefaultValueTextFromLastParameter(targetMethod);
            }
        }
    }

    private static string? DefaultValueTextFromLastParameter(MethodDeclarationSyntax targetMethod)
    {
        return targetMethod.ParameterList.Parameters.Last().Default?.Value.ToString();
    }

    private bool ComputeAttached(bool isStatic, ImmutableArray<IParameterSymbol> symbolParameters) =>
        isStatic && symbolParameters.Length > 0 &&
        symbolParameters[0].Type.FullyQualifiedName() == "System.Windows.DependencyObject";

    private ITypeSymbol? FilterTypeSymbolForNullability(ITypeSymbol? sym)
    {
        if ( sym == null || !IsNullableReferenceType(sym)) return sym;
        Nullable = true;
        return sym.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
    }
    private static bool IsNullableReferenceType(ITypeSymbol rawType) =>
        rawType.IsReferenceType && 
        rawType.NullableAnnotation == NullableAnnotation.Annotated;

    private static Regex ModifyMethodNameParser = new (@"^On(\w+)Changed$");
    private bool TryParseModifyMethodName(string name, out string methName)
    {
        var match = ModifyMethodNameParser.Match(name);
        methName = match.Success ? match.Groups[1].Value : "";
        return match.Success;
    }

    private const int publicKind = 8343;
    private const int staticKind = 8347;
    private const int readonlyKind = 8348;
    private void TryParseDpDeclaration(FieldDeclarationSyntax fds)
    {
        if (!(HasModifier(fds, publicKind) && 
              HasModifier(fds, staticKind) &&
              HasModifier(fds, readonlyKind))) return;
        TryParseDpDeclaration(fds.Declaration);
    }

    private static bool HasModifier(FieldDeclarationSyntax fds, int modifierKind) => 
        fds.Modifiers.Any(i=>i.RawKind== modifierKind);

    private void TryParseDpDeclaration(VariableDeclarationSyntax vds)
    {
        if (!vds.Type.ToString().EndsWith("DependencyProperty") ||
            vds.Variables.Count != 1) return;
        TryParseDpDeclaration(vds.Variables[0]);
    }

    private void TryParseDpDeclaration(VariableDeclaratorSyntax vds)
    {
        if (!TryParseDpDeclaration(vds.Initializer?.Value as InvocationExpressionSyntax))
            return;
        storedDependencyPropertyName = vds.Identifier.ToString();
    }

    private bool TryParseDpDeclaration(InvocationExpressionSyntax? invocation)
    {
        if (invocation == null) return false;
        if (TryGetAttachedStatus(invocation.Expression.ToString(), out var isAttached) &&
            invocation.ArgumentList.Arguments.Count == 4 &&
            TryGetPropName(invocation.ArgumentList.Arguments[0], out var propName)&&
            TryGetType(invocation.ArgumentList.Arguments[1], out var type))
        {
            Attached = isAttached;
            PropName = propName;
            Type = type;
            return true;
        }

        return false;
    }

    private bool TryGetType(ArgumentSyntax arg, out ITypeSymbol? type)
    {
        if (arg.Expression is TypeOfExpressionSyntax toes)
        {
            type = toes.ToTypeSymbol(semanticModel);
            return true;
        }
        type = null;
        return false;
    }

    private bool TryGetPropName(ArgumentSyntax argument, out string name)
    {
        name = argument.ToString().Trim('"');
        return true;
    }

    private bool TryGetAttachedStatus(string method, out bool attached)
    {
        attached = false;
        if (method.EndsWith("DependencyProperty.Register")) return true;
        if (method.EndsWith("DependencyProperty.RegisterAttached"))
        {
            attached = true;
            return true;
        }
        return false;
    }
}