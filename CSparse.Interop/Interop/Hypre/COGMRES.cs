using System;

namespace CSparse.Interop.Hypre
{
    public class COGMRES<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public COGMRES()
        {
            NativeMethods.HYPRE_ParCSRCOGMRESCreate(Constants.MPI_COMM_WORLD, out solver);
        }
        public COGMRES(IHyprePreconditioner<T> precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRCOGMRESSetPrecond(solver, precond, precond_setup, precond_solver);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_COGMRESSetPrintLevel(solver, PrintLevel);
            NativeMethods.HYPRE_COGMRESSetLogging(solver, Logging);

            int restart = 30;
            bool modify = true;

            NativeMethods.HYPRE_COGMRESSetTol(solver, 1e-7);
            //HYPRE_COGMRESSetAbsoluteTol
            //-HYPRE_COGMRESSetConvergenceFactorTol
            //-HYPRE_COGMRESSetMinIter
            NativeMethods.HYPRE_COGMRESSetMaxIter(solver, 1000);
            NativeMethods.HYPRE_COGMRESSetKDim(solver, restart);
            //HYPRE_COGMRESSetUnroll
            //HYPRE_COGMRESSetCGS

            if (modify)
            {
                /* this is an optional call  - if you don't call it, hypre_COGMRESModifyPCDefault
                   is used - which does nothing.  Otherwise, you can define your own, similar to
                   the one used here */
                //HYPRE_COGMRESSetModifyPC(
                //   solver, (HYPRE_PtrToModifyPCFcn)hypre_COGMRESModifyPCAMGExample);
            }

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRCOGMRESSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRCOGMRESSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_COGMRESGetNumIterations(solver, out result.NumIterations);
            NativeMethods.HYPRE_COGMRESGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_COGMRESGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRCOGMRESDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
