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

namespace Structure.Sketching.Formats.Png.Format.ColorFormats
{
    /// <summary>
    /// True color without alpha channel reader
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces.IColorReader"/>
    public class TrueColorNoAlphaReader : IColorReader
    {
        /// <summary>
        /// Reads the scanline.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="header">The header.</param>
        /// <param name="row">The row.</param>
        public unsafe void ReadScanline(byte[] scanline, Color[] pixels, Header header, int row)
        {
            scanline = scanline.ExpandArray(header.BitDepth);

            int BytesPerPixel = (header.BitDepth * 3) / 8;
            int BytesPerChannel = header.BitDepth / 8;

            fixed (Color* PixelPointer = &pixels[row * header.Width])
            {
                Color* PixelPointer2 = PixelPointer;
                fixed (byte* ScanlinePointer = &scanline[0])
                {
                    byte* ScanlinePointer2 = ScanlinePointer;
                    for (int x = 0; x < scanline.Length; x += BytesPerPixel)
                    {
                        (*PixelPointer2).Red = *ScanlinePointer2;
                        ScanlinePointer2 += BytesPerChannel;
                        (*PixelPointer2).Green = *ScanlinePointer2;
                        ScanlinePointer2 += BytesPerChannel;
                        (*PixelPointer2).Blue = *ScanlinePointer2;
                        ScanlinePointer2 += BytesPerChannel;
                        (*PixelPointer2).Alpha = 255;
                        ++PixelPointer2;
                    }
                }
            }
        }
    }
}