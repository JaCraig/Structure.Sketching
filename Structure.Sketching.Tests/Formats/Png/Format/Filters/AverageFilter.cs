using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.Filters
{
    public class AverageFilter
    {
        [Fact]
        public void Creation()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.AverageFilter();
            Assert.Equal(3, TestObject.FilterValue);
        }

        [Fact]
        public void Decode()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.AverageFilter();
            var Result = TestObject.Decode(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 1);
            Assert.Equal(new byte[] { 5, 8, 10, 12, 13, 14, 15, 16, 17 }, Result);
        }

        [Fact]
        public void Encode()
        {
            var TestObject = new Sketching.Formats.Png.Format.Filters.AverageFilter();
            var Result = TestObject.Encode(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }, 1);
            Assert.Equal(new byte[] { 3, 253, 253, 129, 131, 194, 195, 165, 166, 182 }, Result);
        }
    }
}