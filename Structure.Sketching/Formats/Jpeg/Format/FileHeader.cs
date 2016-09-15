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

namespace Structure.Sketching.Formats.Jpeg.Format
{
    /// <summary>
    /// File header
    /// </summary>
    public class FileHeader
    {
        /// <summary>
        /// Gets the file header size.
        /// </summary>
        /// <value>The file header size.</value>
        public static int Size => 2;

        /// <summary>
        /// Gets the file type, 0xFF and 0xD8
        /// </summary>
        /// <value>The file type.</value>
        public byte[] Type => new byte[] { 0xFF, 0xD8 };

        /// <summary>
        /// Reads the header information from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting FileHeader object</returns>
        public static FileHeader Read(Stream stream)
        {
            var TempBuffer = new byte[2];
            stream.Read(TempBuffer, 0, 2);
            return new FileHeader();
        }

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Type);
        }
    }
}