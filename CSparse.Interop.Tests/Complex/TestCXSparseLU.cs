
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

    class TestCXSparseLU : TestBase
    {
        public TestCXSparseLU()
            : base("CXSparse LU")
        {
        }

        protected override void TestRandomMulti(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
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
