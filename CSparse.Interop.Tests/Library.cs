namespace CSparse.Interop
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class Library
    {
        public static void SetImportResolver()
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
        }

        private static IntPtr DllImportResolver(string library, Assembly assembly, DllImportSearchPath? searchPath)
        {
            // Trying to find MKL on Debian.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && library.Equals("mkl_rt.2"))
            {
                return NativeLibrary.Load("libmkl_rt.so", assembly, searchPath);
            }

            // Otherwise, fall back to default import resolver.
            return IntPtr.Zero;
        }
    }
}