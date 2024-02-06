
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization.SuiteSparse;
    using CSparse.Factorization;

    class TestCXSparseCholesky : TestBase
    {
        public TestCXSparseCholesky()
            : base("CXSparse Cholesky")
        {
        }

        protected override void TestRandom(SparseMatrix matrix)
        {
        }

        protected override void TestRandomMulti(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new CXSparseCholesky(matrix, ColumnOrdering.MinimumDegreeAtPlusA);

            if (!symmetric)
            {
                // throw
            }

            return solver;
        }
    }
}
