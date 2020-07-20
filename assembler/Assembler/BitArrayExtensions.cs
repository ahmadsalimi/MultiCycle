using System.Collections;

namespace Assembler
{
    internal static class BitArrayExtensions
    {
        internal static BitArray ToBitArray(this int value)
        {
            return new BitArray(new int[] { value })
            {
                Length = 20
            };
        }
    }
}
