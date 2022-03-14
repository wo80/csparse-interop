using System;

namespace CSparse.Interop.Hypre
{
    public class CGNR<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public CGNR()
        {
            NativeMethods.HYPRE_ParCSRCGNRCreate(Constants.MPI_COMM_WORLD, out solver);
        }

        /*
        public CGNR(IHyprePreconditioner precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precondT,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRCGNRSetPrecond(solver, precond, precondT, precond_setup, precond_solver);
        }
        //*/

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_CGNRSetLogging(solver, Logging);

            NativeMethods.HYPRE_CGNRSetTol(solver, 1e-7);
            //-HYPRE_CGNRSetStopCrit
            //-HYPRE_CGNRSetMinIter
            NativeMethods.HYPRE_CGNRSetMaxIter(solver, 1000);

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRCGNRSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRCGNRSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_CGNRGetNumIterations(solver, out result.NumIterations);
            //NativeMethods.HYPRE_CGNRGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_CGNRGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            result.Converged = 1;

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRCGNRDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
