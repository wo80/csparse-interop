using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0 || args[0] == "--test")
            {
                int size = GetSize(args);

                CSparse.Double.TestRunner.Run(size);
                CSparse.Complex.TestRunner.Run(size);
            }
            else if (args[0] == "--cuda")
            {
                int size = GetSize(args);

                CSparse.Double.Tests.TestCuda.Run(size);
                CSparse.Complex.Tests.TestCuda.Run(size);
            }
            else
            {
                var c = GetCommandLine(args);

                CSparse.Double.BenchmarkRunner.Run(c["data"], c["benchmark"]);
            }

            Console.WriteLine("Done.");
        }

        private static int GetSize(string[] args)
        {
            int size = 1000;

            if (args.Length > 1)
            {
                int.TryParse(args[1], out size);
            }

            if (size < 0 || size > 10000)
            {
                Console.WriteLine("Parameter 'size' out of range: reset to default (1000)");

                size = 1000;
            }

            return size;
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
