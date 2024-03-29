﻿
namespace CSparse.Complex.Factorization.SuiteSparse
{
    using CSparse.Interop.Common;
    using CSparse.Interop.SuiteSparse.CXSparse;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.InteropServices;

    public class CXSparseLU : CXSparseContext<Complex>
    {
        private readonly double tol;

        /// <summary>
        /// Initializes a new instance of the SuperLU class.
        /// </summary>
        public CXSparseLU(CompressedColumnStorage<Complex> matrix, ColumnOrdering ordering, double tol)
            : base(matrix, ordering)
        {
            this.tol = tol;
        }

        public override void Solve(Complex[] input, Complex[] result)
        {
            if (!factorized && DoFactorize() != 0)
            {
                throw new Exception(); // TODO: exception
            }

            var n = matrix.RowCount;

            var h = new List<GCHandle>();

            var b = InteropHelper.Pin(input, h);
            var x = InteropHelper.Pin(result, h);
            var t = InteropHelper.Pin(w, h);

            try
            {
                // x = b(p)
                if (NativeMethods.cs_ci_ipvec(N.pinv, b, t, n) == 0) return;

                // x = L\x
                if (NativeMethods.cs_ci_lsolve(N.L, t) == 0) return;

                // x = U\x
                if (NativeMethods.cs_ci_usolve(N.U, t) == 0) return;

                // b(q) = x 
                if (NativeMethods.cs_ci_pvec(S.q, t, x, n) == 0) return;
            }
            finally
            {
                InteropHelper.Free(h);
            }
        }

        protected override int DoFactorize()
        {
            var h = new List<GCHandle>();
            var A = CreateSparse(matrix, h);

            try
            {
                int order = (int)ordering;

                // ordering and symbolic analysis
                var pS = NativeMethods.cs_ci_sqr(order, ref A, 0);

                if (pS == IntPtr.Zero)
                {
                    return -1;
                }

                S = Marshal.PtrToStructure<css>(pS);

                // numeric LU factorization
                var pN = NativeMethods.cs_ci_lu(ref A, ref S, tol);

                if (pN == IntPtr.Zero)
                {
                    return -1;
                }

                N = Marshal.PtrToStructure<csn>(pN);
            }
            finally
            {
                InteropHelper.Free(h);
            }

            return 0;
        }
    }
}
