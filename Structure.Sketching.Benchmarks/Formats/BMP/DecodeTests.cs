using BenchmarkDotNet.Attributes;
using System.IO;

namespace Structure.Sketching.Benchmarks.Formats.BMP
{
    public class DecodeTests
    {
        [Benchmark(Baseline = true, Description = "FileStream reading")]
        public void FileStreamReading()
        {
            using (var TestStream = File.Open("../../../../TestImage/BitmapFilter.bmp", FileMode.Open))
            {
                new Structure.Sketching.Formats.Bmp.BmpFormat().Decode(TestStream);
            }
        }

        [Benchmark(Description = "MemoryStream reading")]
        public void MemoryStreamReading()
        {
            using (var TestStream = File.Open("../../../../TestImage/BitmapFilter.bmp", FileMode.Open))
            {
                byte[] Data = new byte[TestStream.Length];
                TestStream.Read(Data, 0, (int)TestStream.Length);
                using (var MemStream = new MemoryStream(Data))
                {
                    new Structure.Sketching.Formats.Bmp.BmpFormat().Decode(MemStream);
                }
            }
        }
    }
}