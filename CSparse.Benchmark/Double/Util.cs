
using CSparse.Storage;

namespace CSparse.Double
{
    static class Util
    {
        public static double ComputeError(double[] actual, double[] expected, bool relativeError = true)
        {
            var e = Vector.Clone(actual);

            Vector.Axpy(-1.0, expected, e);

            if (relativeError)
            {
                return Vector.Norm(e) / Vector.Norm(expected);
            }

            return Vector.Norm(e);
        }

        public static double ComputeResidual(CompressedColumnStorage<double> A, double[] x, double[] b, bool relativeError = true)
        {
            var e = Vector.Clone(b);

            A.Multiply(-1.0, x, 1.0, e);

            if (relativeError)
            {
                return Vector.Norm(e) / (A.FrobeniusNorm() * Vector.Norm(b));
            }

            return Vector.Norm(e);
        }
    }
}
