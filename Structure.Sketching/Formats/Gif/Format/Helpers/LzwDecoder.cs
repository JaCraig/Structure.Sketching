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

using Structure.Sketching.ExtensionMethods;
using System.IO;

namespace Structure.Sketching.Formats.Gif.Format.Helpers
{
    /// <summary>
    /// LZW Decoder
    /// </summary>
    public class LZWDecoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LZWDecoder"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public LZWDecoder(Stream stream)
        {
            Stream = stream;
        }

        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public Stream Stream { get; set; }

        /// <summary>
        /// Decodes the stream to a byte array.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dataSize">Size of the data.</param>
        /// <returns>The resulting byte array.</returns>
        public byte[] Decode(int width, int height, int dataSize)
        {
            byte[] Pixels = new byte[width * height];
            int ClearCode = 1 << dataSize;
            int CodeSize = dataSize + 1;
            int EndCode = ClearCode + 1;
            int AvailableCode = ClearCode + 2;
            int OldCode = -1;
            int CodeMask = (1 << CodeSize) - 1;
            int Bits = 0;

            int[] Prefix = new int[4096];
            int[] Suffix = new int[4096];
            int[] PixelStack = new int[4097];

            int Top = 0;
            int Count = 0;
            int BI = 0;
            int XYZ = 0;

            int Data = 0;
            int First = 0;

            for (int x = 0; x < ClearCode; ++x)
            {
                Prefix[x] = 0;
                Suffix[x] = (byte)x;
            }

            byte[] Buffer = null;
            int Code = ClearCode;
            while (XYZ < Pixels.Length)
            {
                if (Top == 0)
                {
                    if (Bits < CodeSize)
                    {
                        if (Count == 0)
                        {
                            Buffer = ReadBlock(Stream);
                            Count = Buffer.Length;
                            if (Count == 0)
                            {
                                break;
                            }

                            BI = 0;
                        }

                        if (Buffer != null)
                        {
                            Data += Buffer[BI] << Bits;
                        }

                        Bits += 8;
                        BI++;
                        Count--;
                        continue;
                    }
                    Code = Data & CodeMask;
                    Data >>= CodeSize;
                    Bits -= CodeSize;

                    if (Code > AvailableCode || Code == EndCode)
                    {
                        break;
                    }

                    if (Code == ClearCode)
                    {
                        CodeSize = dataSize + 1;
                        CodeMask = (1 << CodeSize) - 1;
                        AvailableCode = ClearCode + 2;
                        OldCode = -1;
                        continue;
                    }

                    if (OldCode == -1)
                    {
                        PixelStack[Top++] = Suffix[Code];
                        OldCode = Code;
                        First = Code;
                        continue;
                    }

                    int inCode = Code;
                    if (Code == AvailableCode)
                    {
                        PixelStack[Top++] = (byte)First;

                        Code = OldCode;
                    }

                    while (Code > ClearCode)
                    {
                        PixelStack[Top++] = Suffix[Code];
                        Code = Prefix[Code];
                    }

                    First = Suffix[Code];
                    PixelStack[Top++] = Suffix[Code];

                    if (AvailableCode < 4096)
                    {
                        Prefix[AvailableCode] = OldCode;
                        Suffix[AvailableCode] = First;
                        AvailableCode++;
                        if (AvailableCode == CodeMask + 1 && AvailableCode < 4096)
                        {
                            CodeSize++;
                            CodeMask = (1 << CodeSize) - 1;
                        }
                    }

                    OldCode = inCode;
                }

                Top--;
                Pixels[XYZ++] = (byte)PixelStack[Top];
            }

            return Pixels;
        }

        /// <summary>
        /// Reads the next block.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The next block</returns>
        private byte[] ReadBlock(Stream stream)
        {
            var Size = stream.ReadByte();
            return stream.ReadBytes(Size);
        }
    }
}