
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Double.Factorization.SuiteSparse;
    using CSparse.Factorization;

    class TestCXSparseLU : TestBase
    {
        public TestCXSparseLU()
            : base("CXSparse LU")
        {
        }

        protected override void TestRandomMulti(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new CXSparseLU(matrix, ColumnOrdering.MinimumDegreeAtPlusA, 0.1);

            if (!symmetric)
            {
                // throw
            }

            return solver;
        }
    }
}
