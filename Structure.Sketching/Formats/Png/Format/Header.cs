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

using Structure.Sketching.Formats.Png.Format.Enums;
using Structure.Sketching.Formats.Png.Format.Helpers;
using System;

namespace Structure.Sketching.Formats.Png.Format
{
    /// <summary>
    /// PNG Header information
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public Header(Image image)
            : this(image.Width, image.Height, 8, 6, 0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="colorType">Type of the color.</param>
        /// <param name="compressionMethod">The compression method.</param>
        /// <param name="filterMethod">The filter method.</param>
        /// <param name="interlaceMethod">The interlace method.</param>
        public Header(int width, int height, byte bitDepth, byte colorType, byte compressionMethod, byte filterMethod, byte interlaceMethod)
        {
            Width = width;
            Height = height;
            BitDepth = bitDepth;
            ColorType = (ColorType)colorType;
            CompressionMethod = compressionMethod;
            FilterMethod = filterMethod;
            InterlaceMethod = interlaceMethod;
            BytesPerPixel = CalculateBytesPerPixel();
        }

        /// <summary>
        /// Gets or sets the bit depth.
        /// </summary>
        /// <value>The bit depth.</value>
        public byte BitDepth { get; set; }

        /// <summary>
        /// Gets or sets the bytes per pixel.
        /// </summary>
        /// <value>The bytes per pixel.</value>
        public int BytesPerPixel { get; set; }

        /// <summary>
        /// Gets or sets the type of the color.
        /// </summary>
        /// <value>The type of the color.</value>
        public ColorType ColorType { get; set; }

        /// <summary>
        /// Gets or sets the compression method.
        /// </summary>
        /// <value>The compression method.</value>
        public byte CompressionMethod { get; set; }

        /// <summary>
        /// Gets or sets the filter method.
        /// </summary>
        /// <value>The filter method.</value>
        public byte FilterMethod { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the interlace method.
        /// </summary>
        /// <value>The interlace method.</value>
        public byte InterlaceMethod { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Header"/> to <see cref="Chunk"/>.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Chunk(Header header)
        {
            var TempData = new byte[13];
            var WidthBytes = BitConverter.GetBytes(header.Width);
            var HeightBytes = BitConverter.GetBytes(header.Height);
            Array.Reverse(WidthBytes);
            Array.Reverse(HeightBytes);
            Array.Copy(WidthBytes, TempData, WidthBytes.Length);
            Array.Copy(HeightBytes, 0, TempData, WidthBytes.Length, HeightBytes.Length);
            TempData[8] = header.BitDepth;
            TempData[9] = (byte)header.ColorType;
            TempData[10] = header.CompressionMethod;
            TempData[11] = header.FilterMethod;
            TempData[12] = header.InterlaceMethod;
            return new Chunk(13, ChunkTypes.Header, TempData);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Chunk"/> to <see cref="Header"/>.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Header(Chunk chunk)
        {
            Array.Reverse(chunk.Data, 0, 4);
            Array.Reverse(chunk.Data, 4, 4);
            return new Header(BitConverter.ToInt32(chunk.Data, 0),
                BitConverter.ToInt32(chunk.Data, 4),
                chunk.Data[8],
                chunk.Data[9],
                chunk.Data[10],
                chunk.Data[11],
                chunk.Data[12]);
        }

        /// <summary>
        /// Calculates the bytes per pixel.
        /// </summary>
        /// <returns>The bytes per pixel</returns>
        private int CalculateBytesPerPixel()
        {
            switch (ColorType)
            {
                case ColorType.Greyscale:
                case ColorType.Palette:
                    return 1;

                case ColorType.GreyscaleWithAlpha:
                    return 2;

                case ColorType.TrueColor:
                    return 3;
            }
            return 4;
        }
    }
}