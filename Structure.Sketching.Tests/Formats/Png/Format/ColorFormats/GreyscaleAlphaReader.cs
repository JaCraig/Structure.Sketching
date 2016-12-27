using Structure.Sketching.Colors;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.ColorFormats
{
    public class GreyscaleAlphaReader
    {
        [Fact]
        public void ReadScanline()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var Result = new Color[10];
            var ExpectedResult = new Color[] {
                new Color(1, 1, 1, 2),
                new Color(3, 3, 3, 4),
                new Color(5, 5, 5, 6),
                new Color(7, 7, 7, 8),
                new Color(9, 9, 9, 0),
                new Color(1, 1, 1, 2),
                new Color(3, 3, 3, 4),
                new Color(5, 5, 5, 6),
                new Color(7, 7, 7, 8),
                new Color(9, 9, 9, 0)
            };
            var TestObject = new Structure.Sketching.Formats.Png.Format.ColorFormats.GreyscaleAlphaReader();
            TestObject.ReadScanline(data, Result, new Sketching.Formats.Png.Format.Header(10, 1, 8, 4, 0, 0, 0), 0);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}