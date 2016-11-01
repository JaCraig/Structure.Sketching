using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format
{
    public class Header
    {
        [Fact]
        public void Create()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.Header(100, 101, 8, 6, 8, 7, 7);
            Assert.Equal(8, TestObject.BitDepth);
            Assert.Equal(6, (byte)TestObject.ColorType);
            Assert.Equal(8, TestObject.CompressionMethod);
            Assert.Equal(7, TestObject.FilterMethod);
            Assert.Equal(101, TestObject.Height);
            Assert.Equal(7, TestObject.InterlaceMethod);
            Assert.Equal(100, TestObject.Width);
        }

        [Fact]
        public void CreateFromChunk()
        {
            byte[] data = { 0, 0, 0, 100, 0, 0, 0, 101, 8, 6, 8, 7, 7 };
            Structure.Sketching.Formats.Png.Format.Header TestObject = new Sketching.Formats.Png.Format.Helpers.Chunk(12, "ASDF", data, 12);
            Assert.Equal(8, TestObject.BitDepth);
            Assert.Equal(6, (byte)TestObject.ColorType);
            Assert.Equal(8, TestObject.CompressionMethod);
            Assert.Equal(7, TestObject.FilterMethod);
            Assert.Equal(101, TestObject.Height);
            Assert.Equal(7, TestObject.InterlaceMethod);
            Assert.Equal(100, TestObject.Width);
        }
    }
}