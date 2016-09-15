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

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Application extension
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class ApplicationExtension : SectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationExtension"/> class.
        /// </summary>
        /// <param name="repeatCount">The repeat count.</param>
        /// <param name="frameCount">The frame count.</param>
        public ApplicationExtension(ushort repeatCount, int frameCount)
        {
            FrameCount = frameCount;
            RepeatCount = repeatCount;
        }

        /// <summary>
        /// Gets or sets the frame count.
        /// </summary>
        /// <value>
        /// The frame count.
        /// </value>
        public int FrameCount { get; set; }

        /// <summary>
        /// Gets or sets the repeat count.
        /// </summary>
        /// <value>
        /// The repeat count.
        /// </value>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public static int Size => 12;

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Reads an application extension.</returns>
        public static ApplicationExtension Read(Stream stream)
        {
            Skip(stream, Size);
            return new ApplicationExtension(0, 0);
        }

        /// <summary>
        /// Writes the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>True if it writes successfully, false otherwise</returns>
        public override bool Write(EndianBinaryWriter writer)
        {
            if (RepeatCount != 1 && FrameCount > 0)
            {
                byte[] ext =
                {
                    SectionTypes.ExtensionIntroducer,
                    SectionTypes.ApplicationExtensionLabel,
                    (byte)Size
                };

                writer.Write(ext);

                writer.Write("NETSCAPE2.0");
                writer.Write((byte)3);
                writer.Write((byte)1);

                RepeatCount = (ushort)(Math.Max((ushort)0, RepeatCount) - 1);

                writer.Write(RepeatCount);

                writer.Write(SectionTypes.Terminator);
            }
            return true;
        }
    }
}