﻿
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization.SuiteSparse;
    using CSparse.Factorization;

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

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
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
