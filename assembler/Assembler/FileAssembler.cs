using Assembler.Assemblers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assembler
{
    internal static class FileAssembler
    {
        private static readonly Dictionary<string, IAssembler> InstructionAssemblers = new Dictionary<string, IAssembler>
        {
            ["add"] = new AddAssembler(),
            ["sub"] = new RTypeAssembler(1),
            ["srl"] = new ShiftAssembler(2),
            ["sll"] = new ShiftAssembler(3),
            ["noop"] = new NoOpAssembler(),
            ["nand"] = new RTypeAssembler(5),
            ["min"] = new RTypeAssembler(6),
            ["slt"] = new RTypeAssembler(7),
            ["li"] = new LoadAssembler()
        };
        private static readonly Regex EmptyRegex = new Regex("\\s*(#.*)?\\s*$");

        internal static async Task Assemble(string source, string target, ILogger logger)
        {
            using var reader = new StreamReader(source);
            using var writer = new StreamWriter(File.Open(target, FileMode.Create));

            await writer.WriteLineAsync("DEPTH = 65536;\nWIDTH = 20;\nADDRESS_RADIX = HEX;\nDATA_RADIX = BIN;\nCONTENT\nBEGIN");

            int address = 0;
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                line = EmptyRegex.Replace(line, "");
                if (!line.Any())
                {
                    logger.LogInformation("Scaping {line}", line);
                    continue;
                }

                logger.LogInformation("Assembling {line}", line);

                await writer.WriteAsync($"{address:X} : ");

                foreach (var bit in InstructionAssemblers[GetInstruction(line)].Assemble(line))
                {
                    await writer.WriteAsync(bit.ToString());
                }
                await writer.WriteLineAsync(";");

                address++;
            }

            await writer.WriteLineAsync("END;");
        }

        internal static string GetInstruction(string line) => Regex.Split(line, "\\s+")[0];
    }
}
