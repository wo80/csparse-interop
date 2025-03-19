
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Double.Factorization.MKL;
    using CSparse.Factorization;
    using CSparse.Interop.MKL.Pardiso;
    using CSparse.Storage;

    class TestPardiso : TestBase
    {
        public TestPardiso()
            : base("PARDISO")
        {
        }
        
        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            if (symmetric && !matrix.IsLower())
            {
                // The PARDISO spd solver expects a triangular matrix.
                matrix = (SparseMatrix)matrix.ToLower();
            }

            int mtype = symmetric ? PardisoMatrixType.RealSymmetricPositiveDefinite : PardisoMatrixType.RealNonsymmetric;

            var solver = new Pardiso(matrix, mtype);

            var options = solver.Options;

            // Fill-in reordering from METIS.
            options.ColumnOrderingMethod = PardisoOrdering.NestedDissection;

            // Max numbers of iterative refinement steps.
            options.IterativeRefinement = 2;

            // Perturb the pivot elements with 1E-13.
            options.PivotingPerturbation = 13;

            // Use non-symmetric permutation and scaling MPS.
            options.Scaling = true;

            // Maximum weighted matching algorithm is switched-on (default for non-symmetric).
            options.WeightedMatching = true;

            if (symmetric)
            {
                // Maximum weighted matching algorithm is switched-off (default for symmetric).
                // Try to enable in case of inappropriate accuracy.
                options.WeightedMatching = false;
            }

            options.ZeroBasedIndexing = true;
            options.CheckMatrix = true;

            return solver;
        }
    }
}
