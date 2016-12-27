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

using Structure.Sketching.Colors;
using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces;
using Structure.Sketching.Formats.Png.Format.Enums;

namespace Structure.Sketching.Formats.Png.Format.ColorFormats
{
    /// <summary>
    /// Palette reader
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces.IColorReader"/>
    public class PaletteReader : IColorReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteReader"/> class.
        /// </summary>
        /// <param name="palette">The palette.</param>
        /// <param name="alphaPalette">The alpha palette.</param>
        public PaletteReader(Palette palette, Palette alphaPalette)
        {
            Palette = palette ?? new Palette(new byte[0], PaletteType.Color);
            AlphaPalette = alphaPalette ?? new Palette(new byte[0], PaletteType.Alpha);
        }

        /// <summary>
        /// Gets or sets the alpha palette.
        /// </summary>
        /// <value>The alpha palette.</value>
        public Palette AlphaPalette { get; set; }

        /// <summary>
        /// Gets or sets the palette.
        /// </summary>
        /// <value>The palette.</value>
        public Palette Palette { get; set; }

        /// <summary>
        /// Reads the scanline.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="header">The header.</param>
        /// <param name="row">The row.</param>
        public void ReadScanline(byte[] scanline, Color[] pixels, Header header, int row)
        {
            scanline = scanline.ExpandArray(header.BitDepth);

            if (AlphaPalette.Data.Length > 0)
            {
                for (int x = 0; x < header.Width; ++x)
                {
                    int Offset = (row * header.Width) + x;
                    int PixelOffset = scanline[x] * 3;
                    pixels[Offset].Red = Palette.Data[PixelOffset];
                    pixels[Offset].Green = Palette.Data[PixelOffset + 1];
                    pixels[Offset].Blue = Palette.Data[PixelOffset + 2];
                    pixels[Offset].Alpha = (byte)(AlphaPalette.Data.Length > scanline[x] ? AlphaPalette.Data[scanline[x]] : 255);
                }
            }
            else
            {
                for (int x = 0; x < header.Width; ++x)
                {
                    int Offset = (row * header.Width) + x;
                    int PixelOffset = scanline[x] * 3;

                    pixels[Offset].Red = Palette.Data[PixelOffset];
                    pixels[Offset].Green = Palette.Data[PixelOffset + 1];
                    pixels[Offset].Blue = Palette.Data[PixelOffset + 2];
                    pixels[Offset].Alpha = 255;
                }
            }
        }
    }
}