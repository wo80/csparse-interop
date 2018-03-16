
namespace CSparse.Complex.Factorization
{
    using CSparse.Interop.Cholmod;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// CHOLMOD wrapper.
    /// </summary>
    public class Cholmod : CholmodContext<Complex>
    {
        private double[] tempStorage;

        /// <summary>
        /// Initializes a new instance of the Cholmod class.
        /// </summary>
        public Cholmod(SparseMatrix matrix)
            : base(matrix)
        {
        }

        /// <inheritdoc />
        public override void Solve(Complex[] input, Complex[] result)
        {
            Solve(new DenseMatrix(matrix.RowCount, 1, input), new DenseMatrix(matrix.ColumnCount, 1, result));
        }

        protected override CholmodDense CreateDense(DenseColumnMajorStorage<Complex> matrix, List<GCHandle> handles)
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

        protected override CholmodSparse CreateSparse(CompressedColumnStorage<Complex> matrix, List<GCHandle> handles)
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

        protected override void CopyDense(CholmodDense dense, DenseColumnMajorStorage<Complex> matrix)
        {
            int count = 2 * (int)dense.nzmax;

            if (tempStorage == null)
            {
                tempStorage = new double[count];
            }

            Marshal.Copy(dense.x, tempStorage, 0, count);

            var values = matrix.Values;

            count = count / 2;

            for (int i = 0; i < count; i++)
            {
                values[i] = new Complex(tempStorage[2 * i], tempStorage[2 * i + 1]);
            }
        }
    }
}
