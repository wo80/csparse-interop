using System;

namespace CSparse.Interop.Hypre
{
    public class GMRES<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public GMRES()
        {
            NativeMethods.HYPRE_ParCSRGMRESCreate(Constants.MPI_COMM_WORLD, out solver);
        }
        public GMRES(IHyprePreconditioner<T> precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRGMRESSetPrecond(solver, precond, precond_setup, precond_solver);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_GMRESSetPrintLevel(solver, PrintLevel);
            NativeMethods.HYPRE_GMRESSetLogging(solver, Logging);

            int restart = 30;
            bool modify = true;

            NativeMethods.HYPRE_GMRESSetTol(solver, 1e-7);
            //HYPRE_GMRESSetAbsoluteTol
            //-HYPRE_GMRESSetConvergenceFactorTol
            //-HYPRE_GMRESSetStopCrit
            //-HYPRE_GMRESSetMinIter
            NativeMethods.HYPRE_GMRESSetMaxIter(solver, 1000);
            NativeMethods.HYPRE_GMRESSetKDim(solver, restart);
            //HYPRE_GMRESSetRelChange
            //HYPRE_GMRESSetSkipRealResidualCheck

            if (modify)
            {
                /* this is an optional call  - if you don't call it, hypre_GMRESModifyPCDefault
                   is used - which does nothing.  Otherwise, you can define your own, similar to
                   the one used here */
                //HYPRE_GMRESSetModifyPC(
                //   solver, (HYPRE_PtrToModifyPCFcn)hypre_GMRESModifyPCAMGExample);
            }

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRGMRESSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRGMRESSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_GMRESGetNumIterations(solver, out result.NumIterations);
            NativeMethods.HYPRE_GMRESGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_GMRESGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRGMRESDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
