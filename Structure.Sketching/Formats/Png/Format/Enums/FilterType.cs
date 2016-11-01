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
    /// Scan line filter type
    /// </summary>
    public enum FilterType : byte
    {
        /// <summary>
        /// As is, no filter
        /// </summary>
        None = 0,

        /// <summary>
        /// Byte to the left is used
        /// </summary>
        Sub = 1,

        /// <summary>
        /// Byte above is used
        /// </summary>
        Up = 2,

        /// <summary>
        /// The average of left and above
        /// </summary>
        Average = 3,

        /// <summary>
        /// Up, Left, or Upper Left. Whichever is closest to p = A + B - C.
        /// </summary>
        Paeth = 4
    }
}