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

namespace Structure.Sketching.Formats.Gif.Format.Helpers
{
    /// <summary>
    /// LZW Encoder class
    /// </summary>
    public class LZWEncoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LZWEncoder"/> class.
        /// </summary>
        /// <param name="indexedPixels">The indexed pixels.</param>
        /// <param name="colorDepth">The color depth.</param>
        public LZWEncoder(byte[] indexedPixels, int colorDepth)
        {
            pixelArray = indexedPixels;
            initialCodeSize = Math.Max(2, colorDepth);
        }

        private const int Bits = 12;
        private const int Eof = -1;
        private const int HashSize = 5003;

        private readonly byte[] accumulators = new byte[256];

        private readonly int[] codeTable = new int[HashSize];
        private readonly int[] hashTable = new int[HashSize];
        private readonly int initialCodeSize;

        private readonly int[] masks =
            {
                0x0000, 0x0001, 0x0003, 0x0007, 0x000F, 0x001F, 0x003F, 0x007F, 0x00FF,
                0x01FF, 0x03FF, 0x07FF, 0x0FFF, 0x1FFF, 0x3FFF, 0x7FFF, 0xFFFF
            };

        private readonly byte[] pixelArray;

        private int accumulatorCount;
        private int bitCount;

        private int clearCode;

        private bool clearFlag;

        private int curPixel;
        private int currentAccumulator;

        private int currentBits;

        private int eofCode;

        private int freeEntry;

        private int globalInitialBits;

        private int hsize = HashSize;

        private int maxbits = Bits;

        private int maxcode;

        private int maxmaxcode = 1 << Bits;

        /// <summary>
        /// Encodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Encode(Stream stream)
        {
            stream.WriteByte((byte)initialCodeSize);
            curPixel = 0;
            Compress(initialCodeSize + 1, stream);
            stream.WriteByte(SectionTypes.Terminator);
        }

        /// <summary>
        /// Gets the maxcode.
        /// </summary>
        /// <param name="bitCount">The bit count.</param>
        /// <returns></returns>
        private static int GetMaxcode(int bitCount)
        {
            return (1 << bitCount) - 1;
        }

        /// <summary>
        /// Adds the character.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="stream">The stream.</param>
        private void AddCharacter(byte c, Stream stream)
        {
            accumulators[accumulatorCount++] = c;
            if (accumulatorCount >= 254)
            {
                FlushPacket(stream);
            }
        }

        /// <summary>
        /// Clears the block.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private void ClearBlock(Stream stream)
        {
            ResetCodeTable(hsize);
            freeEntry = clearCode + 2;
            clearFlag = true;

            Output(clearCode, stream);
        }

        /// <summary>
        /// Compresses the specified intial bits.
        /// </summary>
        /// <param name="intialBits">The intial bits.</param>
        /// <param name="stream">The stream.</param>
        private void Compress(int intialBits, Stream stream)
        {
            int fcode;
            int c;
            int ent;
            int hsizeReg;
            int hshift;

            // Set up the globals:  globalInitialBits - initial number of bits
            globalInitialBits = intialBits;

            // Set up the necessary values
            clearFlag = false;
            bitCount = globalInitialBits;
            maxcode = GetMaxcode(bitCount);

            clearCode = 1 << (intialBits - 1);
            eofCode = clearCode + 1;
            freeEntry = clearCode + 2;

            accumulatorCount = 0; // clear packet

            ent = NextPixel();

            hshift = 0;
            for (fcode = hsize; fcode < 65536; fcode *= 2)
            {
                ++hshift;
            }
            hshift = 8 - hshift; // set hash code range bound

            hsizeReg = hsize;

            ResetCodeTable(hsizeReg); // clear hash table

            Output(clearCode, stream);

            while ((c = NextPixel()) != Eof)
            {
                fcode = (c << maxbits) + ent;
                int i = (c << hshift) ^ ent /* = 0 */;

                if (hashTable[i] == fcode)
                {
                    ent = codeTable[i];
                    continue;
                }

                // Non-empty slot
                if (hashTable[i] >= 0)
                {
                    int disp = hsizeReg - i;
                    if (i == 0)
                        disp = 1;
                    do
                    {
                        if ((i -= disp) < 0)
                        {
                            i += hsizeReg;
                        }

                        if (hashTable[i] == fcode)
                        {
                            ent = codeTable[i];
                            break;
                        }
                    }
                    while (hashTable[i] >= 0);

                    if (hashTable[i] == fcode)
                    {
                        continue;
                    }
                }

                Output(ent, stream);
                ent = c;
                if (freeEntry < maxmaxcode)
                {
                    codeTable[i] = freeEntry++; // code -> hashtable
                    hashTable[i] = fcode;
                }
                else ClearBlock(stream);
            }

            // Put out the final code.
            Output(ent, stream);

            Output(eofCode, stream);
        }

        /// <summary>
        /// Flushes the packet.
        /// </summary>
        /// <param name="outs">The outs.</param>
        private void FlushPacket(Stream outs)
        {
            if (accumulatorCount > 0)
            {
                outs.WriteByte((byte)accumulatorCount);
                outs.Write(accumulators, 0, accumulatorCount);
                accumulatorCount = 0;
            }
        }

        /// <summary>
        /// Nexts the pixel.
        /// </summary>
        /// <returns></returns>
        private int NextPixel()
        {
            if (curPixel == pixelArray.Length)
            {
                return Eof;
            }

            if (curPixel == pixelArray.Length)
                return Eof;

            curPixel++;
            return pixelArray[curPixel - 1] & 0xff;
        }

        /// <summary>
        /// Outputs the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="outs">The outs.</param>
        private void Output(int code, Stream outs)
        {
            currentAccumulator &= masks[currentBits];

            if (currentBits > 0) currentAccumulator |= code << currentBits;
            else currentAccumulator = code;

            currentBits += bitCount;

            while (currentBits >= 8)
            {
                AddCharacter((byte)(currentAccumulator & 0xff), outs);
                currentAccumulator >>= 8;
                currentBits -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if (freeEntry > maxcode || clearFlag)
            {
                if (clearFlag)
                {
                    maxcode = GetMaxcode(bitCount = globalInitialBits);
                    clearFlag = false;
                }
                else
                {
                    ++bitCount;
                    maxcode = bitCount == maxbits
                        ? maxmaxcode
                        : GetMaxcode(bitCount);
                }
            }

            if (code == eofCode)
            {
                // At EOF, write the rest of the buffer.
                while (currentBits > 0)
                {
                    AddCharacter((byte)(currentAccumulator & 0xff), outs);
                    currentAccumulator >>= 8;
                    currentBits -= 8;
                }

                FlushPacket(outs);
            }
        }

        /// <summary>
        /// Resets the code table.
        /// </summary>
        /// <param name="size">The size.</param>
        private void ResetCodeTable(int size)
        {
            for (int i = 0; i < size; ++i)
            {
                hashTable[i] = -1;
            }
        }
    }
}