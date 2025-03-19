
namespace CSparse.Interop.Hypre
{
    using System;

    public class BoomerAMG<T> : HypreSolver<T>, IHyprePreconditioner<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public BoomerAMG()
        {
            NativeMethods.HYPRE_BoomerAMGCreate(out solver);
        }

        public void Bind(HypreSolver<T> solver)
        {
            solver.SetPreconditioner(NativeMethods.HYPRE_BoomerAMGSolve, NativeMethods.HYPRE_BoomerAMGSetup, this.solver);
        }

        public void SetTol(double tol)
        {
            NativeMethods.HYPRE_BoomerAMGSetTol(solver, tol);
        }

        public void SetMaxLevels(int max_levels)
        {
            NativeMethods.HYPRE_BoomerAMGSetMaxLevels(solver, max_levels);
        }

        public void SetNumSweeps(int num_sweeps)
        {
            NativeMethods.HYPRE_BoomerAMGSetNumSweeps(solver, num_sweeps);
        }

        public void SetRelaxOrder(int relax_order)
        {
            NativeMethods.HYPRE_BoomerAMGSetRelaxOrder(solver, relax_order);
        }

        public void SetRelaxType(int relax_type)
        {
            NativeMethods.HYPRE_BoomerAMGSetRelaxType(solver, relax_type);
        }

        public void SetCoarsenType(int coarsen)
        {
            NativeMethods.HYPRE_BoomerAMGSetCoarsenType(solver, coarsen);
        }

        public void SetOldDefault()
        {
            NativeMethods.HYPRE_BoomerAMGSetOldDefault(solver);
        }

        public void SetMaxIter(int max_iter)
        {
            NativeMethods.HYPRE_BoomerAMGSetMaxIter(solver, max_iter);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_BoomerAMGSetPrintLevel(solver, PrintLevel);
            NativeMethods.HYPRE_BoomerAMGSetLogging(solver, Logging);

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_BoomerAMGSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_BoomerAMGSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_BoomerAMGGetNumIterations(solver, out result.NumIterations);
            //NativeMethods.HYPRE_BoomerAMGGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_BoomerAMGGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            result.Converged = 1;

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_BoomerAMGDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
