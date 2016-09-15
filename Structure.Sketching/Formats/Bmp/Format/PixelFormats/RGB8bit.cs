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
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Bmp.Format.PixelFormats
{
    /// <summary>
    /// RGB 8bit pixel format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public class RGB8bit : PixelFormatBase
    {
        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override int BPP => 1;

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
            int ppb = 8 / BPP;
            int mask = 0xFF >> (8 - BPP);
            byte[] ReturnValue = new byte[width * height * 4];
            Parallel.For(0, height, y =>
            {
                int RowOffset = y * (width + alignment);
                int CurrentY = height - y - 1;
                if (CurrentY < 0)
                    CurrentY = 0;
                if (CurrentY >= height)
                    CurrentY = height - 1;
                for (int x = 0; x < width; ++x)
                {
                    int Offset = RowOffset + x;
                    int colOffset = x * ppb;

                    for (int shift = 0; shift < ppb && (colOffset + shift) < width; shift++)
                    {
                        int colorIndex = ((data[Offset] >> (8 - BPP - (shift * BPP))) & mask) * 4;
                        int arrayOffset = ((CurrentY * width) + (colOffset + shift)) * 4;
                        ReturnValue[arrayOffset] = palette.Data[colorIndex + 2];
                        ReturnValue[arrayOffset + 1] = palette.Data[colorIndex + 1];
                        ReturnValue[arrayOffset + 2] = palette.Data[colorIndex];
                        ReturnValue[arrayOffset + 3] = 1;
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
            return data;
        }
    }
}