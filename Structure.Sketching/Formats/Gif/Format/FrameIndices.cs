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

using Structure.Sketching.Formats.Gif.Format.BaseClasses;
using Structure.Sketching.Formats.Gif.Format.Helpers;
using Structure.Sketching.IO;
using System.IO;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Frame indices
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class FrameIndices : SectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameIndices" /> class.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="bitDepth">The bit depth.</param>
        public FrameIndices(byte[] indices, byte bitDepth)
        {
            Indices = indices;
            BitDepth = bitDepth;
        }

        /// <summary>
        /// Gets or sets the bit depth.
        /// </summary>
        /// <value>
        /// The bit depth.
        /// </value>
        public byte BitDepth { get; set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public byte[] Indices { get; set; }

        /// <summary>
        /// Reads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>
        /// The resulting FrameIndices object
        /// </returns>
        public static FrameIndices Read(Stream stream, ImageDescriptor descriptor)
        {
            int DataSize = stream.ReadByte();
            var Decoder = new LZWDecoder(stream);
            return new FrameIndices(Decoder.Decode(descriptor.Width, descriptor.Height, DataSize), 0);
        }

        /// <summary>
        /// Writes to the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise
        /// </returns>
        public override bool Write(EndianBinaryWriter writer)
        {
            var encoder = new LZWEncoder(Indices, BitDepth);
            encoder.Encode(writer.BaseStream);
            return true;
        }
    }
}