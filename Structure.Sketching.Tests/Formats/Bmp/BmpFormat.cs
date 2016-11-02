using Structure.Sketching.Tests.Formats.BaseClasses;
using System;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp
{
    public class BmpFormat : FormatTestBase
    {
        public override string ExpectedDirectory => "./ExpectedResults/Formats/Bmp/";

        public override string InputDirectory => "./TestImages/Formats/Bmp/";

        public override string OutputDirectory => "./TestOutput/Formats/Bmp/";

        public static readonly TheoryData<string> InputFileNames = new TheoryData<string> {
            {"Car.bmp"},
            {"Test24.bmp"},
            {"EncodingTest.bmp"},
            {"Test8.bmp" },
            {"Test4.bmp" }
        };

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
            using (var TempFile = File.OpenRead("./TestImages/Formats/Bmp/EncodingTest.bmp"))
            {
                var ImageFormat = new Structure.Sketching.Formats.Bmp.BmpFormat();
                var TempImage = ImageFormat.Decode(TempFile);
                Assert.Equal(7040, TempImage.Pixels.Length);
                Assert.Equal(44, TempImage.Width);
                Assert.Equal(40, TempImage.Height);
                Assert.Equal(44d / 40d, TempImage.PixelRatio);
            }
        }

        [Theory]
        [MemberData("InputFileNames")]
        public void Encode(string fileName)
        {
            using (var TempFile = File.OpenRead(InputDirectory + fileName))
            {
                var ImageFormat = new Sketching.Formats.Bmp.BmpFormat();
                var TempImage = ImageFormat.Decode(TempFile);
                using (var TempFile2 = File.OpenWrite(OutputDirectory + fileName))
                {
                    Assert.True(ImageFormat.Encode(new BinaryWriter(TempFile2), TempImage));
                }
            }
            Assert.True(CheckFileCorrect(fileName));
        }
    }
}