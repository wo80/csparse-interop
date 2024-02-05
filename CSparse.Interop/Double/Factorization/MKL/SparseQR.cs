
namespace CSparse.Double.Factorization.MKL
{
    using CSparse.Interop.Common;
    using CSparse.Interop.MKL;
    using CSparse.Interop.MKL.SparseQR;
    using CSparse.Storage;
    using System;

    /// <summary>
    /// SparseQR wrapper.
    /// </summary>
    public class SparseQR : SparseQRContext<double>
    {
        /// <summary>
        /// Initializes a new instance of the SparseQR class.
        /// </summary>
        public SparseQR(CompressedColumnStorage<double> matrix)
            : base(new CompressedRowStorage<double>(matrix))
        {
        }

        /// <summary>
        /// Initializes a new instance of the SparseQR class.
        /// </summary>
        public SparseQR(CompressedRowStorage<double> matrix)
            : base(matrix)
        {
        }

        /// <inheritdoc />
        public override void Solve(double[] input, double[] result)
        {
            var b = new DenseMatrix(matrix.RowCount, 1, input);
            var x = new DenseMatrix(matrix.ColumnCount, 1, result);
            Solve(b, x);
        }

        protected override SparseStatus DoInitialize(CompressedRowStorage<double> matrix)
        {
            descr.type = SparseMatrixType.General;

            var ap = InteropHelper.Pin(matrix.RowPointers, handles);
            var ai = InteropHelper.Pin(matrix.ColumnIndices, handles);
            var ax = InteropHelper.Pin(matrix.Values, handles);

            return NativeMethods.mkl_sparse_d_create_csr(ref csrA, SparseIndexBase.Zero, matrix.RowCount, matrix.ColumnCount, ap, IntPtr.Add(ap, Constants.SizeOfInt), ai, ax);
        }

        protected override SparseStatus DoFactorize()
        {
            return NativeMethods.mkl_sparse_d_qr_factorize(csrA, IntPtr.Zero);
        }

        protected override SparseStatus DoSolve(IntPtr A, int columns, IntPtr x, int ldx, IntPtr b, int ldb)
        {
            // We assume type of x and b is DenseColumnMajorStorage<double>.
            var denseLayout = SparseLayout.ColumnMajor;

            return NativeMethods.mkl_sparse_d_qr_solve(SparseOperation.NonTranspose, csrA, IntPtr.Zero, denseLayout, columns, x, ldx, b, ldb);
        }
    }
}
