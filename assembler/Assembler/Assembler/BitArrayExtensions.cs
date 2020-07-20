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

        internal static void SetRange(this BitArray source, int value, int from, int length)
        {
            var bitArray = new BitArray(new int[] { value });
            for (int i = 0; i < length; ++i)
            {
                var sourceIndex = from + length - bitArray.Length + i;
                source.Set(sourceIndex, bitArray.Get(bitArray.Length - length + i));
            }
        }

        internal static byte[] ToByteArray(this BitArray bits)
        {
            // Who knows, might change
            const int BITSPERBYTE = 8;

            // Get the size of bytes needed to store all bytes
            int bytesize = bits.Length / BITSPERBYTE;
            // Any bit left over another byte is necessary
            if (bits.Length % BITSPERBYTE > 0)
            {
                bytesize++;
            }

            // For the result
            byte[] bytes = new byte[bytesize];

            // Must init to good value, all zero bit byte has value zero
            // Lowest significant bit has a place value of 1, each position to
            // to the left doubles the value
            byte value = 0;
            byte significance = 1;

            // Remember where in the input/output arrays
            int bytepos = 0;
            int bitpos = 0;

            while (bitpos < bits.Length)
            {
                // If the bit is set add its value to the byte
                if (true == bits[bitpos])
                {
                    value += significance;
                }
                bitpos++;
                if (0 == bitpos % BITSPERBYTE)
                {
                    // A full byte has been processed, store it
                    // increase output buffer index and reset work values
                    bytes[bytepos] = value;
                    bytepos++;
                    value = 0;
                    significance = 1;
                }
                else
                {
                    // Another bit processed, next has doubled value
                    significance *= 2;
                }
            }
            return bytes;
        }
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }
    }
}
