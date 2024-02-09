
namespace CSparse.Interop.SuiteSparse.Cholmod
{
    using System;
    using System.Runtime.InteropServices;

#if X64
    using size_t = System.UInt64;
    using SuiteSparse_long = System.Int64;
#else
    using size_t = System.UInt32;
    using SuiteSparse_long = System.Int32;
#endif

    /* ... so make them void * pointers if the GPU is not being used */
    using CHOLMOD_CUBLAS_HANDLE = System.IntPtr;
    using CHOLMOD_CUDASTREAM = System.IntPtr;
    using CHOLMOD_CUDAEVENT = System.IntPtr;

    // IntPtr = const char *
    public delegate void ErrorHandler(int status, IntPtr file, int line, IntPtr message);

    [StructLayout(LayoutKind.Sequential)]
    public struct CholmodMethod
    {

        //----------------------------------------------------------------------
        // statistics from the ordering
        //----------------------------------------------------------------------

        public double lnz;    // number of nonzeros in L
        public double fl;     // Cholesky flop count for this ordering (each
            // multiply and each add counted once (doesn't count complex
            // flops).

        //----------------------------------------------------------------------
        // ordering parameters:
        //----------------------------------------------------------------------

        public double prune_dense;    // dense row/col control.  Default: 10.
            // Rows/cols with more than max (prune_dense*sqrt(n),16) are
            // removed prior to orderingm and placed last.  If negative,
            // only completely dense rows/cols are removed.  Removing these
            // rows/cols with many entries can speed up the ordering, but
            // removing too many can reduce the ordering quality.
            //
            // For AMD, SYMAMD, and CSYMAMD, this is the only dense row/col
            // parameter.  For COLAMD and CCOLAMD, this parameter controls
            // how dense columns are handled.

        public double prune_dense2;   // dense row control for COLAMD and CCOLAMD.
            // Default -1.  When computing the Cholesky factorization of AA'
            // rows with more than max(prune_dense2*sqrt(n),16) entries
            // are removed prior to ordering.  If negative, only completely
            // dense rows are removed.

        public double nd_oksep;   // for CHOLMOD's nesdis method. Default 1.
            // A node separator with nsep nodes is discarded if
            // nsep >= nd_oksep*n.

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] other_1;    // unused, for future expansion

        public size_t nd_small;   // for CHOLMOD's nesdis method. Default 200.
            // Subgraphs with fewer than nd_small nodes are not partitioned.

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public double[] other_2;    // unused, for future expansion

        public int aggressive;    // if true, AMD, COLAMD, SYMAMD, CCOLAMD, and
            // CSYMAMD perform aggresive absorption.  Default: true

        public int order_for_lu;  // Default: false.  If the CHOLMOD analysis/
            // ordering methods are used as an ordering method for an LU
            // factorization, then set this to true.  For use in a Cholesky
            // factorization by CHOLMOD itself, never set this to true.

        public int nd_compress;   // if true, then the graph and subgraphs are
            // compressed before partitioning them in CHOLMOD's nesdis
            // method.  Default: true.

        public int nd_camd;   // if 1, then CHOLMOD's nesdis is followed by
            // CAMD.  If 2: followed by CSYMAMD.  If nd_small is very small,
            // then use 0, which skips CAMD or CSYMAMD.  Default: 1.

        public int nd_components; // CHOLMOD's nesdis can partition a graph and then
            // find that the subgraphs are unconnected.  If true, each of these
            // components is partitioned separately.  If false, the whole
            // subgraph is partitioned.  Default: false.

        public int ordering;  // ordering method to use

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        internal size_t[] other_3;    // unused, for future expansion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CholmodCommon
    {
        //--------------------------------------------------------------------------
        // primary parameters for factorization and update/downdate
        //--------------------------------------------------------------------------

        public double dbound; // Bounds the diagonal entries of D for LDL'
            // factorization and update/downdate/rowadd.  Entries outside this
            // bound are replaced with dbound.  Default: 0.  dbound is used for
            // double precision factorization only.  See sbound for single
            // precision factorization.

        public double grow0;      // default: 1.2
        public double grow1;      // default: 1.2
        public size_t grow2;      // default: 5
            // Initial space for simplicial factorization is max(grow0,1) times the
            // required space.  If space is exhausted, L is grown by max(grow0,1.2)
            // times the required space.  grow1 and grow2 control how each column
            // of L can grow in an update/downdate; if space runs out, then
            // grow1*(required space) + grow2 is allocated.

        public size_t maxrank;    // maximum rank for update/downdate.  Valid values are
            // 2, 4, and 8.  Default is 8.  If a larger update/downdate is done, it
            // is done in steps of maxrank.

        public double supernodal_switch;  // default: 40
        public int supernodal;            // default: CHOLMOD_AUTO.
            // Controls supernodal vs simplicial factorization.  If
            // Common->supernodal is CHOLMOD_SIMPLICIAL, a simplicial factorization
            // is always done; if CHOLMOD_SUPERNODAL, a supernodal factorization is
            // always done.  If CHOLMOD_AUTO, then a simplicial factorization is
            // down if flops/nnz(L) < Common->supernodal_switch.

        public int final_asis;    // if true, other final_* parameters are ignored,
            // except for final_pack and the factors are left as-is when done.
            // Default: true.

        public int final_super;   // if true, leave factor in supernodal form.
            // if false, convert to simplicial.  Default: true.

        public int final_ll;      // if true, simplicial factors are converted to LL',
            // otherwise left as LDL.  Default: false.

        public int final_pack;    // if true, the factorize are allocated with exactly
            // the space required.  Set this to false if you expect future
            // updates/downdates (giving a little extra space for future growth),
            // Default: true.

        public int final_monotonic;   // if true, columns are sorted when done, by
            // ascending row index.  Default: true.

        public int final_resymbol;    // if true, a supernodal factorization converted
            // to simplicial is reanalyzed, to remove zeros added for relaxed
            // amalgamation.  Default: false.

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public double[] zrelax;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public size_t[] nrelax;
            // The zrelax and nrelax parameters control relaxed supernodal
            // amalgamation,  If ns is the # of columns in two adjacent supernodes,
            // and z is the fraction of zeros in the two supernodes if merged, then
            // the two supernodes are merged if any of the 5 following condition
            // are true:
            //
            //      no new zero entries added if the two supernodes are merged
            //      (ns <= nrelax [0])
            //      (ns <= nrelax [1] && z < zrelax [0])
            //      (ns <= nrelax [2] && z < zrelax [1])
            //      (z < zrelax [2])
            //
            // With the defaults, the rules become:
            //
            //      no new zero entries added if the two supernodes are merged
            //      (ns <=  4)
            //      (ns <= 16 && z < 0.8)
            //      (ns <= 48 && z < 0.1)
            //      (z < 0.05)

        public int prefer_zomplex;    // if true, and a complex system is solved,
            // X is returned as zomplex (with two arrays, one for the real part
            // and one for the imaginary part).  If false, then X is returned as
            // a single array with interleaved real and imaginary parts.
            // Default: false.

        public int prefer_upper;  // if true, then a preference is given for holding
            // a symmetric matrix by just its upper triangular form.  This gives
            // the best performance by the CHOLMOD analysis and factorization
            // methods.  Only used by cholmod_read.  Default: true.

        public int quick_return_if_not_posdef;    // if true, a supernodal factorization
            // returns immediately if it finds the matrix is not positive definite.
            // If false, the failed supernode is refactorized, up to but not
            // including the failed column (required by MATLAB).

        public int prefer_binary; // if true, cholmod_read_triplet converts a symmetric
            // pattern-only matrix to a real matrix with all values set to 1.
            // if false, diagonal entries A(k,k) are set to one plus the # of
            // entries in row/column k, and off-diagonals are set to -1.
            // Default: false.

        //--------------------------------------------------------------------------
        // printing and error handling options
        //--------------------------------------------------------------------------

        public int print;     // print level.  Default is 3.
        public int precise;   // if true, print 16 digits, otherwise 5. Default: false.

        public int try_catch; // if true, ignore errors (CHOLMOD is assumed to be inside
            // a try/catch block.  No error messages are printed and the
            // error_handler function is not called.  Default: false.

        public ErrorHandler error_handler;
            // User error handling routine; default is NULL.
            // This function is called if an error occurs, with parameters:
            // status: the Common->status result.
            // file: filename where the error occurred.
            // line: line number where the error occurred.
            // message: a string that describes the error.

        //--------------------------------------------------------------------------
        // ordering options
        //--------------------------------------------------------------------------

        // CHOLMOD can try many ordering options and then pick the best result it
        // finds.  The default is to use one or two orderings: the user's
        // permutation (if given), and AMD.

        // Common->nmethods is the number of methods to try.  If the
        // Common->method array is left unmodified, the methods are:

        // (0) given (skipped if no user permutation)
        // (1) amd
        // (2) metis
        // (3) nesdis with defaults (CHOLMOD's nested dissection, based on METIS)
        // (4) natural
        // (5) nesdis: stop at subgraphs of 20000 nodes
        // (6) nesdis: stop at subgraphs of 4 nodes, do not use CAMD
        // (7) nesdis: no pruning on of dense rows/cols
        // (8) colamd

        // To use all 9 of the above methods, set Common->nmethods to 9.  The
        // analysis will take a long time, but that might be worth it if the
        // ordering will be reused many many times.

        // Common->nmethods and Common->methods can be revised to use a different
        // set of orderings.  For example, to use just a single method
        // (AMD with a weighted postordering):
        //
        //      Common->nmethods = 1 ;
        //      Common->method [0].ordering = CHOLMOD_AMD ;
        //      Common->postorder = TRUE ;
        //
        //

        public int nmethods;  // Number of methods to try, default is 0.
            // The value of 0 is a special case, and tells CHOLMOD to use the user
            // permutation (if not NULL) and then AMD.  Next, if fl is lnz are the
            // flop counts and number of nonzeros in L as found by AMD, then the
            // this ordering is used if fl/lnz < 500 or lnz/anz < 5, where anz is
            // the number of entries in A.  If this condition fails, METIS is tried
            // as well.
            //
            // Otherwise, if Common->nmethods > 0, then the methods defined by
            // Common->method [0 ... Common->nmethods-1] are used.

        public int current;   // The current method being tried in the analysis.
        public int selected;  // The selected method: Common->method [Common->selected]

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.CHOLMOD_MAXMETHODS + 1)]
        public CholmodMethod[] method;

        public int postorder; // if true, CHOLMOD performs a weighted postordering
            // after its fill-reducing ordering, which improves supernodal
            // amalgamation.  Has no effect on flop count or nnz(L).
            // Default: true.

        public int default_nesdis;    //  If false, then the default ordering strategy
            // when Common->nmethods is zero is to try the user's permutation
            // if given, then AMD, and then METIS if the AMD ordering results in
            // a lot of fill-in.  If true, then nesdis is used instead of METIS.
            // Default: false.

        //--------------------------------------------------------------------------
        // METIS workarounds
        //--------------------------------------------------------------------------

        // These workarounds were put into place for METIS 4.0.1.  They are safe
        // to use with METIS 5.1.0, but they might not longer be necessary.

        public double metis_memory;   // default: 0.  If METIS terminates your
            // program when it runs out of memory, try 2, or higher.
        public double metis_dswitch;  // default: 0.66
        public size_t metis_nswitch;  // default: 3000
            // If a matrix has n > metis_nswitch and a density (nnz(A)/n^2) >
            // metis_dswitch, then METIS is not used.

        //--------------------------------------------------------------------------
        // workspace
        //--------------------------------------------------------------------------

        // This workspace is kept in the CHOLMOD Common object.  cholmod_start
        // sets these arrays to NULL, and cholmod_finish frees them.

        public size_t nrow;       // Flag has size nrow, Head has size nrow+1
        public long mark;  // Flag is cleared if Flag [0..nrow-1] < mark.
        public size_t iworksize;  // size of Iwork, in Ints (int32 or int64).
                            // This is at most 6*nrow + ncol.
        public size_t xworkbytes; // size of Xwork, in bytes.
            // NOTE: in CHOLMOD v4 and earlier, this variable was called xworksize,
            // and was in terms of # of doubles, not # of bytes.

        internal IntPtr Flag;    // size nrow.  If this is "cleared" then
            // Flag [i] < mark for all i = 0:nrow-1.  Flag is kept cleared between
            // calls to CHOLMOD.

        internal IntPtr Head;    // size nrow+1.  If Head [i] = EMPTY (-1) then that
            // entry is "cleared".  Head is kept cleared between calls to CHOLMOD.

        internal IntPtr Xwork;   // a double or float array.  It has size nrow for most
            // routines, or 2*nrow if complex matrices are being handled.
            // It has size 2*nrow for cholmod_rowadd/rowdel, and maxrank*nrow for
            // cholmod_updown, where maxrank is 2, 4, or 8.  Xwork is kept all
            // zero between calls to CHOLMOD.

        internal IntPtr Iwork;   // size iworksize integers (int32's or int64's).
            // Uninitialized integer workspace, of size at most 6*nrow+ncol.

        public int itype;     // cholmod_start (for int32's) sets this to CHOLMOD_INT,
            // and cholmod_l_start sets this to CHOLMOD_LONG.  It defines the
            // integer sizes for th Flag, Head, and Iwork arrays, and also
            // defines the integers for all objects created by CHOLMOD.
            // The itype of the Common object must match the function name
            // and all objects passed to it.

        public int other_5;   // unused: for future expansion

        public int no_workspace_reallocate;   // an internal flag, usually false.
            // This is set true to disable any reallocation of the workspace
            // in the Common object.

        //--------------------------------------------------------------------------
        // statistics
        //--------------------------------------------------------------------------

        public int status;    // status code (0: ok, negative: error, pos: warning)

        public double fl;     // flop count from last analysis
        public double lnz;    // nnz(L) from last analysis
        public double anz;    // in last analysis: nnz(tril(A)) or nnz(triu(A)) if A
                        // symmetric, or tril(A*A') if A is unsymmetric.
        public double modfl;  // flop count from last update/downdate/rowadd/rowdel,
                        // not included the flops to revise the solution to Lx=b,
                        // if that was performed.

        public size_t malloc_count;   // # of malloc'd objects not yet freed
        public size_t memory_usage;   // peak memory usage in bytes
        public size_t memory_inuse;   // current memory usage in bytes

        public double nrealloc_col;   // # of column reallocations
        public double nrealloc_factor;// # of factor reallocations due to col. reallocs
        public double ndbounds_hit;   // # of times diagonal modified by dbound

        public double rowfacfl;       // flop count of cholmod_rowfac
        public double aatfl;          // flop count to compute A(:,f)*A(:,f)'

        public int called_nd; // true if last analysis used nesdis or METIS.
        public int blas_ok;   // true if no integer overflow has occured when trying to
            // call the BLAS.  The typical BLAS library uses 32-bit integers for
            // its input parameters, even on a 64-bit platform.  CHOLMOD uses int64
            // in its cholmod_l_* methods, and these must be typecast to the BLAS
            // integer.  If integer overflow occurs, this is set false.

        //--------------------------------------------------------------------------
        // SuiteSparseQR control parameters and statistics
        //--------------------------------------------------------------------------

        // SPQR uses the CHOLMOD Common object for its control and statistics.
        // These parameters are not used by CHOLMOD itself.

        // control parameters:
        public double SPQR_grain;     // task size is >= max (total flops / grain)
        public double SPQR_small;     // task size is >= small
        public int SPQR_shrink;       // controls stack realloc method
        public int SPQR_nthreads;     // number of TBB threads, 0 = auto

        // statistics:
        public double SPQR_flopcount;         // flop count for SPQR
        public double SPQR_analyze_time;      // analysis time in seconds for SPQR
        public double SPQR_factorize_time;    // factorize time in seconds for SPQR
        public double SPQR_solve_time;        // backsolve time in seconds
        public double SPQR_flopcount_bound;   // upper bound on flop count
        public double SPQR_tol_used;          // tolerance used
        public double SPQR_norm_E_fro;        // Frobenius norm of dropped entries
 
        //--------------------------------------------------------------------------
        // Revised for CHOLMOD v5.0
        //--------------------------------------------------------------------------

        // was size 10 in CHOLMOD v4.2; reduced to 8 in CHOLMOD v5:
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public long[] SPQR_istat;        // other statistics

        //--------------------------------------------------------------------------
        // Added for CHOLMOD v5.0
        //--------------------------------------------------------------------------

        // These terms have been added to the CHOLMOD Common struct for v5.0, and
        // on most systems they will total 16 bytes.  The preceding term,
        // SPQR_istat, was reduced by 16 bytes, since those last 2 entries were
        // unused in CHOLMOD v4.2.  As a result, the Common struct in v5.0 has the
        // same size as v4.0, and all entries would normally be in the same offset,
        // as well.  This mitigates any changes between v4.0 and v5.0, and may make
        // it easier to upgrade from v4 to v5.

        public double nsbounds_hit;   // # of times diagonal modified by sbound.
                        // This ought to be int64_t, but ndbounds_hit was double in
                        // v4 (see above), so nsbounds_hit is made the same type
                        // for consistency.
        public float sbound;  // Same as dbound,
                        // but for single precision factorization.
        public float other_6; // for future expansion

        //--------------------------------------------------------------------------
        // GPU configuration and statistics
        //--------------------------------------------------------------------------

        public int useGPU; // 1 if GPU is requested for CHOLMOD
                     // 0 if GPU is not requested for CHOLMOD
                     // -1 if the use of the GPU is in CHOLMOD controled by the
                     // CHOLMOD_USE_GPU environment variable.

        public size_t maxGpuMemBytes;     // GPU control for CHOLMOD
        public double maxGpuMemFraction;  // GPU control for CHOLMOD

        // for SPQR:
        public size_t gpuMemorySize;      // Amount of memory in bytes on the GPU
        public double gpuKernelTime;      // Time taken by GPU kernels
        public long gpuFlops;             // Number of flops performed by the GPU
        public int gpuNumKernelLaunches;  // Number of GPU kernel launches

        internal CHOLMOD_CUBLAS_HANDLE cublasHandle;

        // a set of streams for general use
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.CHOLMOD_HOST_SUPERNODE_BUFFERS)]
        internal CHOLMOD_CUDASTREAM[] gpuStream;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        internal CHOLMOD_CUDAEVENT[] cublasEventPotrf;
        internal CHOLMOD_CUDAEVENT updateCKernelsComplete;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.CHOLMOD_HOST_SUPERNODE_BUFFERS)]
        internal CHOLMOD_CUDAEVENT[] updateCBuffersFree;

        internal IntPtr dev_mempool; // pointer to single allocation of device memory
        internal size_t dev_mempool_size;

        internal IntPtr host_pinned_mempool; // pointer to single alloc of pinned mem
        internal size_t host_pinned_mempool_size;

        internal size_t devBuffSize;
        internal int ibuffer;
        internal double syrkStart;          // time syrk started

        // run times of the different parts of CHOLMOD (GPU and CPU):
        internal double cholmod_cpu_gemm_time;
        internal double cholmod_cpu_syrk_time;
        internal double cholmod_cpu_trsm_time;
        internal double cholmod_cpu_potrf_time;
        internal double cholmod_gpu_gemm_time;
        internal double cholmod_gpu_syrk_time;
        internal double cholmod_gpu_trsm_time;
        internal double cholmod_gpu_potrf_time;
        internal double cholmod_assemble_time;
        internal double cholmod_assemble_time2;

        // number of times the BLAS are called on the CPU and the GPU:
        internal size_t cholmod_cpu_gemm_calls;
        internal size_t cholmod_cpu_syrk_calls;
        internal size_t cholmod_cpu_trsm_calls;
        internal size_t cholmod_cpu_potrf_calls;
        internal size_t cholmod_gpu_gemm_calls;
        internal size_t cholmod_gpu_syrk_calls;
        internal size_t cholmod_gpu_trsm_calls;
        internal size_t cholmod_gpu_potrf_calls;

        public double chunk;      // chunksize for computing # of OpenMP threads to use.
            // Given nwork work to do, # of threads is
            // max (1, min (floor (work / chunk), nthreads_max))

        public int nthreads_max; // max # of OpenMP threads to use in CHOLMOD.
            // Defaults to SUITESPARSE_OPENMP_MAX_THREADS.

        public void Initialize()
        {
            zrelax = new double[3];
            nrelax = new size_t[3];

            method = new CholmodMethod[Constants.CHOLMOD_MAXMETHODS + 1];

            for (int i = 0; i < Constants.CHOLMOD_MAXMETHODS + 1; i++)
            {
                method[i].other_1 = new double[4];
                method[i].other_2 = new double[4];
                method[i].other_3 = new size_t[4];
            }

            SPQR_istat = new SuiteSparse_long[10];

            gpuStream = new CHOLMOD_CUBLAS_HANDLE[Constants.CHOLMOD_HOST_SUPERNODE_BUFFERS];
            cublasEventPotrf = new CHOLMOD_CUBLAS_HANDLE[3];
            updateCBuffersFree = new CHOLMOD_CUBLAS_HANDLE[Constants.CHOLMOD_HOST_SUPERNODE_BUFFERS];
        }
    }
}
