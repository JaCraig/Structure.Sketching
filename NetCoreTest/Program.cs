using Structure.Sketching;
using Structure.Sketching.Colors;
using System;
using System.IO;

namespace NetCoreTest
{
    public class Program
    {
        private static void ImageWrite()
        {
            using (var stream = new MemoryStream(File.ReadAllBytes("example.png")))
            {
                var image = new Image(stream).Apply(new Structure.Sketching.Filters.Convolution.Emboss())
                                             .Apply(new Structure.Sketching.Filters.Binary.AdaptiveThreshold(10, Color.White, Color.Black, 0.5f));
                image.Save("result.bmp");
                image.Save("result.png");
                image.Save("result.jpg");
                image.Save("result.gif");
            }
        }

        private static bool LegalImage()
        {
            using (var stream = new MemoryStream(File.ReadAllBytes("example.png")))
            {
                var image = new Image(stream);
                return image.Width > 0 && image.Height > 0;
            }
        }

        private static void Main(string[] args)
        {
            ImageWrite();
            Console.WriteLine(LegalImage());
            Console.ReadKey();
        }
    }
}