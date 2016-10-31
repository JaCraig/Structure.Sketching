using Structure.Sketching;
using System;
using System.IO;

namespace NetCoreTest
{
    public class Program
    {
        private static bool LegalImage()
        {
            using (var stream = new MemoryStream(File.ReadAllBytes("example.png")))
            {
                var image = new Image(stream);
                return image.Width > 0 && image.Height > 0;
            }
        }

        private static void ImageWrite()
        {
            using (var stream = new MemoryStream(File.ReadAllBytes("example.png")))
            {
                var image = new Image(stream);
                image.Save("result.bmp");
                image.Save("result.png");
                image.Save("result.jpg");
                image.Save("result.gif");
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