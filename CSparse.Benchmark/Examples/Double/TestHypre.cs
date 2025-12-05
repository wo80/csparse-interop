
namespace CSparse.Double.Examples
{
    using System;
    using CSparse.Interop.Hypre;

    static class TestHypre
    {
        public static void Run()
        {
            NativeMethods.HYPRE_Init();

            int n = 33;
            var h = 1.0 / (n + 1);

            var A = CSparse.Double.Generate.Laplacian(n, n);
            var b = Vector.Create(A.RowCount, h * h);

            using (var hypreA = new HypreMatrix<double>(A, true))
            {
                Check(TestAMG(hypreA, b), "AMG");
                Check(TestPCG(hypreA, b), "PCG");
                Check(TestPCG_AMG(hypreA, b), "PCG + AMG");
                Check(TestPCG_ParaSails(hypreA, b), "PCG + ParaSails");
                Check(TestFlexGMRES_AMG(hypreA, b), "FlexGMRES + AMG");
            }

            NativeMethods.HYPRE_Finalize();
        }

        private static void Check(HypreResult result, string name)
        {
            Console.WriteLine(name);
            Console.WriteLine("  Iterations = {0}", result.NumIterations);
            Console.WriteLine("  Final Relative Residual Norm = {0}", result.RelResidualNorm);
            Console.WriteLine();
        }

        private static HypreResult TestAMG(HypreMatrix<double> A, double[] _b)
        {
            var _x = Vector.Create(A.RowCount, 0.0);

            using var x = new HypreVector<double>(_x);
            using var b = new HypreVector<double>(_b);

            using var solver = new BoomerAMG<double>();

            solver.SetOldDefault(); // Falgout coarsening with modified classical interpolation
            solver.SetRelaxType(3); // G-S/Jacobi hybrid relaxation
            solver.SetRelaxOrder(1); // uses C/F relaxation
            solver.SetNumSweeps(1);  // Sweeps on each level
            solver.SetMaxLevels(20);
            solver.SetTol(1e-7);
            //solver.PrintLevel = 3;

            return solver.Solve(A, x, b);
        }

        private static HypreResult TestPCG(HypreMatrix<double> A, double[] _b)
        {
            var _x = Vector.Create(A.RowCount, 0.0);

            using var x = new HypreVector<double>(_x);
            using var b = new HypreVector<double>(_b);

            using var solver = new PCG<double>();

            solver.SetMaxIter(1000);
            solver.SetTol(1e-7);
            solver.SetTwoNorm(1); // use the two norm as the stopping criteria
            //solver.PrintLevel = 2;
            //solver.Logging = 1;

            return solver.Solve(A, x, b);
        }

        private static HypreResult TestPCG_AMG(HypreMatrix<double> A, double[] _b)
        {
            var _x = Vector.Create(A.RowCount, 0.0);

            using var x = new HypreVector<double>(_x);
            using var b = new HypreVector<double>(_b);

            using var precond = new BoomerAMG<double>();

            //precond.PrintLevel = 1;
            precond.SetCoarsenType(6);
            precond.SetOldDefault();
            precond.SetRelaxType(6); // Sym G.S./Jacobi hybrid
            precond.SetNumSweeps(1);
            precond.SetTol(0.0);
            precond.SetMaxIter(1); // do only one iteration!

            using var solver = new PCG<double>(precond);

            solver.SetMaxIter(1000);
            solver.SetTol(1e-7);
            solver.SetTwoNorm(1); // use the two norm as the stopping criteria
            //solver.PrintLevel = 2;
            //solver.Logging = 1;

            return solver.Solve(A, x, b);
        }

        private static HypreResult TestPCG_ParaSails(HypreMatrix<double> A, double[] _b)
        {
            var _x = Vector.Create(A.RowCount, 0.0);

            using var x = new HypreVector<double>(_x);
            using var b = new HypreVector<double>(_b);

            using var precond = new ParaSails<double>(true);

            precond.SetParams(0.1, 1);
            precond.SetFilter(0.05);
            //precond.Logging = 3;

            using var solver = new PCG<double>(precond);

            solver.SetMaxIter(1000);
            solver.SetTol(1e-7);
            solver.SetTwoNorm(1); // use the two norm as the stopping criteria
            //solver.PrintLevel = 2;
            //solver.Logging = 1;

            return solver.Solve(A, x, b);
        }

        private static HypreResult TestFlexGMRES_AMG(HypreMatrix<double> A, double[] _b)
        {
            var _x = Vector.Create(A.RowCount, 0.0);

            using var x = new HypreVector<double>(_x);
            using var b = new HypreVector<double>(_b);

            using var precond = new BoomerAMG<double>();

            //precond.PrintLevel = 1;
            precond.SetCoarsenType(6);
            precond.SetOldDefault();
            precond.SetRelaxType(6); // Sym G.S./Jacobi hybrid
            precond.SetNumSweeps(1);
            precond.SetTol(0.0); // conv. tolerance zero
            precond.SetMaxIter(1); // do only one iteration!

            using var solver = new FlexGMRES<double>(precond);

            solver.SetKDim(30);
            solver.SetMaxIter(1000);
            solver.SetTol(1e-7);
            //solver.PrintLevel = 2;
            //solver.Logging = 1;

            return solver.Solve(A, x, b);
        }
    }
}