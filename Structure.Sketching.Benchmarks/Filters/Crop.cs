using BenchmarkDotNet.Attributes;
using Structure.Sketching.Benchmarks.Filters.TestClasses;
using System.Drawing;

namespace Structure.Sketching.Benchmarks.Filters
{
    public class Crop
    {
        [Params(10, 100, 1000, 5000)]
        public int Count { get; set; }

        private int Height => 8000;
        private int Width => 8000;

        [Benchmark(Description = "Structure.Sketching Crop")]
        public void CropStructureSketching()
        {
            var TestImage = new Image(Width, Height, new byte[Width * Height * 4]);
            var CropFilter = new Sketching.Filters.Resampling.Crop();
            CropFilter.Apply(TestImage, new Numerics.Rectangle(0, 0, Count, Count));
        }

        [Benchmark(Description = "Structure.Sketching Test Crop")]
        public void CropStructureSketchingTest()
        {
            var TestImage = new Image(Width, Height, new byte[Width * Height * 4]);
            var TestCropFilter = new CropFilter();
            TestCropFilter.Apply(TestImage, new Numerics.Rectangle(0, 0, Count, Count));
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Crop")]
        public void CropSystemDrawing()
        {
            using (Bitmap source = new Bitmap(Width, Height))
            {
                using (var destination = source.Clone(new Rectangle(0, 0, Count, Count), source.PixelFormat))
                {
                }
            }
        }
    }
}