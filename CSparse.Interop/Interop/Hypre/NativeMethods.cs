
namespace CSparse.Interop.Hypre
{
    using System;
    using System.Runtime.InteropServices;

    using HYPRE_Int = System.Int32;
    using HYPRE_BigInt = System.Int32;
    using HYPRE_Real = System.Double;
    using HYPRE_Complex = System.Double;
    using HYPRE_Matrix = HYPRE_IJMatrix;
    using HYPRE_Vector = HYPRE_IJVector;


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HYPRE_Int HYPRE_PtrToParSolverFcn(HYPRE_Solver solver,
        HYPRE_ParCSRMatrix matrix,
        HYPRE_ParVector x,
        HYPRE_ParVector b);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate HYPRE_Int HYPRE_PtrToModifyPCFcn(HYPRE_Solver solver, HYPRE_Int iterations, HYPRE_Real rel_residual_norm);

    public static class NativeMethods
    {
        const string HYPRE_DLL = "libhypre";

        #region HYPRE_utilities.h

        /// <summary>
        /// HYPRE init.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_Init();

        /// <summary>
        /// HYPRE finalize.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_Finalize();

        // HYPRE error user functions

        /// <summary>
        /// Return the current hypre error flag.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GetError();

        /// <summary>
        /// Check if the given error flag contains the given error code.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CheckError(HYPRE_Int hypre_ierr, HYPRE_Int hypre_error_code);

        /// <summary>
        /// Return the index of the argument (counting from 1) where argument error (HYPRE_ERROR_ARG) has occured.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GetErrorArg();

        /// <summary>
        /// Clears the hypre error flag.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ClearAllErrors();

        /// <summary>
        /// Clears the given error code from the hypre error flag.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ClearError(HYPRE_Int hypre_error_code);

        /// <summary>
        /// Print GPU information.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PrintDeviceInfo();

        // HYPRE Version routines

        /// <summary>
        /// Allocates and returns a string with version number information in it.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_Version(ref IntPtr version_ptr);

        /// <summary>
        /// Returns version number information in integer form.
        /// </summary>
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_VersionNumber(out HYPRE_Int major_ptr,
            out HYPRE_Int minor_ptr,
            out HYPRE_Int patch_ptr,
            out HYPRE_Int single_ptr);

        #endregion

        #region HYPRE_IJ_mv.h

        #region IJMatrix

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixCreate(int comm,
            HYPRE_BigInt ilower, HYPRE_BigInt iupper,
            HYPRE_BigInt jlower, HYPRE_BigInt jupper,
            out HYPRE_IJMatrix matrix);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixDestroy(HYPRE_IJMatrix matrix);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern void HYPRE_IJMatrixInitialize(HYPRE_IJMatrix matrix);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixSetValues(HYPRE_IJMatrix matrix,
            HYPRE_Int nrows,
            ref HYPRE_Int ncols,
            ref HYPRE_BigInt rows,
            HYPRE_BigInt[] cols,
            IntPtr values);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixSetValues(HYPRE_IJMatrix matrix,
            HYPRE_Int nrows,
            HYPRE_Int[] ncols,
            HYPRE_BigInt[] rows,
            HYPRE_BigInt[] cols,
            IntPtr values);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixAssemble(HYPRE_IJMatrix matrix);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixSetObjectType(HYPRE_IJMatrix matrix, HYPRE_Int type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixGetObjectType(HYPRE_IJMatrix matrix, out HYPRE_Int type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJMatrixGetObject(HYPRE_IJMatrix matrix, out HYPRE_ParCSRMatrix obj);

        #endregion

        #region IJVector

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorCreate(int comm,
            HYPRE_BigInt ilower,
            HYPRE_BigInt iupper,
            out HYPRE_IJVector vector);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorDestroy(HYPRE_IJVector vector);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorInitialize(HYPRE_IJVector vector);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorSetValues(HYPRE_IJVector vector,
            HYPRE_Int nvalues,
            HYPRE_BigInt[] indices,
            HYPRE_Complex[] values);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorSetValues(HYPRE_IJVector vector,
            HYPRE_Int nvalues, IntPtr indices, IntPtr values);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorGetValues(HYPRE_IJVector vector,
            HYPRE_Int nvalues, IntPtr indices, IntPtr values);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorAssemble(HYPRE_IJVector vector);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorSetObjectType(HYPRE_IJVector vector, HYPRE_Int type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorGetObjectType(HYPRE_IJVector vector,
                                              out HYPRE_Int type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_IJVectorGetObject(HYPRE_IJVector vector, out HYPRE_ParVector obj);

        #endregion

        #endregion

        #region HYPRE_parcsr_ls.h

        #region BoomerAMG

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGCreate(out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetPrintLevel(HYPRE_Solver solver, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetTol(HYPRE_Solver solver, double v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetMaxLevels(HYPRE_Solver solver, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetNumSweeps(HYPRE_Solver solver, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetRelaxOrder(HYPRE_Solver solver, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetRelaxType(HYPRE_Solver solver, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetOldDefault(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetup(HYPRE_Solver solver, HYPRE_ParCSRMatrix parcsr_A, HYPRE_ParVector par_b, HYPRE_ParVector par_x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSolve(HYPRE_Solver solver, HYPRE_ParCSRMatrix parcsr_A, HYPRE_ParVector par_b, HYPRE_ParVector par_x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGGetNumIterations(HYPRE_Solver solver, out int num_iterations);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGGetFinalRelativeResidualNorm(HYPRE_Solver solver, out double final_res_norm);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetCoarsenType(HYPRE_Solver precond, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetMaxIter(HYPRE_Solver precond, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetILUType(HYPRE_Solver solver,
                                     HYPRE_Int ilu_type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetILULevel(HYPRE_Solver solver,
                                      HYPRE_Int ilu_lfil);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetILUMaxRowNnz(HYPRE_Solver solver,
                                          HYPRE_Int ilu_max_row_nnz);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetILUMaxIter(HYPRE_Solver solver,
                                        HYPRE_Int ilu_max_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetILUDroptol(HYPRE_Solver solver,
                                        HYPRE_Real ilu_droptol);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetRestriction(HYPRE_Solver solver,
                                        HYPRE_Int restr_par);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetIsTriangular(HYPRE_Solver solver,
                                         HYPRE_Int is_triangular);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetGMRESSwitchR(HYPRE_Solver solver,
                                         HYPRE_Int gmres_switch);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetADropTol(HYPRE_Solver solver,
                            HYPRE_Real A_drop_tol);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetADropType(HYPRE_Solver solver,
                             HYPRE_Int A_drop_type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BoomerAMGSetLogging(HYPRE_Solver solver,
                                    HYPRE_Int logging);

        #endregion

        #region ILU

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUCreate(out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetup(HYPRE_Solver solver,
                          HYPRE_ParCSRMatrix A,
                          HYPRE_ParVector b,
                          HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSolve(HYPRE_Solver solver,
                          HYPRE_ParCSRMatrix A,
                          HYPRE_ParVector b,
                          HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetMaxIter(HYPRE_Solver solver, HYPRE_Int max_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetTol(HYPRE_Solver solver, HYPRE_Real tol);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetLevelOfFill(HYPRE_Solver solver, HYPRE_Int lfil);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetMaxNnzPerRow(HYPRE_Solver solver, HYPRE_Int nzmax);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetDropThreshold(HYPRE_Solver solver, HYPRE_Real threshold);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetDropThresholdArray(HYPRE_Solver solver, HYPRE_Real[] threshold);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetNSHDropThreshold(HYPRE_Solver solver, HYPRE_Real threshold);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetNSHDropThresholdArray(HYPRE_Solver solver, HYPRE_Real[] threshold);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetSchurMaxIter(HYPRE_Solver solver, HYPRE_Int ss_max_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetType(HYPRE_Solver solver, HYPRE_Int ilu_type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetLocalReordering(HYPRE_Solver solver, HYPRE_Int reordering_type);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetPrintLevel(HYPRE_Solver solver, HYPRE_Int print_level);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUSetLogging(HYPRE_Solver solver, HYPRE_Int logging);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ILUGetNumIterations(HYPRE_Solver solver, out HYPRE_Int num_iterations);

        #endregion

        #region ParaSails

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsCreate(int comm, out HYPRE_Solver precond);

        /**
         * Set up the ParaSails preconditioner.  This function should be passed
         * to the iterative solver \e SetPrecond function.
         *
         * @param solver [IN] Preconditioner object to set up.
         * @param A [IN] ParCSR matrix used to construct the preconditioner.
         * @param b Ignored by this function.
         * @param x Ignored by this function.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsSetup(HYPRE_Solver solver,
                                       HYPRE_ParCSRMatrix A,
                                       HYPRE_ParVector b,
                                       HYPRE_ParVector x);

        /**
         * Apply the ParaSails preconditioner.  This function should be passed
         * to the iterative solver \e SetPrecond function.
         *
         * @param solver [IN] Preconditioner object to apply.
         * @param A Ignored by this function.
         * @param b [IN] Vector to precondition.
         * @param x [OUT] Preconditioned vector.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsSolve(HYPRE_Solver solver,
                                       HYPRE_ParCSRMatrix A,
                                       HYPRE_ParVector b,
                                       HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsSetParams(HYPRE_Solver precond, double sai_threshold, int sai_max_levels);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsSetFilter(HYPRE_Solver precond, double sai_filter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsSetSym(HYPRE_Solver precond, int sai_sym);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsSetLogging(HYPRE_Solver precond, int v);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParaSailsDestroy(HYPRE_Solver precond);

        #endregion

        #region PCG

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRPCGCreate(int comm, out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRPCGDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRPCGSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix parcsr_A,
            HYPRE_ParVector par_b,
            HYPRE_ParVector par_x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRPCGSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix parcsr_A,
            HYPRE_ParVector par_b,
            HYPRE_ParVector par_x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRPCGSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region GMRES

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRGMRESCreate(int comm,
                                          out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRGMRESDestroy(HYPRE_Solver solver);


        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRGMRESSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRGMRESSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRGMRESSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region COGMRES

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCOGMRESCreate(int comm,
                                         out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCOGMRESDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCOGMRESSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);


        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCOGMRESSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCOGMRESSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region FlexGMRES

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRFlexGMRESCreate(int comm, out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRFlexGMRESDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRFlexGMRESSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix parcsr_A,
            HYPRE_ParVector par_b,
            HYPRE_ParVector par_x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRFlexGMRESSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix parcsr_A,
            HYPRE_ParVector par_b,
            HYPRE_ParVector par_x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRFlexGMRESSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region LGMRES

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRLGMRESCreate(int comm,
                                         out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRLGMRESDestroy(HYPRE_Solver solver);


        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRLGMRESSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRLGMRESSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRLGMRESSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region BiCGSTAB

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRBiCGSTABCreate(int comm, out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRBiCGSTABDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRBiCGSTABSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRBiCGSTABSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRBiCGSTABSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region Hybrid

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRHybridCreate(out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRHybridDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRHybridSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRHybridSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRHybridSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #region CGNR

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCGNRCreate(int comm, out HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCGNRDestroy(HYPRE_Solver solver);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCGNRSetup(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCGNRSolve(HYPRE_Solver solver,
            HYPRE_ParCSRMatrix A,
            HYPRE_ParVector b,
            HYPRE_ParVector x);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_ParCSRCGNRSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precondT,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        #endregion

        #endregion

        #region HYPRE_krylov.h

        #region PCG Solver

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetup(HYPRE_Solver solver,
                                 HYPRE_Matrix A,
                                 HYPRE_Vector b,
                                 HYPRE_Vector x);

        /**
         * Solve the system.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSolve(HYPRE_Solver solver,
                                 HYPRE_Matrix A,
                                 HYPRE_Vector b,
                                 HYPRE_Vector x);

        /**
         * (Optional) Set the relative convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetTol(HYPRE_Solver solver,
                                  HYPRE_Real tol);

        /**
         * (Optional) Set the absolute convergence tolerance (default is
         * 0). If one desires the convergence test to check the absolute
         * convergence tolerance \e only, then set the relative convergence
         * tolerance to 0.0.  (The default convergence test is \f$ <C*r,r> \leq\f$
         * max(relative\f$\_\f$tolerance\f$^{2} \ast <C*b, b>\f$, absolute\f$\_\f$tolerance\f$^2\f$).)
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetAbsoluteTol(HYPRE_Solver solver,
                                          HYPRE_Real a_tol);

        /**
         * (Optional) Set a residual-based convergence tolerance which checks if
         * \f$\|r_{old}-r_{new}\| < rtol \|b\|\f$. This is useful when trying to converge to
         * very low relative and/or absolute tolerances, in order to bail-out before
         * roundoff errors affect the approximation.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetResidualTol(HYPRE_Solver solver,
                                          HYPRE_Real rtol);
        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetAbsoluteTolFactor(HYPRE_Solver solver,
                                                HYPRE_Real abstolf);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetConvergenceFactorTol(HYPRE_Solver solver,
                                                   HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetStopCrit(HYPRE_Solver solver,
                                       HYPRE_Int stop_crit);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetMaxIter(HYPRE_Solver solver,
                                      HYPRE_Int max_iter);

        /**
         * (Optional) Use the two-norm in stopping criteria.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetTwoNorm(HYPRE_Solver solver,
                                      HYPRE_Int two_norm);

        /**
         * (Optional) Additionally require that the relative difference in
         * successive iterates be small.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetRelChange(HYPRE_Solver solver,
                                        HYPRE_Int rel_change);

        /**
         * (Optional) Recompute the residual at the end to double-check convergence.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetRecomputeResidual(HYPRE_Solver solver,
                                                HYPRE_Int recompute_residual);

        /**
         * (Optional) Periodically recompute the residual while iterating.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetRecomputeResidualP(HYPRE_Solver solver,
                                                 HYPRE_Int recompute_residual_p);

        /**
         * (Optional) Set the preconditioner to use.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetPrecond(HYPRE_Solver solver,
            HYPRE_PtrToParSolverFcn precond,
            HYPRE_PtrToParSolverFcn precond_setup,
            HYPRE_Solver precond_solver);

        /**
        * (Optional) Set the amount of logging to do.
        **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetLogging(HYPRE_Solver solver,
                                      HYPRE_Int logging);

        /**
         * (Optional) Set the amount of printing to do to the screen.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGSetPrintLevel(HYPRE_Solver solver,
                                         HYPRE_Int level);

        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetNumIterations(HYPRE_Solver solver,
                                            out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                        out HYPRE_Real norm);

        /**
         * Return the residual.
         **/
        //public static extern HYPRE_Int HYPRE_PCGGetResidual(HYPRE_Solver solver,
        //                               void* residual);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetTol(HYPRE_Solver solver,
                                  out HYPRE_Real tol);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetResidualTol(HYPRE_Solver solver,
                                          out HYPRE_Real rtol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetAbsoluteTolFactor(HYPRE_Solver solver,
                                                out HYPRE_Real abstolf);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetConvergenceFactorTol(HYPRE_Solver solver,
                                                   out HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetStopCrit(HYPRE_Solver solver,
                                       out HYPRE_Int stop_crit);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetMaxIter(HYPRE_Solver solver,
                                      out HYPRE_Int max_iter);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetTwoNorm(HYPRE_Solver solver,
                                      out HYPRE_Int two_norm);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetRelChange(HYPRE_Solver solver,
                                        out HYPRE_Int rel_change);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetSkipRealResidualCheck(HYPRE_Solver solver,
                                                      out HYPRE_Int skip_real_r_check);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetPrecond(HYPRE_Solver solver,
                                      out HYPRE_Solver precond_data_ptr);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetLogging(HYPRE_Solver solver,
                                      out HYPRE_Int level);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetPrintLevel(HYPRE_Solver solver,
                                         out HYPRE_Int level);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_PCGGetConverged(HYPRE_Solver solver,
                                        out HYPRE_Int converged);

        #endregion

        #region GMRES Solver

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetup(HYPRE_Solver solver,
                                   HYPRE_Matrix A,
                                   HYPRE_Vector b,
                                   HYPRE_Vector x);

        /**
         * Solve the system.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSolve(HYPRE_Solver solver,
                                   HYPRE_Matrix A,
                                   HYPRE_Vector b,
                                   HYPRE_Vector x);

        /**
         * (Optional) Set the relative convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetTol(HYPRE_Solver solver,
                                    HYPRE_Real tol);

        /**
         * (Optional) Set the absolute convergence tolerance (default is 0).
         * If one desires
         * the convergence test to check the absolute convergence tolerance \e only, then
         * set the relative convergence tolerance to 0.0.  (The convergence test is
         * \f$\|r\| \leq\f$ max(relative\f$\_\f$tolerance\f$\ast \|b\|\f$, absolute\f$\_\f$tolerance).)
         *
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetAbsoluteTol(HYPRE_Solver solver,
                                            HYPRE_Real a_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetConvergenceFactorTol(HYPRE_Solver solver,
                                                     HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetStopCrit(HYPRE_Solver solver,
                                         HYPRE_Int stop_crit);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetMinIter(HYPRE_Solver solver,
                                        HYPRE_Int min_iter);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetMaxIter(HYPRE_Solver solver,
                                        HYPRE_Int max_iter);

        /**
         * (Optional) Set the maximum size of the Krylov space.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetKDim(HYPRE_Solver solver,
                                     HYPRE_Int k_dim);

        /**
         * (Optional) Additionally require that the relative difference in
         * successive iterates be small.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetRelChange(HYPRE_Solver solver,
                                          HYPRE_Int rel_change);

        /**
         * (Optional) By default, hypre checks for convergence by evaluating the actual
         * residual before returnig from GMRES (with restart if the true residual does
         * not indicate convergence). This option allows users to skip the evaluation
         * and the check of the actual residual for badly conditioned problems where
         * restart is not expected to be beneficial.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetSkipRealResidualCheck(HYPRE_Solver solver,
                                                      HYPRE_Int skip_real_r_check);

        /**
         * (Optional) Set the preconditioner to use.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetPrecond(HYPRE_Solver solver,
                                        HYPRE_PtrToParSolverFcn precond,
                                        HYPRE_PtrToParSolverFcn precond_setup,
                                        HYPRE_Solver precond_solver);

        /**
         * (Optional) Set the amount of logging to do.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetLogging(HYPRE_Solver solver,
                                        HYPRE_Int logging);

        /**
         * (Optional) Set the amount of printing to do to the screen.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESSetPrintLevel(HYPRE_Solver solver,
                                           HYPRE_Int level);

        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetNumIterations(HYPRE_Solver solver,
                                              out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                          out HYPRE_Real norm);

        /**
         * Return the residual.
         **/
        //public static extern HYPRE_Int HYPRE_GMRESGetResidual(HYPRE_Solver solver,
        //                                 void* residual);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetTol(HYPRE_Solver solver,
                                    out HYPRE_Real tol);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetAbsoluteTol(HYPRE_Solver solver,
                                            out HYPRE_Real tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetConvergenceFactorTol(HYPRE_Solver solver,
                                                     out HYPRE_Real cf_tol);

        /*
         * OBSOLETE
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetStopCrit(HYPRE_Solver solver,
                                         out HYPRE_Int stop_crit);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetMinIter(HYPRE_Solver solver,
                                        out HYPRE_Int min_iter);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetMaxIter(HYPRE_Solver solver,
                                        out HYPRE_Int max_iter);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetKDim(HYPRE_Solver solver,
                                     out HYPRE_Int k_dim);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetRelChange(HYPRE_Solver solver,
                                          out HYPRE_Int rel_change);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetPrecond(HYPRE_Solver solver,
                                        out HYPRE_Solver precond_data_ptr);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetLogging(HYPRE_Solver solver,
                                        out HYPRE_Int level);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetPrintLevel(HYPRE_Solver solver,
                                           out HYPRE_Int level);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_GMRESGetConverged(HYPRE_Solver solver,
                                          out HYPRE_Int converged);

        #endregion

        #region FlexGMRES Solver

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetup(HYPRE_Solver solver,
                                       HYPRE_Matrix A,
                                       HYPRE_Vector b,
                                       HYPRE_Vector x);

        /**
         * Solve the system.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSolve(HYPRE_Solver solver,
                                       HYPRE_Matrix A,
                                       HYPRE_Vector b,
                                       HYPRE_Vector x);

        /**
         * (Optional) Set the convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetTol(HYPRE_Solver solver,
                                        HYPRE_Real tol);

        /**
         * (Optional) Set the absolute convergence tolerance (default is 0).
         * If one desires
         * the convergence test to check the absolute convergence tolerance \e only, then
         * set the relative convergence tolerance to 0.0.  (The convergence test is
         * \f$\|r\| \leq\f$ max(relative\f$\_\f$tolerance\f$\ast \|b\|\f$, absolute\f$\_\f$tolerance).)
         *
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetAbsoluteTol(HYPRE_Solver solver,
                                                HYPRE_Real a_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetConvergenceFactorTol(HYPRE_Solver solver, HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetMinIter(HYPRE_Solver solver, HYPRE_Int min_iter);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetMaxIter(HYPRE_Solver solver,
                                            HYPRE_Int max_iter);

        /**
         * (Optional) Set the maximum size of the Krylov space.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetKDim(HYPRE_Solver solver,
                                         HYPRE_Int k_dim);

        /**
         * (Optional) Set the preconditioner to use.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetPrecond(HYPRE_Solver solver,
                                            HYPRE_PtrToParSolverFcn precond,
                                            HYPRE_PtrToParSolverFcn precond_setup,
                                            HYPRE_Solver precond_solver);

        /**
         * (Optional) Set the amount of logging to do.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetLogging(HYPRE_Solver solver,
                                            HYPRE_Int logging);

        /**
         * (Optional) Set the amount of printing to do to the screen.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetPrintLevel(HYPRE_Solver solver,
                                               HYPRE_Int level);

        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetNumIterations(HYPRE_Solver solver,
                                                  out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                              out HYPRE_Real norm);

        /**
         * Return the residual.
         **/
        //public static extern HYPRE_Int HYPRE_FlexGMRESGetResidual(HYPRE_Solver solver,
        //                                     void* residual);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetTol(HYPRE_Solver solver,
                                        out HYPRE_Real tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetConvergenceFactorTol(HYPRE_Solver solver,
                                                         out HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetStopCrit(HYPRE_Solver solver,
                                             out HYPRE_Int stop_crit);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetMinIter(HYPRE_Solver solver,
                                            out HYPRE_Int min_iter);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetMaxIter(HYPRE_Solver solver,
                                            out HYPRE_Int max_iter);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetKDim(HYPRE_Solver solver,
                                         out HYPRE_Int k_dim);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetPrecond(HYPRE_Solver solver,
                                            out HYPRE_Solver precond_data_ptr);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetLogging(HYPRE_Solver solver,
                                            out HYPRE_Int level);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetPrintLevel(HYPRE_Solver solver,
                                               out HYPRE_Int level);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESGetConverged(HYPRE_Solver solver,
                                              out HYPRE_Int converged);

        /**
         * (Optional) Set a user-defined function to modify solve-time preconditioner
         * attributes.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_FlexGMRESSetModifyPC(HYPRE_Solver solver,
                                             HYPRE_PtrToModifyPCFcn modify_pc);

        #endregion

        #region LGMRES Solver

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetup(HYPRE_Solver solver,
                                    HYPRE_Matrix A,
                                    HYPRE_Vector b,
                                    HYPRE_Vector x);

        /**
         * Solve the system. Details on LGMRES may be found in A. H. Baker,
         * E.R. Jessup, and T.A. Manteuffel, "A technique for accelerating the
         * convergence of restarted GMRES." SIAM Journal on Matrix Analysis and
         * Applications, 26 (2005), pp. 962-984. LGMRES(m,k) in the paper
         * corresponds to LGMRES(Kdim+AugDim, AugDim).
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSolve(HYPRE_Solver solver,
                                    HYPRE_Matrix A,
                                    HYPRE_Vector b,
                                    HYPRE_Vector x);

        /**
         * (Optional) Set the convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetTol(HYPRE_Solver solver,
                                     HYPRE_Real tol);
        /**
         * (Optional) Set the absolute convergence tolerance (default is 0).
         * If one desires
         * the convergence test to check the absolute convergence tolerance \e only, then
         * set the relative convergence tolerance to 0.0.  (The convergence test is
         * \f$\|r\| \leq\f$ max(relative\f$\_\f$tolerance\f$\ast \|b\|\f$, absolute\f$\_\f$tolerance).)
         *
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetAbsoluteTol(HYPRE_Solver solver,
                                             HYPRE_Real a_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetConvergenceFactorTol(HYPRE_Solver solver,
                                                      HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetMinIter(HYPRE_Solver solver,
                                         HYPRE_Int min_iter);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetMaxIter(HYPRE_Solver solver,
                               HYPRE_Int max_iter);

        /**
         * (Optional) Set the maximum size of the approximation space
         * (includes the augmentation vectors).
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetKDim(HYPRE_Solver solver,
                                      HYPRE_Int k_dim);

        /**
         * (Optional) Set the number of augmentation vectors  (default: 2).
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetAugDim(HYPRE_Solver solver,
                                        HYPRE_Int aug_dim);

        /**
         * (Optional) Set the preconditioner to use.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetPrecond(HYPRE_Solver solver,
                               HYPRE_PtrToParSolverFcn precond,
                               HYPRE_PtrToParSolverFcn precond_setup,
                               HYPRE_Solver precond_solver);

        /**
         * (Optional) Set the amount of logging to do.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetLogging(HYPRE_Solver solver,
                                         HYPRE_Int logging);

        /**
         * (Optional) Set the amount of printing to do to the screen.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESSetPrintLevel(HYPRE_Solver solver,
                                            HYPRE_Int level);

        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetNumIterations(HYPRE_Solver solver,
                                               out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                           out HYPRE_Real norm);

        /**
         * Return the residual.
         **/
        //public static extern HYPRE_Int HYPRE_LGMRESGetResidual(HYPRE_Solver solver,
        //                                  void* residual);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetTol(HYPRE_Solver solver,
                                     out HYPRE_Real tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetConvergenceFactorTol(HYPRE_Solver solver,
                                                      out HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetStopCrit(HYPRE_Solver solver,
                                          out HYPRE_Int stop_crit);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetMinIter(HYPRE_Solver solver,
                                         out HYPRE_Int min_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetMaxIter(HYPRE_Solver solver,
                                         out HYPRE_Int max_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetKDim(HYPRE_Solver solver,
                                      out HYPRE_Int k_dim);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetAugDim(HYPRE_Solver solver,
                                        out HYPRE_Int k_dim);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetPrecond(HYPRE_Solver solver,
                                         out HYPRE_Solver precond_data_ptr);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetLogging(HYPRE_Solver solver,
                                         out HYPRE_Int level);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetPrintLevel(HYPRE_Solver solver,
                                            out HYPRE_Int level);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_LGMRESGetConverged(HYPRE_Solver solver,
                                           out HYPRE_Int converged);

        #endregion

        #region COGMRES Solver

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetup(HYPRE_Solver solver,
                                     HYPRE_Matrix A,
                                     HYPRE_Vector b,
                                     HYPRE_Vector x);

        /**
         * Solve the system.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSolve(HYPRE_Solver solver,
                                     HYPRE_Matrix A,
                                     HYPRE_Vector b,
                                     HYPRE_Vector x);

        /**
         * (Optional) Set the convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetTol(HYPRE_Solver solver,
                                      HYPRE_Real tol);

        /**
         * (Optional) Set the absolute convergence tolerance (default is 0).
         * If one desires
         * the convergence test to check the absolute convergence tolerance \e only, then
         * set the relative convergence tolerance to 0.0.  (The convergence test is
         * \f$\|r\| \leq\f$ max(relative\f$\_\f$tolerance\f$\ast \|b\|\f$, absolute\f$\_\f$tolerance).)
         *
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetAbsoluteTol(HYPRE_Solver solver,
                                              HYPRE_Real a_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetConvergenceFactorTol(HYPRE_Solver solver,
                                                       HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetMinIter(HYPRE_Solver solver,
                                          HYPRE_Int min_iter);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetMaxIter(HYPRE_Solver solver,
                                          HYPRE_Int max_iter);

        /**
         * (Optional) Set the maximum size of the Krylov space.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetKDim(HYPRE_Solver solver,
                                       HYPRE_Int k_dim);

        /**
         * (Optional) Set number of unrolling in mass funcyions in COGMRES
         * Can be 4 or 8. Default: no unrolling.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetUnroll(HYPRE_Solver solver,
                                         HYPRE_Int unroll);

        /**
         * (Optional) Set the number of orthogonalizations in COGMRES (at most 2).
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetCGS(HYPRE_Solver solver,
                                      HYPRE_Int cgs);

        /**
         * (Optional) Set the preconditioner to use.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetPrecond(HYPRE_Solver solver,
                                          HYPRE_PtrToParSolverFcn precond,
                                          HYPRE_PtrToParSolverFcn precond_setup,
                                          HYPRE_Solver precond_solver);

        /**
         * (Optional) Set the amount of logging to do.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetLogging(HYPRE_Solver solver,
                                          HYPRE_Int logging);

        /**
         * (Optional) Set the amount of printing to do to the screen.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetPrintLevel(HYPRE_Solver solver,
                                             HYPRE_Int level);

        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetNumIterations(HYPRE_Solver solver,
                                                out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                            out HYPRE_Real norm);

        /**
         * Return the residual.
         **/
        //public static extern HYPRE_Int HYPRE_COGMRESGetResidual(HYPRE_Solver solver,
        //                                   void* residual);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetTol(HYPRE_Solver solver,
                                      out HYPRE_Real tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetConvergenceFactorTol(HYPRE_Solver solver,
                                                       out HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetMinIter(HYPRE_Solver solver,
                                          out HYPRE_Int min_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetMaxIter(HYPRE_Solver solver,
                                          out HYPRE_Int max_iter);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetKDim(HYPRE_Solver solver,
                                       out HYPRE_Int k_dim);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetUnroll(HYPRE_Solver solver,
                                         out HYPRE_Int unroll);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetCGS(HYPRE_Solver solver,
                                      out HYPRE_Int cgs);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetPrecond(HYPRE_Solver solver,
                                          out HYPRE_Solver precond_data_ptr);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetLogging(HYPRE_Solver solver,
                                          out HYPRE_Int level);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetPrintLevel(HYPRE_Solver solver,
                                             out HYPRE_Int level);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESGetConverged(HYPRE_Solver solver,
                                            out HYPRE_Int converged);

        /**
         * (Optional) Set a user-defined function to modify solve-time preconditioner
         * attributes.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_COGMRESSetModifyPC(HYPRE_Solver solver,
                                           HYPRE_PtrToModifyPCFcn modify_pc);

        #endregion

        #region BiCGSTAB Solver

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABDestroy(HYPRE_Solver solver);

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetup(HYPRE_Solver solver,
                                      HYPRE_Matrix A,
                                      HYPRE_Vector b,
                                      HYPRE_Vector x);

        /**
         * Solve the system.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSolve(HYPRE_Solver solver,
                                      HYPRE_Matrix A,
                                      HYPRE_Vector b,
                                      HYPRE_Vector x);

        /**
         * (Optional) Set the convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetTol(HYPRE_Solver solver,
                                       HYPRE_Real tol);

        /**
         * (Optional) Set the absolute convergence tolerance (default is 0).
         * If one desires
         * the convergence test to check the absolute convergence tolerance \e only, then
         * set the relative convergence tolerance to 0.0.  (The convergence test is
         * \f$\|r\| \leq\f$ max(relative\f$\_\f$tolerance \f$\ast \|b\|\f$, absolute\f$\_\f$tolerance).)
         *
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetAbsoluteTol(HYPRE_Solver solver,
                                               HYPRE_Real a_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetConvergenceFactorTol(HYPRE_Solver solver,
                                                        HYPRE_Real cf_tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetStopCrit(HYPRE_Solver solver,
                                            HYPRE_Int stop_crit);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetMinIter(HYPRE_Solver solver,
                                           HYPRE_Int min_iter);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetMaxIter(HYPRE_Solver solver,
                                           HYPRE_Int max_iter);

        /**
         * (Optional) Set the preconditioner to use.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetPrecond(HYPRE_Solver solver,
                                           HYPRE_PtrToParSolverFcn precond,
                                           HYPRE_PtrToParSolverFcn precond_setup,
                                           HYPRE_Solver precond_solver);

        /**
         * (Optional) Set the amount of logging to do.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetLogging(HYPRE_Solver solver,
                                           HYPRE_Int logging);

        /**
         * (Optional) Set the amount of printing to do to the screen.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABSetPrintLevel(HYPRE_Solver solver,
                                              HYPRE_Int level);

        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABGetNumIterations(HYPRE_Solver solver,
                                                 out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                             out HYPRE_Real norm);

        /**
         * Return the residual.
         **/
        //public static extern HYPRE_Int HYPRE_BiCGSTABGetResidual(HYPRE_Solver solver,
        //                                    void* residual);

        /**
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_BiCGSTABGetPrecond(HYPRE_Solver solver,
                                           out HYPRE_Solver precond_data_ptr);

        #endregion

        #region CGNR Solver

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRDestroy(HYPRE_Solver solver);

        /**
         * Prepare to solve the system.  The coefficient data in \e b and \e x is
         * ignored here, but information about the layout of the data may be used.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetup(HYPRE_Solver solver,
                                  HYPRE_Matrix A,
                                  HYPRE_Vector b,
                                  HYPRE_Vector x);

        /**
         * Solve the system.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSolve(HYPRE_Solver solver,
                                  HYPRE_Matrix A,
                                  HYPRE_Vector b,
                                  HYPRE_Vector x);

        /**
         * (Optional) Set the convergence tolerance.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetTol(HYPRE_Solver solver,
                                   HYPRE_Real tol);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetStopCrit(HYPRE_Solver solver,
                                        HYPRE_Int stop_crit);

        /*
         * RE-VISIT
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetMinIter(HYPRE_Solver solver,
                                       HYPRE_Int min_iter);

        /**
         * (Optional) Set maximum number of iterations.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetMaxIter(HYPRE_Solver solver,
                                       HYPRE_Int max_iter);

        /**
         * (Optional) Set the preconditioner to use.
         * Note that the only preconditioner available in hypre for use with
         * CGNR is currently BoomerAMG. It requires to use Jacobi as
         * a smoother without CF smoothing, i.e. relax_type needs to be set to 0
         * or 7 and relax_order needs to be set to 0 by the user, since these
         * are not default values. It can be used with a relaxation weight for
         * Jacobi, which can significantly improve convergence.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetPrecond(HYPRE_Solver solver,
                                       HYPRE_PtrToParSolverFcn precond,
                                       HYPRE_PtrToParSolverFcn precondT,
                                       HYPRE_PtrToParSolverFcn precond_setup,
                                       HYPRE_Solver precond_solver);

        /**
         * (Optional) Set the amount of logging to do.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRSetLogging(HYPRE_Solver solver,
                                       HYPRE_Int logging);


        /**
         * Return the number of iterations taken.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRGetNumIterations(HYPRE_Solver solver,
                                             out HYPRE_Int num_iterations);

        /**
         * Return the norm of the final relative residual.
         **/
        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRGetFinalRelativeResidualNorm(HYPRE_Solver solver,
                                                         out HYPRE_Real norm);

        [DllImport(HYPRE_DLL, ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern HYPRE_Int HYPRE_CGNRGetPrecond(HYPRE_Solver solver,
                                       out HYPRE_Solver precond_data_ptr);

        #endregion

        #endregion
    }
}
