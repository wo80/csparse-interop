
namespace CSparse.Double
{
    static class Helper
    {
        public static double ComputeError(int n, double[] actual, double[] expected, bool relativeError = true)
        {
            var e = Vector.Clone(actual);

            Vector.Axpy(-1.0, expected, e);

            if (relativeError)
            {
                return Vector.Norm(n, e) / Vector.Norm(n, expected);
            }

            return Vector.Norm(n, e);
        }
    }
}
