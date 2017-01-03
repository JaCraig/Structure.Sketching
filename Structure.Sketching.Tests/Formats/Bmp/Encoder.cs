using Structure.Sketching.Tests.Formats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp
{
    public class Encoder : FormatTestBase
    {
        public override string ExpectedDirectory => "./ExpectedResults/Formats/Bmp/";

        public override string InputDirectory => "./TestImages/Formats/Bmp/";

        public override string OutputDirectory => "./TestOutput/Formats/Bmp/Encoder/";

        public static readonly TheoryData<string> InputFileNames = new TheoryData<string> {
            {"Car.bmp"},
            {"Test24.bmp"},
            {"EncodingTest.bmp"},
            {"Test8.bmp" },
            {"Test4.bmp" },
            {"Test16.bmp" },
            {"Test32.bmp" },
            {"TestRLE8.bmp" },
            {"Test1.bmp" }
        };

        [Fact]
        public void CanEncode()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("ASDF.bmp"));
            Assert.True(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("ASDF.dib"));
            Assert.False(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("ASDF.jpg"));
            Assert.False(new Structure.Sketching.Formats.Bmp.Encoder().CanEncode("bmp.gif"));
        }

        [Theory]
        [MemberData("InputFileNames")]
        public void Encode(string fileName)
        {
            using (var TempFile = File.OpenRead(InputDirectory + fileName))
            {
                var TempDecoder = new Structure.Sketching.Formats.Bmp.Decoder();
                var TempImage = TempDecoder.Decode(TempFile);
                var TempEncoder = new Structure.Sketching.Formats.Bmp.Encoder();
                using (var TempFile2 = File.OpenWrite(OutputDirectory + fileName))
                {
                    TempEncoder.Encode(new BinaryWriter(TempFile2), TempImage);
                }
            }
            Assert.True(CheckFileCorrect(fileName));
        }
    }
}