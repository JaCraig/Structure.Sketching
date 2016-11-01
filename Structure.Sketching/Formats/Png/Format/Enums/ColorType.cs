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

namespace Structure.Sketching.Formats.Png.Format.Enums
{
    /// <summary>
    /// Color type enum
    /// </summary>
    public enum ColorType : byte
    {
        /// <summary>
        /// The greyscale
        /// </summary>
        Greyscale = 0,

        /// <summary>
        /// The RGB
        /// </summary>
        TrueColor = 2,

        /// <summary>
        /// The palette
        /// </summary>
        Palette = 3,

        /// <summary>
        /// The grayscale with alpha
        /// </summary>
        GreyscaleWithAlpha = 4,

        /// <summary>
        /// The RGB with alpha
        /// </summary>
        TrueColorWithAlpha = 6
    }
}