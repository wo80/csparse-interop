namespace CSparse.Double.Examples
{
    using CSparse.Double;
    using System;

    /// <summary>
    /// Examples taken from ARPACK++.
    /// </summary>
    static class Generate
    {
        #region Symmetric

        /// <summary>
        /// Function template for the matrix
        ///
        ///            | T -I          |
        ///            |-I  T -I       |
        ///        A = |   -I  T       |
        ///            |        ...  -I|
        ///            |           -I T|
        ///
        /// derived from the standard central difference discretization of the
        /// 2-dimensional Laplacian on the unit square with zero Dirichlet
        /// boundary conditions.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\sym\LSMatrxA.h
        /// </remarks>
        public static SparseMatrix SymmetricMatrixA(int nx, char uplo = 'U')
        {
            // Defining internal variables.

            int i, j;
            double h2, df, dd;

            // Defining constants.

            h2 = 1.0 / ((nx + 1) * (nx + 1));
            dd = 4.0 / h2;
            df = -1.0 / h2;

            // Defining the number of columns and nonzero elements of matrix.

            int n = nx * nx;
            int nnz = 3 * n - 2 * nx;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Defining  matrix A.

            ap[0] = 0;
            i = 0;

            if (uplo == 'U')
            {
                for (j = 0; j < n; j++)
                {
                    if (j >= nx)
                    {
                        ax[i] = df; ai[i++] = j - nx;
                    }
                    if ((j % nx) != 0)
                    {
                        ax[i] = df; ai[i++] = j - 1;
                    }
                    ax[i] = dd; ai[i++] = j;
                    ap[j + 1] = i;
                }
            }
            else
            {
                for (j = 0; j < n; j++)
                {
                    ax[i] = dd; ai[i++] = j;
                    if (((j + 1) % nx) != 0)
                    {
                        ax[i] = df; ai[i++] = j + 1;
                    }
                    if (j < n - nx)
                    {
                        ax[i] = df; ai[i++] = j + nx;
                    }
                    ap[j + 1] = i;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Function template for the one dimensional discrete Laplacian on
        /// the interval [0, 1], with zero Dirichlet boundary conditions.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\sym\LSMatrxB.h
        /// </remarks>
        public static SparseMatrix SymmetricMatrixB(int n, char uplo = 'U')
        {
            // Defining internal variables.

            int i, j;
            double h2, df, dd;

            // Defining constants.

            h2 = ((n + 1) * (n + 1));
            dd = 2.0 * h2;
            df = -h2;

            // Defining the number of nonzero elements in A.

            int nnz = 2 * n - 1;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Defining matrix A.

            ap[0] = 0;
            i = 0;

            if (uplo == 'U')
            {
                for (j = 0; j < n; j++)
                {
                    if (j != 0)
                    {
                        ax[i] = df; ai[i++] = j - 1;
                    }
                    ax[i] = dd; ai[i++] = j;
                    ap[j + 1] = i;
                }
            }
            else
            {
                for (j = 0; j < n; j++)
                {
                    ax[i] = dd; ai[i++] = j;
                    if (n - j - 1 != 0)
                    {
                        ax[i] = df; ai[i++] = j + 1;
                    }
                    ap[j + 1] = i;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Function template for the one dimensional discrete Laplacian on the interval [0, 1],
        /// with zero Dirichlet boundary conditions. The difference between the function shown
        /// here and SymmetricMatrixB is only the scaling factor.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\sym\LSMatrxC.h
        /// </remarks>
        public static SparseMatrix SymmetricMatrixC(int n, char uplo = 'U')
        {
            // Defining internal variables.

            int i, j;
            double h, df, dd;

            // Defining constants.

            h = (n + 1);
            dd = 2.0 * h;
            df = -h;

            // Defining the number of nonzero elements in A.

            int nnz = 2 * n - 1;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Defining matrix A.

            ap[0] = 0;
            i = 0;

            if (uplo == 'U')
            {
                for (j = 0; j < n; j++)
                {
                    if (j != 0)
                    {
                        ax[i] = df; ai[i++] = j - 1;
                    }
                    ax[i] = dd; ai[i++] = j;
                    ap[j + 1] = i;
                }
            }
            else
            {
                for (j = 0; j < n; j++)
                {
                    ax[i] = dd; ai[i++] = j;
                    if (n - j - 1 != 0)
                    {
                        ax[i] = df; ai[i++] = j + 1;
                    }
                    ap[j + 1] = i;
                }
            }

            return matrix;
        }

        /// <summary>
        /// Function template for the 1-dimensional mass matrix on the interval [0,1].
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\sym\LSMatrxD.h
        /// </remarks>
        public static SparseMatrix SymmetricMatrixD(int n, char uplo = 'U')
        {
            // Defining internal variables.

            int i, j;
            double h, df, dd;

            // Defining constants.

            h = 1.0 / (n + 1);
            dd = 4.0 / 6.0 * h;
            df = h / 6.0;

            // Defining the number of nonzero elements in A.

            int nnz = 2 * n - 1;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Defining matrix A.

            ap[0] = 0;
            i = 0;

            if (uplo == 'U')
            {
                for (j = 0; j < n; j++)
                {
                    if (j != 0)
                    {
                        ax[i] = df; ai[i++] = j - 1;
                    }
                    ax[i] = dd; ai[i++] = j;
                    ap[j + 1] = i;
                }
            }
            else
            {
                for (j = 0; j < n; j++)
                {
                    ax[i] = dd; ai[i++] = j;
                    if (n - j - 1 != 0)
                    {
                        ax[i] = df; ai[i++] = j + 1;
                    }
                    ap[j + 1] = i;
                }
            }

            return matrix;
        }

        #endregion

        #region General

        /// <summary>
        /// Function template for the 2-D Brusselator Wave model. 
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxA.h
        /// </remarks>
        public static SparseMatrix BrusselatorMatrix(double L, double delta1, double delta2, double alpha, double beta, int n)
        {
            // Defining internal variables.

            int i, j, icount;
            int m, m2;
            double h, tau1, tau2;
            double d1, d2, alpha2;

            // Defining constants.

            const double one = 1.0;
            const double four = 4.0;

            m = (int)Math.Sqrt(n / 2);
            m2 = 2 * m;
            h = one / (m + 1);
            tau1 = delta1 / (h * h * L * L);
            tau2 = delta2 / (h * h * L * L);
            alpha2 = alpha * alpha;
            d1 = -tau1 * four + beta - one;
            d2 = -tau2 * four - alpha2;

            // Defining the number of nonzero elements of matrix.

            int nnz = 6 * n - 8 * m;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Creating brusselator matrix.

            ap[0] = 0;

            // First two columns.

            ax[0] = d1; ai[0] = 0;
            ax[1] = -beta; ai[1] = 1;
            ax[2] = tau1; ai[2] = 2;
            ax[3] = tau1; ai[3] = m2;
            ap[1] = 4;

            ax[4] = alpha2; ai[4] = 0;
            ax[5] = d2; ai[5] = 1;
            ax[6] = tau2; ai[6] = 3;
            ax[7] = tau2; ai[7] = m2 + 1;
            ap[2] = 8;

            // Next 2m-2 columns.

            i = ap[2];

            for (j = 2; j < m2; j += 2)
            {
                ax[i] = tau1; ai[i++] = j - 2;
                ax[i] = d1; ai[i++] = j;
                ax[i] = -beta; ai[i++] = j + 1;
                if (j != m2 - 2)
                {
                    ax[i] = tau1; ai[i++] = j + 2;
                }
                ax[i] = tau1; ai[i++] = m2 + j;
                ap[j + 1] = i;

                ax[i] = tau2; ai[i++] = j - 1;
                ax[i] = alpha2; ai[i++] = j;
                ax[i] = d2; ai[i++] = j + 1;
                if (j != m2 - 2)
                {
                    ax[i] = tau2; ai[i++] = j + 3;
                }
                ax[i] = tau2; ai[i++] = m2 + j + 1;
                ap[j + 2] = i;
            }

            // Next n-4m columns.

            icount = 0;

            for (j = m2; j < n - m2; j += 2)
            {
                ax[i] = tau1; ai[i++] = j - m2;
                if (icount != 0)
                {
                    ax[i] = tau1; ai[i++] = j - 2;
                }
                ax[i] = d1; ai[i++] = j;
                ax[i] = -beta; ai[i++] = j + 1;
                if (icount != m2 - 2)
                {
                    ax[i] = tau1; ai[i++] = j + 2;
                }
                ax[i] = tau1; ai[i++] = j + m2;
                ap[j + 1] = i;

                ax[i] = tau2; ai[i++] = j - m2 + 1;
                if (icount != 0)
                {
                    ax[i] = tau2; ai[i++] = j - 1;
                }
                ax[i] = alpha2; ai[i++] = j;
                ax[i] = d2; ai[i++] = j + 1;
                if (icount != m2 - 2)
                {
                    ax[i] = tau2; ai[i++] = j + 3;
                }
                ax[i] = tau2; ai[i++] = j + m2 + 1;
                ap[j + 2] = i;

                icount = (icount + 2) % (m2);
            }

            // Next 2m-2 columns.

            for (j = n - m2; j < n - 2; j += 2)
            {
                ax[i] = tau1; ai[i++] = j - m2;
                if (j != n - m2)
                {
                    ax[i] = tau1; ai[i++] = j - 2;
                }
                ax[i] = d1; ai[i++] = j;
                ax[i] = -beta; ai[i++] = j + 1;
                ax[i] = tau1; ai[i++] = j + 2;
                ap[j + 1] = i;

                ax[i] = tau2; ai[i++] = j - m2 + 1;
                if (j != n - m2)
                {
                    ax[i] = tau2; ai[i++] = j - 1;
                }
                ax[i] = alpha2; ai[i++] = j;
                ax[i] = d2; ai[i++] = j + 1;
                ax[i] = tau2; ai[i++] = j + 3;
                ap[j + 2] = i;

            }

            // Last two columns.  

            ax[i] = tau1; ai[i++] = n - m2 - 2;
            ax[i] = tau1; ai[i++] = n - 4;
            ax[i] = d1; ai[i++] = n - 2;
            ax[i] = -beta; ai[i++] = n - 1;
            ap[n - 1] = i;

            ax[i] = tau2; ai[i++] = n - m2 - 1;
            ax[i] = tau2; ai[i++] = n - 3;
            ax[i] = alpha2; ai[i++] = n - 2;
            ax[i] = d2; ai[i++] = n - 1;
            ap[n] = i;

            return matrix;
        }

        /// <summary>
        /// Function template for the matrix
        ///
        ///               | T -I          |
        ///               |-I  T -I       |
        ///           A = |   -I  T       |
        ///               |        ...  -I|
        ///               |           -I T|
        ///
        /// derived from the standard central difference discretization of the
        /// 2-dimensional convection-diffusion operator (Laplacian u) + rho*(du/dx)
        /// on a unit square with zero Dirichlet boundary conditions.
        /// When rho*h/2 &lt;= 1, the discrete convection-diffusion operator has real
        /// eigenvalues.  When rho*h/2 > 1, it has COMPLEX eigenvalues.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxB.h
        /// </remarks>
        public static SparseMatrix BlockTridMatrix(int nx, double rho = 0.0)
        {
            // Defining internal variables.

            int i, j;
            double h, h2, df;
            double dd, dl, du;

            // Defining constants.

            h = 1.0 / (nx + 1);
            h2 = h * h;
            dd = 4.0 / h2;
            df = -1.0 / h2;
            dl = df - 0.5 * rho / h;
            du = df + 0.5 * rho / h;

            // Defining the number of columns and nonzero elements of matrix.

            int n = nx * nx;
            int nnz = 5 * n - 4 * nx;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Creating matrix.

            ap[0] = 0;
            i = 0;

            for (j = 0; j < n; j++)
            {
                if (j >= nx)
                {
                    ax[i] = df; ai[i++] = j - nx;
                }
                if ((j % nx) != 0)
                {
                    ax[i] = du; ai[i++] = j - 1;
                }
                ax[i] = dd; ai[i++] = j;
                if (((j + 1) % nx) != 0)
                {
                    ax[i] = dl; ai[i++] = j + 1;
                }
                if (j < n - nx)
                {
                    ax[i] = df; ai[i++] = j + nx;
                }
                ap[j + 1] = i;
            }

            return matrix;
        }

        /// <summary>
        /// Function template for the stiffness matrix obtained from the finite
        /// element discretization of the 1-dimensional convection diffusion operator
        /// d^2u/dx^2 + rho*(du/dx) on the interval [0,1] with zero Dirichlet boundary
        /// condition using linear elements.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxC.h
        /// </remarks>
        public static SparseMatrix StiffnessMatrix(int n, double rho)
        {
            int i, j;
            double dd, dl, du, s, h;

            // Defining constants.

            const double one = 1.0;
            const double two = 2.0;

            h = one / (n + 1);
            s = rho / two;
            dd = two / h;
            dl = -one / h - s;
            du = -one / h + s;

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;
            j = 0;
            for (i = 0; i != n; i++)
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
        /// Function template for the mass matrix formed by using piecewise linear elements on [0, 1].
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxD.h
        /// </remarks>
        public static SparseMatrix MassMatrix(int n)
        {
            int i, j;
            double diag, sub;

            // Defining constants.

            sub = 1.0 / 6.0 / (n + 1);
            diag = 4.0 / 6.0 / (n + 1);

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;
            j = 0;
            for (i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = sub;
                }
                ai[j] = i;
                ax[j++] = diag;
                if (i != (n - 1))
                {
                    ai[j] = i + 1;
                    ax[j++] = sub;
                }
                ap[i + 1] = j;
            }

            return matrix;
        }

        /// <summary>
        /// Function template that generates a nonsymmetric tridiagonal matrix with 2 on
        /// the main diagonal, 3 on the superdiagonal and -2 on the subdiagonal.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxE.h
        /// </remarks>
        public static SparseMatrix NonSymMatrixE(int n)
        {
            int i, j;

            // Defining constants.

            const double three = 3.0;
            const double two = 2.0;

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;
            j = 0;
            for (i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = three;
                }
                ai[j] = i;
                ax[j++] = two;
                if (i != (n - 1))
                {
                    ai[j] = i + 1;
                    ax[j++] = -two;
                }
                ap[i + 1] = j;
            }

            return matrix;
        }

        /// <summary>
        /// Function template that generates a tridiagonal nonsymmetric matrix.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxF.h
        /// </remarks>
        public static SparseMatrix NonSymMatrixF(int n)
        {
            int i, j;
            double diag, sub;

            // Defining constants.

            sub = 1.0;
            diag = 4.0;

            // Defining the number of nonzero matrix elements.

            int nnz = 3 * n - 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;
            j = 0;
            for (i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = sub;
                }
                ai[j] = i;
                ax[j++] = diag;
                if (i != (n - 1))
                {
                    ai[j] = i + 1;
                    ax[j++] = sub;
                }
                ap[i + 1] = j;
            }

            return matrix;
        }

        /// <summary>
        /// Function template that generates a  sparse (2n x n) rectangular matrix.
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxV.h
        /// </remarks>
        public static SparseMatrix RectangularMatrix(int n)
        {
            int i, j;
            double dd, dl, du;

            // Defining constants.

            dl = 1.0;
            dd = 4.0;
            du = 2.0;

            // Defining the number of nonzero elements and lines of the matrix.

            int nnz = n * 6 - 2;
            int m = n * 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(m, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;
            j = 0;

            for (i = 0; i != n; i++)
            {
                if (i != 0)
                {
                    ai[j] = i - 1;
                    ax[j++] = du;
                }

                ai[j] = i;
                ax[j++] = dd;

                ai[j] = i + 1;
                ax[j++] = dl;

                ai[j] = i + n - 1;
                ax[j++] = dl;

                ai[j] = i + n;
                ax[j++] = dd;

                if (i != (n - 1))
                {
                    ai[j] = i + n + 1;
                    ax[j++] = du;
                }

                ap[i + 1] = j;

            }

            return matrix;
        }

        /// <summary>
        /// Class template for the m by n matrix A with entries
        ///
        ///  A(i,j) = k*(s(i))*(t(j)-1), if j-nl &lt;= i &lt;= j;
        ///           k*(t(j))*(s(i)-1), if j &lt; i &lt;= j+nu,
        ///
        /// where s(i) = i/(m+1), t(j) = j/(n+1) and k = 1/(n+1).
        /// </summary>
        /// <remarks>
        /// MODULE examples\matrices\nonsym\LNMatrxW.h
        /// </remarks>
        public static SparseMatrix MatrixW(int m, int n, int nl, int nu, int nnz)
        {
            int i, j, l, u, mn, p;
            double h, k;

            // Defining constants.

            l = nl - m + n;
            l = (l < 0) ? 0 : ((l > nl) ? nl : l);
            u = nu - n + m;
            u = (u < 0) ? 0 : ((u > nu) ? nu : u);
            h = 1.0 / (m + 1);
            k = 1.0 / (n + 1);

            // Defining the number of nonzero elements and lines of the matrix.

            mn = (m < n) ? m : n;
            nnz = mn * (nl + nu + 1) - (l * l + l) / 2 - (u * u + u) / 2;

            // Creating output vectors.

            var matrix = new SparseMatrix(n, n, nnz);

            var ax = matrix.Values;
            var ai = matrix.RowIndices;
            var ap = matrix.ColumnPointers;

            // Filling A, ai and ap.

            ap[0] = 0;
            p = 0;

            for (j = 0; j != n; j++)
            {
                for (i = ((j - nu) > 0) ? (j - nu) : 0; ((i <= j) && (i < m)); i++)
                {
                    ai[p] = i;
                    ax[p++] = k * ((i + 1) * h - 1.0) * ((j + 1) * k);
                }

                for (i = j + 1; ((i <= (j + nl)) && (i < m)); i++)
                {
                    ai[p] = i;
                    ax[p++] = k * ((j + 1) * k - 1.0) * ((i + 1) * h);
                }

                ap[j + 1] = p;

            }

            return matrix;
        }

        #endregion
    }
}
