
namespace CSparse.Interop.Hypre
{
    using System;

    public struct HYPRE_ParCSRMatrix
    {
        public IntPtr Ptr;
    }

    public struct HYPRE_ParVector
    {
        public IntPtr Ptr;
    }

    public struct HYPRE_IJMatrix
    {
        public IntPtr Ptr;
    }

    public struct HYPRE_IJVector
    {
        public IntPtr Ptr;
    }

    public struct HYPRE_Solver
    {
        public IntPtr Ptr;
    }
}
