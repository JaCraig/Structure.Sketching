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

using Structure.Sketching.Formats.Gif.Format.Interfaces;
using Structure.Sketching.IO;
using System.IO;

namespace Structure.Sketching.Formats.Gif.Format.BaseClasses
{
    /// <summary>
    /// Section base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.Interfaces.ISection" />
    public abstract class SectionBase : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionBase"/> class.
        /// </summary>
        protected SectionBase()
        {
        }

        /// <summary>
        /// Writes to the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>True if it writes successfully, false otherwise</returns>
        public abstract bool Write(EndianBinaryWriter writer);

        /// <summary>
        /// Skips the specified number of bytes in the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length.</param>
        /// <returns>The stream</returns>
        protected static Stream Skip(Stream stream, int length)
        {
            stream.Seek(length, SeekOrigin.Current);
            int Flag = stream.ReadByte();
            while (Flag != 0)
            {
                stream.Seek(Flag, SeekOrigin.Current);
                Flag = stream.ReadByte();
            }
            return stream;
        }
    }
}