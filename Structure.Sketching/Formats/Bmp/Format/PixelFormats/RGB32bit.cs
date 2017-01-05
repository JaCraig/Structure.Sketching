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
    /// RGB 32bit pixel format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public class RGB32bit : PixelFormatBase
    {
        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override double BPP => 4;

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
                for (int x = 0; x < width; ++x)
                {
                    int SourceX = x * (int)BPP;
                    int SourceOffset = SourceX + SourceRowOffset;
                    int DestinationX = x * 4;
                    int DestinationOffset = DestinationX + DestinationRowOffset;
                    ReturnValue[DestinationOffset] = data[SourceOffset + 3];
                    ReturnValue[DestinationOffset + 1] = data[SourceOffset + 2];
                    ReturnValue[DestinationOffset + 2] = data[SourceOffset + 1];
                    ReturnValue[DestinationOffset + 3] = header.AlphaMask == 0 ? (byte)255 : data[SourceOffset];
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
                for (int x = 0; x < width; ++x)
                {
                    int SourceX = x * 4;
                    int SourceOffset = SourceX + SourceRowOffset;
                    int DestinationX = x * (int)BPP;
                    int DestinationOffset = DestinationX + DestinationRowOffset;
                    ReturnValue[DestinationOffset] = data[SourceOffset + 2];
                    ReturnValue[DestinationOffset + 1] = data[SourceOffset + 1];
                    ReturnValue[DestinationOffset + 2] = data[SourceOffset];
                }
            });
            return ReturnValue;
        }
    }
}