namespace CSparse.Interop.Tests
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    internal class Program
    {
        static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Library.SetImportResolver();
            }

            ParseCommandlineArgs(args, out int size, out double density);

            Double.TestRunner.Run(size, density);
            Complex.TestRunner.Run(size, density);
            AMD.TestRunner.Run();

            //Double.TestCuda.Run(size, density);
            //Complex.TestCuda.Run(size, density);
        }

        private static void ParseCommandlineArgs(string[] args, out int size, out double density)
        {
            // Default matrix size.
            size = 1000;

            // Default density (non-zeros = size x size x density).
            density = 0.01;

            int length = args.Length;

            for (int i = 0; i < length; i++)
            {
                var arg = args[i];

                if (arg.Equals("--size") && i + 1 < length)
                {
                    int.TryParse(args[i + 1], out size);
                    i++;
                }
                else if (arg.Equals("--density") && i + 1 < length)
                {
                    double.TryParse(args[i + 1], NumberStyles.Any, NumberFormatInfo.InvariantInfo, out density);
                    i++;
                }
            }

            if (size < 0 || size > 10000)
            {
                Console.WriteLine("Parameter 'size' out of range: reset to default (1000)");

                size = 1000;
            }

            if (density < 1e-6 || density > 0.1)
            {
                Console.WriteLine("Parameter 'density' out of range: reset to default (0.01)");

                density = 0.01;
            }
        }
    }
}
