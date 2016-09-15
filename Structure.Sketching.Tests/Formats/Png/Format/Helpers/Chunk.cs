using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.Helpers
{
    public class Chunk
    {
        [Fact]
        public void Create()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.Helpers.Chunk(10, "ASDF", new byte[] { 1, 2, 3, 4 }, 12);
            Assert.Equal(10, TestObject.Length);
            Assert.Equal("ASDF", TestObject.Type);
            Assert.Equal(new byte[] { 1, 2, 3, 4 }, TestObject.Data);
            Assert.Equal((uint)12, TestObject.Crc);
        }
    }
}