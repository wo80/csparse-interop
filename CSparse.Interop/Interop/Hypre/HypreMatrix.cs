
namespace CSparse.Interop.Hypre
{
    using CSparse.Storage;
    using System;
    using System.Runtime.InteropServices;

    public class HypreMatrix<T> : IDisposable
        where T : struct, IEquatable<T>, IFormattable
    {
        HYPRE_IJMatrix mat;

        CompressedColumnStorage<T> A;

        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public HypreMatrix(CompressedColumnStorage<T> matrix, bool symmetric)
        {
            // HYPRE uses CSR storage.
            A = symmetric ? matrix : matrix.Transpose();

            RowCount = matrix.RowCount;
            ColumnCount = matrix.ColumnCount;

            NativeMethods.HYPRE_IJMatrixCreate(Constants.MPI_COMM_WORLD,
                0, A.RowCount - 1,
                0, A.ColumnCount - 1,
                out mat);

            NativeMethods.HYPRE_IJMatrixSetObjectType(mat, Constants.HYPRE_PARCSR);
            NativeMethods.HYPRE_IJMatrixInitialize(mat);

            SetValues();

            NativeMethods.HYPRE_IJMatrixAssemble(mat);
        }

        protected virtual int SetValues()
        {
            var ap = A.ColumnPointers;
            var aj = A.RowIndices;
            var ax = A.Values;

            int cols = A.ColumnCount;
        
            int size = (int)(A.NonZerosCount / (double)A.ColumnCount) + 1;

            var columns = new int[size];
            var values = new T[size];

            var hValues = GCHandle.Alloc(values, GCHandleType.Pinned);
            var pValues = hValues.AddrOfPinnedObject();

            for (int i = 0; i < cols; i++)
            {
                int j = ap[i];
                int end = ap[i + 1];
                int nnz = end - j;

                if (nnz == 0) continue;

                if (size < nnz)
                {
                    size = nnz;
                    columns = new int[size];
                    values = new T[size];

                    hValues.Free();

                    hValues = GCHandle.Alloc(values, GCHandleType.Pinned);
                    pValues = hValues.AddrOfPinnedObject();
                }

                for (int k = 0; j < end; j++, k++)
                {
                    columns[k] = aj[j];
                    values[k] = ax[j];
                }

                int status = NativeMethods.HYPRE_IJMatrixSetValues(mat, 1, ref nnz, ref i, columns, pValues);

                if (status != Constants.HYPRE_OK)
                {
                    return status;
                }
            }

            return Constants.HYPRE_OK;
        }

        internal HYPRE_ParCSRMatrix GetObject()
        {
            NativeMethods.HYPRE_IJMatrixGetObject(mat, out var par_mat);

            return par_mat;
        }

        public void Dispose()
        {
            if (mat.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_IJMatrixDestroy(mat);
                mat.Ptr = IntPtr.Zero;
            }
        }
    }
}
