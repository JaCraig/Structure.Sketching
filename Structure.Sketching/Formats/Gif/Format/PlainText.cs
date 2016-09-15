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
using System.IO;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Plain text
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class PlainText : SectionBase
    {
        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public static int Size => 13;

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The plain text object</returns>
        public static PlainText Read(Stream stream)
        {
            Skip(stream, Size);
            return new PlainText();
        }

        /// <summary>
        /// Writes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>True if it writes successfully, false otherwise</returns>
        public override bool Write(EndianBinaryWriter writer)
        {
            return true;
        }
    }
}