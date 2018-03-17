
namespace CSparse.Complex
{
    using CSparse.Storage;
    using System.Numerics;

    static class Util
    {
        public static double ComputeError(Complex[] actual, Complex[] expected, bool relativeError = true)
        {
            var e = Vector.Clone(actual);

            Vector.Axpy(-1.0, expected, e);

            if (relativeError)
            {
                return Vector.Norm(e) / Vector.Norm(expected);
            }

            return Vector.Norm(e);
        }

        public static double ComputeResidual(CompressedColumnStorage<Complex> A, Complex[] x, Complex[] b, bool relativeError = true)
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
