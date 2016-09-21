using BenchmarkDotNet.Attributes;
using System.IO;

namespace Structure.Sketching.Benchmarks.GenericSpeedTests
{
    public class FileReader
    {
        [Benchmark(Description = "File.Read")]
        public void FileRead()
        {
            using (var TestStream = File.Open("../../../../TestImage/BitmapFilter.bmp", FileMode.Open))
            {
                byte[] Data = new byte[TestStream.Length];
                TestStream.Read(Data, 0, (int)TestStream.Length);
            }
        }

        [Benchmark(Description = "File.Read in loop, 1024")]
        public void FileReadLoop1024()
        {
            byte[] Data = new byte[1024];
            using (var TestStream = File.Open("../../../../TestImage/BitmapFilter.bmp", FileMode.Open))
            {
                while (TestStream.Read(Data, 0, 1024) == 1024) { }
            }
        }

        [Benchmark(Description = "File.Read in loop, 2048")]
        public void FileReadLoop2048()
        {
            byte[] Data = new byte[2048];
            using (var TestStream = File.Open("../../../../TestImage/BitmapFilter.bmp", FileMode.Open))
            {
                while (TestStream.Read(Data, 0, 2048) == 2048) { }
            }
        }

        [Benchmark(Baseline = true, Description = "File.Read in loop, 4096")]
        public void FileReadLoop4096()
        {
            byte[] Data = new byte[4096];
            using (var TestStream = File.Open("../../../../TestImage/BitmapFilter.bmp", FileMode.Open))
            {
                while (TestStream.Read(Data, 0, 4096) == 4096) { }
            }
        }

        [Benchmark(Description = "File.ReadAllBytes")]
        public void ReadAllBytes()
        {
            var Data = File.ReadAllBytes("../../../../TestImage/BitmapFilter.bmp");
        }
    }
}