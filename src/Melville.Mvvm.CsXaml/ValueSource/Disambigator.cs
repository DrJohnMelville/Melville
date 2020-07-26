namespace Melville.Mvvm.CsXaml.ValueSource
{
    public class Disambigator<TBase, TChild> where TChild:TBase
    {
        private Disambigator()
        {
            // con can't create this class, it is a compiler hack so that we can use generic
            // constraints as part of the method signature
        }
    }
}