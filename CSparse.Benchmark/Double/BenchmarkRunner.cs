
namespace CSparse.Double
{
    using CSparse.Benchmark;
    using CSparse.Double.Benchmarks;
    using System;
    using System.IO;

    public class BenchmarkRunner
    {
        public static void Run(string dataDirectory, string benchmarkFile, bool preload = true)
        {
            if (!File.Exists(benchmarkFile))
            {
                Console.WriteLine("File not found: '{0}'", benchmarkFile);
                return;
            }

            if (!Directory.Exists(dataDirectory))
            {
                Console.WriteLine("Directory not found: '{0}'", dataDirectory);
                return;
            }

            var collection = new MatrixFileCollection(benchmarkFile, dataDirectory);

            // Preloading will speed up the benchmark, but consume more memory.
            if (preload)
            {
                Console.WriteLine("Loading matrix collection ...");
                collection.Preload<double>();
            }

            Console.WriteLine("Starting UMFPACK benchmark ...");
            RunUmfpack(collection);

            Console.WriteLine("Starting CHOLMOD benchmark ...");
            RunCholmod(collection);

            Console.WriteLine("Starting SuperLU benchmark ...");
            RunSuperLU(collection);

            Console.WriteLine("Starting PARDISO benchmark ...");
            RunPardiso(collection);
        }

        private static void RunUmfpack(MatrixFileCollection collection)
        {
            var benchmark = new BenchmarkUmfpack(collection);

            var export = new JsonExport("UMFPACK", DateTime.UtcNow.ToString("s"));

            benchmark.Run(export);

            export.Save("benchmark-umfpack.json");
        }

        private static void RunCholmod(MatrixFileCollection collection)
        {
            var benchmark = new BenchmarkCholmod(collection);

            var export = new JsonExport("CHOLMOD", DateTime.UtcNow.ToString("s"));

            benchmark.Run(export);

            export.Save("benchmark-cholmod.json");
        }

        private static void RunSuperLU(MatrixFileCollection collection)
        {
            var benchmark = new BenchmarkSuperLU(collection);

            var export = new JsonExport("SuperLU", DateTime.UtcNow.ToString("s"));

            benchmark.Run(export);

            export.Save("benchmark-superlu.json");
        }

        private static void RunPardiso(MatrixFileCollection collection)
        {
            var benchmark = new BenchmarkPardiso(collection);

            var export = new JsonExport("PARDISO", DateTime.UtcNow.ToString("s"));

            benchmark.Run(export);

            export.Save("benchmark-pardiso.json");
        }
    }
}
