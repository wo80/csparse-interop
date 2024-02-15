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
                var c = ParseCommandlineArgs(args);

                CSparse.Double.BenchmarkRunner.Run(c["data"], c["benchmark"]);
            }

            Console.WriteLine("Done.");
        }

        private static Dictionary<string, string> ParseCommandlineArgs(string[] args)
        {
            var c = new Dictionary<string, string>();

            // The data directory.
            c["data"] = Path.GetFullPath("./data");

            // The benchmark file.
            c["benchmark"] = "benchmark.lst";

            int length = args.Length;

            for (int i = 0; i < length; i++)
            {
                var arg = args[i];

                if ((arg == "-d" || arg == "--data-dir") && i + 1 < length)
                {
                    c["data"] = Path.GetFullPath(args[i + 1]);
                    i++;
                }
                else if ((arg == "-b" || arg == "--benchmark-file") && i + 1 < length)
                {
                    c["benchmark"] = args[i + 1];
                    i++;
                }
            }

            return c;
        }
    }
}
