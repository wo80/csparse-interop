namespace CSparse.Interop.Tests.AMD
{
    using CSparse.Interop.SuiteSparse.Ordering;

    class TestCOLAMD
    {
        public bool TestCOLAMD1()
        {
            GetData_COLAMD(out int a_rows, out int a_cols, out int[] ap, out int[] ai, out int b_n, out int[] bp, out int[] bi);

            var colamd = new COLAMD();

            var info = colamd.Order(a_rows, a_cols, ai, ap);

            var p = ap;

            return info.Status >= 0 && Permutation.IsValid(p, a_cols);

            /*
            var symamd = new SYMAMD();

            p = new int[b_n + 1];

            info = symamd.Order(b_n, bi, bp, p);

            return info.Status >= 0 && Permutation.IsValid(p);
            //*/
        }

        public bool TestCCOLAMD1()
        {
            GetData_COLAMD(out int a_rows, out int a_cols, out int[] ap, out int[] ai, out int b_n, out int[] bp, out int[] bi);

            var colamd = new COLAMD();

            var info = colamd.Order(a_rows, a_cols, ai, ap, null);

            var p = ap;

            return info.Status >= 0 && Permutation.IsValid(p, a_cols);
        }

        #region Test data

        public static void GetData_COLAMD(out int a_rows, out int a_cols, out int[] ap, out int[] ai, out int b_n, out int[] bp, out int[] bi)
        {
            a_rows = 5;
            a_cols = 4;

            ap = new int[] { 0, 3, 5, 9, 11 };
            ai = new int[] {
            /* col  0 */  0, 1, 4,
            /* col  1 */  2, 4,
            /* col  2 */  0, 1, 2, 3,
            /* col  3 */  1, 3
            };

            b_n = 5;

            bp = new int[] { 0, 1, 3, 3, 4, 4 };
            bi = new int[] {
        	/* col  0 */  1,
        	/* col  1 */  2, 3,
        	/* col  2 */
        	/* col  3 */  4
        	};

            // Note: only strictly lower triangular part  is included, since symamd
            // ignores the diagonal and upper triangular part of B.
        }

        #endregion
    }
}
