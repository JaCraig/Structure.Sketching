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

using Structure.Sketching.Formats.Png.Format.ColorFormats;
using Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces;
using Structure.Sketching.Formats.Png.Format.Enums;
using Structure.Sketching.Formats.Png.Format.Filters;
using Structure.Sketching.Formats.Png.Format.Filters.Interfaces;
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
        /// Initializes a new instance of the <see cref="Data"/> class.
        /// </summary>
        /// <param name="imageData">The image data.</param>
        public Data(byte[] imageData)
        {
            ImageData = imageData;
            ColorTypes = new Dictionary<ColorType, ColorTypeInformation>
            {
                [ColorType.Greyscale] = new ColorTypeInformation(1, new[] { 1, 2, 4, 8 }, (x, y) => new GreyscaleNoAlphaReader()),
                [ColorType.TrueColor] = new ColorTypeInformation(3, new[] { 8 }, (x, y) => new TrueColorNoAlphaReader()),
                [ColorType.Palette] = new ColorTypeInformation(1, new[] { 1, 2, 4, 8 }, (x, y) => new PaletteReader(x, y)),
                [ColorType.GreyscaleWithAlpha] = new ColorTypeInformation(2, new[] { 8 }, (x, y) => new GreyscaleAlphaReader()),
                [ColorType.TrueColorWithAlpha] = new ColorTypeInformation(4, new[] { 8 }, (x, y) => new TrueColorAlphaReader())
            };
            Filters = new Dictionary<FilterType, IScanFilter>
            {
                [FilterType.Average] = new AverageFilter(),
                [FilterType.None] = new NoFilter(),
                [FilterType.Paeth] = new PaethFilter(),
                [FilterType.Sub] = new SubFilter(),
                [FilterType.Up] = new UpFilter()
            };
        }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        /// <value>The image data.</value>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the color types.
        /// </summary>
        /// <value>The color types.</value>
        private Dictionary<ColorType, ColorTypeInformation> ColorTypes { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        private Dictionary<FilterType, IScanFilter> Filters { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Data"/> to <see cref="Chunk"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Chunk(Data data)
        {
            return new Chunk(data.ImageData.Length, ChunkTypes.Data, data.ImageData);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Chunk"/> to <see cref="Data"/>.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Data(Chunk chunk)
        {
            return new Data(chunk.Data);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="Object1">The object1.</param>
        /// <param name="Object2">The object2.</param>
        /// <returns>The result of the operator.</returns>
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
            byte[] Pixels = new byte[header.Width * header.Height * 4];
            var ColorTypeInfo = ColorTypes[header.ColorType];

            if (ColorTypeInfo != null)
            {
                var ColorReader = ColorTypeInfo.CreateColorReader(palette, alphaPalette);
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

        private static byte[] ToScanlines(Image image)
        {
            byte[] data = new byte[(image.Width * image.Height * 4) + image.Height];
            int RowLength = (image.Width * 4) + 1;

            Parallel.For(0, image.Width, x =>
              {
                  int dataOffset = (x * 4) + 1;
                  int PixelOffset = x * 4;
                  data[dataOffset] = image.Pixels[PixelOffset];
                  data[dataOffset + 1] = image.Pixels[PixelOffset + 1];
                  data[dataOffset + 2] = image.Pixels[PixelOffset + 2];
                  data[dataOffset + 3] = image.Pixels[PixelOffset + 3];
                  data[0] = 0;
                  for (int y = 1; y < image.Height; ++y)
                  {
                      dataOffset = (y * RowLength) + (x * 4) + 1;
                      PixelOffset = ((image.Width * y) + x) * 4;
                      int AbovePixelOffset = ((image.Width * (y - 1)) + x) * 4;
                      data[dataOffset] = (byte)(image.Pixels[PixelOffset] - image.Pixels[AbovePixelOffset]);
                      data[dataOffset + 1] = (byte)(image.Pixels[PixelOffset + 1] - image.Pixels[AbovePixelOffset + 1]);
                      data[dataOffset + 2] = (byte)(image.Pixels[PixelOffset + 2] - image.Pixels[AbovePixelOffset + 2]);
                      data[dataOffset + 3] = (byte)(image.Pixels[PixelOffset + 3] - image.Pixels[AbovePixelOffset + 3]);
                      data[y * RowLength] = 2;
                  }
              });

            using (MemoryStream TempMemoryStream = new MemoryStream())
            {
                using (ZlibDeflateStream TempDeflateStream = new ZlibDeflateStream(TempMemoryStream, 6))
                {
                    TempDeflateStream.Write(data, 0, data.Length);
                }
                return TempMemoryStream.ToArray();
            }
        }

        /// <summary>
        /// Reads the scanlines.
        /// </summary>
        /// <param name="dataStream">The data stream.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="colorReader">The color reader.</param>
        /// <param name="colorTypeInformation">The color type information.</param>
        /// <param name="header">The header.</param>
        private void ReadScanlines(MemoryStream dataStream, byte[] pixels, IColorReader colorReader, ColorTypeInformation colorTypeInformation, Header header)
        {
            dataStream.Seek(0, SeekOrigin.Begin);

            var ScanlineLength = CalculateScanlineLength(colorTypeInformation, header);
            var ScanlineStep = CalculateScanlineStep(colorTypeInformation, header);

            byte[] LastScanline = new byte[ScanlineLength];
            byte[] CurrentScanline = new byte[ScanlineLength];
            byte[] Result = null;

            using (InflateStream CompressedStream = new InflateStream(dataStream))
            {
                using (MemoryStream DecompressedStream = new MemoryStream())
                {
                    CompressedStream.CopyTo(DecompressedStream);
                    DecompressedStream.Flush();
                    byte[] DecompressedArray = DecompressedStream.ToArray();
                    for (int y = 0, Column = 0; y < header.Height; ++y, Column += (ScanlineLength + 1))
                    {
                        Array.Copy(DecompressedArray, Column + 1, CurrentScanline, 0, ScanlineLength);
                        if (DecompressedArray[Column] < 0)
                            break;
                        Result = Filters[(FilterType)DecompressedArray[Column]].Decode(CurrentScanline, LastScanline, ScanlineStep);
                        colorReader.ReadScanline(Result, pixels, header, y);
                        Array.Copy(CurrentScanline, LastScanline, ScanlineLength);
                    }
                }
            }
        }
    }
}