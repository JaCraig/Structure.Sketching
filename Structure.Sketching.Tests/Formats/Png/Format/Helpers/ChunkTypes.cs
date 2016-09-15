using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format.Helpers
{
    public class ChunkTypes
    {
        [Fact]
        public void Create()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.Helpers.ChunkTypes("ASDF");
            Assert.Equal("ASDF", TestObject.Value);
            Assert.Equal("ASDF", TestObject);
        }

        [Fact]
        public void Equality()
        {
            var TestObject1 = new Structure.Sketching.Formats.Png.Format.Helpers.ChunkTypes("ASDF");
            var TestObject2 = new Structure.Sketching.Formats.Png.Format.Helpers.ChunkTypes("ASDF");
            Assert.True(TestObject1 == TestObject2);
            Assert.True("ASDF" == TestObject2);
            Assert.True(TestObject1 == "ASDF");
        }

        [Fact]
        public void Inequality()
        {
            var TestObject1 = new Structure.Sketching.Formats.Png.Format.Helpers.ChunkTypes("ASDF");
            var TestObject2 = new Structure.Sketching.Formats.Png.Format.Helpers.ChunkTypes("ASDF2");
            Assert.False(TestObject1 == TestObject2);
            Assert.False("ASDF" == TestObject2);
            Assert.False(TestObject1 == "ASDF2");
        }
    }
}