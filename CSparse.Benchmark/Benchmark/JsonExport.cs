
namespace CSparse.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public class JsonExport : IBenchmarkResultWriter
    {
        string name;
        string info;

        Dictionary<string, List<BenchmarkResult>> sections;
        List<Tuple<string, double>> totals;

        public JsonExport(string name, string info)
        {
            this.name = name;
            this.info = info;

            sections = new Dictionary<string, List<BenchmarkResult>>();
            totals = new List<Tuple<string, double>>();
        }

        public void Add(string name, List<BenchmarkResult> results)
        {
            sections[name] = results;
        }

        public void Save(string file)
        {
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("{");

                writer.WriteLine("\"name\": \"{0}\",", name);
                writer.WriteLine("\"info\": \"{0}\",", info);
                
                foreach (var section in sections)
                {
                    WriteSection(section.Key, section.Value, writer);
                }

                WriteTotal(writer);

                writer.WriteLine("}");
            }
        }

        private void WriteTotal(StreamWriter writer)
        {
            writer.WriteLine("\"total\": [");

            int count = totals.Count;

            foreach (var tuple in totals)
            {
                writer.WriteLine("    {{ \"{0}\": {1} }}{2}",
                    tuple.Item1, tuple.Item2.ToString("0.000", CultureInfo.InvariantCulture),
                    (--count == 0) ? "" : ",");
            }

            writer.WriteLine("]");
        }

        private void WriteSection(string name, List<BenchmarkResult> results, StreamWriter writer)
        {
            double total = 0.0;

            writer.WriteLine("\"{0}\": [", name);

            int count = results.Count;

            foreach (var result in results)
            {
                WriteResult(result, writer, (--count == 0));

                total += result.Time;
            }

            writer.WriteLine("],");

            totals.Add(new Tuple<string, double>(name, total));
        }

        private void WriteResult(BenchmarkResult result, StreamWriter writer, bool last)
        {
            string name = Path.GetFileName(result.File.Path);

            if (result.Exception != null)
            {
                writer.WriteLine("    {{ \"file\": \"{0}\", \"exception\": \"{1}\" }},", name, result.Exception);

                return;
            }

            writer.WriteLine("    {{ \"file\": \"{0}\", \"size\": {1}, \"values\": {2}, \"time\": {3}, \"error\": {4} }}{5}",
                name, result.RowCount, result.NonZerosCount,
                result.Time.ToString("0.000", CultureInfo.InvariantCulture),
                result.Residual.ToString("0.00000e00", CultureInfo.InvariantCulture),
                last ? "" : ",");
        }
    }
}