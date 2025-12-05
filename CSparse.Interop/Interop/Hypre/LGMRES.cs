using System;

namespace CSparse.Interop.Hypre
{
    public class LGMRES<T> : HypreSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public LGMRES()
        {
            NativeMethods.HYPRE_ParCSRLGMRESCreate(Constants.MPI_COMM_WORLD, out solver);
        }
        public LGMRES(IHyprePreconditioner<T> precond)
            : this()
        {
            precond.Bind(this);
        }

        internal override void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            NativeMethods.HYPRE_ParCSRLGMRESSetPrecond(solver, precond, precond_setup, precond_solver);
        }

        public override HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b)
        {
            NativeMethods.HYPRE_LGMRESSetPrintLevel(solver, PrintLevel);
            NativeMethods.HYPRE_LGMRESSetLogging(solver, Logging);

            int restart = 30;
            bool modify = true;

            NativeMethods.HYPRE_LGMRESSetTol(solver, 1e-7);
            //HYPRE_LGMRESSetAbsoluteTol
            //-HYPRE_LGMRESSetConvergenceFactorTol
            //-HYPRE_LGMRESSetMinIter
            NativeMethods.HYPRE_LGMRESSetMaxIter(solver, 1000);
            NativeMethods.HYPRE_LGMRESSetKDim(solver, restart);
            //HYPRE_LGMRESSetAugDim

            if (modify)
            {
                /* this is an optional call  - if you don't call it, hypre_LGMRESModifyPCDefault
                   is used - which does nothing.  Otherwise, you can define your own, similar to
                   the one used here */
                //HYPRE_LGMRESSetModifyPC(
                //   solver, (HYPRE_PtrToModifyPCFcn)hypre_LGMRESModifyPCAMGExample);
            }

            var par_A = A.GetObject();
            var par_b = b.GetObject();
            var par_x = x.GetObject();

            NativeMethods.HYPRE_ParCSRLGMRESSetup(solver, par_A, par_b, par_x);
            NativeMethods.HYPRE_ParCSRLGMRESSolve(solver, par_A, par_b, par_x);

            x.Synchronize();

            HypreResult result;

            NativeMethods.HYPRE_LGMRESGetNumIterations(solver, out result.NumIterations);
            NativeMethods.HYPRE_LGMRESGetConverged(solver, out result.Converged);
            NativeMethods.HYPRE_LGMRESGetFinalRelativeResidualNorm(solver, out result.RelResidualNorm);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (solver.Ptr != IntPtr.Zero)
            {
                NativeMethods.HYPRE_ParCSRLGMRESDestroy(solver);
                solver.Ptr = IntPtr.Zero;
            }
        }
    }
}
