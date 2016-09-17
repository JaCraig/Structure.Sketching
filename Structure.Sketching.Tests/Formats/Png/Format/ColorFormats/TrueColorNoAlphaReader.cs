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
            var Result = new byte[40];
            var ExpectedResult = new byte[] {
                1 , 2, 3, 255,
                4 , 5 , 6 , 255,
                7 , 8 , 9 , 255,
                0, 1 , 2 , 255,
                3 , 4 , 5 , 255,
                6 , 7 , 8 , 255,
                9 , 0, 1 , 255,
                2 , 3 , 4 , 255,
                5 , 6 , 7 , 255,
                8 , 9 , 0, 255
            };
            var TestObject = new Structure.Sketching.Formats.Png.Format.ColorFormats.TrueColorNoAlphaReader();
            TestObject.ReadScanline(data, Result, new Sketching.Formats.Png.Format.Header(10, 1, 8, 3, 0, 0, 0), 0);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}