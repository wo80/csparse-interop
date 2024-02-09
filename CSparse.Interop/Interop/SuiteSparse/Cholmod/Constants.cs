namespace CSparse.Interop.SuiteSparse.Cholmod
{
    #region Enums

    /// <summary>
    /// Defines what the numerical type is (double or float)
    /// </summary>
    public enum Dtype : int
    {
        /// <summary>
        /// all numerical values are double
        /// </summary>
        Double = 0,
        /// <summary>
        /// all numerical values are float
        /// </summary>
        Single = 4
    }

    /// <summary>
    /// Defines the kind of numerical values used.
    /// </summary>
    /// <remarks>
    /// The xtype of all parameters for all CHOLMOD routines must match.
    /// 
    /// CHOLMOD_PATTERN: x and z are ignored.
    /// CHOLMOD_DOUBLE:  x is non-null of size nzmax, z is ignored.
    /// CHOLMOD_COMPLEX: x is non-null of size 2* nzmax doubles, z is ignored.
    /// CHOLMOD_ZOMPLEX: x and z are non-null of size nzmax
    ///
    /// In the real case, z is ignored.The kth entry in the matrix is x[k].
    /// There are two methods for the complex case.  In the ANSI C99-compatible
    /// CHOLMOD_COMPLEX case, the real and imaginary parts of the kth entry
    /// are in x[2 /// k] and x[2 /// k + 1], respectively.z is ignored.In the
    /// MATLAB-compatible CHOLMOD_ZOMPLEX case, the real and imaginary
    /// parts of the kth entry are in x[k] and z[k].
    ///
    /// Scalar floating-point values are always passed as double arrays of size 2
    /// (real and imaginary parts).  The imaginary part of a scalar is ignored if
    /// the routine operates on a real matrix.
    ///
    /// These Modules support complex and zomplex matrices, with a few exceptions:
    ///
    /// Check       all routines
    /// Cholesky    all routines
    /// Core        all except cholmod_aat, add, band, copy
    /// Demo        all routines
    /// Partition   all routines
    /// Supernodal  all routines support any real, complex, or zomplex input.
    /// 
    /// There will never be a supernodal zomplex L; a complex
    /// supernodal L is created if A is zomplex.
    ///
    /// These Modules do not support complex and zomplex matrices at all:
    ///
    /// Modify      all routines support real matrices only
    /// </remarks>
    public enum Xtype : int
    {
        /// <summary>
        /// Pattern only, no numerical values.
        /// </summary>
        Pattern = 0,
        /// <summary>
        /// Real (double or single), not complex.
        /// </summary>
        Real = 1,
        /// <summary>
        /// Complex (double or single), interleaved.
        /// </summary>
        Complex = 2,
        /// <summary>
        /// Complex (double or single), with real and imag parts held in different arrays.
        /// </summary>
        Zomplex = 3
    }

    /// <summary>
    /// Describes what parts of the matrix are considered.
    /// </summary>
    public enum Stype : int
    {
        /// <summary>
        /// Matrix is square and symmetric, use upper triangular part.
        /// Entries in the lower triangular part are ignored.
        /// </summary>
        Upper = -1,
        /// <summary>
        /// Matrix is "unsymmetric": use both upper and lower triangular parts
        /// (the matrix may actually be symmetric in pattern and value, but
        /// both parts are explicitly stored and used).  May be square or
        /// rectangular.
        /// </summary>
        General = 0,
        /// <summary>
        /// Matrix is square and symmetric, use lower triangular part.
        /// Entries in the upper triangular part are ignored.
        /// </summary>
        Lower = 1
    }

    /// <summary>
    /// Cholmod solve codes.
    /// </summary>
    /// <remarks>
    /// Solves one of many linear systems with a dense right-hand-side, using the
    /// factorization from cholmod_factorize (or as modified by any other CHOLMOD
    /// routine).  D is identity for LL' factorizations.
    /// </remarks>
    public enum CholmodSolve : int
    {
        /// <summary>
        /// solve Ax=b
        /// </summary>
        A = 0,
        /// <summary>
        /// solve LDL'x=b
        /// </summary>
        LDLt = 1,
        /// <summary>
        /// solve LDx=b
        /// </summary>
        LD = 2,
        /// <summary>
        /// solve DL'x=b
        /// </summary>
        DLt = 3,
        /// <summary>
        /// solve Lx=b
        /// </summary>
        L = 4,
        /// <summary>
        /// solve L'x=b
        /// </summary>
        Lt = 5,
        /// <summary>
        /// solve Dx=b
        /// </summary>
        D = 6,
        /// <summary>
        /// permute x=Px
        /// </summary>
        P = 7,
        /// <summary>
        /// permute x=P'x
        /// </summary>
        Pt = 8
    }

    /// <summary>
    /// Scaling modes.
    /// </summary>
    public enum CholmodScale : int
    {
        /// <summary>
        /// A = s*A
        /// </summary>
        Scalar = 0,
        /// <summary>
        /// A = diag(s)*A.
        /// </summary>
        Row = 1,
        /// <summary>
        /// A = A*diag(s).
        /// </summary>
        Column = 2,
        /// <summary>
        /// A = diag(s)*A*diag(s).
        /// </summary>
        Sym = 3
    }

    /// <summary>
    /// Ordering method to use.
    /// </summary>
    public enum CholmodOrdering : int
    {
        /// <summary>
        /// Use natural ordering.
        /// </summary>
        Natural = 0,
        /// <summary>
        /// Use given permutation.
        /// </summary>
        Given = 1,
        /// <summary>
        /// Use minimum degree (AMD).
        /// </summary>
        AMD = 2,
        /// <summary>
        /// Use METIS' nested dissection.
        /// </summary>
        METIS = 3,
        /// <summary>
        /// Use CHOLMOD's version of nested dissection.
        /// </summary>
        NESDIS = 4,
        /// <summary>
        /// Use AMD for A, COLAMD for A*A'.
        /// </summary>
        COLAMD = 5,
        /// <summary>
        /// Natural ordering, postordered.
        /// </summary>
        PostOrdered = 5
    }

    #endregion

    internal static class Constants
    {

        /* Solves one of many linear systems with a dense right-hand-side, using the
        * factorization from cholmod_factorize (or as modified by any other CHOLMOD
        * routine).  D is identity for LL' factorizations. */

        internal const int CHOLMOD_A = 0;		/* solve Ax=b */
        internal const int CHOLMOD_LDLt = 1;		/* solve LDL'x=b */
        internal const int CHOLMOD_LD = 2;		/* solve LDx=b */
        internal const int CHOLMOD_DLt = 3;		/* solve DL'x=b */
        internal const int CHOLMOD_L = 4;		/* solve Lx=b */
        internal const int CHOLMOD_Lt = 5;		/* solve L'x=b */
        internal const int CHOLMOD_D = 6;		/* solve Dx=b */
        internal const int CHOLMOD_P = 7;		/* permute x=Px */
        internal const int CHOLMOD_Pt = 8;		/* permute x=P'x */

        /* scaling modes, selected by the scale input parameter: */
        internal const int CHOLMOD_SCALAR = 0;	/* A = s*A */
        internal const int CHOLMOD_ROW = 1;		/* A = diag(s)*A */
        internal const int CHOLMOD_COL = 2;		/* A = A*diag(s) */
        internal const int CHOLMOD_SYM = 3;		/* A = diag(s)*A*diag(s) */

        /* ========================================================================== */
        /* === CHOLMOD objects ====================================================== */
        /* ========================================================================== */

        /* Each CHOLMOD object has its own type code. */

        internal const int CHOLMOD_COMMON = 0;
        internal const int CHOLMOD_SPARSE = 1;
        internal const int CHOLMOD_FACTOR = 2;
        internal const int CHOLMOD_DENSE = 3;
        internal const int CHOLMOD_TRIPLET = 4;

        /* ========================================================================== */
        /* === CHOLMOD Common ======================================================= */
        /* ========================================================================== */

        // itype: integer sizes
        // The itype is held in the Common object and must match the method used.
        internal const int CHOLMOD_INT  = 0;    /* int32, for cholmod_* methods (no _l_) */
        internal const int CHOLMOD_LONG = 2;    /* int64, for cholmod_l_* methods        */

        // dtype: floating point sizes (double or float)
        // The dtype of all parameters for all CHOLMOD routines must match.
        // NOTE: CHOLMOD_SINGLE is still under development.
        internal const int CHOLMOD_DOUBLE = 0;  /* matrix or factorization is double precision */
        internal const int CHOLMOD_SINGLE = 4;  /* matrix or factorization is single precision */

        // xtype: pattern, real, complex, or zomplex
        internal const int CHOLMOD_PATTERN = 0; /* no numerical values                            */
        internal const int CHOLMOD_REAL    = 1; /* real (double or single), not complex           */
        internal const int CHOLMOD_COMPLEX = 2; /* complex (double or single), interleaved        */
        internal const int CHOLMOD_ZOMPLEX = 3; /* complex (double or single), with real and imag */
        /* parts held in different arrays                 */

        // xdtype is (xtype + dtype), which combines the two type parameters into
        // a single number handling all 8 cases:
        //
        //  (0) CHOLMOD_DOUBLE + CHOLMOD_PATTERN    a pattern-only matrix
        //  (1) CHOLMOD_DOUBLE + CHOLMOD_REAL       a double real matrix
        //  (2) CHOLMOD_DOUBLE + CHOLMOD_COMPLEX    a double complex matrix
        //  (3) CHOLMOD_DOUBLE + CHOLMOD_ZOMPLEX    a double zomplex matrix
        //  (4) CHOLMOD_SINGLE + CHOLMOD_PATTERN    a pattern-only matrix
        //  (5) CHOLMOD_SINGLE + CHOLMOD_REAL       a float real matrix
        //  (6) CHOLMOD_SINGLE + CHOLMOD_COMPLEX    a float complex matrix
        //  (7) CHOLMOD_SINGLE + CHOLMOD_ZOMPLEX    a float zomplex matrix

        /* Definitions for cholmod_common: */
        public const int CHOLMOD_MAXMETHODS = 9;	/* maximum number of different methods that */
        /* cholmod_analyze can try. Must be >= 9. */

        // Common->status for error handling: 0 is ok, negative is a fatal
        // error, and positive is a warning

        public const int CHOLMOD_OK = 0;
        public const int CHOLMOD_NOT_INSTALLED = -1; /* module not installed          */
        public const int CHOLMOD_OUT_OF_MEMORY = -2; /* malloc/calloc/realloc failed  */
        public const int CHOLMOD_TOO_LARGE     = -3; /* integer overflow              */
        public const int CHOLMOD_INVALID       = -4; /* input invalid                 */
        public const int CHOLMOD_GPU_PROBLEM   = -5; /* CUDA error                    */
        public const int CHOLMOD_NOT_POSDEF    =  1; /* matrix not positive definite  */
        public const int CHOLMOD_DSMALL        =  2; /* diagonal entry very small     */

        public const int TRUE = 1;

        // ordering method
        public const int CHOLMOD_NATURAL = 0; /* no preordering                             */
        public const int CHOLMOD_GIVEN   = 1; /* user-provided permutation                  */
        public const int CHOLMOD_AMD     = 2; /* AMD: approximate minimum degree            */
        public const int CHOLMOD_METIS   = 3; /* METIS: mested dissection                   */
        public const int CHOLMOD_NESDIS  = 4; /* CHOLMOD's nested dissection                */
        public const int CHOLMOD_COLAMD  = 5; /* AMD for A, COLAMD for AA' or A'A           */
        public const int CHOLMOD_POSTORDERED = 6; /* natural then postordered               */

        // supernodal strategy
        public const int CHOLMOD_SIMPLICIAL = 0; /* always use simplicial method            */
        public const int CHOLMOD_AUTO       = 1; /* auto select simplicial vs supernodal    */
        public const int CHOLMOD_SUPERNODAL = 2; /* always use supernoda method             */

        public const int CHOLMOD_HOST_SUPERNODE_BUFFERS = 8;	/* always do supernodal */
    }
}
