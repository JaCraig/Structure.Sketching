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
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// The decoded data
        /// </returns>
        public override byte[] Decode(Header header, byte[] data, Palette palette)
        {
            int width = header.Width;
            int height = header.Height;
            int alignment = (4 - ((width * BPP) % 4)) % 4;
            byte[] ReturnValue = new byte[width * height * 4];
            Parallel.For(0, height, y =>
            {
                int SourceY = y * (width + alignment);
                int DestinationY = height - y - 1;
                int SourceOffset = SourceY;
                int DestinationOffset = DestinationY * width * 4;
                for (int x = 0; x < width; ++x)
                {
                    int ColorIndex = data[SourceOffset] * 4;
                    ReturnValue[DestinationOffset] = palette.Data[ColorIndex + 2];
                    ReturnValue[DestinationOffset + 1] = palette.Data[ColorIndex + 1];
                    ReturnValue[DestinationOffset + 2] = palette.Data[ColorIndex];
                    ReturnValue[DestinationOffset + 3] = palette.Data[ColorIndex + 3];
                    DestinationOffset += 4;
                    ++SourceOffset;
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
        /// <returns>
        /// The encoded data
        /// </returns>
        public override byte[] Encode(Header header, byte[] data, Palette palette)
        {
            return data;
        }
    }
}