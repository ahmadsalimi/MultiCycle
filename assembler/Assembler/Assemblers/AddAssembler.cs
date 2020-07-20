using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assembler.Assemblers
{
    internal class AddAssembler : IAssembler
    {
        private static readonly BitArray Opcode = 0.ToBitArray();
        private static readonly Regex regex = new Regex("^\\s*add\\s+\\$(\\d+)\\s*,\\s*\\$(\\d+)\\s*,\\s*\\$(\\d+)(\\s*,\\s*([01]))?\\s*$", RegexOptions.IgnoreCase);

        public IEnumerable<int> Assemble(string line)
        {
            var match = regex.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Cannot Assemble {line}");
            }

            var array = new BitArray(20);
            array.Or(Opcode);
            array.LeftShift(1);
            array.Set(0, match.Groups[5].Value == "1");                 // cin
            array.LeftShift(5);
            array.Or(int.Parse(match.Groups[2].Value).ToBitArray());    // in1
            array.LeftShift(5);
            array.Or(int.Parse(match.Groups[3].Value).ToBitArray());    // in2
            array.LeftShift(5);
            array.Or(int.Parse(match.Groups[1].Value).ToBitArray());    // out

            var result = new bool[20];
            array.CopyTo(result, 0);
            return result.Reverse().Select(b => b ? 1 : 0);
        }
    }
}
