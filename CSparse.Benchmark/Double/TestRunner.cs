
namespace CSparse.Double
{
    using CSparse.Double.Tests;
    using System;

    public static class TestRunner
    {
        public static void Run(int size, double density)
        {
            var A = Generate.Random(size, size, density);
            var B = Generate.RandomSymmetric(size, density, true);

            Console.WriteLine("Running tests (Double) ... [N = {0}]", size);
            Console.WriteLine();

            new TestSparseCholesky().Run(A, B);
            new TestSparseLU().Run(A, B);
            new TestSparseQR().Run(A, B);

            new TestUmfpack().Run(A, B);
            new TestCholmod().Run(A, B);
            new TestSPQR().Run(A, B);
            new TestSuperLU().Run(A, B);
            new TestPardiso().Run(A, B);

            Console.WriteLine();
            Console.WriteLine("Running eigensolver tests (Double) ... [N = {0}]", size);
            Console.WriteLine();

            new TestArpack().Run(size);
            new TestSpectra().Run(size);
            new TestFeast().Run(size);
            new TestExtendedEigensolver().Run(size);

            Console.WriteLine();
        }
    }
}
