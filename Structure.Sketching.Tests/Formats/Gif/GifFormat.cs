using Structure.Sketching.Tests.Formats.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Gif
{
    public class GifFormat : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/Formats/Jpg/EncodingTest.bmp";
        public override string InputFileName => "./TestImages/Formats/Gif/giphy.gif";

        public override string OutputFileName => "./TestOutput/Formats/Gif/BMPFormatTest.jpg";

        [Fact]
        public void Test()
        {
            new Image(InputFileName).Save(OutputFileName);
            new Image("./TestImages/Formats/Gif/rings.gif").Save("./TestOutput/Formats/Gif/BMPFormatTest2.jpg");
            new Animation("./TestImages/Formats/Gif/giphy.gif").Save("./TestOutput/Formats/Gif/BMPFormatTest2.jpg");
            new Image("./TestImages/Formats/Gif/rings.gif").Save("./TestOutput/Formats/Gif/rings.gif");
            new Animation("./TestImages/Formats/Gif/giphy.gif").Save("./TestOutput/Formats/Gif/giphy.gif");
        }
    }
}