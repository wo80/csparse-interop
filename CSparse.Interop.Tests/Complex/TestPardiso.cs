
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization.MKL;
    using CSparse.Factorization;
    using CSparse.Interop.MKL.Pardiso;
    using System.Numerics;

    class TestPardiso : TestBase
    {
        public TestPardiso()
            : base("PARDISO")
        {
        }
        
        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            // TODO: why does 'ComplexHermitianPositiveDefinite' not work?
            int mtype = symmetric ? PardisoMatrixType.ComplexStructurallySymmetric : PardisoMatrixType.ComplexNonsymmetric;

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
