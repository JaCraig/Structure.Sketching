using Structure.Sketching.Tests.Formats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp
{
    public class Encoder : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/EncodingTest.bmp";
        public override string InputFileName => "./TestImages/EncodingTest.bmp";

        public override string OutputFileName => "./TestOutput/EncoderTest.bmp";

        [Fact]
        public void CanEncode()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("ASDF.bmp"));
            Assert.True(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("ASDF.dib"));
            Assert.False(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("ASDF.jpg"));
            Assert.False(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("bmp.gif"));
        }

        [Fact]
        public void Encode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var TempDecoder = new Structure.Sketching.Formats.Bmp.Decoder();
                var TempImage = TempDecoder.Decode(TempFile);
                var TempEncoder = new Structure.Sketching.Formats.Bmp.Encoder();
                using (var TempFile2 = File.OpenWrite(OutputFileName))
                {
                    TempEncoder.Encode(new BinaryWriter(TempFile2), TempImage);
                }
            }
            Assert.True(CheckFileCorrect());
        }
    }
}