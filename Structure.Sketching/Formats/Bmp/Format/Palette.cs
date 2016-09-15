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

using System;
using System.IO;

namespace Structure.Sketching.Formats.Bmp.Format
{
    /// <summary>
    /// Palette used by the bitmap
    /// </summary>
    public class Palette
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Palette" /> class.
        /// </summary>
        /// <param name="numberOfColors">The number of colors.</param>
        /// <param name="data">The data.</param>
        public Palette(int numberOfColors, byte[] data)
        {
            NumberOfColors = numberOfColors;
            if (NumberOfColors > 0)
            {
                Data = new byte[NumberOfColors * 4];
                Array.Copy(data, Data, NumberOfColors * 4);
            }
        }

        /// <summary>
        /// Gets the data inside the palette
        /// </summary>
        /// <value>The data inside the palette</value>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets or sets the number of colors.
        /// </summary>
        /// <value>The number of colors.</value>
        public int NumberOfColors { get; private set; }

        /// <summary>
        /// Reads the specified palette information
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting palette information</returns>
        public static Palette Read(Header header, Stream stream)
        {
            var NumberOfColors = header.ColorsUsed == 0 && header.BPP < 16 ? (int)Math.Pow(2, header.BPP) : header.ColorsUsed;
            var Data = new byte[NumberOfColors * 4];
            if (NumberOfColors > 0)
                stream.Read(Data, 0, NumberOfColors * 4);
            return new Palette(NumberOfColors, Data);
        }

        /// <summary>
        /// Writes the data to the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer)
        {
        }
    }
}