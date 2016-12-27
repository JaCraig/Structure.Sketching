using Structure.Sketching.Colors;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.ColorFormats
{
    public class TrueColorNoAlphaReader
    {
        [Fact]
        public void ReadScanline()
        {
            var data = new byte[] {
                1, 2, 3,
                4, 5, 6,
                7, 8, 9,
                0, 1, 2,
                3, 4, 5,
                6, 7, 8,
                9, 0, 1,
                2, 3, 4,
                5, 6, 7,
                8, 9, 0
            };
            var Result = new Color[10];
            var ExpectedResult = new Color[] {
                new Color(1, 2, 3, 255),
                new Color(4, 5, 6, 255),
                new Color(7, 8, 9, 255),
                new Color(0, 1, 2, 255),
                new Color(3, 4, 5, 255),
                new Color(6, 7, 8, 255),
                new Color(9, 0, 1, 255),
                new Color(2, 3, 4, 255),
                new Color(5, 6, 7, 255),
                new Color(8, 9, 0, 255)
            };
            var TestObject = new Structure.Sketching.Formats.Png.Format.ColorFormats.TrueColorNoAlphaReader();
            TestObject.ReadScanline(data, Result, new Sketching.Formats.Png.Format.Header(10, 1, 8, 3, 0, 0, 0), 0);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}