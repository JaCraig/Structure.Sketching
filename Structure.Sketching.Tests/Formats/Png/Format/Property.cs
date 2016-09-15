using System.Text;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format
{
    public class Property
    {
        [Fact]
        public void Create()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.Property("TestKey", "TestValue");
            Assert.Equal("TestKey", TestObject.Key);
            Assert.Equal("TestValue", TestObject.Value);
            TestObject = new Structure.Sketching.Formats.Png.Format.Property(null, null);
            Assert.Equal("", TestObject.Key);
            Assert.Equal("", TestObject.Value);
        }

        [Fact]
        public void ReadFromChunk()
        {
            Structure.Sketching.Formats.Png.Format.Property TestObject = new Sketching.Formats.Png.Format.Helpers.Chunk(10, "Something", Encoding.UTF8.GetBytes("Testing\0THIS"), 1234);
            Assert.Equal("Testing", TestObject.Key);
            Assert.Equal("THIS", TestObject.Value);
        }
    }
}