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

using Structure.Sketching.Formats.Bmp.Format.PixelFormats;
using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats.Bmp.Format
{
    /// <summary>
    /// BMP body
    /// </summary>
    public class Body
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Body"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Body(byte[] data)
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Body"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public Body(Image image)
            : this(new RGB24bit().Encode(image.Width, image.Height, image.Pixels.SelectMany(x => (byte[])x).ToArray(), null))
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public byte[] Data { get; set; }

        /// <summary>
        /// The pixel formats
        /// </summary>
        private static Dictionary<int, IPixelFormat> PixelFormats = new Dictionary<int, IPixelFormat>
        {
            [32] = new RGB32bit(),
            [24] = new RGB24bit(),
            [16] = new RGB16bit(),
            [8] = new RGB8bit(),
            [4] = new RGB4bit()
        };

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting Body information</returns>
        public static Body Read(Header header, Palette palette, Stream stream)
        {
            var Data = PixelFormats[header.BPP].Read(header, stream);
            var Data2 = PixelFormats[header.BPP].Decode(header.Width, header.Height, Data, palette);
            return new Body(Data2);
        }

        /// <summary>
        /// Writes to the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
            int Amount = (Data.Length * 3) % 4;
            if (Amount != 0)
            {
                Amount = 4 - Amount;
            }
            writer.Write(Data, 0, Data.Length);
            for (int x = 0; x < Amount; ++x)
            {
                writer.Write((byte)0);
            }
        }
    }
}