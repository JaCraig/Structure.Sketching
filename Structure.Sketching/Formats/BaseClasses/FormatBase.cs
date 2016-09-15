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

using Structure.Sketching.Formats.Interfaces;
using System.IO;

namespace Structure.Sketching.Formats.BaseClasses
{
    /// <summary>
    /// Format base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Interfaces.IFormat"/>
    public abstract class FormatBase : IFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBase"/> class.
        /// </summary>
        protected FormatBase()
        {
        }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public abstract FileFormats Format { get; }

        /// <summary>
        /// Gets the decoder.
        /// </summary>
        /// <value>The decoder.</value>
        protected abstract IDecoder Decoder { get; }

        /// <summary>
        /// Gets the encoder.
        /// </summary>
        /// <value>The encoder.</value>
        protected abstract IEncoder Encoder { get; }

        /// <summary>
        /// Determines whether this instance can decode the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// True if it can, false otherwise
        /// </returns>
        public bool CanDecode(Stream stream)
        {
            if (stream == null) return false;
            return Decoder.CanDecode(stream);
        }

        /// <summary>
        /// Determines whether this instance can decode the specified header.
        /// </summary>
        /// <param name="header">The header data</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanDecode(byte[] header)
        {
            if (header == null) return false;
            return Decoder.CanDecode(header);
        }

        /// <summary>
        /// Determines whether this instance can decode the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanDecode(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            return Decoder.CanDecode(fileName);
        }

        /// <summary>
        /// Determines whether this instance can encode the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// True if it can, false otherwise
        /// </returns>
        public bool CanEncode(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            return Encoder.CanEncode(fileName);
        }

        /// <summary>
        /// Decodes the specified stream and returns an image
        /// </summary>
        /// <param name="stream">The stream containing the image data.</param>
        /// <returns>The resulting image</returns>
        public Image Decode(Stream stream)
        {
            return Decoder.Decode(stream);
        }

        /// <summary>
        /// Encodes an image and places it in the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="image">The image to encode.</param>
        /// <returns>True if it is encoded, false otherwise</returns>
        public bool Encode(BinaryWriter writer, Image image)
        {
            return Encoder.Encode(writer, image);
        }
    }
}