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
using Structure.Sketching.IO;
using System;
using System.IO;
using System.Text;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Comment section
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class Comment : SectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Comment(string data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; private set; }

        /// <summary>
        /// Reads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting comment</returns>
        public static Comment Read(Stream stream)
        {
            var Size = stream.ReadByte();
            var Builder = new StringBuilder();

            while (Size != 0)
            {
                byte[] TempBuffer = new byte[Size];
                stream.Read(TempBuffer, 0, Size);
                Size = stream.ReadByte();
                Builder.Append(BitConverter.ToString(TempBuffer));
            }
            return new Comment(Builder.ToString());
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
            return true;
        }
    }
}