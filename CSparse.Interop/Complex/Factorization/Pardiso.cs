
namespace CSparse.Complex.Factorization
{
    using CSparse.Interop.Pardiso;
    using System.Numerics;

    /// <summary>
    /// PARDISO wrapper.
    /// </summary>
    public class Pardiso : PardisoContext<Complex>
    {
        /// <summary>
        /// Initializes a new instance of the Pardiso class.
        /// </summary>
        public Pardiso(SparseMatrix matrix)
            : base(matrix, PardisoMatrixType.ComplexNonsymmetric)
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
        protected override void Solve(int sys, Complex[] input, Complex[] result)
        {
            Solve(sys, new DenseMatrix(matrix.RowCount, 1, input), new DenseMatrix(matrix.ColumnCount, 1, result));
        }
    }
}
