using Structure.Sketching.Tests.Formats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Gif
{
    public class GifFormat : FormatTestBase
    {
        public override string ExpectedDirectory => "./ExpectedResults/Formats/Gif/";

        public override string InputDirectory => "./TestImages/Formats/Gif/";

        public override string OutputDirectory => "./TestOutput/Formats/Gif/";

        public static readonly TheoryData<string> InputFileNames = new TheoryData<string> {
            {"giphy.gif"},
            {"rings.gif"}
        };

        [Theory]
        [MemberData("InputFileNames")]
        public void Encode(string fileName)
        {
            using (var TempFile = File.OpenRead(InputDirectory + fileName))
            {
                var ImageFormat = new Sketching.Formats.Gif.GifFormat();
                var TempImage = ImageFormat.DecodeAnimation(TempFile);
                using (var TempFile2 = File.OpenWrite(OutputDirectory + fileName))
                {
                    Assert.True(ImageFormat.Encode(new BinaryWriter(TempFile2), TempImage));
                }
            }
            Assert.True(CheckFileCorrect(fileName));
        }
    }
}