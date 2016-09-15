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

using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;
using System.IO;

namespace Structure.Sketching.Formats.Bmp.Format.PixelFormats.BaseClasses
{
    /// <summary>
    /// Pixel format base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public abstract class PixelFormatBase : IPixelFormat
    {
        /// <summary>
        /// The bytes per pixel for this format.
        /// </summary>
        /// <value>The BPP.</value>
        public abstract int BPP { get; }

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The decoded data</returns>
        public abstract byte[] Decode(int width, int height, byte[] data, Palette palette);

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The encoded data</returns>
        public abstract byte[] Encode(int width, int height, byte[] data, Palette palette);

        /// <summary>
        /// Reads the byte array from the stream
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>The byte array of the data</returns>
        public byte[] Read(int width, int height, Stream stream)
        {
            int dataWidth = width;
            int alignment = (4 - ((width * BPP) % 4)) % 4;
            int size = ((dataWidth * BPP) + alignment) * height;
            byte[] data = new byte[size];
            stream.Read(data, 0, size);
            return data;
        }
    }
}