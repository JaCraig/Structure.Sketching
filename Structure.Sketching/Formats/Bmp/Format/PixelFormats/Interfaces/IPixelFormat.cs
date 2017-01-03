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

using System.IO;

namespace Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces
{
    /// <summary>
    /// Pixel format decoder/encoder interface
    /// </summary>
    public interface IPixelFormat
    {
        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// The decoded data
        /// </returns>
        byte[] Decode(Header header, byte[] data, Palette palette);

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>
        /// The encoded data
        /// </returns>
        byte[] Encode(Header header, byte[] data, Palette palette);

        /// <summary>
        /// Reads the byte array from the stream
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// The byte array of the data
        /// </returns>
        byte[] Read(Header header, Stream stream);
    }
}