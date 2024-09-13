
namespace CSparse.Factorization
{
    using System;

    public interface IDisposableSolver<T> : IDisposable, ISolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        /// Gets the non-zeros count of the factors, if available, otherwise <c>-1</c>.
        /// </summary>
        int NonZerosCount { get; }
    }
}
