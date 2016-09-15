using Structure.Sketching.Tests.Formats.BaseClasses;
using System;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png
{
    public class Decoder : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/Formats/Png/Decoder-splash.png";
        public override string InputFileName => "./TestImages/Formats/Png/splash.png";

        public override string OutputFileName => "./TestOutput/Formats/Png/Decoder-splash.png";

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
            Assert.True(new Structure.Sketching.Formats.Png.Decoder().CanDecode(Header));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode(BitConverter.GetBytes((int)19777)));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode(BitConverter.GetBytes((int)19779)));
        }

        [Fact]
        public void CanDecodeFileName()
        {
            Assert.True(new Structure.Sketching.Formats.Png.Decoder().CanDecode("test.png"));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode("test.dib"));
            Assert.True(new Structure.Sketching.Formats.Png.Decoder().CanDecode("TEST.PNG"));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode("TEST.DIB"));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode("test.jpg"));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode("PNG.jpg"));
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
            Assert.True(new Structure.Sketching.Formats.Png.Decoder().CanDecode(new MemoryStream(Header)));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19777))));
            Assert.False(new Structure.Sketching.Formats.Png.Decoder().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19779))));
        }

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var TempDecoder = new Structure.Sketching.Formats.Png.Decoder();
                var TempImage = TempDecoder.Decode(TempFile);
                Assert.Equal(241500, TempImage.Pixels.Length);
                Assert.Equal(500, TempImage.Width);
                Assert.Equal(483, TempImage.Height);
                Assert.Equal(500d / 483d, TempImage.PixelRatio);
            }
        }
    }
}