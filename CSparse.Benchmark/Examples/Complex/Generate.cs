namespace CSparse.Examples.Complex
{
    using CSparse.Complex;
    using System.Numerics;

    /// <summary>
    /// Examples taken from ARPACK++.
    /// </summary>
    static class Generate
    {
        /// <summary>
        /// Function template for the nx*nx by nx*nx block tridiagonal matrix
        ///
        ///             | T -I          |
        ///             |-I  T -I       |
        ///        OP = |   -I  T       |
        ///             |        ...  -I|
        ///             |           -I T|
        ///
        /// derived from the standard central difference discretization of
        /// the 2 dimensional convection-diffusion operator
        ///                       (Laplacian u) + rho*(du/dx) 
        /// on a unit square with zero boundary conditions. T is a nx by nx
        /// tridiagonal matrix with DD on the diagonal, DL on the subdiagonal,
        /// and DU on the superdiagonal.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\complex\LCMatrxA.h
        /// </remarks>
        public static SparseMatrix CompMatrixA(int nx)
        {
            Complex h, h2, dd, dl, du, f;

            Complex rho = new Complex(1.0e2, 0.0);

            h = 1.0 / (nx + 1);
            h2 = h * h;
            f = -(1.0 / h2);
            dd = 4.0 / h2;
            dl = f - 0.5 * rho / h;
            du = f + 0.5 * rho / h;

            // Defining the number of nonzero matrix elements.

            int nnz = (5 * nx - 4) * nx;

            // Creating output vectors.

            var matrix = new SparseMatrix(nx * nx, nx * nx, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;

            int j = 0;
            int id = 0;

            for (int k = 0; k != nx; k++)
            {
                for (int i = 0; i != nx; i++)
                {

                    if (k != 0)
                    {
                        ai[j] = id - nx;
                        ax[j++] = f;
                    }

                    if (i != 0)
                    {
                        ai[j] = id - 1;
                        ax[j++] = du;
                    }

                    ai[j] = id;
                    ax[j++] = dd;

                    if (i != (nx - 1))
                    {
                        ai[j] = id + 1;
                        ax[j++] = dl;
                    }

                    if (k != (nx - 1))
                    {
                        ai[j] = id + nx;
                        ax[j++] = f;
                    }

                    ap[++id] = j;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Function template for the tridiagonal matrix derived from the standard central
        /// difference of the 1-d convection diffusion operator u" + rho*u' on the interval
        /// [0, 1] with zero Dirichlet boundary conditions.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\complex\LCMatrxB.h
        /// </remarks>
        public static SparseMatrix CompMatrixB(int n, Complex rho)
        {
            Complex dd, dl, du, s, h, h2;
            
            h = 1.0 / (n + 1);
            h2 = h * h;
            s = rho / 2.0;
            dd = 2.0 / h2;
            dl = -(1.0 / h2) - s / h;
            du = -(1.0 / h2) + s / h;

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;

            int j = 0;

            for (int i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = du;
                }

                ai[j] = i;
                ax[j++] = dd;

                if (i != (n - 1))
                {
                    ai[j] = i + 1;
                    ax[j++] = dl;
                }

                ap[i + 1] = j;
            }

            return matrix;
        }

        /// <summary>
        /// Function template for the stiffness matrix formed by using piecewise linear elements on [0,1].
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\complex\LCMatrxE.h
        /// </remarks>
        public static SparseMatrix CompMatrixE(int n, Complex rho)
        {
            Complex dd, dl, du, s, h;

            h = 1.0 / (n + 1);
            s = rho / 2.0;
            dd = 2.0 / h;
            dl = -(1.0 / h) - s;
            du = -(1.0 / h) + s;

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;

            int j = 0;

            for (int i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = du;
                }

                ai[j] = i;
                ax[j++] = dd;

                if (i != (n - 1))
                {
                    ai[j] = i + 1;
                    ax[j++] = dl;
                }

                ap[i + 1] = j;
            }

            return matrix;
        }

        /// <summary>
        /// Function template for a tridiagonal complex matrix ().
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\complex\LCMatrxF.h
        /// </remarks>
        public static SparseMatrix CompMatrixF(int n)
        {
            Complex h, dd, ds;

            h = 1.0 / (n + 1);
            dd = 4.0 * h;
            ds = 1.0 * h;

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;

            int j = 0;

            for (int i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = ds;
                }

                ai[j] = i;
                ax[j++] = dd;

                if (i != (n - 1))
                {
                    ai[j] = i + 1;
                    ax[j++] = ds;
                }

                ap[i + 1] = j;
            }

            return matrix;
        }
    }
}
