using System;
using System.Collections;
using System.IO;

namespace Assembler
{
    class BitArrayWriter : IDisposable
    {
        private readonly BinaryWriter binaryWriter;
        private BitArray? waitingArray;

        public BitArrayWriter(Stream stream) => binaryWriter = new BinaryWriter(stream);

        public void Write(BitArray bitArray)
        {
            if (waitingArray is null)
            {
                waitingArray = bitArray;
            }
            else
            {
                var bytes = waitingArray
                    .Append(bitArray)
                    .ToByteArray();

                waitingArray = null;

                binaryWriter.Write(bytes);
            }
        }

        public void Dispose() => binaryWriter.Dispose();
    }
}
