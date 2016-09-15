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

namespace Structure.Sketching.Formats.Jpeg.Format.Segments
{
    /// <summary>
    /// The different segment types
    /// </summary>
    public class SegmentTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentTypes"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public SegmentTypes(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public byte Value { get; private set; }

        /// <summary>
        /// The application 0 tag
        /// </summary>
        public static SegmentTypes Application0 = 0xE0;

        /// <summary>
        /// The application 14 tag
        /// </summary>
        public static SegmentTypes Application14 = 0xEE;

        /// <summary>
        /// The application 15 tag
        /// </summary>
        public static SegmentTypes Application15 = 0xEF;

        /// <summary>
        /// The comment tag
        /// </summary>
        public static SegmentTypes Comment = 0xFE;

        /// <summary>
        /// The define huffman table tag
        /// </summary>
        public static SegmentTypes DefineHuffmanTable = 0xC4;

        /// <summary>
        /// The define quantization table tag
        /// </summary>
        public static SegmentTypes DefineQuantizationTable = 0xDB;

        /// <summary>
        /// The define restart interval tag
        /// </summary>
        public static SegmentTypes DefineRestartInterval = 0xDD;

        /// <summary>
        /// The end of image tag
        /// </summary>
        public static SegmentTypes EndOfImage = 0xD9;

        /// <summary>
        /// The restart 0 tag
        /// </summary>
        public static SegmentTypes Restart0 = 0xD0;

        /// <summary>
        /// The restart 7 tag
        /// </summary>
        public static SegmentTypes Restart7 = 0xD7;

        /// <summary>
        /// The start of frame 0 tag
        /// </summary>
        public static SegmentTypes StartOfFrame0 = 0xC0;

        /// <summary>
        /// The start of frame 1 tag
        /// </summary>
        public static SegmentTypes StartOfFrame1 = 0xC1;

        /// <summary>
        /// The start of frame 2 tag
        /// </summary>
        public static SegmentTypes StartOfFrame2 = 0xC2;

        /// <summary>
        /// The start of image tag
        /// </summary>
        public static SegmentTypes StartOfImage = 0xD8;

        /// <summary>
        /// The start of scan tag
        /// </summary>
        public static SegmentTypes StartOfScan = 0xDA;

        /// <summary>
        /// Performs an implicit conversion from <see cref="SegmentTypes"/> to <see cref="byte"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte(SegmentTypes value)
        {
            if (value == null) return 0;
            return value.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="byte"/> to <see cref="SegmentTypes"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SegmentTypes(byte value)
        {
            return new SegmentTypes(value);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(SegmentTypes value1, SegmentTypes value2)
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
        public static bool operator ==(SegmentTypes value1, SegmentTypes value2)
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
            var Value2 = obj as SegmentTypes;
            if (ReferenceEquals(Value2, null))
                return false;
            return this == (SegmentTypes)obj;
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
            return new string(new char[] { (char)Value });
        }
    }
}