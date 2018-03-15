
namespace CSparse.Benchmark
{
    using System.Collections.Generic;

    public interface IBenchmarkResultWriter
    {
        void Add(string section, List<BenchmarkResult> results);

        void Save(string file);
    }
}