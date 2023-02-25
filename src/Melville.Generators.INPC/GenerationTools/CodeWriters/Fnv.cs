namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public static class Fnv
{
    private const uint offsetBasis = 0x811c9dc5;
    private const uint prime = 0x01000193;

    public static uint FromString(string s)
    {
        unchecked
        {
            var hash = offsetBasis;
            foreach (var character in s)
            {
                hash = (hash * prime) ^ (uint)(character & 0xFF);
            }

            return hash;

        }
    }
}