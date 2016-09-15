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
    /// Bitmap file header
    /// </summary>
    public class FileHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileHeader"/> class.
        /// </summary>
        /// <param name="fileSize">Size of the file.</param>
        /// <param name="offset">The offset.</param>
        public FileHeader(int fileSize, int offset)
        {
            FileSize = fileSize;
            Offset = offset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHeader"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public FileHeader(byte[] data)
            : this(BitConverter.ToInt32(data, 2), BitConverter.ToInt32(data, 10))
        {
        }

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <value>The size of the file.</value>
        public int FileSize { get; private set; }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        /// <value>The offset.</value>
        public int Offset { get; private set; }

        /// <summary>
        /// Gets the reserved.
        /// </summary>
        /// <value>The reserved.</value>
        public int Reserved => 0;

        /// <summary>
        /// Gets the file header size.
        /// </summary>
        /// <value>The file header size.</value>
        public static int Size => 14;

        /// <summary>
        /// Gets the file type, 'BM'
        /// </summary>
        /// <value>The file type.</value>
        public short Type => 19778;

        /// <summary>
        /// Reads the header information from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting FileHeader object</returns>
        public static FileHeader Read(Stream stream)
        {
            byte[] data = new byte[Size];
            stream.Read(data, 0, Size);
            return new FileHeader(data);
        }

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(FileSize);
            writer.Write(Reserved);
            writer.Write(Offset);
        }
    }
}