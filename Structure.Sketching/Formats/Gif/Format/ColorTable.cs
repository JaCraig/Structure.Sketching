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

using Structure.Sketching.Colors.ColorSpaces;
using Structure.Sketching.Formats.Gif.Format.BaseClasses;
using Structure.Sketching.IO;
using Structure.Sketching.Quantizers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Global color table
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class ColorTable : SectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorTable"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="BitDepth">The bit depth.</param>
        public ColorTable(QuantizedImage image, int BitDepth)
        {
            Bgra[] palette = image.Palette;
            int pixelCount = palette.Length;

            // Get max colors for bit depth.
            int colorTableLength = (int)Math.Pow(2, BitDepth) * 3;
            byte[] colorTable = new byte[colorTableLength];

            Parallel.For(0, pixelCount,
                i =>
                {
                    int offset = i * 3;
                    Bgra color = palette[i];

                    colorTable[offset] = color.Red;
                    colorTable[offset + 1] = color.Green;
                    colorTable[offset + 2] = color.Blue;
                });
            Data = colorTable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Format.ColorTable"/> class.
        /// </summary>
        /// <param name="data">The color table.</param>
        public ColorTable(byte[] data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the color table.
        /// </summary>
        /// <value>
        /// The color table.
        /// </value>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Reads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="size">The size.</param>
        /// <returns>
        /// The resulting global color table
        /// </returns>
        public static ColorTable Read(Stream stream, int size)
        {
            byte[] ColorTable = new byte[size * 3];
            stream.Read(ColorTable, 0, ColorTable.Length);
            return new ColorTable(ColorTable);
        }

        /// <summary>
        /// Writes to the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>True if it writes successfully, false otherwise</returns>
        public override bool Write(EndianBinaryWriter writer)
        {
            writer.Write(Data, 0, Data.Length);
            return true;
        }
    }
}