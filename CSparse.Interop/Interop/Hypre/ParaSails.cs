using System;

namespace CSparse.Interop.Hypre
{
    public class ParaSails<T> : IHyprePreconditioner<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected HYPRE_Solver precond;

        public ParaSails(bool symmetric)
        {
            NativeMethods.HYPRE_ParaSailsCreate(Constants.MPI_COMM_WORLD, out precond);

            NativeMethods.HYPRE_ParaSailsSetSym(precond, symmetric ? 1 : 0);
        }

        public void SetParams(double threshold, int max_levels)
        {
            NativeMethods.HYPRE_ParaSailsSetParams(precond, threshold, max_levels);
        }

        public void SetFilter(double filter)
        {
            NativeMethods.HYPRE_ParaSailsSetFilter(precond, filter);
        }

        public void Bind(HypreSolver<T> solver)
        {
            solver.SetPreconditioner(NativeMethods.HYPRE_ParaSailsSolve, NativeMethods.HYPRE_ParaSailsSetup, precond);
        }

        public void Dispose()
        {
            if (precond.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParaSailsDestroy(precond);
                precond.Ptr = IntPtr.Zero;
            }
        }
    }
}
