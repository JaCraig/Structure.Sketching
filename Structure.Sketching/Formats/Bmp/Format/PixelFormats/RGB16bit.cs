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
using Structure.Sketching.Formats.Bmp.Format.PixelFormats.BaseClasses;
using System;
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Bmp.Format.PixelFormats
{
    /// <summary>
    /// RGB 16bit pixel format
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces.IPixelFormat"/>
    public class RGB16bit : PixelFormatBase
    {
        /// <summary>
        /// The bytes per pixel
        /// </summary>
        /// <value>The BPP.</value>
        public override double BPP => 2;

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The decoded data</returns>
        public override byte[] Decode(Header header, byte[] data, Palette palette)
        {
            int width = header.Width;
            int height = header.Height;
            int alignment = (4 - ((width * (int)BPP) % 4)) % 4;
            byte[] ReturnValue = new byte[width * height * 4];
            Parallel.For(0, height, y =>
            {
                int RowOffset = y * ((width * (int)BPP) + alignment);
                int CurrentY = height - y - 1;
                if (CurrentY < 0)
                    CurrentY = 0;
                if (CurrentY >= height)
                    CurrentY = height - 1;
                for (int x = 0; x < width; ++x)
                {
                    int Offset = RowOffset + (x * (int)BPP);
                    var TempValue = BitConverter.ToInt16(data, Offset);
                    var r = (int)(((TempValue & header.RedMask) >> header.RedOffset) * header.RedMultiplier);
                    var g = (int)(((TempValue & header.GreenMask) >> header.GreenOffset) * header.GreenMultiplier);
                    var b = (int)(((TempValue & header.BlueMask) >> header.BlueOffset) * header.BlueMultiplier);
                    var a = (int)(header.AlphaMask == 0 ? 255 : ((TempValue & header.AlphaMask) >> header.AlphaOffset) * header.AlphaMultiplier);

                    int ArrayOffset = ((CurrentY * width) + x) * 4;
                    ReturnValue[ArrayOffset] = (byte)r.Clamp(0, 255);
                    ReturnValue[ArrayOffset + 1] = (byte)g.Clamp(0, 255);
                    ReturnValue[ArrayOffset + 2] = (byte)b.Clamp(0, 255);
                    ReturnValue[ArrayOffset + 3] = (byte)a.Clamp(0, 255);
                }
            });
            return ReturnValue;
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        /// <param name="palette">The palette.</param>
        /// <returns>The encoded data</returns>
        public override byte[] Encode(Header header, byte[] data, Palette palette)
        {
            int width = header.Width;
            int height = header.Height;
            int alignment = (4 - ((width * (int)BPP) % 4)) % 4;
            var ReturnValue = new byte[((width * (int)BPP) + alignment) * height];
            Parallel.For(0, height, y =>
            {
                int SourceY = height - y - 1;
                if (SourceY < 0)
                    SourceY = 0;
                if (SourceY >= height)
                    SourceY = height - 1;
                int SourceRowOffset = SourceY * width * 4;
                int DestinationY = y;
                int DestinationRowOffset = DestinationY * ((width * (int)BPP) + alignment);
                for (int x = 0; x < width; ++x)
                {
                    int SourceX = x * 4;
                    int SourceOffset = SourceX + SourceRowOffset;
                    int DestinationX = x * (int)BPP;
                    int DestinationOffset = DestinationX + DestinationRowOffset;
                    int r = data[SourceOffset + 2] >> 3;
                    int g = data[SourceOffset + 1] >> 2;
                    int b = data[SourceOffset] >> 3;
                    var TempValue = (short)((r << 11) | (g << 5) | b);
                    var Values = BitConverter.GetBytes(TempValue);

                    ReturnValue[DestinationOffset] = Values[0];
                    ReturnValue[DestinationOffset + 1] = Values[1];
                }
            });
            return ReturnValue;
        }
    }
}