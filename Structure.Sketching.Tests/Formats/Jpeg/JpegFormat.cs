using Structure.Sketching.Tests.Formats.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Jpeg
{
    public class JpegFormat : FormatTestBase
    {
        public override string ExpectedDirectory => "./ExpectedResults/Formats/Jpg/";

        public override string InputDirectory => "./TestImages/Formats/Jpg/";

        public override string OutputDirectory => "./TestOutput/Formats/Jpg/";

        public static readonly TheoryData<string> InputFileNames = new TheoryData<string> {
            {"Calliphora.jpg"},
            {"Floorplan.jpeg"},
            {"gamma_dalai_lama_gray.jpg"},
            {"rgb.jpg"},
            {"maltese_puppy-wide.jpg"}
        };

        [Theory]
        [MemberData("InputFileNames")]
        public void Encode(string fileName)
        {
            using (var TempFile = File.OpenRead(InputDirectory + fileName))
            {
                var ImageFormat = new Sketching.Formats.Jpeg.JpegFormat();
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