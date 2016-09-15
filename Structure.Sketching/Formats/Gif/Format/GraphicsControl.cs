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
using Structure.Sketching.Formats.Gif.Format.Enums;
using Structure.Sketching.Helpers;
using Structure.Sketching.IO;
using System;
using System.IO;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Graphics control info holder
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class GraphicsControl : SectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsControl"/> class.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <param name="transparencyIndex">Index of the transparency.</param>
        /// <param name="transparencyFlag">if set to <c>true</c> [transparency flag].</param>
        /// <param name="disposalMethod">The disposal method.</param>
        public GraphicsControl(short delay, byte transparencyIndex, bool transparencyFlag, DisposalMethod disposalMethod)
        {
            Delay = delay;
            TransparencyIndex = transparencyIndex;
            TransparencyFlag = transparencyFlag;
            DisposalMethod = disposalMethod;
        }

        /// <summary>
        /// Gets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public short Delay { get; private set; }

        /// <summary>
        /// Gets the disposal method.
        /// </summary>
        /// <value>
        /// The disposal method.
        /// </value>
        public DisposalMethod DisposalMethod { get; private set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public static int Size => 6;

        /// <summary>
        /// Gets a value indicating whether [transparency flag].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [transparency flag]; otherwise, <c>false</c>.
        /// </value>
        public bool TransparencyFlag { get; private set; }

        /// <summary>
        /// Gets the index of the transparency.
        /// </summary>
        /// <value>
        /// The index of the transparency.
        /// </value>
        public byte TransparencyIndex { get; private set; }

        /// <summary>
        /// Reads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The resulting GraphicsControl object</returns>
        public static GraphicsControl Read(Stream stream)
        {
            byte[] TempBuffer = new byte[Size];
            stream.Read(TempBuffer, 0, TempBuffer.Length);
            byte Packed = TempBuffer[1];

            return new GraphicsControl(BitConverter.ToInt16(TempBuffer, 2),
                                        TempBuffer[4],
                                        (Packed & 0x01) == 1,
                                        (DisposalMethod)((Packed & 0x1C) >> 2));
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
            byte[] intro = {
                SectionTypes.ExtensionIntroducer,
                SectionTypes.GraphicControlLabel,
                4
            };

            writer.Write(intro);

            var field = new PackedField();
            field.SetBits(3, 3, (int)DisposalMethod);
            field.SetBit(6, false);
            field.SetBit(7, TransparencyFlag);

            writer.Write(field.Byte);
            writer.Write((ushort)Delay);
            writer.Write(TransparencyIndex);
            writer.Write(SectionTypes.Terminator);
            return true;
        }
    }
}