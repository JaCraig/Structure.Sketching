using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.Filters
{
    public class UpFilter
    {
        [Fact]
        public void Creation()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.UpFilter();
            Assert.Equal(2, TestObject.FilterValue);
        }

        [Fact]
        public void Decode()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.UpFilter();
            var Result = TestObject.Decode(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 1);
            Assert.Equal(new byte[] { 10, 10, 10, 10, 10, 10, 10, 10, 10 }, Result);
        }

        [Fact]
        public void Encode()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.UpFilter();
            var Result = TestObject.Encode(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 1);
            Assert.Equal(new byte[] { 2, 248, 250, 252, 254, 0, 2, 4, 6, 8 }, Result);
        }
    }
}