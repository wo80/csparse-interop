
namespace CSparse.Double.Solver
{
    using CSparse.Interop.Common;
    using CSparse.Interop.ARPACK;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System;
    using System.Numerics;

    public sealed class Arpack : ArpackContext<double>
    {
        public Arpack(SparseMatrix A)
            : this(A, false)
        {
        }

        public Arpack(SparseMatrix A, bool symmetric)
            : base(A, symmetric)
        {
        }
        public Arpack(SparseMatrix A, SparseMatrix B)
            : this(A, B, false)
        {
        }

        public Arpack(SparseMatrix A, SparseMatrix B, bool symmetric)
            : base(A, B, symmetric)
        {
        }

        #region Overriden methods

        /// <summary>
        /// Solve the standard eigenvalue problem.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged eigenvalues.</returns>
        public override ArpackResult<double> SolveStandard(int k, string job)
        {
            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for given eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            if (symmetric)
            {
                conv = NativeMethods.ar_di_ss(ToStringBuilder(job), k, ArnoldiCount,
                    Iterations, Tolerance, ref a, ref e);
            }
            else
            {
                conv = NativeMethods.ar_di_ns(ToStringBuilder(job), k, ArnoldiCount,
                    Iterations, Tolerance, ref a, ref e);
            }

            result.IterationsTaken = e.iterations;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        /// <summary>
        /// Solve the standard eigenvalue problem in shift-invert mode.
        /// </summary>
        public override ArpackResult<double> SolveStandard(int k, double sigma, string job = Job.LargestMagnitude)
        {
            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for given eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            if (symmetric)
            {
                conv = NativeMethods.ar_di_ss_shift(ToStringBuilder(job), k, ArnoldiCount, Iterations,
                    Tolerance, sigma, ref a, ref e);
            }
            else
            {
                conv = NativeMethods.ar_di_ns_shift(ToStringBuilder(job), k, ArnoldiCount, Iterations,
                    Tolerance, sigma, ref a, ref e);
            }

            result.IterationsTaken = e.iterations;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        /// <summary>
        /// Solve the generalized eigenvalue problem.
        /// </summary>
        public override ArpackResult<double> SolveGeneralized(int k, string job)
        {
            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for given eigenvalue problem.", "job");
            }
            
            var result = new ArpackResult(k, size, ComputeEigenVectors);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var b = GetMatrix(B, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            if (symmetric)
            {
                conv = NativeMethods.ar_di_sg(ToStringBuilder(job), k, ArnoldiCount,
                    Iterations, Tolerance, ref a, ref b, ref e);
            }
            else
            {
                conv = NativeMethods.ar_di_ng(ToStringBuilder(job), k, ArnoldiCount,
                    Iterations, Tolerance, ref a, ref b, ref e);
            }

            result.IterationsTaken = e.iterations;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        /// <summary>
        /// Solve the generalized eigenvalue problem in user-defined shift-invert mode.
        /// </summary>
        public override ArpackResult<double> SolveGeneralized(int k, double sigma, string job = Job.LargestMagnitude)
        {
            return SolveGeneralized(k, sigma, ShiftMode.Regular, job);
        }

        /// <summary>
        /// Solve the generalized eigenvalue problem in user-defined shift-invert mode.
        /// </summary>
        public ArpackResult<double> SolveGeneralized(int k, double sigma, ShiftMode mode, string job = Job.LargestMagnitude)
        {
            if (!symmetric)
            {
                throw new InvalidOperationException("This mode is only available for symmetric eigenvalue problems.");
            }

            if (!Job.ValidateSymmetric(job))
            {
                throw new ArgumentException("Invalid job for symmetric eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var b = GetMatrix(B, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            if (symmetric)
            {
                char m = 'S';

                if (mode == ShiftMode.Buckling)
                {
                    m = 'B';
                }
                else if (mode == ShiftMode.Cayley)
                {
                    m = 'C';
                }

                conv = NativeMethods.ar_di_sg_shift(ToStringBuilder(job), m, k, ArnoldiCount, Iterations,
                    Tolerance, sigma, ref a, ref b, ref e);
            }
            else
            {
                conv = NativeMethods.ar_di_ng_shift(ToStringBuilder(job), k, ArnoldiCount, Iterations,
                    Tolerance, sigma, ref a, ref b, ref e);
            }

            result.IterationsTaken = e.iterations;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        // TODO: make complex shift mode public

        /// <summary>
        /// Special case solving the standard real generalized eigenvalue problem with complex shift.
        /// </summary>
        private ArpackResult<double> SolveGeneralized(int k, Complex sigma, char part, string job = Job.LargestMagnitude)
        {
            if (symmetric)
            {
                throw new InvalidOperationException("Complex shift doesn't apply to real symmetric eigenvalue problems.");
            }

            if (!Job.ValidateGeneral(job))
            {
                throw new ArgumentException("Invalid job for non-symmetric eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var b = GetMatrix(B, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            conv = NativeMethods.ar_di_ng_shift_cx(ToStringBuilder(job),
                k, ArnoldiCount, Iterations, Tolerance,
                part, sigma.Real, sigma.Imaginary, ref a, ref b, ref e);


            result.IterationsTaken = e.iterations;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        #endregion
    }
}
