using System;

namespace CSparse.Interop.Hypre
{
    public class FlexGMRES<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public FlexGMRES()
        {
            NativeMethods.HYPRE_ParCSRFlexGMRESCreate(Constants.MPI_COMM_WORLD, out solver);
        }
        public FlexGMRES(IHyprePreconditioner<T> precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRFlexGMRESSetPrecond(solver, precond, precond_setup, precond_solver);
        }

        //HYPRE_FlexGMRESSetAbsoluteTol
        //-HYPRE_FlexGMRESSetConvergenceFactorTol
        //-HYPRE_FlexGMRESSetMinIter

        public void SetTol(double tol)
        {
            NativeMethods.HYPRE_FlexGMRESSetTol(solver, tol);
        }

        public void SetKDim(int restart)
        {
            NativeMethods.HYPRE_FlexGMRESSetKDim(solver, restart);
        }

        public void SetMaxIter(int max_iter)
        {
            NativeMethods.HYPRE_FlexGMRESSetMaxIter(solver, max_iter);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_FlexGMRESSetLogging(solver, Logging);
            NativeMethods.HYPRE_FlexGMRESSetPrintLevel(solver, PrintLevel);

            int restart = 30;
            bool modify = true;

            if (modify)
            {
                /* this is an optional call  - if you don't call it, hypre_FlexGMRESModifyPCDefault
                   is used - which does nothing.  Otherwise, you can define your own, similar to
                   the one used here */
                //HYPRE_FlexGMRESSetModifyPC(
                //   solver, (HYPRE_PtrToModifyPCFcn)hypre_FlexGMRESModifyPCAMGExample);
            }

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRFlexGMRESSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRFlexGMRESSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_FlexGMRESGetNumIterations(solver, out result.NumIterations);
            NativeMethods.HYPRE_FlexGMRESGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_FlexGMRESGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRFlexGMRESDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
