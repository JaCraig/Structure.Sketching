using Structure.Sketching.Tests.Formats.BaseClasses;
using System;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp
{
    public class BmpFormat : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/EncodingTest.bmp";
        public override string InputFileName => "./TestImages/EncodingTest.bmp";

        public override string OutputFileName => "./TestOutput/BMPFormatTest.bmp";

        [Fact]
        public void CanDecodeByteArray()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode(BitConverter.GetBytes((int)19778)));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode(BitConverter.GetBytes((int)19777)));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode(BitConverter.GetBytes((int)19779)));
        }

        [Fact]
        public void CanDecodeFileName()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode("test.bmp"));
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode("test.dib"));
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode("TEST.BMP"));
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode("TEST.DIB"));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode("test.jpg"));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode("BMP.jpg"));
        }

        [Fact]
        public void CanDecodeStream()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19778))));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19777))));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19779))));
        }

        [Fact]
        public void CanEncode()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanEncode("ASDF.bmp"));
            Assert.True(new Structure.Sketching.Formats.Bmp.BmpFormat().CanEncode("ASDF.dib"));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanEncode("ASDF.jpg"));
            Assert.False(new Structure.Sketching.Formats.Bmp.BmpFormat().CanEncode("bmp.gif"));
        }

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var ImageFormat = new Structure.Sketching.Formats.Bmp.BmpFormat();
                var TempImage = ImageFormat.Decode(TempFile);
                Assert.Equal(7040, TempImage.Pixels.Length);
                Assert.Equal(44, TempImage.Width);
                Assert.Equal(40, TempImage.Height);
                Assert.Equal(1.1d, TempImage.PixelRatio);
            }
        }

        [Fact]
        public void Encode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var ImageFormat = new Structure.Sketching.Formats.Bmp.BmpFormat();
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