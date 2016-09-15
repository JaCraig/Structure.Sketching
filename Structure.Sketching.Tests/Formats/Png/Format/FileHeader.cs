using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Png.Format
{
    public class FileHeader
    {
        [Fact]
        public void Create()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.FileHeader();
            Assert.Equal(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, TestObject.header);
        }

        [Fact]
        public void CreateFromStream()
        {
            var TestObject = Structure.Sketching.Formats.Png.Format.FileHeader.Read(new MemoryStream());
            Assert.Equal(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, TestObject.header);
        }

        [Fact]
        public void Write()
        {
            var TestObject = new Structure.Sketching.Formats.Png.Format.FileHeader();
            using (var Writer = new BinaryWriter(new MemoryStream()))
            {
                TestObject.Write(Writer);
                Writer.BaseStream.Seek(0, SeekOrigin.Begin);
                var Result = new byte[8];
                Writer.BaseStream.Read(Result, 0, 8);
                Assert.Equal(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, Result);
            }
        }
    }
}