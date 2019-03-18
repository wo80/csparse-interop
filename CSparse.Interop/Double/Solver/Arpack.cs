
namespace CSparse.Double.Solver
{
    using CSparse.Interop.Common;
    using CSparse.Interop.ARPACK;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System;

    public sealed class Arpack : ArpackContext<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Arpack"/> class for the standard eigenvalue problem.
        /// </summary>
        /// <param name="A">Real matrix.</param>
        public Arpack(SparseMatrix A)
            : this(A, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arpack"/> class for the standard eigenvalue problem.
        /// </summary>
        /// <param name="A">Real matrix.</param>
        /// <param name="symmetric">Set to true, if the matrix A is symmetric.</param>
        public Arpack(SparseMatrix A, bool symmetric)
            : base(A, symmetric)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arpack"/> class for the generalized eigenvalue problem.
        /// </summary>
        /// <param name="A">Real matrix.</param>
        /// <param name="B">Real matrix for generalized problem.</param>
        public Arpack(SparseMatrix A, SparseMatrix B)
            : this(A, B, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Arpack"/> class for the generalized eigenvalue problem.
        /// </summary>
        /// <param name="A">Real matrix.</param>
        /// <param name="B">Real matrix for generalized problem.</param>
        /// <param name="symmetric">Set to true, if the matrix A is symmetric and B is symmetric positive definite.</param>
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
            if (!CheckSquare(A))
            {
                throw new InvalidOperationException("Cannot solve eigenvalue problem with non-square matrix.");
            }

            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for given eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors, symmetric);

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
            result.ArnoldiCount = e.ncv;
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
            if (!CheckSquare(A))
            {
                throw new InvalidOperationException("Cannot solve eigenvalue problem with non-square matrix.");
            }

            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for given eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors, symmetric);

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
            result.ArnoldiCount = e.ncv;
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
            if (!CheckSquare(A))
            {
                throw new InvalidOperationException("Cannot solve eigenvalue problem with non-square matrix.");
            }

            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for given eigenvalue problem.", "job");
            }
            
            var result = new ArpackResult(k, size, ComputeEigenVectors, symmetric);

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
            result.ArnoldiCount = e.ncv;
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
            if (!symmetric && !(mode == ShiftMode.None || mode == ShiftMode.Regular))
            {
                throw new InvalidOperationException("This mode is only available for symmetric eigenvalue problems.");
            }

            if (!CheckSquare(A))
            {
                throw new InvalidOperationException("Cannot solve eigenvalue problem with non-square matrix.");
            }

            if (!Job.Validate(symmetric, job))
            {
                throw new ArgumentException("Invalid job for symmetric eigenvalue problem.", "job");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors, symmetric);

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
            result.ArnoldiCount = e.ncv;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        /// <summary>
        /// Special case solving the standard real generalized eigenvalue problem with complex shift.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="sigma_r">The real part of the complex shift.</param>
        /// <param name="sigma_i">The imaginary part of the complex shift.</param>
        /// <param name="part">Part to apply ('R' for real, 'I' for imaginary).</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged eigenvalues.</returns>
        public ArpackResult<double> SolveGeneralized(int k, double sigma_r, double sigma_i, char part, string job = Job.LargestMagnitude)
        {
            if (symmetric)
            {
                throw new InvalidOperationException("Complex shift doesn't apply to real symmetric eigenvalue problems.");
            }

            if (!CheckSquare(A))
            {
                throw new InvalidOperationException("Cannot solve eigenvalue problem with non-square matrix.");
            }

            if (!Job.ValidateGeneral(job))
            {
                throw new ArgumentException("Invalid job for non-symmetric eigenvalue problem.", "job");
            }

            part = char.ToUpperInvariant(part);

            if (part != 'R' && part != 'I')
            {
                throw new ArgumentException("Invalid part specified for complex shift.", "part");
            }

            var result = new ArpackResult(k, size, ComputeEigenVectors, symmetric);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var b = GetMatrix(B, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            conv = NativeMethods.ar_di_ng_shift_cx(ToStringBuilder(job),
                k, ArnoldiCount, Iterations, Tolerance,
                part, sigma_r, sigma_i, ref a, ref b, ref e);


            result.IterationsTaken = e.iterations;
            result.ArnoldiCount = e.ncv;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        /// <summary>
        /// Compute singular values and the partial singular value decomposition.
        /// </summary>
        /// <param name="k">The number of singular values to compute.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged singular values.</returns>
        public ArpackResult<double> SingularValues(int k, string job = Job.LargestMagnitude)
        {
            return SingularValues(k, false, job);
        }

        /// <summary>
        /// Compute singular values and the partial singular value decomposition.
        /// </summary>
        /// <param name="k">The number of singular values to compute.</param>
        /// <param name="normal">Use normal equation to compute (squared) singular values.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged singular values.</returns>
        /// <remarks>
        /// 
        /// </remarks>
        public ArpackResult<double> SingularValues(int k, bool normal, string job = Job.LargestMagnitude)
        {
            if (!Job.ValidateSymmetric(job))
            {
                throw new ArgumentException("Invalid job for singular value decomposition.", "job");
            }

            int m = A.RowCount;
            int n = A.ColumnCount;

            if (normal && m < n)
            {
                throw new InvalidOperationException("Number of columns must not be smaller than number of rows (use transposed matrix).");
            }

            int size = normal ? n : m + n;
            
            var result = new ArpackResult(k, size, ComputeEigenVectors, true);

            var handles = new List<GCHandle>();

            var a = GetMatrix(A, handles);
            var e = result.GetEigenvalueStorage(handles);

            int conv = 0;

            if (normal)
            {
                conv = NativeMethods.ar_di_svd_nrm(ToStringBuilder(job),
                    k, ArnoldiCount, Iterations, Tolerance, ref a, ref e);
            }
            else
            {
                conv = NativeMethods.ar_di_svd(ToStringBuilder(job),
                    k, ArnoldiCount, Iterations, Tolerance, ref a, ref e);
            }

            result.IterationsTaken = e.iterations;
            result.ArnoldiCount = e.ncv;
            result.ConvergedEigenvalues = conv;
            result.ErrorCode = e.info;

            InteropHelper.Free(handles);

            return result;
        }

        #endregion
    }
}
