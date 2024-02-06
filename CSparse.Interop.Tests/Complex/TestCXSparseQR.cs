
namespace CSparse.Interop.Tests.Complex
{
    using CSparse.Complex;
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

    class TestCXSparseQR : TestBase
    {
        public TestCXSparseQR()
            : base("CXSparse QR")
        {
        }

        protected override void TestRandomSymmetric(SparseMatrix matrix)
        {
        }

        protected override void TestRandomMulti(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new CXSparseQR(matrix, ColumnOrdering.MinimumDegreeAtA);

            if (!symmetric)
            {
                // throw
            }

            return solver;
        }
    }
}
