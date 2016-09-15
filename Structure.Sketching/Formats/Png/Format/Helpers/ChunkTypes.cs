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

namespace Structure.Sketching.Formats.Png.Format.Helpers
{
    /// <summary>
    /// The different chunk types
    /// </summary>
    public class ChunkTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkTypes"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ChunkTypes(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; private set; }

        /// <summary>
        /// The background color
        /// </summary>
        public static ChunkTypes BackgroundColor = "bKGD";

        /// <summary>
        /// The chromaticity
        /// </summary>
        public static ChunkTypes Chromaticity = "cHRM";

        /// <summary>
        /// The compressed text
        /// </summary>
        public static ChunkTypes CompressedText = "zTXt";

        /// <summary>
        /// The image data
        /// </summary>
        public static ChunkTypes Data = "IDAT";

        /// <summary>
        /// The end of the file
        /// </summary>
        public static ChunkTypes End = "IEND";

        /// <summary>
        /// The gamma
        /// </summary>
        public static ChunkTypes Gamma = "gAMA";

        /// <summary>
        /// The header
        /// </summary>
        public static ChunkTypes Header = "IHDR";

        /// <summary>
        /// The histogram
        /// </summary>
        public static ChunkTypes Histogram = "hIST";

        /// <summary>
        /// The icc color profile
        /// </summary>
        public static ChunkTypes ICCColorProfile = "iCCP";

        /// <summary>
        /// The iso text
        /// </summary>
        public static ChunkTypes ISOText = "tEXt";

        /// <summary>
        /// The last modified
        /// </summary>
        public static ChunkTypes LastModified = "tIME";

        /// <summary>
        /// The palette
        /// </summary>
        public static ChunkTypes Palette = "PLTE";

        /// <summary>
        /// The physical ratio/pixel size
        /// </summary>
        public static ChunkTypes Physical = "pHYs";

        /// <summary>
        /// The RGB color space used
        /// </summary>
        public static ChunkTypes RGBColorSpaceUsed = "sRGB";

        /// <summary>
        /// The significant bits
        /// </summary>
        public static ChunkTypes SignificantBits = "sBIT";

        /// <summary>
        /// The stereoscopic image
        /// </summary>
        public static ChunkTypes StereoscopicImage = "sTER";

        /// <summary>
        /// The suggested palette
        /// </summary>
        public static ChunkTypes SuggestedPalette = "sPLT";

        /// <summary>
        /// The UTF-8 text
        /// </summary>
        public static ChunkTypes Text = "iTXt";

        /// <summary>
        /// The transparency information
        /// </summary>
        public static ChunkTypes TransparencyInfo = "tRNS";

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="ChunkTypes"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ChunkTypes(string value)
        {
            if (string.IsNullOrEmpty(value)) return new ChunkTypes(string.Empty);
            return new ChunkTypes(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ChunkTypes"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ChunkTypes value)
        {
            if (value == null) return string.Empty;
            return value.Value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ChunkTypes value1, ChunkTypes value2)
        {
            return !(value1 == value2);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ChunkTypes value1, ChunkTypes value2)
        {
            if (ReferenceEquals(value1, null) || ReferenceEquals(value2, null))
                return false;
            return value1.Value == value2.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var Value2 = obj as ChunkTypes;
            if (ReferenceEquals(Value2, null))
                return false;
            return this == (ChunkTypes)obj;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this;
        }
    }
}