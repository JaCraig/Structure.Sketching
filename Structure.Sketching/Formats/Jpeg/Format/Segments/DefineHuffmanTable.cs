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

using Structure.Sketching.Formats.Jpeg.Format.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats.Jpeg.Format.Segments
{
    /// <summary>
    /// Define huffman table segment
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Jpeg.Format.Segments.SegmentBase"/>
    public class DefineHuffmanTable : SegmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefineHuffmanTable"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public DefineHuffmanTable(ByteBuffer buffer)
            : base(SegmentTypes.DefineHuffmanTable, buffer)
        {
            HuffmanCodes = new Huffman[MAXIMUM_TC + 1, MAXIMUM_TH + 1];
            for (int i = 0; i < MAXIMUM_TC + 1; i++)
                for (int j = 0; j < MAXIMUM_TH + 1; j++)
                    HuffmanCodes[i, j] = new Huffman();
            for (int i = 0; i < theHuffmanSpec.Length; i++)
            {
                HuffmanLookUpTables[i] = new HuffmanLookUpTable(theHuffmanSpec[i]);
            }
        }

        /// <summary>
        /// Gets the huffman codes.
        /// </summary>
        /// <value>The huffman codes.</value>
        public Huffman[,] HuffmanCodes { get; private set; }

        /// <summary>
        /// The huffman look up tables
        /// </summary>
        public HuffmanLookUpTable[] HuffmanLookUpTables = new HuffmanLookUpTable[4];

        private const int LOOKUP_TABLE_SIZE = 8;
        private const int MAXIMUM_CODE_LENGTH = 16;
        private const int MAXIMUM_NUMBER_CODES = 256;
        private const int MAXIMUM_TC = 1;
        private const int MAXIMUM_TH = 3;

        /// <summary>
        /// The huffman spec
        /// </summary>
        private readonly HuffmanSpec[] theHuffmanSpec =
        {
            // Luminance DC.
            new HuffmanSpec(
                new byte[] { 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }),
            new HuffmanSpec(
                new byte[] { 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 125 },
                new byte[]
                {
                    0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12,
                    0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07,
                    0x22, 0x71, 0x14, 0x32, 0x81, 0x91, 0xa1, 0x08,
                    0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0,
                    0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0a, 0x16,
                    0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28,
                    0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
                    0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
                    0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
                    0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
                    0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
                    0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
                    0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98,
                    0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
                    0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
                    0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5,
                    0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2, 0xd3, 0xd4,
                    0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2,
                    0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
                    0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
                    0xf9, 0xfa
                }),
            new HuffmanSpec(
                new byte[] { 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 },
                new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }),

            // Chrominance AC.
            new HuffmanSpec(
                new byte[] { 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 119 },
                new byte[]
                {
                    0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21,
                    0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71,
                    0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
                    0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0,
                    0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16, 0x24, 0x34,
                    0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26,
                    0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38,
                    0x39, 0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
                    0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
                    0x59, 0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
                    0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
                    0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
                    0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96,
                    0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
                    0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
                    0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3,
                    0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2,
                    0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda,
                    0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
                    0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
                    0xf9, 0xfa
                })
        };

        /// <summary>
        /// Setups the specified segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <exception cref="System.Exception">
        /// DHT has wrong length or bad Tc value or bad Th value or Huffman table has zero length or
        /// Huffman table has excessive length or DHT has wrong length
        /// </exception>
        public override void Setup(IEnumerable<SegmentBase> segments)
        {
            Length = GetLength(Bytes);
            var Frame = segments.OfType<StartOfFrame>().FirstOrDefault();
            var n = Length;
            while (n > 0)
            {
                if (n < 17)
                    throw new Exception("DHT has wrong length");
                Bytes.ReadFull(TempData, 0, 17);

                int tc = TempData[0] >> 4;
                if (tc > MAXIMUM_TC)
                    throw new Exception("bad Tc value");

                int th = TempData[0] & 0x0f;
                if (th > MAXIMUM_TH || (Frame != null && !Frame.Progressive && th > 1))
                    throw new Exception("bad Th value");

                Huffman h = HuffmanCodes[tc, th];

                h.NumberOfCodes = 0;

                int[] ncodes = new int[MAXIMUM_CODE_LENGTH];
                for (int i = 0; i < ncodes.Length; i++)
                {
                    ncodes[i] = TempData[i + 1];
                    h.NumberOfCodes += ncodes[i];
                }

                if (h.NumberOfCodes == 0)
                    throw new Exception("Huffman table has zero length");
                if (h.NumberOfCodes > MAXIMUM_NUMBER_CODES)
                    throw new Exception("Huffman table has excessive length");

                n -= h.NumberOfCodes + 17;
                if (n < 0)
                    throw new Exception("DHT has wrong length");

                Bytes.ReadFull(h.DecodedValues, 0, h.NumberOfCodes);

                // Derive the look-up table.
                for (int i = 0; i < h.LookUpTable.Length; i++)
                    h.LookUpTable[i] = 0;

                uint x = 0, code = 0;

                for (int i = 0; i < LOOKUP_TABLE_SIZE; i++)
                {
                    code <<= 1;

                    for (int j = 0; j < ncodes[i]; j++)
                    {
                        var base2 = (byte)(code << (7 - i));
                        var lutValue = (ushort)(((ushort)h.DecodedValues[x] << 8) | (2 + i));
                        for (int k = 0; k < 1 << (7 - i); k++)
                            h.LookUpTable[base2 | k] = lutValue;
                        code++;
                        x++;
                    }
                }

                // Derive minCodes, maxCodes, and valsIndices.
                int c = 0, index = 0;
                for (int i = 0; i < ncodes.Length; i++)
                {
                    int nc = ncodes[i];
                    if (nc == 0)
                    {
                        h.MinimumCode[i] = -1;
                        h.MaximumCode[i] = -1;
                        h.ValueIndices[i] = -1;
                    }
                    else
                    {
                        h.MinimumCode[i] = c;
                        h.MaximumCode[i] = c + nc - 1;
                        h.ValueIndices[i] = index;
                        c += nc;
                        index += nc;
                    }
                    c <<= 1;
                }
            }
        }

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public override void Write(BinaryWriter writer)
        {
            byte[] headers = { 0x00, 0x10, 0x01, 0x11 };
            Length = 2;
            HuffmanSpec[] specs = theHuffmanSpec;

            foreach (var s in specs)
            {
                Length += 1 + 16 + s.Values.Length;
            }
            WriteSegmentHeader(writer);

            for (int i = 0; i < specs.Length; i++)
            {
                var s = specs[i];
                writer.Write(headers[i]);
                writer.Write(s.Count, 0, s.Count.Length);
                writer.Write(s.Values, 0, s.Values.Length);
            }
        }
    }
}