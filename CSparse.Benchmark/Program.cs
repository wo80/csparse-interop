using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "--arpack")
            {
                CSparse.Examples.Double.TestArpack.Run();
                CSparse.Examples.Complex.TestArpack.Run();
                CSparse.Examples.Double.TestSpectra.Run();
            }
            else
            {
                var c = GetCommandLine(args);

                CSparse.Double.BenchmarkRunner.Run(c["data"], c["benchmark"]);
            }

            Console.WriteLine("Done.");
        }

        private static Dictionary<string, string> GetCommandLine(string[] args)
        {
            var c = new Dictionary<string, string>();

            // The data directory.
            c["data"] = Path.GetFullPath("./data");

            // The benchmark file.
            c["benchmark"] = "benchmark.lst";

            int length = args.Length;

            for (int i = 0; i < length; i++)
            {
                if (args[i] == "-d" && (i + 1 < length))
                {
                    c["data"] = Path.GetFullPath(args[i + 1]);
                    i++;
                }
                else if (args[i] == "-b" && (i + 1 < length))
                {
                    c["benchmark"] = args[i + 1];
                    i++;
                }
            }

            return c;
        }
    }
}
