using BenchmarkDotNet.Attributes;
using ImageProcessor;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Structure.Sketching.Benchmarks.Filters
{
    public class Resize
    {
        [Benchmark(Description = "Structure.Sketching Resize")]
        public void CropStructureSketching()
        {
            var TestImage = new Image(2000, 2000);
            var Filter = new Sketching.Filters.Resampling.Resize(400, 400, Sketching.Filters.Resampling.Enums.ResamplingFiltersAvailable.NearestNeighbor);
            Filter.Apply(TestImage);
        }

        [Benchmark(Description = "ImageProcessorCore Resize")]
        public void ResizeCore()
        {
            var image = new ImageFactory();
            image.Load("../../TestImage/BitmapFilter.bmp");
            image.Resize(new Size(400, 400));
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Resize")]
        public void ResizeSystemDrawing()
        {
            using (Bitmap source = new Bitmap(2000, 2000))
            {
                using (Bitmap destination = new Bitmap(400, 400))
                {
                    using (Graphics graphics = Graphics.FromImage(destination))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.DrawImage(source, 0, 0, 400, 400);
                    }
                }
            }
        }
    }
}