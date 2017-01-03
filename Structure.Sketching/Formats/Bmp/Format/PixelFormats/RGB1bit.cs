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
    /// RGB 1bit pixel format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public class RGB1bit : PixelFormatBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RGB1bit"/> class.
        /// </summary>
        public RGB1bit()
        {
            byte Value = 1;
            ByteMasks = new byte[8];
            for (int x = 0; x < 8; ++x)
            {
                ByteMasks[x] = Value;
                Value = (byte)(Value << 1);
            }
        }

        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override int BPP => 1;

        /// <summary>
        /// Gets or sets the byte masks.
        /// </summary>
        /// <value>
        /// The byte masks.
        /// </value>
        private byte[] ByteMasks { get; set; }

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
                int SourceY = y * ((width / 8) + alignment);
                int DestinationY = height - y - 1;
                int SourceOffset = SourceY;
                int DestinationOffset = DestinationY * width * 4;
                for (int x = 0; x < width; ++x)
                {
                    for (int z = 0; z < 8; ++z)
                    {
                        if ((data[SourceOffset] & ByteMasks[z]) == 1)
                        {
                            ReturnValue[DestinationOffset] = 0;
                            ReturnValue[DestinationOffset + 1] = 0;
                            ReturnValue[DestinationOffset + 2] = 0;
                            ReturnValue[DestinationOffset + 3] = 255;
                        }
                        else
                        {
                            ReturnValue[DestinationOffset] = 255;
                            ReturnValue[DestinationOffset + 1] = 255;
                            ReturnValue[DestinationOffset + 2] = 255;
                            ReturnValue[DestinationOffset + 3] = 255;
                        }
                        DestinationOffset += 4;
                        if (DestinationOffset >= ReturnValue.Length)
                            break;
                    }
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