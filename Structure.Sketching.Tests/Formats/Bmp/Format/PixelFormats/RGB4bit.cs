using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;
using Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats
{
    public class RGB4bit : FormatBaseFixture
    {
        public override string FileName => "./TestImages/Formats/Bmp/Test4.bmp";
        public override IPixelFormat Format => new Sketching.Formats.Bmp.Format.PixelFormats.RGB4bit();

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Data = Format.Decode(44, 40, Data, new Sketching.Formats.Bmp.Format.Palette(16, new byte[64]));
                Assert.Equal(7040, Data.Length);
            }
        }

        [Fact]
        public void Encode()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Data = Format.Decode(44, 40, Data, new Sketching.Formats.Bmp.Format.Palette(16, new byte[64]));
                Data = Format.Encode(44, 40, Data, new Sketching.Formats.Bmp.Format.Palette(16, new byte[64]));
                Assert.Equal(7040, Data.Length);
            }
        }

        [Fact]
        public void Read()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Assert.Equal(1760, Data.Length);
            }
        }
    }
}