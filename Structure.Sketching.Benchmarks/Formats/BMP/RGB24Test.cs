using BenchmarkDotNet.Attributes;
using Structure.Sketching.Benchmarks.Formats.BMP.TestClasses;
using Structure.Sketching.Formats.Bmp.Format.PixelFormats;

namespace Structure.Sketching.Benchmarks.Formats.BMP
{
    public class RGB24Test
    {
        [Params(100, 1000, 10000)]
        public int Count { get; set; }

        [Benchmark(Baseline = true, Description = "Without pointers")]
        public void Current()
        {
            new RGB24bit().Decode(new Sketching.Formats.Bmp.Format.Header(Count, Count, 0, Count * Count * 3, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), new byte[Count * Count * 3], new Sketching.Formats.Bmp.Format.Palette(0, new byte[0]));
        }

        [Benchmark(Description = "With fixed array pointers")]
        public void TestClass()
        {
            new RGB24bitTest().Decode(new Sketching.Formats.Bmp.Format.Header(Count, Count, 0, Count * Count * 3, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), new byte[Count * Count * 3], new Sketching.Formats.Bmp.Format.Palette(0, new byte[0]));
        }
    }
}