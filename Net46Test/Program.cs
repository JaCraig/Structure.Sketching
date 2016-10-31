using Structure.Sketching;
using System;
using System.IO;

namespace Net46Test
{
    internal class Program
    {
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
            Console.WriteLine(LegalImage());
            Console.ReadKey();
        }
    }
}