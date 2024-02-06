
namespace CSparse.Interop.Tests.Complex
{
    using CSparse.Complex;
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

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
