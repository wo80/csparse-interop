
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

    class TestSPQR : Test
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
