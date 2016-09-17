using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.ColorFormats
{
    public class GreyscaleNoAlphaReader
    {
        [Fact]
        public void ReadScanline()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var Result = new byte[40];
            var ExpectedResult = new byte[] { 1, 1, 1, 1, 2, 2, 2, 1, 3, 3, 3, 1, 4, 4, 4, 1, 5, 5, 5, 1, 6, 6, 6, 1, 7, 7, 7, 1, 8, 8, 8, 1, 9, 9, 9, 1, 0, 0, 0, 1 };
            var TestObject = new Structure.Sketching.Formats.Png.Format.ColorFormats.GreyscaleNoAlphaReader();
            TestObject.ReadScanline(data, Result, new Sketching.Formats.Png.Format.Header(10, 1, 8, 0, 0, 0, 0), 0);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}