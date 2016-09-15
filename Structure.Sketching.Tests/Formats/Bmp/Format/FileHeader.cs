using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format
{
    public class FileHeader
    {
        [Fact]
        public void CreateByteArray()
        {
            var data = (new byte[][] { BitConverter.GetBytes((short)19778), BitConverter.GetBytes((int)1000), BitConverter.GetBytes((int)0), BitConverter.GetBytes((int)54) }).SelectMany(x => x).ToArray();
            var TestFileHeader = new Sketching.Formats.Bmp.Format.FileHeader(data);
            Assert.Equal(1000, TestFileHeader.FileSize);
            Assert.Equal(54, TestFileHeader.Offset);
            Assert.Equal(0, TestFileHeader.Reserved);
            Assert.Equal(19778, TestFileHeader.Type);
        }

        [Fact]
        public void CreateValues()
        {
            var TestFileHeader = new Sketching.Formats.Bmp.Format.FileHeader(1000, 54);
            Assert.Equal(1000, TestFileHeader.FileSize);
            Assert.Equal(54, TestFileHeader.Offset);
            Assert.Equal(0, TestFileHeader.Reserved);
            Assert.Equal(19778, TestFileHeader.Type);
        }

        [Fact]
        public void Read()
        {
            var data = (new byte[][] { BitConverter.GetBytes((short)19778), BitConverter.GetBytes((int)1000), BitConverter.GetBytes((int)0), BitConverter.GetBytes((int)54) }).SelectMany(x => x).ToArray();
            using (var Stream = new MemoryStream(data))
            {
                var TestFileHeader = Sketching.Formats.Bmp.Format.FileHeader.Read(Stream);
                Assert.Equal(1000, TestFileHeader.FileSize);
                Assert.Equal(54, TestFileHeader.Offset);
                Assert.Equal(0, TestFileHeader.Reserved);
                Assert.Equal(19778, TestFileHeader.Type);
            }
        }

        [Fact]
        public void Write()
        {
            var TestFileHeader = new Sketching.Formats.Bmp.Format.FileHeader(1000, 54);
            using (var BWriter = new BinaryWriter(new MemoryStream()))
            {
                TestFileHeader.Write(BWriter);
                Assert.Equal(14, BWriter.BaseStream.Length);
            }
        }
    }
}