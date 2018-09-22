
namespace CSparse.Double.Solver
{
    using CSparse.Interop.MKL;
    using CSparse.Storage;

    public class ExtendedEigensolverResult : ExtendedEigensolverResult<double>
    {
        /// <inheritdoc />
        public ExtendedEigensolverResult(SparseStatus info, int size, int k, double[] e, DenseColumnMajorStorage<double> x, double[] r)
            : base(info, size, k, e, x, r)
        {
        }
    }
}
