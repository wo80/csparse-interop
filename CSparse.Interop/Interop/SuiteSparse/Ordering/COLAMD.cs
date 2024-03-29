﻿
namespace CSparse.Interop.SuiteSparse.Ordering
{
    using CSparse.Storage;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The COLAMD column approximate minimum degree ordering algorithm computes a
    /// permutation vector P such that the LU factorization of A(:, P) tends to be
    /// sparser than that of A.The Cholesky factorization of (A (:, P))'*(A (:,P))
    /// will also tend to be sparser than that of A'* A.
    /// </summary>
    public class COLAMD
    {
        #region COLAMD constants

        internal const int COLAMD_KNOBS = 20;
        internal const int COLAMD_STATS = 20;

        // Error codes returned in stats[3]

        internal const int COLAMD_OK = 0;
        internal const int COLAMD_ERROR_A_not_present = -1;
        internal const int COLAMD_ERROR_p_not_present = -2;
        internal const int COLAMD_ERROR_nrow_negative = -3;
        internal const int COLAMD_ERROR_ncol_negative = -4;
        internal const int COLAMD_ERROR_nnz_negative = -5;
        internal const int COLAMD_ERROR_p0_nonzero = -6;
        internal const int COLAMD_ERROR_A_too_small = -7;
        internal const int COLAMD_ERROR_col_length_negative = -8;
        internal const int COLAMD_ERROR_row_index_out_of_bounds = -9;
        internal const int COLAMD_ERROR_out_of_memory = -10;
        internal const int COLAMD_ERROR_internal_error = -999;

        #endregion

        #region Public properties (COLAMD options)

        /// <summary>
        /// Gets or sets a value used to determine whether or not a given input row is "dense" (default = 10.0).
        /// </summary>
        /// <remarks>
        /// Rows with more than max(16, DenseRow * sqrt(n_col)) entries are removed prior to ordering.
        /// </remarks>
        public double DenseRow { get; set; } = 10.0;

        /// <summary>
        /// Gets or sets a value used to determine whether or not a given input column is "dense" (default = 10.0).
        /// </summary>
        /// <remarks>
        /// Columns with more than max(16, DenseCol * sqrt (min(n_row,n_col))) entries are removed prior
        /// to ordering, and placed last in the output column ordering. 
        /// </remarks>
        public double DenseColumn { get; set; } = 10.0;

        /// <summary>
        /// Gets or sets a value used to determine whether or not aggressive absorption is to be performed (default = true).
        /// </summary>
        public bool Aggressive { get; set; } = true;

        #endregion

        #region Statistics

        public class Info
        {
            internal int[] data = new int[COLAMD_STATS];

            /// <summary>
            /// Gets the number of dense or empty rows ignored.
            /// </summary>
            public int IgnoredRows => data[0];

            /// <summary>
            /// Gets the number of dense or empty columns ignored.
            /// </summary>
            public int IgnoredColumns => data[1];

            /// <summary>
            /// Gets the number of garbage collections performed.
            /// </summary>
            public int NCMPA => data[2];

            /// <summary>
            /// Gets the return value of colamd.
            /// </summary>
            public int Status => data[3];
        }

        #endregion

        /// <summary>
        /// Return the COLAMD version.
        /// </summary>
        /// <returns>The COLAMD version.</returns>
        public static Version Version()
        {
            int[] version = new int[3];
            NativeMethods.colamd_version(version);
            return new Version(version[0], version[1], version[2]);
        }

        /// <summary>
        /// Generate a fill-reducing ordering using the COLAMD algorithm.
        /// </summary>
        /// <param name="A">The matrix.</param>
        /// <returns>The permutation vector or NULL, if an error occurred.</returns>
        public static int[] Generate<T>(CompressedColumnStorage<T> A)
            where T : struct, IEquatable<T>, IFormattable
        {
            int rows = A.RowCount;
            int cols = A.ColumnCount;

            var p = new int[cols + 1];

            A.ColumnPointers.CopyTo(p, 0);

            var amd = new COLAMD();

            var info = amd.Order(rows, cols, A.RowIndices, p);

            if (info.Status < COLAMD_OK)
            {
                // error
                return null;
            }

            return p;
        }

        /// <summary>
        /// Generate a fill-reducing ordering using the CCOLAMD algorithm.
        /// </summary>
        /// <param name="A">The matrix.</param>
        /// <param name="constraints">Constraint set of A (of size ncol).</param>
        /// <returns>The permutation vector or NULL, if an error occurred.</returns>
        public static int[] Generate<T>(CompressedColumnStorage<T> A, int[] constraints)
            where T : struct, IEquatable<T>, IFormattable
        {
            int rows = A.RowCount;
            int cols = A.ColumnCount;

            var p = new int[cols + 1];

            A.ColumnPointers.CopyTo(p, 0);

            var amd = new COLAMD();

            var info = amd.Order(rows, cols, A.RowIndices, p, constraints);

            if (info.Status < COLAMD_OK)
            {
                // error
                return null;
            }

            return p;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows">Number of rows in A.</param>
        /// <param name="cols">Number of columns in A.</param>
        /// <param name="rowIndices">Row indices of A (of size nnz).</param>
        /// <param name="p">Column pointers of A on input, permutation vector on output (of size cols+1).</param>
        /// <returns>COLAMD output statistics and error codes.</returns>
        public Info Order(int rows, int cols, int[] rowIndices, int[] p)
        {
            var info = new Info();

            int alen = NativeMethods.colamd_recommended(p[cols], rows, cols);

            if (alen == 0)
            {
                throw new ArgumentException();
            }

            var A = new int[alen];

            rowIndices.CopyTo(A, 0);

            NativeMethods.colamd(rows, cols, alen, A, p, GetControl(), info.data);

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows">Number of rows in A.</param>
        /// <param name="cols">Number of columns in A.</param>
        /// <param name="rowIndices">Row indices of A (of size nnz).</param>
        /// <param name="p">Column pointers of A on input, permutation vector on output (of size cols+1).</param>
        /// <param name="cmember">Constraint set of A (of size cols).</param>
        /// <returns></returns>
        public Info Order(int rows, int cols, int[] rowIndices, int[] p, int[] cmember)
        {
            var info = new Info();

            int alen = NativeMethods.ccolamd_recommended(p[cols], rows, cols);

            if (alen == 0)
            {
                throw new ArgumentException();
            }

            var A = new int[alen];

            rowIndices.CopyTo(A, 0);

            NativeMethods.ccolamd(rows, cols, alen, A, p, GetControl(), info.data, cmember);

            return info;
        }

        private double[] GetControl()
        {
            var a = new double[COLAMD_KNOBS];

            a[0] = DenseRow;
            a[1] = DenseColumn;
            a[2] = Aggressive ? 1.0 : 0.0;

            return a;
        }

        internal static class NativeMethods
        {
#if SUITESPARSE_AIO
            const string COLAMD_DLL = "suitesparse";
            const string CCOLAMD_DLL = "suitesparse";
#else
            const string COLAMD_DLL = "colamd";
            const string CCOLAMD_DLL = "ccolamd";
#endif

            #region COLAMD

            [DllImport(COLAMD_DLL)]
            public static extern void colamd_version(int[] version);

            /// <summary>
            /// Returns recommended value of alen.
            /// </summary>
            /// <param name="nnz">Non-zeros in A.</param>
            /// <param name="n_row">Number of rows in A.</param>
            /// <param name="n_col">Number of columns in A.</param>
            /// <returns>Returns recommended value of alen, or 0 if input arguments are erroneous.</returns>
            [DllImport(COLAMD_DLL)]
            public static extern int colamd_recommended(int nnz, int n_row, int n_col);

            /// <summary>
            /// Sets default parameters for COLAMD.
            /// </summary>
            /// <param name="knobs">The knobs array.</param>
            /// <returns></returns>
            [DllImport(COLAMD_DLL)]
            public static extern int colamd_set_defaults(
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_KNOBS)] double[] knobs
            );

            /// <summary>
            /// Computes a column ordering (Q) of A such that P(AQ)=LU or (AQ)'AQ=LL' have less
            /// fill-in and require fewer floating point operations than factorizing the unpermuted
            /// matrix A or A'A, respectively.
            /// </summary>
            /// <param name="n_row">Number of rows in A.</param>
            /// <param name="n_col">Number of columns in A.</param>
            /// <param name="Alen">Size of the array A.</param>
            /// <param name="A">Row indices of A, of size Alen.</param>
            /// <param name="p">Column pointers of A, of size n_col+1.</param>
            /// <param name="knobs">Parameter settings for COLAMD.</param>
            /// <param name="stats">COLAMD output statistics and error codes.</param>
            /// <returns>Returns 1 if successful, 0 otherwise.</returns>
            /// <remarks>
            /// A and p arguments are modified on output.
            /// </remarks>
            [DllImport(COLAMD_DLL)]
            public static extern int colamd(
                int n_row,
                int n_col,
                int Alen,
                [In, Out] int[] A,
                [In, Out] int[] p,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_KNOBS)] double[] knobs,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)] int[] stats
            );

#if SUITESPARSE_AIO
            /// <summary>
            /// Computes an ordering P of a symmetric sparse matrix A such that the Cholesky factorization
            /// PAP' = LL' remains sparse. It is based on a column ordering of a matrix M constructed so that
            /// the nonzero pattern of M'M is the same as A. The matrix A is assumed to be symmetric; only the
            /// strictly lower triangular part is accessed.
            /// </summary>
            /// <param name="n">Number of rows columns in A.</param>
            /// <param name="A">Row indices of A.</param>
            /// <param name="p">Column pointers of A.</param>
            /// <param name="perm">output permutation, size n_col+1</param>
            /// <param name="knobs">Parameter settings for SYMAMD.</param>
            /// <param name="stats">SYMAMD output statistics and error codes.</param>
            /// <returns>Returns 1 if successful, 0 otherwise.</returns>
            [DllImport(COLAMD_DLL)]
            public static extern int symamd_C(
                int n,
                [In, Out] int[] A,
                [In, Out] int[] p,
                [In, Out] int[] perm,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_KNOBS)] double[] knobs,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)] int[] stats
            );
#endif

            /*
            [DllImport(COLAMD_DLL)]
            public static extern int colamd_report(
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)]
                double[] stats
            );
            
            [DllImport(COLAMD_DLL)]
            public static extern int symamd_report(
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)]
                double[] stats
            );
            //*/

            #endregion

            #region CCOLAMD

            [DllImport(CCOLAMD_DLL)]
            public static extern void ccolamd_version(int[] version);

            /// <summary>
            /// Returns recommended value of alen.
            /// </summary>
            /// <param name="nnz">Non-zeros in A.</param>
            /// <param name="n_row">Number of rows in A.</param>
            /// <param name="n_col">Number of columns in A.</param>
            /// <returns>Returns recommended value of alen, or 0 if input arguments are erroneous.</returns>
            [DllImport(CCOLAMD_DLL)]
            public static extern int ccolamd_recommended(int nnz, int n_row, int n_col);

            /// <summary>
            /// Sets default parameters for COLAMD.
            /// </summary>
            /// <param name="knobs">The knobs array.</param>
            /// <returns></returns>
            [DllImport(CCOLAMD_DLL)]
            public static extern int ccolamd_set_defaults(
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_KNOBS)] double[] knobs
            );

            /// <summary>
            /// Computes a column ordering (Q) of A such that P(AQ)=LU or (AQ)'AQ=LL' have less
            /// fill-in and require fewer floating point operations than factorizing the unpermuted
            /// matrix A or A'A, respectively.
            /// </summary>
            /// <param name="n_row">Number of rows in A.</param>
            /// <param name="n_col">Number of columns in A.</param>
            /// <param name="Alen">Size of the array A.</param>
            /// <param name="A">Row indices of A, of size Alen.</param>
            /// <param name="p">Column pointers of A, of size n_col+1.</param>
            /// <param name="knobs">Parameter settings for COLAMD.</param>
            /// <param name="stats">COLAMD output statistics and error codes.</param>
            /// <param name="cmember">Constraint set of A, of size n_col</param>
            /// <returns>Returns 1 if successful, 0 otherwise.</returns>
            /// <remarks>
            /// A and p arguments are modified on output.
            /// </remarks>
            [DllImport(CCOLAMD_DLL)]
            public static extern int ccolamd(
                int n_row,
                int n_col,
                int Alen,
                [In, Out] int[] A,
                [In, Out] int[] p,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_KNOBS)] double[] knobs,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)] int[] stats,
                [In, Out] int[] cmember
            );

#if SUITESPARSE_AIO
            /// <summary>
            /// Computes an ordering P of a symmetric sparse matrix A such that the Cholesky factorization
            /// PAP' = LL' remains sparse. It is based on a column ordering of a matrix M constructed so that
            /// the nonzero pattern of M'M is the same as A. The matrix A is assumed to be symmetric; only the
            /// strictly lower triangular part is accessed.
            /// </summary>
            /// <param name="n">Number of rows columns in A.</param>
            /// <param name="A">Row indices of A.</param>
            /// <param name="p">Column pointers of A.</param>
            /// <param name="perm">output permutation, size n_col+1</param>
            /// <param name="knobs">Parameter settings for SYMAMD.</param>
            /// <param name="stats">SYMAMD output statistics and error codes.</param>
            /// <param name="cmember">Constraint set of A, of size n_col</param>
            /// <param name="stype">0: use both parts, &gt;0: upper, &lt;0: lower</param>
            /// <returns>Returns 1 if successful, 0 otherwise.</returns>
            [DllImport(CCOLAMD_DLL)]
            public static extern int csymamd_C(
                int n,
                [In, Out] int[] A,
                [In, Out] int[] p,
                [In, Out] int[] perm,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_KNOBS)] double[] knobs,
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)] int[] stats,
                [In, Out] int[] cmember,
                int stype
            );
#endif

            /*
            [DllImport(CCOLAMD_DLL)]
            public static extern int ccolamd_report(
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)]
                double[] stats
            );
            
            [DllImport(CCOLAMD_DLL)]
            public static extern int csymamd_report(
                [MarshalAs(UnmanagedType.LPArray, SizeConst = COLAMD_STATS)]
                double[] stats
            );
            //*/

            #endregion
        }
    }
}
