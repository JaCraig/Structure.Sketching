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

using Structure.Sketching.Colors.ColorSpaces;
using Structure.Sketching.Formats.Jpeg.Format.Enums;
using Structure.Sketching.Formats.Jpeg.Format.HelperClasses;
using Structure.Sketching.Formats.Jpeg.Format.HelperClasses.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats.Jpeg.Format.Segments
{
    /// <summary>
    /// Start of scan segment
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Jpeg.Format.Segments.SegmentBase"/>
    public class StartOfScan : SegmentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartOfScan"/> class.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public StartOfScan(ByteBuffer bytes)
            : base(SegmentTypes.StartOfScan, bytes)
        {
            ProgressiveCoefficients = new Block[MAXIMUM_NUMBER_COMPONENTS][];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartOfScan"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="defineHuffmanTableSegment">The define huffman table segment.</param>
        /// <param name="defineQuantizationTableSegment">The define quantization table segment.</param>
        public StartOfScan(Image image, DefineHuffmanTable defineHuffmanTableSegment, DefineQuantizationTable defineQuantizationTableSegment)
            : base(SegmentTypes.StartOfScan, null)
        {
            Image = image;
            DefineHuffmanTableSegment = defineHuffmanTableSegment;
            DefineQuantizationTableSegment = defineQuantizationTableSegment;
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the define huffman table segment.
        /// </summary>
        /// <value>The define huffman table segment.</value>
        private DefineHuffmanTable DefineHuffmanTableSegment { get; set; }

        /// <summary>
        /// Gets or sets the define quantization table segment.
        /// </summary>
        /// <value>The define quantization table segment.</value>
        private DefineQuantizationTable DefineQuantizationTableSegment { get; set; }

        /// <summary>
        /// The grey image
        /// </summary>
        public GreyImage img1;

        private const int acTable = 1;
        private const int dcTable = 0;
        private const int MAXIMUM_NUMBER_COMPONENTS = 4;

        private const int MAXIMUM_TH = 3;

        private static readonly int[] unzig =
        {
            0, 1, 8, 16, 9, 2, 3, 10,
            17, 24, 32, 25, 18, 11, 4, 5,
            12, 19, 26, 33, 40, 48, 41, 34,
            27, 20, 13, 6, 7, 14, 21, 28,
            35, 42, 49, 56, 57, 50, 43, 36,
            29, 22, 15, 23, 30, 37, 44, 51,
            58, 59, 52, 45, 38, 31, 39, 46,
            53, 60, 61, 54, 47, 55, 62, 63
        };

        private readonly byte[] bitCount =
        {
            0, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8
        };

        /// <summary>
        /// The sos header for y cb cr
        /// </summary>
        private readonly byte[] sosHeaderYCbCr =
        {
            0xff, 0xda, 0x00, 0x0c, 0x03, 0x01, 0x00, 0x02,
            0x11, 0x03, 0x11, 0x00, 0x3f, 0x00
        };

        private uint bits;
        private ushort EndOfBlockRun;
        private YcbcrImg img3;
        private uint nBits;
        private Block[][] ProgressiveCoefficients;

        /// <summary>
        /// Converts the scan information into an image
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="segments">The segments.</param>
        /// <returns>The resulting image</returns>
        public Image Convert(Image image, IEnumerable<SegmentBase> segments)
        {
            var Frame = segments.OfType<StartOfFrame>().FirstOrDefault();
            if (this.img1 != null)
            {
                return img1.Convert(Frame.Width, Frame.Height, image, segments);
            }

            if (this.img3 != null)
            {
                return img3.Convert(Frame.Width, Frame.Height, image, segments);
            }
            return image;
        }

        /// <summary>
        /// Setups the specified segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <exception cref="System.Exception">
        /// missing SOF marker or SOS has wrong length or SOS length inconsistent with number of
        /// components or Unknown component selector or Repeated component selector or bad Td value
        /// or bad Ta value or Total sampling factors too large. or bad spectral selection bounds or
        /// progressive AC coefficients for more than one component or bad successive approximation
        /// values or Excessive DC component or Too many components or Too many components or Bad RST marker
        /// </exception>
        public override void Setup(IEnumerable<SegmentBase> segments)
        {
            Length = GetLength(Bytes);
            var n = Length;
            var StartOfFrameSegment = segments.OfType<StartOfFrame>().FirstOrDefault();
            var DefineHuffmanTableSegment = segments.OfType<DefineHuffmanTable>().FirstOrDefault();
            var DefineRestartIntervalSegment = segments.OfType<DefineRestartInterval>().FirstOrDefault() ?? new DefineRestartInterval(null);
            var DefineQuantizationTableSegment = segments.OfType<DefineQuantizationTable>().FirstOrDefault();
            if (StartOfFrameSegment == null)
            {
                throw new Exception("missing SOF marker");
            }

            if (n < 6 || 4 + (2 * (int)StartOfFrameSegment.TypeOfImage) < n || n % 2 != 0)
            {
                throw new Exception("SOS has wrong length");
            }

            Bytes.ReadFull(TempData, 0, n);
            byte lnComp = TempData[0];

            if (n != 4 + 2 * lnComp)
            {
                throw new Exception("SOS length inconsistent with number of components");
            }

            Scan[] scan = new Scan[MAXIMUM_NUMBER_COMPONENTS];
            int totalHV = 0;

            for (int i = 0; i < lnComp; i++)
            {
                int cs = TempData[1 + (2 * i)];
                int compIndex = -1;
                for (int j = 0; j < (int)StartOfFrameSegment.TypeOfImage; j++)
                {
                    Component compv = StartOfFrameSegment.Components[j];
                    if (cs == compv.ComponentIdentifier)
                    {
                        compIndex = j;
                    }
                }

                if (compIndex < 0)
                {
                    throw new Exception("Unknown component selector");
                }

                scan[i].ComponentIndex = (byte)compIndex;
                for (int j = 0; j < i; j++)
                {
                    if (scan[i].ComponentIndex == scan[j].ComponentIndex)
                    {
                        throw new Exception("Repeated component selector");
                    }
                }

                totalHV += StartOfFrameSegment.Components[compIndex].HorizontalSamplingFactor * StartOfFrameSegment.Components[compIndex].VerticalSamplingFactor;

                scan[i].Td = (byte)(TempData[2 + (2 * i)] >> 4);
                if (scan[i].Td > MAXIMUM_TH)
                {
                    throw new Exception("bad Td value");
                }

                scan[i].Ta = (byte)(TempData[2 + (2 * i)] & 0x0f);
                if (scan[i].Ta > MAXIMUM_TH)
                {
                    throw new Exception("bad Ta value");
                }
            }

            if (StartOfFrameSegment.TypeOfImage != ImageType.GreyScale && totalHV > 10)
            {
                throw new Exception("Total sampling factors too large.");
            }

            int zigStart = 0;
            int zigEnd = Block.BlockSize - 1;
            int ah = 0;
            int al = 0;

            if (StartOfFrameSegment.Progressive)
            {
                zigStart = (int)TempData[1 + (2 * lnComp)];
                zigEnd = (int)TempData[2 + (2 * lnComp)];
                ah = (int)(TempData[3 + (2 * lnComp)] >> 4);
                al = (int)(TempData[3 + (2 * lnComp)] & 0x0f);

                if ((zigStart == 0 && zigEnd != 0) || zigStart > zigEnd || zigEnd >= Block.BlockSize)
                {
                    throw new Exception("bad spectral selection bounds");
                }

                if (zigStart != 0 && lnComp != 1)
                {
                    throw new Exception("progressive AC coefficients for more than one component");
                }

                if (ah != 0 && ah != al + 1)
                {
                    throw new Exception("bad successive approximation values");
                }
            }

            int h0 = StartOfFrameSegment.Components[0].HorizontalSamplingFactor;
            int v0 = StartOfFrameSegment.Components[0].VerticalSamplingFactor;
            int mxx = (StartOfFrameSegment.Width + (8 * h0) - 1) / (8 * h0);
            int myy = (StartOfFrameSegment.Height + (8 * v0) - 1) / (8 * v0);

            if (img1 == null && img3 == null)
            {
                makeImg(mxx, myy, StartOfFrameSegment);
            }

            if (StartOfFrameSegment.Progressive)
            {
                for (int i = 0; i < lnComp; i++)
                {
                    int compIndex = scan[i].ComponentIndex;
                    if (ProgressiveCoefficients[compIndex] == null)
                    {
                        ProgressiveCoefficients[compIndex] = new Block[mxx * myy * StartOfFrameSegment.Components[compIndex].HorizontalSamplingFactor * StartOfFrameSegment.Components[compIndex].VerticalSamplingFactor];

                        for (int j = 0; j < ProgressiveCoefficients[compIndex].Length; j++)
                        {
                            ProgressiveCoefficients[compIndex][j] = new Block();
                        }
                    }
                }
            }

            Bytes.Bits = new BitsBuffer();

            int mcu = 0;
            byte expectedRST = SegmentTypes.Restart0;

            var b = new Block();
            int[] dc = new int[MAXIMUM_NUMBER_COMPONENTS];

            int bx, by, blockCount = 0;

            for (int my = 0; my < myy; my++)
            {
                for (int mx = 0; mx < mxx; mx++)
                {
                    for (int i = 0; i < lnComp; i++)
                    {
                        int compIndex = scan[i].ComponentIndex;
                        int hi = StartOfFrameSegment.Components[compIndex].HorizontalSamplingFactor;
                        int vi = StartOfFrameSegment.Components[compIndex].VerticalSamplingFactor;
                        Block qt = DefineQuantizationTableSegment.quant[StartOfFrameSegment.Components[compIndex].QuatizationTableDestSelector];

                        for (int j = 0; j < hi * vi; j++)
                        {
                            if (lnComp != 1)
                            {
                                bx = hi * mx + j % hi;
                                by = vi * my + j / hi;
                            }
                            else
                            {
                                int q = mxx * hi;
                                bx = blockCount % q;
                                by = blockCount / q;
                                blockCount++;
                                if (bx * 8 >= StartOfFrameSegment.Width || by * 8 >= StartOfFrameSegment.Height)
                                    continue;
                            }
                            if (StartOfFrameSegment.Progressive)
                                b = ProgressiveCoefficients[compIndex][by * mxx * hi + bx];
                            else
                                b = new Block();

                            if (ah != 0)
                            {
                                refine(b, DefineHuffmanTableSegment.HuffmanCodes[acTable, scan[i].Ta], zigStart, zigEnd, 1 << al);
                            }
                            else
                            {
                                int zig = zigStart;
                                if (zig == 0)
                                {
                                    zig++;
                                    var value = DefineHuffmanTableSegment.HuffmanCodes[dcTable, scan[i].Td].DecodeHuffman(Bytes);
                                    if (value > 16)
                                    {
                                        throw new Exception("Excessive DC component");
                                    }

                                    var dcDelta = Bytes.receiveExtend(value);
                                    dc[compIndex] += dcDelta;
                                    b[0] = dc[compIndex] << al;
                                }

                                if (zig <= zigEnd && EndOfBlockRun > 0)
                                {
                                    EndOfBlockRun--;
                                }
                                else
                                {
                                    var huffv = DefineHuffmanTableSegment.HuffmanCodes[acTable, scan[i].Ta];
                                    for (; zig <= zigEnd; zig++)
                                    {
                                        var value = huffv.DecodeHuffman(Bytes);
                                        var val0 = (byte)(value >> 4);
                                        var val1 = (byte)(value & 0x0f);
                                        if (val1 != 0)
                                        {
                                            zig += val0;
                                            if (zig > zigEnd)
                                                break;

                                            var ac = Bytes.receiveExtend(val1);
                                            b[unzig[zig]] = ac << al;
                                        }
                                        else
                                        {
                                            if (val0 != 0x0f)
                                            {
                                                EndOfBlockRun = (ushort)(1 << val0);
                                                if (val0 != 0)
                                                {
                                                    EndOfBlockRun |= (ushort)Bytes.DecodeBits(val0);
                                                }
                                                EndOfBlockRun--;
                                                break;
                                            }
                                            zig += 0x0f;
                                        }
                                    }
                                }
                            }

                            if (StartOfFrameSegment.Progressive)
                            {
                                if (zigEnd != Block.BlockSize - 1 || al != 0)
                                {
                                    ProgressiveCoefficients[compIndex][by * mxx * hi + bx] = b;
                                    continue;
                                }
                            }
                            for (int zig = 0; zig < Block.BlockSize; zig++)
                                b[unzig[zig]] *= qt[zig];

                            IDCT.Transform(b);

                            byte[] dst = null;
                            int offset = 0;
                            int stride = 0;

                            if (StartOfFrameSegment.TypeOfImage == ImageType.GreyScale)
                            {
                                dst = img1.Pixels;
                                stride = img1.Stride;
                                offset = img1.Offset + 8 * (by * img1.Stride + bx);
                            }
                            else
                            {
                                switch (compIndex)
                                {
                                    case 0:
                                        dst = img3.YPixels;
                                        stride = img3.YStride;
                                        offset = img3.YOffset + 8 * (by * img3.YStride + bx);
                                        break;

                                    case 1:
                                        dst = img3.CBPixels;
                                        stride = img3.CStride;
                                        offset = img3.COffset + 8 * (by * img3.CStride + bx);
                                        break;

                                    case 2:
                                        dst = img3.CRPixels;
                                        stride = img3.CStride;
                                        offset = img3.COffset + 8 * (by * img3.CStride + bx);
                                        break;

                                    case 3:
                                        throw new Exception("Too many components");

                                    default:
                                        throw new Exception("Too many components");
                                }
                            }

                            for (int y = 0; y < 8; y++)
                            {
                                int y8 = y * 8;
                                int yStride = y * stride;

                                for (int x = 0; x < 8; x++)
                                {
                                    int c = b[y8 + x];
                                    if (c < -128)
                                    {
                                        c = 0;
                                    }
                                    else if (c > 127)
                                    {
                                        c = 255;
                                    }
                                    else
                                    {
                                        c += 128;
                                    }
                                    dst[yStride + x + offset] = (byte)c;
                                }
                            }
                        }
                    }

                    mcu++;

                    if (DefineRestartIntervalSegment.RestartInterval > 0 && mcu % DefineRestartIntervalSegment.RestartInterval == 0 && mcu < mxx * myy)
                    {
                        Bytes.ReadFull(TempData, 0, 2);
                        if (TempData[0] != 0xff || TempData[1] != expectedRST)
                        {
                            throw new Exception("Bad RST marker");
                        }

                        expectedRST++;
                        if (expectedRST == SegmentTypes.Restart7 + 1)
                        {
                            expectedRST = SegmentTypes.Restart0;
                        }
                        Bytes.Bits = new BitsBuffer();
                        dc = new int[MAXIMUM_NUMBER_COMPONENTS];
                        EndOfBlockRun = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Writes the information to the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(sosHeaderYCbCr, 0, sosHeaderYCbCr.Length);
            encode444(writer);
            emit(0x7f, 7, writer);
        }

        private static int div(int a, int b)
        {
            if (a >= 0)
                return (a + (b >> 1)) / b;
            else
                return -((-a + (b >> 1)) / b);
        }

        private void emit(uint bits, uint nBits, BinaryWriter writer)
        {
            nBits += this.nBits;
            bits <<= (int)(32 - nBits);
            bits |= this.bits;
            while (nBits >= 8)
            {
                var b = (byte)(bits >> 24);
                writer.Write(b);
                if (b == 0xff)
                    writer.Write((byte)0x00);
                bits <<= 8;
                nBits -= 8;
            }
            this.bits = bits;
            this.nBits = nBits;
        }

        private void emitHuff(HuffmanIndex h, int v, BinaryWriter writer)
        {
            uint x = DefineHuffmanTableSegment.HuffmanLookUpTables[(int)h].Values[v];
            emit(x & ((1 << 24) - 1), x >> 24, writer);
        }

        private void emitHuffRLE(HuffmanIndex h, int runLength, int v, BinaryWriter writer)
        {
            int a = v;
            int b = v;
            if (a < 0)
            {
                a = -v;
                b = v - 1;
            }
            uint nBits = 0;
            if (a < 0x100)
                nBits = bitCount[a];
            else
                nBits = 8 + (uint)bitCount[a >> 8];

            emitHuff(h, (int)((uint)(runLength << 4) | nBits), writer);
            if (nBits > 0) emit((uint)b & (uint)((1 << ((int)nBits)) - 1), nBits, writer);
        }

        private void encode444(BinaryWriter writer)
        {
            var b = new Block();
            var cb = new Block();
            var cr = new Block();
            int prevDCY = 0, prevDCCb = 0, prevDCCr = 0;

            for (int y = 0; y < Image.Height; y += 8)
            {
                for (int x = 0; x < Image.Width; x += 8)
                {
                    toYCbCr(x, y, b, cb, cr);
                    prevDCY = writeBlock(b, (QuantIndex)0, prevDCY, writer);
                    prevDCCb = writeBlock(cb, (QuantIndex)1, prevDCCb, writer);
                    prevDCCr = writeBlock(cr, (QuantIndex)1, prevDCCr, writer);
                }
            }
        }

        private void makeImg(int mxx, int myy, StartOfFrame frame)
        {
            if (frame.TypeOfImage == ImageType.GreyScale)
            {
                var m = new GreyImage(8 * mxx, 8 * myy);
                img1 = m.SubImage(0, 0, frame.Width, frame.Height);
            }
            else
            {
                var h0 = frame.Components[0].HorizontalSamplingFactor;
                var v0 = frame.Components[0].VerticalSamplingFactor;
                var hRatio = h0 / frame.Components[1].HorizontalSamplingFactor;
                var vRatio = v0 / frame.Components[1].VerticalSamplingFactor;

                var ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio444;
                switch ((hRatio << 4) | vRatio)
                {
                    case 0x11:
                        ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio444;
                        break;

                    case 0x12:
                        ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio440;
                        break;

                    case 0x21:
                        ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio422;
                        break;

                    case 0x22:
                        ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio420;
                        break;

                    case 0x41:
                        ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio411;
                        break;

                    case 0x42:
                        ratio = YCbCrSubsampleRatio.YCbCrSubsampleRatio410;
                        break;
                }

                var m = new YcbcrImg(8 * h0 * mxx, 8 * v0 * myy, ratio);
                img3 = m.SubImage(0, 0, frame.Width, frame.Height);
            }
        }

        private void refine(Block b, Huffman h, int zigStart, int zigEnd, int delta)
        {
            if (zigStart == 0)
            {
                if (zigEnd != 0)
                    throw new Exception("Invalid state for zig DC component");

                var bit = Bytes.DecodeBit();
                if (bit)
                    b[0] |= delta;

                return;
            }

            int zig = zigStart;
            if (EndOfBlockRun == 0)
            {
                for (; zig <= zigEnd; zig++)
                {
                    bool done = false;
                    int z = 0;
                    var val = h.DecodeHuffman(Bytes);
                    int val0 = val >> 4;
                    int val1 = val & 0x0f;

                    switch (val1)
                    {
                        case 0:
                            if (val0 != 0x0f)
                            {
                                EndOfBlockRun = (ushort)(1 << val0);
                                if (val0 != 0)
                                {
                                    var bits = Bytes.DecodeBits(val0);
                                    EndOfBlockRun |= (ushort)bits;
                                }

                                done = true;
                            }
                            break;

                        case 1:
                            z = delta;
                            var bit = Bytes.DecodeBit();
                            if (!bit)
                                z = -z;
                            break;

                        default:
                            throw new Exception("unexpected Huffman code");
                    }

                    if (done)
                        break;

                    zig = refineNonZeroes(b, zig, zigEnd, val0, delta);
                    if (zig > zigEnd)
                        throw new Exception(string.Format("too many coefficients {0} > {1}", zig, zigEnd));

                    if (z != 0)
                        b[unzig[zig]] = z;
                }
            }

            if (EndOfBlockRun > 0)
            {
                EndOfBlockRun--;
                refineNonZeroes(b, zig, zigEnd, -1, delta);
            }
        }

        private int refineNonZeroes(Block b, int zig, int zigEnd, int nz, int delta)
        {
            for (; zig <= zigEnd; zig++)
            {
                int u = unzig[zig];
                if (b[u] == 0)
                {
                    if (nz == 0)
                        break;
                    nz--;
                    continue;
                }

                var bit = Bytes.DecodeBit();
                if (!bit)
                    continue;

                if (b[u] >= 0)
                    b[u] += delta;
                else
                    b[u] -= delta;
            }

            return zig;
        }

        private void toYCbCr(int x, int y, Block yBlock, Block cbBlock, Block crBlock)
        {
            int xmax = Image.Width - 1;
            int ymax = Image.Height - 1;
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var Offset = Math.Min(x + i, xmax) + (Math.Min(y + j, ymax) * Image.Width);
                    YCbCr color = Image.Pixels[Offset];
                    int index = (8 * j) + i;
                    yBlock[index] = (int)color.YLuminance;
                    cbBlock[index] = (int)color.CbChroma;
                    crBlock[index] = (int)color.CrChroma;
                }
            }
        }

        private int writeBlock(Block b, QuantIndex q, int prevDC, BinaryWriter writer)
        {
            ForwardDiscreteCosineTransform.Transform(b);

            // Emit the DC delta.
            var dc = div(b[0], 8 * DefineQuantizationTableSegment.quant[(int)q][0]);
            emitHuffRLE((HuffmanIndex)(2 * (int)q + 0), 0, dc - prevDC, writer);

            // Emit the AC components.
            var h = (HuffmanIndex)(2 * (int)q + 1);
            int runLength = 0;

            for (int zig = 1; zig < Block.BlockSize; zig++)
            {
                var ac = div(b[unzig[zig]], 8 * DefineQuantizationTableSegment.quant[(int)q][zig]);

                if (ac == 0)
                {
                    runLength++;
                }
                else
                {
                    while (runLength > 15)
                    {
                        emitHuff(h, 0xf0, writer);
                        runLength -= 16;
                    }

                    emitHuffRLE(h, runLength, ac, writer);
                    runLength = 0;
                }
            }
            if (runLength > 0)
                emitHuff(h, 0x00, writer);
            return dc;
        }
    }
}