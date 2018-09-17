
namespace CSparse.Interop.ARPACK
{
    using System;

    public static class Job
    {
        public const string LargestAlgebraic = "LA";
        public const string LargestMagnitude = "LM";
        public const string LargestRealPart = "LR";
        public const string LargestImaginaryPart = "LI";
        public const string SmallestAlgebraic = "SA";
        public const string SmallestMagnitude = "SM";
        public const string SmallestRealPart = "SR";
        public const string SmallestImaginaryPart = "SI";
        public const string BothEnds = "BE";

        internal static bool Validate(bool symmetric, string job)
        {
            return symmetric ? ValidateSymmetric(job) : ValidateGeneral(job);
        }

        internal static bool ValidateSymmetric(string job)
        {
            var oic = StringComparison.OrdinalIgnoreCase;

            return job.Equals(LargestMagnitude, oic)
                || job.Equals(LargestAlgebraic, oic)
                || job.Equals(SmallestMagnitude, oic)
                || job.Equals(SmallestAlgebraic, oic)
                || job.Equals(BothEnds, oic);
        }

        internal static bool ValidateGeneral(string job)
        {
            var oic = StringComparison.OrdinalIgnoreCase;

            return job.Equals(LargestMagnitude, oic)
                || job.Equals(LargestRealPart, oic)
                || job.Equals(LargestImaginaryPart, oic)
                || job.Equals(SmallestMagnitude, oic)
                || job.Equals(LargestRealPart, oic)
                || job.Equals(SmallestImaginaryPart, oic);
        }
    }
}
