
namespace CSparse.Double.Benchmarks
{
    using CSparse.Benchmark;
    using CSparse.Double.Factorization.MKL;
    using CSparse.Factorization;
    using CSparse.Interop.MKL.Pardiso;
    using CSparse.Storage;

    class BenchmarkPardiso : Benchmark<double>
    {
        public BenchmarkPardiso(MatrixFileCollection collection)
            : base(collection)
        {
        }

        protected override IDisposableSolver<double> CreateSolver(CompressedColumnStorage<double> matrix, bool symmetric)
        {
            int mtype = symmetric ? PardisoMatrixType.RealStructurallySymmetric : PardisoMatrixType.RealNonsymmetric;

            var solver = new Pardiso((SparseMatrix)matrix, mtype);

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

        protected override double[] CreateTestVector(int size)
        {
            return Vector.Create(size, 1.0);
        }

        protected override double ComputeError(double[] actual, double[] expected)
        {
            return Helper.ComputeError(actual, expected);
        }
    }
}
