namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;

public static class RenameFactory
{
    public static IMethodNamer Create(string? filter, string? rename) => (filter, rename) switch
    {
        ({ } f, null) => new FilterOnlyRenamer(f),
        (null, {} ren) => new FilterAndRename("(^.*$)", ren),
        ({} f, {} ren) => new FilterAndRename(f, ren),
        _ => NoRename.Instance
    };
}