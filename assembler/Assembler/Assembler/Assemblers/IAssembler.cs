using System.Collections.Generic;

namespace Assembler.Assemblers
{
    internal interface IAssembler
    {
        IEnumerable<int> Assemble(string line);
    }
}
