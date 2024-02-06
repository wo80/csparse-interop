
namespace CSparse.Interop.Tests.Complex
{
    using CSparse.Complex;
    using CSparse.Interop.Tests.Complex.EigenSolvers;
    using System;

    public static class TestRunner
    {
        public static void Run(int size, double density)
        {
            var A = Generate.Random(size, size, density);
            var B = Generate.RandomHermitian(size, density, true);

            Console.WriteLine("Running tests (Complex) ... [N = {0}]", size);
            Console.WriteLine();

            new TestSparseCholesky().Run(A, B);
            new TestSparseLU().Run(A, B);
            new TestSparseQR().Run(A, B);

            new TestCXSparseCholesky().Run(A, B);
            new TestCXSparseLU().Run(A, B);
            new TestCXSparseQR().Run(A, B);

            new TestUmfpack().Run(A, B);
            new TestCholmod().Run(A, B);
            new TestSPQR().Run(A, B);
            new TestSuperLU().Run(A, B);
            new TestPardiso().Run(A, B);
            
            Console.WriteLine();
            Console.WriteLine("Running eigensolver tests (Complex) ... [N = {0}]", size);
            Console.WriteLine();

            new TestArpack().Run(size);
            new TestFeast().Run(size);

            Console.WriteLine();
        }
    }
}
