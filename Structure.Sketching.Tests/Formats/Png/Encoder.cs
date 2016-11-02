using Structure.Sketching.Tests.Formats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png
{
    public class Encoder : FormatTestBase
    {
        public override string ExpectedDirectory => "./ExpectedResults/Formats/Png/Encoder/";

        public override string InputDirectory => "./TestImages/Formats/Png/";

        public override string OutputDirectory => "./TestOutput/Formats/Png/Encoder/";

        public static readonly TheoryData<string> InputFileNames = new TheoryData<string> {
            {"splash.png"},
            {"48bit.png"},
            {"blur.png"},
            {"indexed.png"},
            {"splashbw.png"}
        };

        [Fact]
        public void CanEncode()
        {
            Assert.True(new Structure.Sketching.Formats.Png.Encoder().CanEncode("ASDF.png"));
            Assert.False(new Structure.Sketching.Formats.Png.Encoder().CanEncode("ASDF.bmp"));
            Assert.False(new Structure.Sketching.Formats.Png.Encoder().CanEncode("ASDF.jpg"));
            Assert.False(new Structure.Sketching.Formats.Png.Encoder().CanEncode("bmp.gif"));
        }

        [Theory]
        [MemberData("InputFileNames")]
        public void Encode(string fileName)
        {
            using (var TempFile = File.OpenRead(InputDirectory + fileName))
            {
                var TempDecoder = new Structure.Sketching.Formats.Png.Decoder();
                var TempImage = TempDecoder.Decode(TempFile);
                var TempEncoder = new Structure.Sketching.Formats.Png.Encoder();
                using (var TempFile2 = File.OpenWrite(OutputDirectory + fileName))
                {
                    TempEncoder.Encode(new BinaryWriter(TempFile2), TempImage);
                }
            }
            Assert.True(CheckFileCorrect(fileName));
        }
    }
}