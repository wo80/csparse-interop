
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.Pardiso;

    class TestPardiso : Test
    {
        public TestPardiso()
            : base("PARDISO")
        {
        }
        
        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            // TODO: why does 'RealSymmetricPositiveDefinite' not work?
            int mtype = symmetric ? PardisoMatrixType.RealStructurallySymmetric : PardisoMatrixType.RealNonsymmetric;

            var solver = new Pardiso(matrix, mtype);

            var options = solver.Options;

            // Fill-in reordering from METIS.
            options.ColumnOrderingMethod = PardisoOrdering.NestedDissection;

            // Max numbers of iterative refinement steps.
            options.IterativeRefinement = 2;

            // Perturb the pivot elements with 1E-13.
            options.PivotingPerturbation = 13;

            // Use nonsymmetric permutation and scaling MPS.
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
