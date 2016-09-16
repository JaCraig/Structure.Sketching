using BenchmarkDotNet.Attributes;
using ImageProcessor;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Structure.Sketching.Benchmarks.Filters
{
    public class Crop
    {
        [Benchmark(Description = "ImageProcessorCore Crop")]
        public void CropResizeImageProcessor()
        {
            ImageFactory image = new ImageFactory();
            image.Load("../../../../TestImage/BitmapFilter.bmp")
                .Crop(new Rectangle(0, 0, 100, 100));
        }

        [Benchmark(Description = "Structure.Sketching Crop")]
        public void CropStructureSketching()
        {
            var TestImage = new Image("../../../../TestImage/BitmapFilter.bmp");
            var CropFilter = new Sketching.Filters.Crop();
            CropFilter.Apply(TestImage, new Numerics.Rectangle(0, 0, 100, 100));
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Crop")]
        public void CropSystemDrawing()
        {
            using (Bitmap source = new Bitmap("../../../../TestImage/BitmapFilter.bmp"))
            {
                using (Bitmap destination = new Bitmap(100, 100))
                {
                    using (Graphics graphics = Graphics.FromImage(destination))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.DrawImage(source, new Rectangle(0, 0, 100, 100), 0, 0, 100, 100, GraphicsUnit.Pixel);
                    }
                }
            }
        }
    }
}