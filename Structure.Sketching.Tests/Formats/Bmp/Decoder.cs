﻿using Structure.Sketching.Tests.Formats.BaseClasses;
using System;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Formats.Bmp
{
    public class Decoder : FormatTestBase
    {
        public override string ExpectedOutputFileName => "./ExpectedResults/EncodingTest.bmp";
        public override string InputFileName => "./TestImages/EncodingTest.bmp";

        public override string OutputFileName => "./TestOutput/DecoderTest.bmp";

        [Fact]
        public void CanDecodeByteArray()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode(BitConverter.GetBytes((int)19778)));
            Assert.False(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode(BitConverter.GetBytes((int)19777)));
            Assert.False(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode(BitConverter.GetBytes((int)19779)));
        }

        [Fact]
        public void CanDecodeFileName()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode("test.bmp"));
            Assert.True(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode("test.dib"));
            Assert.True(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode("TEST.BMP"));
            Assert.True(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode("TEST.DIB"));
            Assert.False(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode("test.jpg"));
            Assert.False(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode("BMP.jpg"));
        }

        [Fact]
        public void CanDecodeStream()
        {
            Assert.True(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19778))));
            Assert.False(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19777))));
            Assert.False(new Structure.Sketching.Formats.Bmp.Decoder().CanDecode(new MemoryStream(BitConverter.GetBytes((int)19779))));
        }

        [Fact]
        public void Decode()
        {
            using (var TempFile = File.OpenRead(InputFileName))
            {
                var TempDecoder = new Structure.Sketching.Formats.Bmp.Decoder();
                var TempImage = TempDecoder.Decode(TempFile);
                Assert.Equal(1760, TempImage.Pixels.Length);
                Assert.Equal(44, TempImage.Width);
                Assert.Equal(40, TempImage.Height);
                Assert.Equal(1.1d, TempImage.PixelRatio);
            }
        }
    }
}