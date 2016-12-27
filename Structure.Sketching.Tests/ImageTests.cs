using Structure.Sketching.Colors;
using Structure.Sketching.Formats;
using Structure.Sketching.Tests.BaseClasses;
using System;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests
{
    public class ImageTests : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Image/";

        public override string OutputDirectory => "./TestOutput/Image/";

        public string SecondImage => "./TestImages/Formats/Bmp/Car.bmp";

        public static readonly TheoryData<string, Func<Image, int, Image>, int> ShiftOperations = new TheoryData<string, Func<Image, int, Image>, int>
        {
            {"ShiftLeft",(x,y)=>x<<y,128 },
            {"ShiftRight",(x,y)=>x>>y,128 }
        };

        public static readonly TheoryData<string, Func<Image, Image>> UnaryOperations = new TheoryData<string, Func<Image, Image>>
        {
            {"Not",(x)=>!x }
        };

        [Fact]
        public void BadDataConstructor()
        {
            var TempImage = new Image(-1, -1, (byte[])null);
            Assert.Equal(1, TempImage.Width);
            Assert.Equal(1, TempImage.Height);
            Assert.Equal(1, TempImage.PixelRatio);
            Assert.Null(TempImage.Pixels);
        }

        [Theory]
        [MemberData("ShiftOperations")]
        public void CheckShiftOperators(string name, Func<Image, int, Image> operation, int value)
        {
            foreach (var file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                var TestImage = new Image(file);
                var ResultImage = operation(TestImage, value);
                ResultImage.Save(OutputDirectory + outputFileName);
            }
            foreach (string file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                Assert.True(CheckFileCorrect(ExpectedDirectory + Path.GetFileName(outputFileName), OutputDirectory + Path.GetFileName(outputFileName)), outputFileName);
            }
        }

        [Theory]
        [MemberData("UnaryOperations")]
        public void CheckUnaryOperators(string name, Func<Image, Image> operation)
        {
            foreach (var file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                var TestImage = new Image(file);
                var ResultImage = operation(TestImage);
                ResultImage.Save(OutputDirectory + outputFileName);
            }
            foreach (string file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                Assert.True(CheckFileCorrect(ExpectedDirectory + Path.GetFileName(outputFileName), OutputDirectory + Path.GetFileName(outputFileName)), outputFileName);
            }
        }

        [Fact]
        public void NoDataConstructor()
        {
            var TempImage = new Image(1, 1);
            Assert.Equal(1, TempImage.Width);
            Assert.Equal(1, TempImage.Height);
            Assert.Equal(1, TempImage.PixelRatio);
            Assert.Equal(new Color(0, 0, 0, 0), TempImage.Pixels[0]);
        }

        [Fact]
        public void ToASCIIArt()
        {
            var TestImage = new Image(1, 10, new byte[] { 25, 51, 76, 102,
                127, 153, 178, 204,
                229, 255, 25, 51,
                76, 102, 127, 153,
                178, 204, 229, 255,
                25, 51, 76, 102,
                127, 153, 178, 204,
                229, 255, 25, 51,
                76, 102, 127, 153,
                178, 204, 229, 255 });
            Assert.Equal("#\r\n.\r\n-\r\n*\r\n=\r\n", TestImage.ToASCIIArt());
        }

        [Fact]
        public void ToBase64String()
        {
            var TestImage = new Image(1, 10, new byte[] { 25, 51, 76, 102, 127, 153, 178, 204, 229, 255, 25, 51, 76, 102, 127, 153, 178, 204, 229, 255, 25, 51, 76, 102, 127, 153, 178, 204, 229, 255, 25, 51, 76, 102, 127, 153, 178, 204, 229, 255 });
            Assert.Equal("Qk1eAAAAAAAAADYAAAAoAAAAAQAAAAoAAAABABgAAAAAACgAAAAAAAAAAAAAAAAAAAAAAAAA5cyyAH9mTAAZ/+UAspl/AEwzGQDlzLIAf2ZMABn/5QCymX8ATDMZAA==", TestImage.ToString(FileFormats.BMP));
        }
    }
}