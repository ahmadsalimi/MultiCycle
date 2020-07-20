using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assembler.Assemblers
{
    internal class NoOpAssembler : IAssembler
    {
        private static readonly BitArray Opcode = 12.ToBitArray();
        private static readonly Regex regex = new Regex("^\\s*noop\\s*$", RegexOptions.IgnoreCase);

        public IEnumerable<int> Assemble(string line)
        {
            var match = regex.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Cannot Assemble {line}");
            }

            var array = new BitArray(20);
            array.Or(Opcode);
            array.LeftShift(16);

            var result = new bool[20];
            array.CopyTo(result, 0);
            return result.Reverse().Select(b => b ? 1 : 0);
        }
    }
}
