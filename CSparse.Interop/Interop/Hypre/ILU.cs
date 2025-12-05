using System;

namespace CSparse.Interop.Hypre
{
    public class ILU<T> : IHyprePreconditioner<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected HYPRE_Solver precond;

        public ILU()
        {
            NativeMethods.HYPRE_ILUCreate(out precond);

            double threshold = 0.1;

            /* Set some parameters (See Reference Manual for more parameters) */
            NativeMethods.HYPRE_ILUSetLevelOfFill(precond, 2);
            NativeMethods.HYPRE_ILUSetMaxNnzPerRow(precond, 3);
            NativeMethods.HYPRE_ILUSetDropThreshold(precond, threshold);
            //NativeMethods.HYPRE_ILUSetType(precond, );
            //NativeMethods.HYPRE_ILUSetLocalReordering(precond, );
            //NativeMethods.HYPRE_ILUSetLogging(precond, 3);
        }

        public void Bind(HypreSolver<T> solver)
        {
            solver.SetPreconditioner(NativeMethods.HYPRE_ILUSolve, NativeMethods.HYPRE_ILUSetup, precond);
        }

        public void Dispose()
        {
            if (precond.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ILUDestroy(precond);
                precond.Ptr = IntPtr.Zero;
            }
        }
    }
}
