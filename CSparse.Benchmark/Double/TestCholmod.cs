
namespace CSparse.Double
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;

    class TestCholmod : Test
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

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
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
