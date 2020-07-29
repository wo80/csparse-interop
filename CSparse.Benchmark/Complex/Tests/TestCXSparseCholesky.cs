
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

    class TestCXSparseCholesky : Test
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

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
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
