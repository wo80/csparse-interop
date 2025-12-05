
namespace CSparse.Interop.Hypre
{
    using System;

    public abstract class HypreSolver<T> : IDisposable
        where T : struct, IEquatable<T>, IFormattable
    {
        protected HYPRE_Solver solver;

        public HypreSolver()
        {
            if (typeof(T) != typeof(double))
            {
                throw new NotSupportedException("This version of HYPRE only supports type 'double'");
            }
        }

        ~HypreSolver()
        {
            Dispose(false);
        }

        public int Logging { get; set; } = 0;

        public int PrintLevel { get; set; } = 0;

        public abstract HypreResult Solve(HypreMatrix<T> A, HypreVector<T> x, HypreVector<T> b);

        internal virtual void SetPreconditioner(
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver)
        {
            throw new NotImplementedException();
        }

        #region IDisposable

        // See https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        #endregion
    }
}
