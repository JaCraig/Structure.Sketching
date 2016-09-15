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

using Structure.Sketching.Formats.Jpeg.Format.HelperClasses.Exceptions;
using System;

namespace Structure.Sketching.Formats.Jpeg.Format.HelperClasses
{
    /// <summary>
    /// Huffman class
    /// </summary>
    public class Huffman
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Huffman"/> class.
        /// </summary>
        public Huffman()
        {
            LookUpTable = new ushort[256];
            DecodedValues = new byte[256];
            MinimumCode = new int[16];
            MaximumCode = new int[16];
            ValueIndices = new int[16];
            NumberOfCodes = 0;
        }

        /// <summary>
        /// Gets the decoded values.
        /// </summary>
        /// <value>
        /// The decoded values.
        /// </value>
        public byte[] DecodedValues { get; private set; }

        /// <summary>
        /// Gets the look up table.
        /// </summary>
        /// <value>
        /// The look up table.
        /// </value>
        public ushort[] LookUpTable { get; private set; }

        /// <summary>
        /// Gets the maximum code.
        /// </summary>
        /// <value>
        /// The maximum code.
        /// </value>
        public int[] MaximumCode { get; private set; }

        /// <summary>
        /// Gets the minimum code.
        /// </summary>
        /// <value>
        /// The minimum code.
        /// </value>
        public int[] MinimumCode { get; private set; }

        /// <summary>
        /// Gets the number of codes.
        /// </summary>
        /// <value>
        /// The number of codes.
        /// </value>
        public int NumberOfCodes { get; set; }

        /// <summary>
        /// Gets the value indices.
        /// </summary>
        /// <value>
        /// The value indices.
        /// </value>
        public int[] ValueIndices { get; private set; }

        /// <summary>
        /// Decodes the byte buffer using the huffman class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">uninitialized Huffman table
        /// or
        /// bad Huffman code</exception>
        public byte DecodeHuffman(ByteBuffer bytes)
        {
            if (NumberOfCodes == 0)
                throw new Exception("uninitialized Huffman table");

            if (bytes.Bits.NumberOfUnreadBits < 8)
            {
                try
                {
                    bytes.EnsureNBits(8);
                }
                catch (MissingFF00)
                {
                    if (bytes.UnreadableBytes != 0)
                        bytes.UnreadByteStuffedByte();
                    goto slowPath;
                }
                catch (ShortHuffmanData)
                {
                    if (bytes.UnreadableBytes != 0)
                        bytes.UnreadByteStuffedByte();
                    goto slowPath;
                }
            }

            ushort v = LookUpTable[(bytes.Bits.Accumulator >> (bytes.Bits.NumberOfUnreadBits - 8)) & 0xff];
            if (v != 0)
            {
                var n = (byte)((v & 0xff) - 1);
                bytes.Bits.NumberOfUnreadBits -= n;
                bytes.Bits.Mask >>= n;
                return (byte)(v >> 8);
            }

            slowPath:
            int code = 0;
            for (int i = 0; i < 16; i++)
            {
                if (bytes.Bits.NumberOfUnreadBits == 0)
                    bytes.EnsureNBits(1);
                if ((bytes.Bits.Accumulator & bytes.Bits.Mask) != 0)
                    code |= 1;
                bytes.Bits.NumberOfUnreadBits--;
                bytes.Bits.Mask >>= 1;
                if (code <= MaximumCode[i])
                    return DecodedValues[ValueIndices[i] + code - MinimumCode[i]];
                code <<= 1;
            }

            throw new Exception("bad Huffman code");
        }
    }
}