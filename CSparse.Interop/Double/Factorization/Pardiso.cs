
namespace CSparse.Double.Factorization
{
    using CSparse.Interop.Pardiso;

    /// <summary>
    /// PARDISO wrapper.
    /// </summary>
    public class Pardiso : PardisoContext<double>
    {
        /// <summary>
        /// Initializes a new instance of the Pardiso class.
        /// </summary>
        public Pardiso(SparseMatrix matrix)
            : base(matrix, PardisoMatrixType.RealNonsymmetric)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Pardiso class.
        /// </summary>
        public Pardiso(SparseMatrix matrix, int mtype)
            : base(matrix, mtype)
        {
        }

        /// <inheritdoc />
        protected override void Solve(int sys, double[] input, double[] result)
        {
            Solve(sys, new DenseMatrix(matrix.RowCount, 1, input), new DenseMatrix(matrix.ColumnCount, 1, result));
        }
    }
}
