
namespace CSparse.Double.Factorization.SuiteSparse
{
    using CSparse.Interop.SuiteSparse.Umfpack;
    using System;

#if X64
    using SuiteSparse_long = System.Int64;
#else
    using SuiteSparse_long = System.Int32;
#endif

    /// <summary>
    /// UMFPACK wrapper.
    /// </summary>
    public class Umfpack_Long : UmfpackContext<double>
    {
        SuiteSparse_long[] columnPointers;
        SuiteSparse_long[] rowIndices;

        /// <summary>
        /// Initializes a new instance of the Umfpack_Long class.
        /// </summary>
        public Umfpack_Long(SparseMatrix matrix)
            : base(matrix)
        {
            columnPointers = Convert(matrix.ColumnPointers);
            rowIndices = Convert(matrix.RowIndices);
        }

        private SuiteSparse_long[] Convert(int[] source)
        {
            int size = source.Length;

            var target = new SuiteSparse_long[size];

            for (int i = 0; i < size; i++)
            {
                target[i] = source[i];
            }

            return target;
        }

        /// <inheritdoc/>
        protected override void DoInitialize()
        {
            NativeMethods.umfpack_dl_defaults(control.Raw);
        }

        /// <inheritdoc/>
        protected override int DoSymbolic()
        {
            return (int)NativeMethods.umfpack_dl_symbolic(matrix.RowCount, matrix.ColumnCount,
                columnPointers, rowIndices, matrix.Values,
                out symbolic, control.Raw, info.Raw);
        }

        /// <inheritdoc/>
        protected override int DoNumeric()
        {
            return (int)NativeMethods.umfpack_dl_numeric(columnPointers, rowIndices, matrix.Values,
                symbolic, out numeric, control.Raw, info.Raw);
        }

        /// <inheritdoc/>
        protected override int DoFactorize()
        {
            int status = (int)NativeMethods.umfpack_dl_symbolic(matrix.RowCount, matrix.ColumnCount,
                columnPointers, rowIndices, matrix.Values,
                out symbolic, control.Raw, info.Raw);

            if (status != Constants.UMFPACK_OK)
            {
                return status;
            }

            return (int)NativeMethods.umfpack_dl_numeric(columnPointers, rowIndices, matrix.Values,
                symbolic, out numeric, control.Raw, info.Raw);
        }

        /// <inheritdoc/>
        protected override int DoSolve(UmfpackSolve sys, double[] input, double[] result)
        {
            return (int)NativeMethods.umfpack_dl_solve((SuiteSparse_long)sys, columnPointers, rowIndices, matrix.Values,
                result, input, numeric, control.Raw, info.Raw);
        }

        /// <inheritdoc/>
        protected override int DoSolve(UmfpackSolve sys, double[] input, double[] result, int[] wi, double[] wx)
        {
            // TODO: make class member.
            var wl = new SuiteSparse_long[wi.Length];

            return (int)NativeMethods.umfpack_dl_wsolve((SuiteSparse_long)sys, columnPointers, rowIndices, matrix.Values,
                result, input, numeric, control.Raw, info.Raw, wl, wx);
        }

        /// <inheritdoc/>
        protected override double[] CreateWorkspace(int n, bool refine)
        {
            return new double[refine ? 5 * n : n];
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (symbolic != IntPtr.Zero)
            {
                NativeMethods.umfpack_dl_free_symbolic(ref symbolic);
            }

            if (numeric != IntPtr.Zero)
            {
                NativeMethods.umfpack_dl_free_numeric(ref numeric);
            }
        }
    }
}
