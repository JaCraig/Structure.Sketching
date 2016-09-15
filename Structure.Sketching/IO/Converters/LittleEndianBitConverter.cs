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

using Structure.Sketching.IO.Converters.BaseClasses;

namespace Structure.Sketching.IO.Converters
{
    /// <summary>
    /// Little endian bit converter
    /// </summary>
    /// <seealso cref="EndianBitConverterBase" />
    public class LittleEndianBitConverter : EndianBitConverterBase
    {
        /// <summary>
        /// Gets a value indicating whether this instance is little endian.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is little endian; otherwise, <c>false</c>.
        /// </value>
        public override bool IsLittleEndian => true;

        /// <summary>
        /// Copies the bytes implementation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected override void CopyBytesImpl(long value, int bytes, byte[] buffer, int index)
        {
            for (int i = 0; i < bytes; i++)
            {
                buffer[i + index] = unchecked((byte)(value & 0xff));
                value = value >> 8;
            }
        }

        /// <summary>
        /// Converts a byte array to a long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesToConvert">The bytes to convert.</param>
        /// <returns>
        /// The resulting long.
        /// </returns>
        protected override long FromBytes(byte[] value, int startIndex, int bytesToConvert)
        {
            long ret = 0;
            for (int i = 0; i < bytesToConvert; i++)
            {
                ret = unchecked((ret << 8) | value[startIndex + bytesToConvert - 1 - i]);
            }

            return ret;
        }
    }
}