using Structure.Sketching.Tests.Formats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png
{
    public class Encoder : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/Formats/Png/Encoder-splash.png";
        public override string InputFileName => "./TestImages/Formats/Png/splash.png";

        public override string OutputFileName => "./TestOutput/Formats/Png/Encoder-splash.png";

        [Fact]
        public void CanEncode()
        {
            Assert.True(new Structure.Sketching.Formats.Png.Encoder().CanEncode("ASDF.png"));
            Assert.False(new Structure.Sketching.Formats.Png.Encoder().CanEncode("ASDF.bmp"));
            Assert.False(new Structure.Sketching.Formats.Png.Encoder().CanEncode("ASDF.jpg"));
            Assert.False(new Structure.Sketching.Formats.Png.Encoder().CanEncode("bmp.gif"));
        }

        [Fact]
        public void Encode()
        {
            new DirectoryInfo("./TestOutput/Formats/Png/").Create();
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var TempDecoder = new Structure.Sketching.Formats.Png.Decoder();
                var TempImage = TempDecoder.Decode(TempFile);
                var TempEncoder = new Structure.Sketching.Formats.Png.Encoder();
                using (var TempFile2 = File.OpenWrite(OutputFileName))
                {
                    TempEncoder.Encode(new BinaryWriter(TempFile2), TempImage);
                }
            }
            Assert.True(CheckFileCorrect());
        }
    }
}