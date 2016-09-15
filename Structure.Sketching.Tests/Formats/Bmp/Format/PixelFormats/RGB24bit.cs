using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;
using Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats
{
    public class RGB24bit : FormatBaseFixture
    {
        public override string FileName => "./TestImages/Test24.bmp";
        public override IPixelFormat Format => new Sketching.Formats.Bmp.Format.PixelFormats.RGB24bit();

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Data = Format.Decode(44, 40, Data, null);
                Assert.Equal(7040, Data.Length);
            }
        }

        [Fact]
        public void Encode()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Data = Format.Decode(44, 40, Data, null);
                Data = Format.Encode(44, 40, Data, null);
                Assert.Equal(5280, Data.Length);
            }
        }

        [Fact]
        public void Read()
        {
            using (var TempFile = File.Open(FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] Data = Format.Read(44, 40, TempFile);
                Assert.Equal(5280, Data.Length);
            }
        }
    }
}