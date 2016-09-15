using Structure.Sketching.Tests.Formats.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Jpeg
{
    public class JpegFormat : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/Formats/Jpg/EncodingTest.bmp";
        public override string InputFileName => "./TestImages/Formats/Jpg/Calliphora.jpg";

        public override string OutputFileName => "./TestOutput/Formats/Jpg/BMPFormatTest.jpg";

        [Fact]
        public void Test()
        {
            new Image(InputFileName).Save(OutputFileName);
            new Image("./TestImages/Formats/Jpg/Floorplan.jpeg").Save("./TestOutput/Formats/Jpg/BMPFormatTest2.jpg");
            new Image("./TestImages/Formats/Jpg/gamma_dalai_lama_gray.jpg").Save("./TestOutput/Formats/Jpg/BMPFormatTest3.jpg");
            new Image("./TestImages/Formats/Jpg/rgb.jpg").Save("./TestOutput/Formats/Jpg/BMPFormatTest4.jpg");
        }
    }
}