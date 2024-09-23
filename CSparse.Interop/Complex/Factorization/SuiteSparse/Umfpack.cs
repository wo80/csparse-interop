
namespace CSparse.Complex.Factorization.SuiteSparse
{
    using CSparse.Interop.SuiteSparse.Umfpack;
    using CSparse.Storage;
    using System;
    using System.Numerics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// UMFPACK wrapper.
    /// </summary>
    public class Umfpack : UmfpackContext<Complex>
    {
        /// <summary>
        /// Initializes a new instance of the Umfpack class.
        /// </summary>
        public Umfpack(CompressedColumnStorage<Complex> matrix)
            : base(matrix)
        {
        }

        /// <inheritdoc />
        public override void GetFactors(out CompressedColumnStorage<Complex> L, out CompressedColumnStorage<Complex> U, out int[] P, out int[] Q, out Complex[] D, out double[] R)
        {
            if (numeric == IntPtr.Zero)
            {
                throw new InvalidOperationException("Numeric factorization unavailable.");
            }

            int rows = info.NROW;
            int cols = info.NCOL;
            int inner = Math.Min(rows, cols);

            L = new SparseMatrix(rows, inner, info.LNZ);
            U = new SparseMatrix(inner, cols, info.UNZ);

            P = new int[rows];
            Q = new int[cols];
            D = new Complex[inner];
            R = new double[rows];

            var Lx = new double[inner];
            var Lz = new double[inner];

            var Ux = new double[inner];
            var Uz = new double[inner];

            var Dx = new double[inner];
            var Dz = new double[inner];

            NativeMethods.umfpack_zi_get_numeric(L.ColumnPointers, L.RowIndices, Lx, Lz, U.ColumnPointers, U.RowIndices, Ux, Uz, P, Q, Dx, Dz, out int recip, R, numeric);

            var Lval = L.Values;

            for (int i = 0; i < info.LNZ; i++)
            {
                Lval[i] = new Complex(Lx[i], Lz[i]);
            }

            var Uval = U.Values;

            for (int i = 0; i < info.UNZ; i++)
            {
                Uval[i] = new Complex(Ux[i], Uz[i]);
            }

            for (int i = 0; i < inner; i++)
            {
                D[i] = new Complex(Dx[i], Dz[i]);
            }
        }

        protected override void DoInitialize()
        {
            NativeMethods.umfpack_zi_defaults(control.Raw);
        }

        protected override int DoSymbolic()
        {
            var h = GCHandle.Alloc(matrix.Values, GCHandleType.Pinned);

            try
            {
                return NativeMethods.umfpack_zi_symbolic(matrix.RowCount, matrix.ColumnCount,
                    matrix.ColumnPointers, matrix.RowIndices, h.AddrOfPinnedObject(), IntPtr.Zero,
                    out symbolic, control.Raw, info.Raw);
            }
            finally
            {
                h.Free();
            }
        }

        protected override int DoNumeric()
        {
            var h = GCHandle.Alloc(matrix.Values, GCHandleType.Pinned);

            try
            {
                return NativeMethods.umfpack_zi_numeric(matrix.ColumnPointers, matrix.RowIndices,
                    h.AddrOfPinnedObject(), IntPtr.Zero, symbolic, out numeric, control.Raw, info.Raw);
            }
            finally
            {
                h.Free();
            }
        }

        protected override int DoFactorize()
        {
            var h = GCHandle.Alloc(matrix.Values, GCHandleType.Pinned);

            try
            {
                int status = NativeMethods.umfpack_zi_symbolic(matrix.RowCount, matrix.ColumnCount,
                    matrix.ColumnPointers, matrix.RowIndices, h.AddrOfPinnedObject(), IntPtr.Zero,
                    out symbolic, control.Raw, info.Raw);

                if (status != Constants.UMFPACK_OK)
                {
                    return status;
                }

                return NativeMethods.umfpack_zi_numeric(matrix.ColumnPointers, matrix.RowIndices,
                    h.AddrOfPinnedObject(), IntPtr.Zero, symbolic, out numeric, control.Raw, info.Raw);
            }
            finally
            {
                h.Free();
            }
        }

        protected override int DoSolve(UmfpackSolve sys, Complex[] input, Complex[] result)
        {
            var ha = GCHandle.Alloc(matrix.Values, GCHandleType.Pinned);
            var hx = GCHandle.Alloc(result, GCHandleType.Pinned);
            var hb = GCHandle.Alloc(input, GCHandleType.Pinned);

            try
            {
                return NativeMethods.umfpack_zi_solve((int)sys, matrix.ColumnPointers, matrix.RowIndices,
                    ha.AddrOfPinnedObject(), IntPtr.Zero,
                    hx.AddrOfPinnedObject(), IntPtr.Zero,
                    hb.AddrOfPinnedObject(), IntPtr.Zero, numeric, control.Raw, info.Raw);
            }
            finally
            {
                ha.Free();
                hx.Free();
                hb.Free();
            }
        }

        protected override int DoSolve(UmfpackSolve sys, Complex[] input, Complex[] result, int[] wi, double[] wx)
        {
            var ha = GCHandle.Alloc(matrix.Values, GCHandleType.Pinned);
            var hx = GCHandle.Alloc(result, GCHandleType.Pinned);
            var hb = GCHandle.Alloc(input, GCHandleType.Pinned);

            try
            {
                return NativeMethods.umfpack_zi_wsolve((int)sys, matrix.ColumnPointers, matrix.RowIndices,
                    ha.AddrOfPinnedObject(), IntPtr.Zero,
                    hx.AddrOfPinnedObject(), IntPtr.Zero,
                    hb.AddrOfPinnedObject(), IntPtr.Zero, numeric, control.Raw, info.Raw, wi, wx);
            }
            finally
            {
                ha.Free();
                hx.Free();
                hb.Free();
            }
        }

        protected override double[] CreateWorkspace(int n, bool refine)
        {
            return new double[refine ? 10 * n : 4 * n];
        }

        protected override void Dispose(bool disposing)
        {
            if (symbolic != IntPtr.Zero)
            {
                NativeMethods.umfpack_zi_free_symbolic(ref symbolic);
            }

            if (numeric != IntPtr.Zero)
            {
                NativeMethods.umfpack_zi_free_numeric(ref numeric);
            }
        }
    }
}
