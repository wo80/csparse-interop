namespace CSparse.Interop.Tests
{
    using System;
    using System.Globalization;

    internal class Program
    {
        static void Main(string[] args)
        {            // Default matrix size.
            int size = 1000;

            // Default density (non-zeros = size x size x density).
            double density = 0.01;

            GetSize(args, ref size, ref density);

            if (args.Length == 0 || args[0] == "--test")
            {
                Double.TestRunner.Run(size, density);
                Complex.TestRunner.Run(size, density);
            }
            else if (args[0] == "--cuda")
            {
                Double.TestCuda.Run(size, density);
                Complex.TestCuda.Run(size, density);
            }
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
    }
}
