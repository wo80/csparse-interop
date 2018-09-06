
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.SuperLU;
    using System.Numerics;

    class TestSuperLU : Test
    {
        public TestSuperLU()
            : base("SuperLU")
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new SuperLU(matrix);

            if (symmetric)
            {
                var options = solver.Options;

                options.SymmetricMode = true;
                options.ColumnOrderingMethod = OrderingMethod.MinimumDegreeAtPlusA;
                options.DiagonalPivotThreshold = 0.001;
            }

            return solver;
        }
    }
}
