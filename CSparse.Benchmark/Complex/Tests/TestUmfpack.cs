
namespace CSparse.Complex
{
    using CSparse.Complex.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.Umfpack;
    using System.Numerics;

    class TestUmfpack : Test
    {
        public TestUmfpack()
            : base("UMFPACK")
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new Umfpack(matrix);

            if (symmetric)
            {
                var options = solver.Control;

                options.Strategy = UmfpackStrategy.Symmetric;
            }

            return solver;
        }
    }
}
