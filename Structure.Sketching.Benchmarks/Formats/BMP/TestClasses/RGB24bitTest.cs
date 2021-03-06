﻿using Structure.Sketching.Formats.Bmp.Format;
using Structure.Sketching.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System.Threading.Tasks;

namespace Structure.Sketching.Benchmarks.Formats.BMP.TestClasses
{
    public unsafe class RGB24bitTest : PixelFormatBase
    {
        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override double BPP => 3;

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The decoded data</returns>
        public override byte[] Decode(Header header, byte[] data, Palette palette)
        {
            int width = header.Width;
            int height = header.Height;
            int alignment = (4 - ((width * (int)BPP) % 4)) % 4;
            byte[] ReturnValue = new byte[width * height * 4];
            Parallel.For(0, height, y =>
            {
                int SourceY = height - y - 1;
                if (SourceY < 0)
                    SourceY = 0;
                if (SourceY >= height)
                    SourceY = height - 1;
                int SourceRowOffset = SourceY * ((width * (int)BPP) + alignment);
                int DestinationY = y;
                int DestinationRowOffset = DestinationY * width * 4;
                fixed (byte* DataFixed = &data[SourceRowOffset])
                fixed (byte* ReturnValueFixed = &ReturnValue[DestinationRowOffset])
                {
                    byte* DataFixed2 = DataFixed;
                    byte* ReturnValueFixed2 = ReturnValueFixed;
                    for (int x = 0; x < width; ++x)
                    {
                        *(ReturnValueFixed2 + 2) = *DataFixed2;
                        ++DataFixed2;
                        *(ReturnValueFixed2 + 1) = *DataFixed2;
                        ++DataFixed2;
                        *ReturnValueFixed2 = *DataFixed2;
                        ++DataFixed2;
                        *(ReturnValueFixed2 + 3) = 255;
                        ReturnValueFixed2 += 4;
                    }
                }
            });
            return ReturnValue;
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The encoded data</returns>
        public override byte[] Encode(Header header, byte[] data, Palette palette)
        {
            int width = header.Width;
            int height = header.Height;
            int alignment = (4 - ((width * (int)BPP) % 4)) % 4;
            var ReturnValue = new byte[((width * (int)BPP) + alignment) * height];
            Parallel.For(0, height, y =>
            {
                int SourceY = height - y - 1;
                if (SourceY < 0)
                    SourceY = 0;
                if (SourceY >= height)
                    SourceY = height - 1;
                int SourceRowOffset = SourceY * width * 4;
                int DestinationY = y;
                int DestinationRowOffset = DestinationY * ((width * (int)BPP) + alignment);
                fixed (byte* DataFixed = &data[SourceRowOffset])
                fixed (byte* ReturnValueFixed = &ReturnValue[DestinationRowOffset])
                {
                    byte* DataFixed2 = DataFixed;
                    byte* ReturnValueFixed2 = ReturnValueFixed;
                    for (int x = 0; x < width; ++x)
                    {
                        *(ReturnValueFixed2) = *(DataFixed2 + 2);
                        ++ReturnValueFixed2;
                        *(ReturnValueFixed2) = *(DataFixed2 + 1);
                        ++ReturnValueFixed2;
                        *(ReturnValueFixed2) = *(DataFixed2);
                        ++ReturnValueFixed2;
                        DataFixed2 += 4;
                    }
                }
            });
            return ReturnValue;
        }
    }
}