using System;

namespace CSparse.Interop.Hypre
{
    public class PCG<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public PCG()
        {
            NativeMethods.HYPRE_ParCSRPCGCreate(Constants.MPI_COMM_WORLD, out solver);
        }

        public PCG(IHyprePreconditioner<T> precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRPCGSetPrecond(solver, precond, precond_setup, precond_solver);
        }

        //HYPRE_PCGSetAbsoluteTol
        //HYPRE_PCGSetResidualTol
        //-HYPRE_PCGSetAbsoluteTolFactor
        //-HYPRE_PCGSetConvergenceFactorTol
        //-HYPRE_PCGSetStopCrit
        //HYPRE_PCGSetRelChange
        //HYPRE_PCGSetRecomputeResidual
        //HYPRE_PCGSetRecomputeResidualP

        public void SetTol(double tol)
        {
            NativeMethods.HYPRE_PCGSetTol(solver, tol);
        }

        public void SetTwoNorm(int two_norm)
        {
            NativeMethods.HYPRE_PCGSetTwoNorm(solver, two_norm);
        }

        public void SetMaxIter(int max_iter)
        {
            NativeMethods.HYPRE_PCGSetMaxIter(solver, max_iter);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_PCGSetLogging(solver, Logging);
            NativeMethods.HYPRE_PCGSetPrintLevel(solver, PrintLevel);

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRPCGSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRPCGSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_PCGGetNumIterations(solver, out result.NumIterations);
            NativeMethods.HYPRE_PCGGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_PCGGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRPCGDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
