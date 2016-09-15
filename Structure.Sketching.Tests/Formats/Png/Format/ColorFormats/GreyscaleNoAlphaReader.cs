using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.ColorFormats
{
    public class GreyscaleNoAlphaReader
    {
        [Fact]
        public void ReadScanline()
        {
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var Result = new float[40];
            var ExpectedResult = new float[] { 1 / 255f, 1 / 255f, 1 / 255f, 1, 2 / 255f, 2 / 255f, 2 / 255f, 1, 3 / 255f, 3 / 255f, 3 / 255f, 1, 4 / 255f, 4 / 255f, 4 / 255f, 1, 5 / 255f, 5 / 255f, 5 / 255f, 1, 6 / 255f, 6 / 255f, 6 / 255f, 1, 7 / 255f, 7 / 255f, 7 / 255f, 1, 8 / 255f, 8 / 255f, 8 / 255f, 1, 9 / 255f, 9 / 255f, 9 / 255f, 1, 0, 0, 0, 1 };
            var TestObject = new Structure.Sketching.Formats.Png.Format.ColorFormats.GreyscaleNoAlphaReader();
            TestObject.ReadScanline(data, Result, new Sketching.Formats.Png.Format.Header(10, 1, 8, 0, 0, 0, 0), 0);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}