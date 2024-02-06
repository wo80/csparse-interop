
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using System.Numerics;

    class TestCholmod : TestBase
    {
        public TestCholmod()
            : base("CHOLMOD")
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
            var solver = new Cholmod(matrix);

            if (!symmetric)
            {
                // throw
            }

            return solver;
        }
    }
}
