using Structure.Sketching.Formats.Bmp.Format;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format
{
    public class Body
    {
        [Fact]
        public void CreateByteArray()
        {
            var data = new byte[1600];
            var TestBody = new Sketching.Formats.Bmp.Format.Body(data);
            Assert.Equal(1600, TestBody.Data.Length);
        }

        [Fact]
        public void CreateImage()
        {
            var data = new byte[1600];
            var image = new Image(10, 40, data);
            var TestBody = new Sketching.Formats.Bmp.Format.Body(image, new Sketching.Formats.Bmp.Format.Header(10, 40, 24, 1280, 0, 0, 0, 0, Compression.RGB));
            Assert.Equal(1280, TestBody.Data.Length);
        }

        [Fact]
        public void Read()
        {
            byte[] data = new byte[5280];
            using (var Stream = new MemoryStream(data))
            {
                var TestBody = Sketching.Formats.Bmp.Format.Body.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 24, 5280, 0, 0, 0, 0, Compression.RGB), null, Stream);
                Assert.Equal(7040, TestBody.Data.Length);
            }
        }

        [Fact]
        public void Write()
        {
            var data = new byte[7040];
            var image = new Image(44, 40, data);
            var TestBody = new Sketching.Formats.Bmp.Format.Body(image, new Sketching.Formats.Bmp.Format.Header(44, 40, 24, 1280, 0, 0, 0, 0, Compression.RGB));
            using (var BWriter = new BinaryWriter(new MemoryStream()))
            {
                TestBody.Write(BWriter);
                Assert.Equal(5280, BWriter.BaseStream.Length);
            }
        }
    }
}