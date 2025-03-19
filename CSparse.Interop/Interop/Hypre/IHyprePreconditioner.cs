
namespace CSparse.Interop.Hypre
{
    using System;

    public interface IHyprePreconditioner<T> : IDisposable
        where T : struct, IEquatable<T>, IFormattable
    {
        void Bind(HypreSolver<T> solver);
    }
}
