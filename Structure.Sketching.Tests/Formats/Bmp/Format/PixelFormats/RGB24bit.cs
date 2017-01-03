using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;
using Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats
{
    public class RGB24bit : FormatBaseFixture
    {
        public override string FileName => "./TestImages/Formats/Bmp/Test24.bmp";
        public override IPixelFormat Format => new Sketching.Formats.Bmp.Format.PixelFormats.RGB24bit();

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 5280, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), TempFile);
                Data = Format.Decode(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 5280, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), Data, null);
                Assert.Equal(7040, Data.Length);
            }
        }

        [Fact]
        public void Encode()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 5280, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), TempFile);
                Data = Format.Decode(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 5280, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), Data, null);
                Data = Format.Encode(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 5280, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), Data, null);
                Assert.Equal(5280, Data.Length);
            }
        }

        [Fact]
        public void Read()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 5280, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), TempFile);
                Assert.Equal(5280, Data.Length);
            }
        }
    }
}