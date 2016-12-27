/*
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
using System;
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Bmp.Format.PixelFormats
{
    /// <summary>
    /// RGB 16bit pixel format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public class RGB16bit : PixelFormatBase
    {
        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override int BPP => 2;

        /// <summary>
        /// The RGB 16bit B mask
        /// </summary>
        private const int RGB16BitBMask = 0x0000001F;

        /// <summary>
        /// /// The RGB 16bit G mask
        /// </summary>
        private const int RGB16BitGMask = 0x000003E0;

        /// <summary>
        /// The RGB 16bit R mask
        /// </summary>
        private const int RGB16BitRMask = 0x00007C00;

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
                int RowOffset = y * ((width * BPP) + alignment);
                int CurrentY = height - y - 1;
                if (CurrentY < 0)
                    CurrentY = 0;
                if (CurrentY >= height)
                    CurrentY = height - 1;
                for (int x = 0; x < width; ++x)
                {
                    int Offset = RowOffset + (x * BPP);
                    var TempValue = BitConverter.ToInt16(data, Offset);
                    int r = ((TempValue & RGB16BitRMask) >> 11) << 3;
                    int g = ((TempValue & RGB16BitGMask) >> 5) << 2;
                    int b = (TempValue & RGB16BitBMask) << 3;

                    int ArrayOffset = ((CurrentY * width) + x) * 4;
                    ReturnValue[ArrayOffset] = (byte)r;
                    ReturnValue[ArrayOffset + 1] = (byte)g;
                    ReturnValue[ArrayOffset + 2] = (byte)b;
                    ReturnValue[ArrayOffset + 3] = 255;
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
                for (int x = 0; x < width; ++x)
                {
                    int SourceX = x * 4;
                    int SourceOffset = SourceX + SourceRowOffset;
                    int DestinationX = x * BPP;
                    int DestinationOffset = DestinationX + DestinationRowOffset;
                    int r = data[SourceOffset + 2] >> 3;
                    int g = data[SourceOffset + 1] >> 2;
                    int b = data[SourceOffset] >> 3;
                    var TempValue = (short)((r << 11) | (g << 5) | b);
                    var Values = BitConverter.GetBytes(TempValue);

                    ReturnValue[DestinationOffset] = Values[0];
                    ReturnValue[DestinationOffset + 1] = Values[1];
                }
            });
            return ReturnValue;
        }
    }
}