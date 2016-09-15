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

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// PNG file header
    /// </summary>
    public class FileHeader
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public byte[] header => new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        /// <summary>
        /// Reads the header information from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting FileHeader object</returns>
        public static FileHeader Read(Stream stream)
        {
            stream.Seek(8, SeekOrigin.Begin);
            return new FileHeader();
        }

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(header);
        }
    }
}