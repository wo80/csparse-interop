
namespace CSparse.Interop.Tests.Complex
{
    using CSparse.Complex;
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

    class TestSPQR : TestBase
    {
        public TestSPQR()
            : base("SPQR")
        {
        }

        protected override void TestRandomSymmetric(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new SPQR(matrix);

            if (symmetric)
            {
            }

            return solver;
        }
    }
}
