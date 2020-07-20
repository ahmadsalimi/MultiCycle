using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assembler
{
    class Program
    {
        private const string DefaultTarget = "\\a.mif";

        static async Task Main(string[] args)
        {
            var logger = CreateLogger();

            var assemblyPath = args.Any() ? args[0] : throw new ArgumentException("Please pass source assembly file", nameof(args));
            var targetPath = args.Length > 1 ? args[1] : Path.GetDirectoryName(assemblyPath) + DefaultTarget;

            logger.LogInformation("Start assembling {sourse} to {target}", Path.GetFileName(assemblyPath), Path.GetFileName(targetPath));

            try
            {
                await FileAssembler.Assemble(assemblyPath, targetPath, logger);
                logger.LogInformation("Assembling finished.");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on Assembling: {message}", e.Message);
            }
        }

        private static ILogger<Program> CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            return new LoggerFactory()
                .AddSerilog()
                .CreateLogger<Program>();
        }
    }
}
