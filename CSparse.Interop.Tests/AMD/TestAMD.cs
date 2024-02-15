namespace CSparse.Interop.Tests.AMD
{
    using CSparse.Interop.SuiteSparse.Ordering;

    class TestAMD
    {
        public bool TestAMD1()
        {
            GetData_AMD(out int n, out int[] ap, out int[] ai);

            var p = new int[n];

            var amd = new AMD();

            var info = amd.Order(n, ap, ai, p);

            return info.Status >= 0 && Permutation.IsValid(p, n);
        }

        public bool TestAMD2()
        {
            GetData_AMD2(out int n, out int[] ap, out int[] ai);

            var p = new int[n];

            var amd = new AMD();

            var info = amd.Order(n, ap, ai, p);

            return info.Status >= 0 && Permutation.IsValid(p, n);
        }

        public bool TestCAMD1()
        {
            GetData_CAMD(out int n, out int[] ap, out int[] ai, out int[] c);

            var p = new int[n];

            var amd = new AMD();

            var info = amd.Order(n, ap, ai, p, c);

            return info.Status >= 0 && Permutation.IsValid(p, n);
        }

        public bool TestCAMD2()
        {
            GetData_CAMD2(out int n, out int[] ap, out int[] ai, out int[] c);

            var p = new int[n];

            var amd = new AMD();

            var info = amd.Order(n, ap, ai, p, c);

            return info.Status >= 0 && Permutation.IsValid(p, n);
        }

        #region Test data

        static void GetData_AMD(out int n, out int[] ap, out int[] ai)
        {
            n = 24;

            ap = new[] { 0, 9, 15, 21, 27, 33, 39, 48, 57, 61, 70, 76, 82, 88, 94, 100, 106, 110, 119, 128, 137, 143, 152, 156, 160 };
            ai = new[] {
            /* col  0 */  0, 5, 6, 12, 13, 17, 18, 19, 21,
            /* col  1 */  1, 8, 9, 13, 14, 17,
            /* col  2 */  2, 6, 11, 20, 21, 22,
            /* col  3 */  3, 7, 10, 15, 18, 19,
            /* col  4 */  4, 7, 9, 14, 15, 16,
            /* col  5 */  0, 5, 6, 12, 13, 17,
            /* col  6 */  0, 2, 5, 6, 11, 12, 19, 21, 23,
            /* col  7 */  3, 4, 7, 9, 14, 15, 16, 17, 18,
            /* col  8 */  1, 8, 9, 14,
            /* col  9 */  1, 4, 7, 8, 9, 13, 14, 17, 18,
            /* col 10 */  3, 10, 18, 19, 20, 21,
            /* col 11 */  2, 6, 11, 12, 21, 23,
            /* col 12 */  0, 5, 6, 11, 12, 23,
            /* col 13 */  0, 1, 5, 9, 13, 17,
            /* col 14 */  1, 4, 7, 8, 9, 14,
            /* col 15 */  3, 4, 7, 15, 16, 18,
            /* col 16 */  4, 7, 15, 16,
            /* col 17 */  0, 1, 5, 7, 9, 13, 17, 18, 19,
            /* col 18 */  0, 3, 7, 9, 10, 15, 17, 18, 19,
            /* col 19 */  0, 3, 6, 10, 17, 18, 19, 20, 21,
            /* col 20 */  2, 10, 19, 20, 21, 22,
            /* col 21 */  0, 2, 6, 10, 11, 19, 20, 21, 22,
            /* col 22 */  2, 20, 21, 22,
            /* col 23 */  6, 11, 12, 23 };
        }

        static void GetData_CAMD(out int n, out int[] ap, out int[] ai, out int[] c)
        {
            GetData_AMD(out n, out ap, out ai);

            c = new[] { 0, 0, 4, 0, 1, 0, 2, 2, 1, 1, 3, 4, 5, 5, 3, 4, 5, 2, 5, 3, 4, 2, 1, 0 };
        }

        static void GetData_AMD2(out int n, out int[] ap, out int[] ai)
        {
            n = 24;

            ap = new[] { 0, 9, 14, 20, 28, 33, 37, 44, 53, 58, 63, 63, 66, 69, 72, 75, 78, 82, 86, 91, 97, 101, 112, 112, 116 };
            ai = new[] {
            /* col  0 */  0, 17, 18, 21, 5, 12, 5, 0, 13,
            /* col  1 */  14, 1, 8, 13, 17,
            /* col  2 */  2, 20, 11, 6, 11, 22,
            /* col  3 */  3, 3, 10, 7, 18, 18, 15, 19,
            /* col  4 */  7, 9, 15, 14, 16,
            /* col  5 */  5, 13, 6, 17,
            /* col  6 */  5, 0, 11, 6, 12, 6, 23,
            /* col  7 */  3, 4, 9, 7, 14, 16, 15, 17, 18,
            /* col  8 */  1, 9, 14, 14, 14,
            /* col  9 */  7, 13, 8, 1, 17,
            /* col 10 */
            /* col 11 */  2, 12, 23,
            /* col 12 */  5, 11, 12,
            /* col 13 */  0, 13, 17,
            /* col 14 */  1, 9, 14,
            /* col 15 */  3, 15, 16,
            /* col 16 */  16, 4, 4, 15,
            /* col 17 */  13, 17, 19, 17,
            /* col 18 */  15, 17, 19, 9, 10,
            /* col 19 */  17, 19, 20, 0, 6, 10,
            /* col 20 */  22, 10, 20, 21,
            /* col 21 */  6, 2, 10, 19, 20, 11, 21, 22, 22, 22, 22,
            /* col 22 */
            /* col 23 */  12, 11, 12, 23 };
        }

        static void GetData_CAMD2(out int n, out int[] ap, out int[] ai, out int[] c)
        {
            GetData_AMD2(out n, out ap, out ai);

            c = new[] { 3, 0, 4, 0, 1, 1, 2, 2, 2, 2, 3, 4, 5, 5, 3, 4, 5, 2, 8, 10, 4, 2, 2, 0 };
        }

        #endregion
    }
}
