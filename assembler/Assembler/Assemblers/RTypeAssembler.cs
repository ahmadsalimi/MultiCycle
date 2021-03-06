﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assembler.Assemblers
{
    class RTypeAssembler : IAssembler
    {
        private static readonly Regex regex = new Regex("^\\s*\\w+\\s+\\$(\\d+)\\s*,\\s*\\$(\\d+)\\s*,\\s*\\$(\\d+)\\s*$", RegexOptions.IgnoreCase);
        private readonly BitArray Opcode;

        public RTypeAssembler(int opcode)
        {
            Opcode = opcode.ToBitArray();
        }

        public IEnumerable<int> Assemble(string line)
        {
            var match = regex.Match(line);

            if (!match.Success)
            {
                throw new InvalidOperationException($"Cannot Assemble {line}");
            }

            var array = new BitArray(20);
            array.Or(Opcode);
            array.LeftShift(6);
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
