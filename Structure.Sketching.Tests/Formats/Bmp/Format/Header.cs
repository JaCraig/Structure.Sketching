using Structure.Sketching.Formats.Bmp.Format;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp.Format
{
    public class Header
    {
        [Fact]
        public void CreateByteArray()
        {
            var data = (new byte[][] {
                BitConverter.GetBytes((int)200),
                BitConverter.GetBytes((int)44),
                BitConverter.GetBytes((int)40),
                BitConverter.GetBytes((short)1),
                BitConverter.GetBytes((short)24),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)1000),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)0)
            }).SelectMany(x => x).ToArray();
            var TestFileHeader = new Sketching.Formats.Bmp.Format.Header(data);
            Assert.Equal(24, TestFileHeader.BPP);
            Assert.Equal(0, TestFileHeader.ColorsImportant);
            Assert.Equal(0, TestFileHeader.ColorsUsed);
            Assert.Equal(Compression.RGB, TestFileHeader.Compression);
            Assert.Equal(40, TestFileHeader.Height);
            Assert.Equal(1000, TestFileHeader.ImageSize);
            Assert.Equal(1, TestFileHeader.Planes);
            Assert.Equal(44, TestFileHeader.Width);
            Assert.Equal(0, TestFileHeader.XPPM);
            Assert.Equal(0, TestFileHeader.YPPM);
        }

        [Fact]
        public void CreateValues()
        {
            var TestFileHeader = new Sketching.Formats.Bmp.Format.Header(44, 40, 24, 1000, 0, 0, 0, 0, Compression.RGB);
            Assert.Equal(24, TestFileHeader.BPP);
            Assert.Equal(0, TestFileHeader.ColorsImportant);
            Assert.Equal(0, TestFileHeader.ColorsUsed);
            Assert.Equal(Compression.RGB, TestFileHeader.Compression);
            Assert.Equal(40, TestFileHeader.Height);
            Assert.Equal(1000, TestFileHeader.ImageSize);
            Assert.Equal(1, TestFileHeader.Planes);
            Assert.Equal(44, TestFileHeader.Width);
            Assert.Equal(0, TestFileHeader.XPPM);
            Assert.Equal(0, TestFileHeader.YPPM);
        }

        [Fact]
        public void Read()
        {
            var data = (new byte[][] {
                BitConverter.GetBytes((int)200),
                BitConverter.GetBytes((int)44),
                BitConverter.GetBytes((int)40),
                BitConverter.GetBytes((short)1),
                BitConverter.GetBytes((short)24),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)1000),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)0),
                BitConverter.GetBytes((int)0)
            }).SelectMany(x => x).ToArray();
            using (var Stream = new MemoryStream(data))
            {
                var TestFileHeader = Sketching.Formats.Bmp.Format.Header.Read(Stream);
                Assert.Equal(24, TestFileHeader.BPP);
                Assert.Equal(0, TestFileHeader.ColorsImportant);
                Assert.Equal(0, TestFileHeader.ColorsUsed);
                Assert.Equal(Compression.RGB, TestFileHeader.Compression);
                Assert.Equal(40, TestFileHeader.Height);
                Assert.Equal(1000, TestFileHeader.ImageSize);
                Assert.Equal(1, TestFileHeader.Planes);
                Assert.Equal(44, TestFileHeader.Width);
                Assert.Equal(0, TestFileHeader.XPPM);
                Assert.Equal(0, TestFileHeader.YPPM);
            }
        }

        [Fact]
        public void Write()
        {
            var TestFileHeader = new Sketching.Formats.Bmp.Format.Header(44, 40, 24, 1000, 0, 0, 0, 0, Compression.RGB);
            using (var BWriter = new BinaryWriter(new MemoryStream()))
            {
                TestFileHeader.Write(BWriter);
                Assert.Equal(40, BWriter.BaseStream.Length);
            }
        }
    }
}