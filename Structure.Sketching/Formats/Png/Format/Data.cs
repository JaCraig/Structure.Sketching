﻿/*
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

using Structure.Sketching.Formats.Png.Format.ColorFormats;
using Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces;
using Structure.Sketching.Formats.Png.Format.Helpers;
using Structure.Sketching.Formats.Png.Format.Helpers.ZLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// PNG image data
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public Data(Image image)
            : this(ToScanlines(image))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data" /> class.
        /// </summary>
        /// <param name="imageData">The image data.</param>
        public Data(byte[] imageData)
        {
            ImageData = imageData;
            ColorTypes = new Dictionary<int, ColorTypeInformation>
            {
                [0] = new ColorTypeInformation(1, new[] { 1, 2, 4, 8 }, (x, y) => new GreyscaleNoAlphaReader()),
                [2] = new ColorTypeInformation(3, new[] { 8 }, (x, y) => new TrueColorNoAlphaReader()),
                [3] = new ColorTypeInformation(1, new[] { 1, 2, 4, 8 }, (x, y) => new PaletteReader(x, y)),
                [4] = new ColorTypeInformation(2, new[] { 8 }, (x, y) => new GreyscaleAlphaReader()),
                [6] = new ColorTypeInformation(4, new[] { 8 }, (x, y) => new TrueColorAlphaReader())
            };
        }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        /// <value>
        /// The image data.
        /// </value>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the color types.
        /// </summary>
        /// <value>
        /// The color types.
        /// </value>
        private Dictionary<int, ColorTypeInformation> ColorTypes { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Data"/> to <see cref="Chunk"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Chunk(Data data)
        {
            return new Chunk(data.ImageData.Length, ChunkTypes.Data, data.ImageData);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Chunk"/> to <see cref="Data"/>.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Data(Chunk chunk)
        {
            return new Data(chunk.Data);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="Object1">The object1.</param>
        /// <param name="Object2">The object2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Data operator +(Data Object1, Data Object2)
        {
            if (Object1 == null && Object2 == null)
                return new Data(new byte[0]);
            if (Object1 == null)
                return new Data(Object2.ImageData);
            if (Object2 == null)
                return new Data(Object1.ImageData);
            var ReturnData = new Data(new byte[Object1.ImageData.Length + Object2.ImageData.Length]);
            Array.Copy(Object1.ImageData, 0, ReturnData.ImageData, 0, Object1.ImageData.Length);
            Array.Copy(Object2.ImageData, 0, ReturnData.ImageData, Object1.ImageData.Length, Object2.ImageData.Length);
            return ReturnData;
        }

        /// <summary>
        /// Parses the specified header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="palette">The palette.</param>
        /// <param name="alphaPalette">The alpha palette.</param>
        /// <returns>The resulting image</returns>
        public Image Parse(Header header, Palette palette, Palette alphaPalette)
        {
            float[] Pixels = new float[header.Width * header.Height * 4];
            var ColorTypeInfo = ColorTypes[header.ColorType];

            if (ColorTypeInfo != null)
            {
                IColorReader ColorReader = ColorTypeInfo.CreateColorReader(palette, alphaPalette);
                using (MemoryStream TempStream = new MemoryStream(ImageData))
                {
                    ReadScanlines(TempStream, Pixels, ColorReader, ColorTypeInfo, header);
                }
            }

            return new Image(header.Width, header.Height, Pixels);
        }

        /// <summary>
        /// Calculates the length of the scanline.
        /// </summary>
        /// <param name="colorTypeInformation">The color type information.</param>
        /// <param name="header">The header.</param>
        /// <returns>The scanline length</returns>
        private static int CalculateScanlineLength(ColorTypeInformation colorTypeInformation, Header header)
        {
            int ScanlineLength = header.Width * header.BitDepth * colorTypeInformation.ScanlineFactor;

            int Amount = ScanlineLength % 8;
            if (Amount != 0)
            {
                ScanlineLength += 8 - Amount;
            }

            return ScanlineLength / 8;
        }

        /// <summary>
        /// Calculates the scanline step.
        /// </summary>
        /// <param name="colorTypeInformation">The color type information.</param>
        /// <param name="header">The header.</param>
        /// <returns>The scanline step</returns>
        private static int CalculateScanlineStep(ColorTypeInformation colorTypeInformation, Header header)
        {
            return header.BitDepth >= 8 ? (colorTypeInformation.ScanlineFactor * header.BitDepth) / 8 : 1;
        }

        /// <summary>
        /// Paethes the predicator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="above">The above.</param>
        /// <param name="upperLeft">The upper left.</param>
        /// <returns>The predicted paeth...</returns>
        private static byte PaethPredicator(byte left, byte above, byte upperLeft)
        {
            int p = left + above - upperLeft;
            int pa = Math.Abs(p - left);
            int pb = Math.Abs(p - above);
            int pc = Math.Abs(p - upperLeft);
            if (pa <= pb && pa <= pc)
            {
                return left;
            }
            if (pb <= pc)
            {
                return above;
            }
            return upperLeft;
        }

        private static byte[] ToScanlines(Image image)
        {
            byte[] data = new byte[(image.Width * image.Height * 4) + image.Height];
            int RowLength = (image.Width * 4) + 1;

            for (int y = 0; y < image.Height; ++y)
            {
                var Compression = (byte)(y > 0 ? 2 : 0);
                data[y * RowLength] = Compression;
                Parallel.For(0, image.Width, x =>
                {
                    int dataOffset = (y * RowLength) + (x * 4) + 1;
                    data[dataOffset] = (byte)(image.Pixels[(image.Width * y) + x].X * 255f);
                    data[dataOffset + 1] = (byte)(image.Pixels[(image.Width * y) + x].Y * 255f);
                    data[dataOffset + 2] = (byte)(image.Pixels[(image.Width * y) + x].Z * 255f);
                    data[dataOffset + 3] = (byte)(image.Pixels[(image.Width * y) + x].W * 255f);

                    if (y > 0)
                    {
                        data[dataOffset] -= (byte)(image.Pixels[(image.Width * (y - 1)) + x].X * 255f);
                        data[dataOffset + 1] -= (byte)(image.Pixels[(image.Width * (y - 1)) + x].Y * 255f);
                        data[dataOffset + 2] -= (byte)(image.Pixels[(image.Width * (y - 1)) + x].Z * 255f);
                        data[dataOffset + 3] -= (byte)(image.Pixels[(image.Width * (y - 1)) + x].W * 255f);
                    }
                });
            }

            byte[] buffer;
            int BufferLength;

            MemoryStream TempMemoryStream = null;
            try
            {
                TempMemoryStream = new MemoryStream();

                using (ZlibDeflateStream TempDeflateStream = new ZlibDeflateStream(TempMemoryStream, 6))
                {
                    TempDeflateStream.Write(data, 0, data.Length);
                }

                BufferLength = (int)TempMemoryStream.Length;
                buffer = TempMemoryStream.ToArray();
            }
            finally
            {
                TempMemoryStream?.Dispose();
            }

            return buffer;
        }

        /// <summary>
        /// Reads the scanlines.
        /// </summary>
        /// <param name="dataStream">The data stream.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="colorReader">The color reader.</param>
        /// <param name="colorTypeInformation">The color type information.</param>
        /// <param name="header">The header.</param>
        private void ReadScanlines(MemoryStream dataStream, float[] pixels, IColorReader colorReader, ColorTypeInformation colorTypeInformation, Header header)
        {
            dataStream.Seek(0, SeekOrigin.Begin);

            int ScanlineLength = CalculateScanlineLength(colorTypeInformation, header);
            int ScanlineStep = CalculateScanlineStep(colorTypeInformation, header);

            byte[] LastScanline = new byte[ScanlineLength];
            byte[] CurrentScanline = new byte[ScanlineLength];
            int Filter = 0, Column = -1, Row = 0;

            using (InflateStream CompressedStream = new InflateStream(dataStream))
            {
                int ReadByte;
                while ((ReadByte = CompressedStream.ReadByte()) >= 0)
                {
                    if (Column == -1)
                    {
                        Filter = ReadByte;
                        ++Column;
                    }
                    else
                    {
                        CurrentScanline[Column] = (byte)ReadByte;
                        byte a;
                        byte b;
                        byte c;
                        if (Column >= ScanlineStep)
                        {
                            a = CurrentScanline[Column - ScanlineStep];
                            c = LastScanline[Column - ScanlineStep];
                        }
                        else
                        {
                            a = 0;
                            c = 0;
                        }

                        b = LastScanline[Column];
                        switch (Filter)
                        {
                            case 1:
                                CurrentScanline[Column] = (byte)(CurrentScanline[Column] + a);
                                break;

                            case 2:
                                CurrentScanline[Column] = (byte)(CurrentScanline[Column] + b);
                                break;

                            case 3:
                                CurrentScanline[Column] = (byte)(CurrentScanline[Column] + (byte)((a + b) / 2));
                                break;

                            case 4:
                                CurrentScanline[Column] = (byte)(CurrentScanline[Column] + PaethPredicator(a, b, c));
                                break;
                        }

                        ++Column;

                        if (Column == ScanlineLength)
                        {
                            colorReader.ReadScanline(CurrentScanline, pixels, header, Row);
                            ++Row;
                            Column = -1;
                            var Holder = CurrentScanline;
                            CurrentScanline = LastScanline;
                            LastScanline = Holder;
                        }
                    }
                }
            }
        }
    }
}