﻿
namespace CSparse.Benchmark
{
    using CSparse.IO;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class MatrixFile
    {
        public bool Symmetric { get; set; }
        public string Path { get; set; }
        public object Matrix { get; set; }
    }

    public class MatrixFileCollection
    {
        private List<MatrixFile> generalList;
        private List<MatrixFile> symmetricList;

        public MatrixFileCollection(string file, string directory)
        {
            generalList = new List<MatrixFile>();
            symmetricList = new List<MatrixFile>();

            ReadMatrixFileList(file, directory);
        }

        public int Count(bool symmetric)
        {
            if (symmetric)
            {
                return symmetricList.Count;
            }

            return generalList.Count;
        }

        public int Preload<T>() where T : struct, IEquatable<T>, IFormattable
        {
            int count = 0;

            foreach (var item in generalList)
            {
                item.Matrix = MatrixMarketReader.ReadMatrix<T>(item.Path);
                count++;
            }

            foreach (var item in symmetricList)
            {
                item.Matrix = MatrixMarketReader.ReadMatrix<T>(item.Path);
                count++;
            }

            return count;
        }

        public List<MatrixFile> Get(bool symmetric)
        {
            if (symmetric)
            {
                return symmetricList;
            }

            return generalList;
        }

        private void ReadMatrixFileList(string file, string directory)
        {
            if (!File.Exists(file))
            {
                throw new Exception("File not found: " + Path.GetFullPath(file));
            }

            using (var reader = new StreamReader(file))
            {
                bool symmetric = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    line = line.Trim();

                    if (line.StartsWith("#"))
                    {
                        continue;
                    }

                    if (line.StartsWith("["))
                    {
                        symmetric = line.Contains("symmetric");

                        continue;
                    }

                    var path = Path.GetFullPath(Path.Combine(directory, line));

                    if (!File.Exists(path))
                    {
                        Console.WriteLine("File not found: {0}", path);
                        continue;
                    }

                    if (symmetric)
                    {
                        symmetricList.Add(new MatrixFile() { Path = path, Symmetric = symmetric });
                    }
                    else
                    {
                        generalList.Add(new MatrixFile() { Path = path, Symmetric = symmetric });
                    }
                }
            }
        }
    }
}
