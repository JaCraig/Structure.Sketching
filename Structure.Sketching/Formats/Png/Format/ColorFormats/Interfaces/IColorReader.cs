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

namespace Structure.Sketching.Formats.Png.Format.ColorFormats.Interfaces
{
    /// <summary>
    /// Color reader interface
    /// </summary>
    public interface IColorReader
    {
        /// <summary>
        /// Reads a scan line and inserts it into the pixels array
        /// </summary>
        /// <param name="scanline">Scanline</param>
        /// <param name="pixels">Resulting pixels</param>
        /// <param name="header">Header information</param>
        /// <param name="row">Current row</param>
        void ReadScanline(byte[] scanline, Color[] pixels, Header header, int row);
    }
}