
namespace CSparse.Interop.MKL.Pardiso
{
    using System;
    using System.Runtime.InteropServices;

    // See https://www.intel.com/content/www/us/en/docs/onemkl/developer-reference-c/2024-2/onemkl-pardiso-parallel-direct-sparse-solver-iface.html

    internal static class NativeMethods
    {
        const string DLL = Helper.LibraryName;

        [DllImport(DLL, EntryPoint = "pardisoinit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void pardisoinit(IntPtr[] pt, /*const*/ ref int mtype, int[] iparm);

        [DllImport(DLL, EntryPoint = "pardiso", CallingConvention = CallingConvention.Cdecl)]
        public static extern void pardiso(IntPtr[] pt, /*const*/ ref int maxfct, /*const*/ ref int mnum, /*const*/ ref int mtype,
            /*const*/ ref int phase, /*const*/ ref int n, /*const*/ IntPtr a, /*const*/ IntPtr ia, /*const*/ IntPtr ja, int[] perm,
            /*const*/ ref int nrhs, int[] iparm, /*const*/ ref int msglvl, IntPtr b, IntPtr x, out int error);
    }
}
