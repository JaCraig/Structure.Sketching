﻿/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Structure.Sketching.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Bmp.Format.PixelFormats
{
    /// <summary>
    /// RGB 24bit pixel format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public unsafe class RGB24bit : PixelFormatBase
    {
        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override int BPP => 3;

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The decoded data</returns>
        public override byte[] Decode(int width, int height, byte[] data, Palette palette)
        {
            int alignment = (4 - ((width * BPP) % 4)) % 4;
            byte[] ReturnValue = new byte[width * height * 4];
            Parallel.For(0, height, y =>
            {
                int SourceY = height - y - 1;
                if (SourceY < 0)
                    SourceY = 0;
                if (SourceY >= height)
                    SourceY = height - 1;
                int SourceRowOffset = SourceY * ((width * BPP) + alignment);
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
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The encoded data</returns>
        public override byte[] Encode(int width, int height, byte[] data, Palette palette)
        {
            int alignment = (4 - ((width * BPP) % 4)) % 4;
            var ReturnValue = new byte[((width * BPP) + alignment) * height];
            Parallel.For(0, height, y =>
            {
                int SourceY = height - y - 1;
                if (SourceY < 0)
                    SourceY = 0;
                if (SourceY >= height)
                    SourceY = height - 1;
                int SourceRowOffset = SourceY * width * 4;
                int DestinationY = y;
                int DestinationRowOffset = DestinationY * ((width * BPP) + alignment);
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