using Structure.Sketching.Formats.Bmp.Format;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format
{
    public class Palette
    {
        [Fact]
        public void CreateByteArray()
        {
            var data = new byte[1024];
            var TestFileHeader = new Sketching.Formats.Bmp.Format.Palette(256, data);
            Assert.Equal(256, TestFileHeader.NumberOfColors);
            Assert.Equal(1024, TestFileHeader.Data.Length);
        }

        [Fact]
        public void Read()
        {
            var data = new byte[1024];
            using (var Stream = new MemoryStream(data))
            {
                var TestFileHeader = Sketching.Formats.Bmp.Format.Palette.Read(new Sketching.Formats.Bmp.Format.Header(1, 1, 24, 1, 0, 0, 256, 0, Compression.RGB), Stream);
                Assert.Equal(256, TestFileHeader.NumberOfColors);
                Assert.Equal(1024, TestFileHeader.Data.Length);
            }
        }

        [Fact]
        public void Write()
        {
            var data = new byte[1024];
            var TestFileHeader = new Sketching.Formats.Bmp.Format.Palette(256, data);
            using (var BWriter = new BinaryWriter(new MemoryStream()))
            {
                TestFileHeader.Write(BWriter);
                Assert.Equal(0, BWriter.BaseStream.Length);
            }
        }
    }
}