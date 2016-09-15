using Structure.Sketching.Formats.Png.Format.Enums;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format
{
    public class Palette
    {
        [Fact]
        public void Create()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.Palette(new byte[512], PaletteType.Color);
            Assert.Equal(512, TestObject.Data.Length);
            Assert.Equal(PaletteType.Color, TestObject.Type);
            TestObject = new Structure.Sketching.Formats.Png.Format.Palette(new byte[12], PaletteType.Alpha);
            Assert.Equal(12, TestObject.Data.Length);
            Assert.Equal(PaletteType.Alpha, TestObject.Type);
        }

        [Fact]
        public void CreateFromChunk()
        {
            Structure.Sketching.Formats.Png.Format.Palette TestObject = new Sketching.Formats.Png.Format.Helpers.Chunk(1, Sketching.Formats.Png.Format.Helpers.ChunkTypes.Palette, new byte[512], 12);
            Assert.Equal(512, TestObject.Data.Length);
            Assert.Equal(PaletteType.Color, TestObject.Type);
            TestObject = new Sketching.Formats.Png.Format.Helpers.Chunk(1, Sketching.Formats.Png.Format.Helpers.ChunkTypes.TransparencyInfo, new byte[12], 12);
            Assert.Equal(12, TestObject.Data.Length);
            Assert.Equal(PaletteType.Alpha, TestObject.Type);
        }
    }
}