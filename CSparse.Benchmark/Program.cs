using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = GetCommandLine(args);

            CSparse.Double.BenchmarkRunner.Run(c["data"], c["benchmark"]);

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

            for (int i = 0; i < length; i += 2)
            {
                if (args[i] == "-d" && (i + 1 < length))
                {
                    c["data"] = Path.GetFullPath(args[i + 1]);
                }

                if (args[i] == "-b" && (i + 1 < length))
                {
                    c["benchmark"] = args[i + 1];
                }
            }

            return c;
        }
    }
}
