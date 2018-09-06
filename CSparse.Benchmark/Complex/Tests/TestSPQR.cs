
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.SPQR;
    using System.Numerics;

    class TestSPQR : Test
    {
        public TestSPQR()
            : base("SPQR")
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
