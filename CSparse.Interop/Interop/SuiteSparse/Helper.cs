namespace CSparse.Interop.SuiteSparse
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

#if X64
    using size_t = System.UInt64;
#else
    using size_t = System.UInt32;
#endif

    public class Helper
    {
        public static void SuiteSparseStart()
        {
            NativeMethods.SuiteSparse_start();
        }

        public static void SuiteSparseFinish()
        {
            NativeMethods.SuiteSparse_finish();
        }

        public static void Free(IntPtr p)
        {
            NativeMethods.SuiteSparse_free(p);
        }

        public static string GetInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SuiteSparse version " + GetVersion());
            sb.AppendLine();
            sb.AppendLine("  BLAS provider    : " + GetBlasLibrary());
            sb.AppendLine("  BLAS integer size: " + GetBlasIntegerSize());
            sb.AppendLine();
            sb.AppendLine("  AMD     : " + Ordering.AMD.Version());
            sb.AppendLine("  CHOLMOD : " + Cholmod.CholmodContext<double>.Version());
            sb.AppendLine("  CXSparse: " + CXSparse.CXSparseContext<double>.Version());
            sb.AppendLine("  SPQR    : " + SPQR.SpqrContext<double>.Version());
            sb.AppendLine("  UMFPACK : " + Umfpack.UmfpackContext<double>.Version());

            return sb.ToString();
        }

        /// <summary>
        /// Gets the name of the linked BLAS library (WARNING: leaks memory).
        /// </summary>
        public static string GetBlasLibrary()
        {
            // NOTE: this leaks native memory, since the returned char* pointer isn't free'd.
            var p = NativeMethods.SuiteSparse_BLAS_library();

            return Marshal.PtrToStringAnsi(p);
        }

        /// <summary>
        /// Gets the integer size of the linked BLAS library.
        /// </summary>
        public static size_t GetBlasIntegerSize()
        {
            return NativeMethods.SuiteSparse_BLAS_integer_size();
        }

        /// <summary>
        /// Gets the SuiteSparse version.
        /// </summary>
        public static Version GetVersion()
        {
            int[] version = new int[3];
            _ = NativeMethods.SuiteSparse_version(version);
            return new Version(version[0], version[1], version[2]);
        }

        #region Native methods

        static class NativeMethods
        {
#if SUITESPARSE_AIO
            const string SUITESPARSECONFIG_DLL = "suitesparse";
#else
            const string SUITESPARSECONFIG_DLL = "suitesparseconfig";
#endif

            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern void SuiteSparse_start();


            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern void SuiteSparse_finish();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="nitems">number of items to calloc (>=1 is enforced)</param>
            /// <param name="size_of_item">sizeof each item</param>
            /// <returns>pointer to allocated block of memory</returns>
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern IntPtr SuiteSparse_malloc(size_t nitems, size_t size_of_item);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="nitems">number of items to calloc (>=1 is enforced)</param>
            /// <param name="size_of_item">sizeof each item</param>
            /// <returns>pointer to allocated block of memory</returns>
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern IntPtr SuiteSparse_calloc(size_t nitems, size_t size_of_item);

            /// <summary>
            /// SuiteSparse_realloc
            /// </summary>
            /// <param name="nitems_new">new number of items in the object</param>
            /// <param name="nitems_old">old number of items in the object</param>
            /// <param name="size_of_item">sizeof each item</param>
            /// <param name="p">old object to reallocate</param>
            /// <param name="ok">1 if successful, 0 otherwise</param>
            /// <returns>pointer to reallocated block of memory, or to original block if the realloc failed.</returns>
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern IntPtr SuiteSparse_realloc(size_t nitems_new, size_t nitems_old, size_t size_of_item, IntPtr p, out int ok);

            // always returns NULL
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern IntPtr SuiteSparse_free(IntPtr p);

            // start the timer
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern void SuiteSparse_tic(double[] tic); // size: 2

            // input: from last call to SuiteSparse_tic
            // return time in seconds since last tic
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern double SuiteSparse_toc(double[] tic); // size: 2

            // returns current wall clock time in seconds
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern double SuiteSparse_time();

            // returns sqrt (x^2 + y^2), computed reliably
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern double SuiteSparse_hypot(double x, double y);

            // complex division of c = a/b
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern int SuiteSparse_divcomplex(double ar, double ai, double br, double bi, out double cr, out double ci);

            // output, not defined on input.  Not used if NULL.  Returns
            // the three version codes in version [0..2]:
            // version [0] is SUITESPARSE_MAIN_VERSION
            // version [1] is SUITESPARSE_SUB_VERSION
            // version [2] is SUITESPARSE_SUBSUB_VERSION
            // returns SUITESPARSE_VERSION
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern int SuiteSparse_version(int[] version);

            // Returns the name of the BLAS library found by SuiteSparse_config
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern IntPtr SuiteSparse_BLAS_library(); /* const char* */

            // SuiteSparse_BLAS_integer_size: return sizeof (SUITESPARSE_BLAS_INT)
            [DllImport(SUITESPARSECONFIG_DLL)]
            public static extern size_t SuiteSparse_BLAS_integer_size();
        }

        #endregion
    }
}
