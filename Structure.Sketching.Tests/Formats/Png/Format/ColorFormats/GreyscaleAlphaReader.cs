using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.ColorFormats
{
    public class GreyscaleAlphaReader
    {
        [Fact]
        public void ReadScanline()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var Result = new byte[40];
            var ExpectedResult = new byte[] { 1, 1, 1, 2, 3, 3, 3, 4, 5, 5, 5, 6, 7, 7, 7, 8, 9, 9, 9, 0, 1, 1, 1, 2, 3, 3, 3, 4, 5, 5, 5, 6, 7, 7, 7, 8, 9, 9, 9, 0 };
            var TestObject = new Structure.Sketching.Formats.Png.Format.ColorFormats.GreyscaleAlphaReader();
            TestObject.ReadScanline(data, Result, new Sketching.Formats.Png.Format.Header(10, 1, 8, 4, 0, 0, 0), 0);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}