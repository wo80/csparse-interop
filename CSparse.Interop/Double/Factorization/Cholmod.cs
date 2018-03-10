
namespace CSparse.Double.Factorization
{
    using CSparse.Interop.Cholmod;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// CHOLMOD wrapper.
    /// </summary>
    public class Cholmod : CholmodContext<double>
    {
        /// <summary>
        /// Initializes a new instance of the Cholmod class.
        /// </summary>
        public Cholmod(SparseMatrix matrix)
            : base(matrix)
        {
        }

        /// <inheritdoc />
        public override void Solve(double[] input, double[] result)
        {
            Solve(new DenseMatrix(matrix.RowCount, 1, input), new DenseMatrix(matrix.ColumnCount, 1, result));
        }

        /// <inheritdoc />
        public override void SolveTranspose(double[] input, double[] result)
        {
            throw new NotImplementedException();
        }

        protected override CholmodDense CreateDense(DenseColumnMajorStorage<double> matrix, List<GCHandle> handles)
        {
            var A = new CholmodDense();

            A.nrow = (uint)matrix.RowCount;
            A.ncol = (uint)matrix.ColumnCount;
            A.nzmax = (uint)(matrix.RowCount * matrix.ColumnCount);

            A.dtype = Dtype.Double;
            A.xtype = Xtype.Real;

            A.x = InteropHelper.Pin(matrix.Values, handles);
            A.z = IntPtr.Zero;
            A.d = (uint)matrix.RowCount; // TODO: cholmod_dense leading dimension?

            return A;
        }

        protected override CholmodSparse CreateSparse(CompressedColumnStorage<double> matrix, List<GCHandle> handles)
        {
            var A = new CholmodSparse();

            A.nrow = (uint)matrix.RowCount;
            A.ncol = (uint)matrix.ColumnCount;

            A.dtype = Dtype.Double;
            A.xtype = Xtype.Real;
            A.stype = Stype.Upper; // TODO: this should be configurable!

            A.itype = Constants.CHOLMOD_INT;

            A.nzmax = (uint)matrix.Values.Length;
            A.packed = 1;
            A.sorted = 1;

            A.nz = IntPtr.Zero;
            A.p = InteropHelper.Pin(matrix.ColumnPointers, handles);
            A.i = InteropHelper.Pin(matrix.RowIndices, handles);
            A.x = InteropHelper.Pin(matrix.Values, handles);
            A.z = IntPtr.Zero;

            return A;
        }

        protected override void CopyDense(CholmodDense dense, DenseColumnMajorStorage<double> matrix)
        {
            Marshal.Copy(dense.x, matrix.Values, 0, (int)dense.nzmax);
        }
    }
}
