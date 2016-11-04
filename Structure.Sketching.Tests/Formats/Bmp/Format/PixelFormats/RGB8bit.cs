using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;
using Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats
{
    public class RGB8bit : FormatBaseFixture
    {
        public override string FileName => "./TestImages/Formats/Bmp/Test8.bmp";
        public override IPixelFormat Format => new Sketching.Formats.Bmp.Format.PixelFormats.RGB8bit();

        [Fact]
        public void Decode()
        {
            byte[] PaletteData = new byte[1024];
            var TempPalette = new Sketching.Formats.Bmp.Format.Palette(256, PaletteData);
            using (var TempFile = System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Data = Format.Decode(44, 40, Data, TempPalette);
                Assert.Equal(7040, Data.Length);
            }
        }

        [Fact(Skip = "Not currently implemented")]
        public void Encode()
        {
            byte[] PaletteData = new byte[1024];
            var TempPalette = new Sketching.Formats.Bmp.Format.Palette(256, PaletteData);
            using (var TempFile = System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Data = Format.Decode(44, 40, Data, TempPalette);
                Data = Format.Encode(44, 40, Data, TempPalette);
                Assert.Equal(1760, Data.Length);
            }
        }

        [Fact]
        public void Read()
        {
            using (var TempFile = System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Assert.Equal(1760, Data.Length);
            }
        }
    }
}