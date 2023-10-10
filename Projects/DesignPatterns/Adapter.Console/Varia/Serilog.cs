using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Adapter.Console.Varia
{
    internal class SerilogExample
    {
        public static void Run()
        {
            Serilog.ILogger serilogLogger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            serilogLogger.Information("Hello world!");
        }
    }
}
