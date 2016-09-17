using Structure.Sketching.Tests.Formats.BaseClasses;
using System;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png
{
    public class PngFormat : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/Formats/Png/splash.png";
        public override string InputFileName => "./TestImages/Formats/Png/splash.png";

        public override string OutputFileName => "./TestOutput/Formats/Png/splash.png";

        [Fact]
        public void CanDecodeByteArray()
        {
            byte[] Header = {
                0x89,
                0x50,
                0x4E,
                0x47,
                0x0D,
                0x0A,
                0x1A,
                0x0A
            };
            Assert.True(new Structure.Sketching.Formats.Png.PngFormat().CanDecode(Header));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode(BitConverter.GetBytes((int)19777)));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode(BitConverter.GetBytes((int)19779)));
        }

        [Fact]
        public void CanDecodeFileName()
        {
            Assert.True(new Structure.Sketching.Formats.Png.PngFormat().CanDecode("test.png"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode("test.dib"));
            Assert.True(new Structure.Sketching.Formats.Png.PngFormat().CanDecode("TEST.PNG"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode("TEST.DIB"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode("test.jpg"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode("PNG.jpg"));
        }

        [Fact]
        public void CanDecodeStream()
        {
            byte[] Header = {
                0x89,
                0x50,
                0x4E,
                0x47,
                0x0D,
                0x0A,
                0x1A,
                0x0A
            };
            Assert.True(new Structure.Sketching.Formats.Png.PngFormat().CanDecode(new MemoryStream(Header)));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19777))));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19779))));
        }

        [Fact]
        public void CanEncode()
        {
            Assert.True(new Structure.Sketching.Formats.Png.PngFormat().CanEncode("ASDF.png"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanEncode("ASDF.bmp"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanEncode("ASDF.jpg"));
            Assert.False(new Structure.Sketching.Formats.Png.PngFormat().CanEncode("bmp.gif"));
        }

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var ImageFormat = new Structure.Sketching.Formats.Png.PngFormat();
                var TempImage = ImageFormat.Decode(TempFile);
                Assert.Equal(966000, TempImage.Pixels.Length);
                Assert.Equal(500, TempImage.Width);
                Assert.Equal(483, TempImage.Height);
                Assert.Equal(500d / 483d, TempImage.PixelRatio);
            }
        }

        [Fact]
        public void Encode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var ImageFormat = new Sketching.Formats.Png.PngFormat();
                var TempImage = ImageFormat.Decode(TempFile);
                using (var TempFile2 = File.OpenWrite(OutputFileName))
                {
                    Assert.True(ImageFormat.Encode(new BinaryWriter(TempFile2), TempImage));
                }
            }
            Assert.True(CheckFileCorrect());
        }
    }
}