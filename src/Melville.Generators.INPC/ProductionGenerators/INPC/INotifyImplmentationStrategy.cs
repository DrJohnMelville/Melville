
using Melville.Generators.INPC.GenerationTools.CodeWriters;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public interface INotifyImplementationStategy
{
    string DeclareInterface();
    void DeclareMethod(CodeWriter cw);
    void PropertyChangeCallPrefix(CodeWriter cw);
}

public static class NotifyImplementationStrategyOperations
{
    public static void RenderPropertyNotification(
        this INotifyImplementationStategy notifier, CodeWriter cw, string propertyName)
    {
        notifier.PropertyChangeCallPrefix(cw);
        cw.AppendLine($"""OnPropertyChanged("{propertyName}");""");
    }
}
    
public sealed class HasMethodStrategy: INotifyImplementationStategy
{
    public string DeclareInterface() => "";

    public void DeclareMethod(CodeWriter cw)
    {
    }

    public void PropertyChangeCallPrefix(CodeWriter cw)
    {
    }
}
    
public class UseInterfaceStrategy: INotifyImplementationStategy
{
    protected string InterfaceName { get; }

    public UseInterfaceStrategy(string interfaceName)
    {
        InterfaceName = interfaceName;
    }

    public virtual string DeclareInterface() => "";

    public virtual void DeclareMethod(CodeWriter cw)
    {
    }

    public void PropertyChangeCallPrefix(CodeWriter cw)
    {
        cw.Append("((");
        cw.Append(InterfaceName);
        cw.Append(")this).");
    }
}

public class DeclareInterfaceStrategy : UseInterfaceStrategy
{
    public DeclareInterfaceStrategy(string interfaceName) : base(interfaceName)
    {
    }

    public override string DeclareInterface() => ": " + InterfaceName;

    public override void DeclareMethod(CodeWriter cw)
    {
        cw.AppendLine("public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;");
        cw.Append("void ");
        cw.Append(InterfaceName);
        cw.AppendLine(".OnPropertyChanged(string propertyName)");
        using var block = cw.CurlyBlock();
        cw.AppendLine("this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));");

    }
}