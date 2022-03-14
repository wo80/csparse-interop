using System;

namespace CSparse.Interop.Hypre
{
    public class BiCGSTAB<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public BiCGSTAB()
        {
            NativeMethods.HYPRE_ParCSRBiCGSTABCreate(Constants.MPI_COMM_WORLD, out solver);
        }

        public BiCGSTAB(IHyprePreconditioner<T> precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRBiCGSTABSetPrecond(solver, precond, precond_setup, precond_solver);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_BiCGSTABSetPrintLevel(solver, PrintLevel);
            NativeMethods.HYPRE_BiCGSTABSetLogging(solver, Logging);

            NativeMethods.HYPRE_BiCGSTABSetTol(solver, 1e-7);
            //-HYPRE_BiCGSTABSetAbsoluteTol
            //-HYPRE_BiCGSTABSetConvergenceFactorTol
            //-HYPRE_BiCGSTABSetStopCrit
            //-HYPRE_BiCGSTABSetMinIter
            NativeMethods.HYPRE_BiCGSTABSetMaxIter(solver, 1000);

            var parcsr_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRBiCGSTABSetup(solver, parcsr_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRBiCGSTABSolve(solver, parcsr_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_BiCGSTABGetNumIterations(solver, out result.NumIterations);
            //NativeMethods.HYPRE_BiCGSTABGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_BiCGSTABGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            result.Converged = 1; // result.NumIterations < max_iter;

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRBiCGSTABDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
