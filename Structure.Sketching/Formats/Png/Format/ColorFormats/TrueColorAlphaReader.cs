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
using Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces;

namespace Structure.Sketching.Formats.Png.Format.ColorFormats
{
    /// <summary>
    /// True color with alpha reader
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces.IColorReader"/>
    public class TrueColorAlphaReader : IColorReader
    {
        /// <summary>
        /// Reads the scanline.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        /// <param name="pixels">The pixels.</param>
        /// <param name="header">The header.</param>
        /// <param name="row">The row.</param>
        public unsafe void ReadScanline(byte[] scanline, byte[] pixels, Header header, int row)
        {
            scanline = scanline.ExpandArray(header.BitDepth);
            fixed (byte* ScanlinePointer = &scanline[0])
            {
                byte* ScanlinePointer2 = ScanlinePointer;
                for (int x = 0; x < scanline.Length; x += 4)
                {
                    int Offset = ((row * header.Width) + (x >> 2)) * 4;
                    pixels[Offset] = *ScanlinePointer2;
                    ++ScanlinePointer2;
                    pixels[Offset + 1] = *ScanlinePointer2;
                    ++ScanlinePointer2;
                    pixels[Offset + 2] = *ScanlinePointer2;
                    ++ScanlinePointer2;
                    pixels[Offset + 3] = *ScanlinePointer2;
                    ++ScanlinePointer2;
                }
            }
        }
    }
}