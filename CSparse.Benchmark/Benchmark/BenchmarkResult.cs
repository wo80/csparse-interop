
namespace CSparse.Benchmark
{
    public class BenchmarkResult
    {
        public MatrixFile File { get; private set; }
        public string Exception { get; private set; }

        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }
        public int NonZerosCount { get; private set; }

        public double Time { get; set; }
        public double Residual { get; set; }

        public BenchmarkResult(MatrixFile file, int rows, int columns, int nnz)
        {
            File = file;

            RowCount = rows;
            ColumnCount = columns;
            NonZerosCount = nnz;
        }

        public BenchmarkResult(MatrixFile file, string message)
        {
            File = file;
            Exception = message;
        }
    }
}