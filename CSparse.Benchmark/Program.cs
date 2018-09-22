using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Default matrix size.
            int size = 1000;

            // Default density (non-zeros = size x size x density).
            double density = 0.01;

            if (args.Length == 0 || args[0] == "--test")
            {
                GetSize(args, ref size, ref density);

                CSparse.Double.TestRunner.Run(size, density);
                CSparse.Complex.TestRunner.Run(size, density);
            }
            else if (args[0] == "--cuda")
            {
                GetSize(args, ref size, ref density);

                CSparse.Double.Tests.TestCuda.Run(size, density);
                CSparse.Complex.Tests.TestCuda.Run(size, density);
            }
            else
            {
                var c = GetCommandLine(args);

                CSparse.Double.BenchmarkRunner.Run(c["data"], c["benchmark"]);
            }

            Console.WriteLine("Done.");
        }

        private static void GetSize(string[] args, ref int size, ref double density)
        {
            if (args.Length > 1)
            {
                int.TryParse(args[1], out size);
            }

            if (size < 0 || size > 10000)
            {
                Console.WriteLine("Parameter 'size' out of range: reset to default (1000)");

                size = 1000;
            }

            if (args.Length > 2)
            {
                double.TryParse(args[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out density);
            }

            if (density < 1e-6 || density > 0.1)
            {
                Console.WriteLine("Parameter 'density' out of range: reset to default (0.01)");

                density = 0.01;
            }
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
