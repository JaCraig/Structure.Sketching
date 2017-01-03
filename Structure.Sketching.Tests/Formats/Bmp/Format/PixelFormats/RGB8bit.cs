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
                byte[] Data = Format.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 1760, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), TempFile);
                Data = Format.Decode(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 1760, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), Data, TempPalette);
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
                byte[] Data = Format.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 1760, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), TempFile);
                Data = Format.Decode(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 1760, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), Data, TempPalette);
                Data = Format.Encode(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 1760, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), Data, TempPalette);
                Assert.Equal(1760, Data.Length);
            }
        }

        [Fact]
        public void Read()
        {
            using (var TempFile = System.IO.File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(new Sketching.Formats.Bmp.Format.Header(44, 40, 0, 1760, 0, 0, 0, 0, Sketching.Formats.Bmp.Format.Compression.RGB), TempFile);
                Assert.Equal(1760, Data.Length);
            }
        }
    }
}