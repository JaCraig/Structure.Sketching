using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.Filters
{
    public class PaethFilter
    {
        [Fact]
        public void Creation()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.PaethFilter();
            Assert.Equal(4, TestObject.FilterValue);
        }

        [Fact]
        public void Decode()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.PaethFilter();
            var Result = TestObject.Decode(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 1);
            Assert.Equal(new byte[] { 10, 11, 14, 18, 23, 29, 36, 44, 53 }, Result);
        }

        [Fact]
        public void Encode()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.PaethFilter();
            var Result = TestObject.Encode(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 1);
            Assert.Equal(new byte[] { 4, 248, 254, 11, 6, 250, 1, 13, 7, 252 }, Result);
        }
    }
}