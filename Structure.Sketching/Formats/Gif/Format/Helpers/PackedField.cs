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

using System;

namespace Structure.Sketching.Helpers
{
    /// <summary>
    /// Packed field
    /// </summary>
    /// <seealso cref="IEquatable{PackedField}" />
    public struct PackedField : IEquatable<PackedField>
    {
        /// <summary>
        /// Gets the byte which represents the data items held in this instance.
        /// </summary>
        public byte Byte
        {
            get
            {
                int returnValue = 0;
                int bitShift = 7;
                foreach (bool bit in Bits)
                {
                    int bitValue;
                    if (bit)
                    {
                        bitValue = 1 << bitShift;
                    }
                    else
                    {
                        bitValue = 0;
                    }
                    returnValue |= bitValue;
                    bitShift--;
                }
                return Convert.ToByte(returnValue & 0xFF);
            }
        }

        /// <summary>
        /// The individual bits representing the packed byte.
        /// </summary>
        private static readonly bool[] Bits = new bool[8];

        /// <summary>
        /// Returns a new <see cref="PackedField"/>  with the bits in the packed fields to
        /// the corresponding bits from the supplied byte.
        /// </summary>
        /// <param name="value">The value to pack.</param>
        /// <returns>The <see cref="PackedField"/></returns>
        public static PackedField FromInt(byte value)
        {
            var packed = new PackedField();
            packed.SetBits(0, 8, value);
            return packed;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var field = obj as PackedField?;

            return this.Byte == field?.Byte;
        }

        /// <inheritdoc/>
        public bool Equals(PackedField other)
        {
            return this.Byte.Equals(other.Byte);
        }

        /// <summary>
        /// Gets the value of the specified bit within the byte.
        /// </summary>
        /// <param name="index">The zero-based index of the bit to get.</param>
        /// <returns>
        /// The value of the specified bit within the byte.
        /// </returns>
        public bool GetBit(int index)
        {
            if (index < 0 || index > 7)
            {
                string message = $"Index must be between 0 and 7. Supplied index: {index}";
                throw new ArgumentOutOfRangeException(nameof(index), message);
            }
            return Bits[index];
        }

        /// <summary>
        /// Gets the value of the specified bits within the byte.
        /// </summary>
        /// <param name="startIndex">The zero-based index of the first bit to get.</param>
        /// <param name="length">The number of bits to get.</param>
        /// <returns>
        /// The value of the specified bits within the byte.
        /// </returns>
        public int GetBits(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex > 7)
            {
                string message = $"Start index must be between 0 and 7. Supplied index: {startIndex}";
                throw new ArgumentOutOfRangeException(nameof(startIndex), message);
            }

            if (length < 1 || startIndex + length > 8)
            {
                string message = "Length must be greater than zero and the sum of length and start index must be less than 8. "
                                 + $"Supplied length: {length}. Supplied start index: {startIndex}";

                throw new ArgumentOutOfRangeException(nameof(length), message);
            }

            int returnValue = 0;
            int bitShift = length - 1;
            for (int i = startIndex; i < startIndex + length; i++)
            {
                int bitValue = (Bits[i] ? 1 : 0) << bitShift;
                returnValue += bitValue;
                bitShift--;
            }
            return returnValue;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Byte.GetHashCode();
        }

        /// <summary>
        /// Sets the specified bit within the packed fields to the supplied
        /// value.
        /// </summary>
        /// <param name="index">
        /// The zero-based index within the packed fields of the bit to set.
        /// </param>
        /// <param name="valueToSet">
        /// The value to set the bit to.
        /// </param>
        public void SetBit(int index, bool valueToSet)
        {
            if (index < 0 || index > 7)
            {
                string message
                    = "Index must be between 0 and 7. Supplied index: "
                    + index;
                throw new ArgumentOutOfRangeException(nameof(index), message);
            }
            Bits[index] = valueToSet;
        }

        /// <summary>
        /// Sets the specified bits within the packed fields to the supplied
        /// value.
        /// </summary>
        /// <param name="startIndex">The zero-based index within the packed fields of the first bit to  set.</param>
        /// <param name="length">The number of bits to set.</param>
        /// <param name="valueToSet">The value to set the bits to.</param>
        public void SetBits(int startIndex, int length, int valueToSet)
        {
            if (startIndex < 0 || startIndex > 7)
            {
                string message = $"Start index must be between 0 and 7. Supplied index: {startIndex}";
                throw new ArgumentOutOfRangeException(nameof(startIndex), message);
            }

            if (length < 1 || startIndex + length > 8)
            {
                string message = "Length must be greater than zero and the sum of length and start index must be less than 8. "
                                 + $"Supplied length: {length}. Supplied start index: {startIndex}";
                throw new ArgumentOutOfRangeException(nameof(length), message);
            }

            int bitShift = length - 1;
            for (int i = startIndex; i < startIndex + length; i++)
            {
                int bitValueIfSet = 1 << bitShift;
                int bitValue = valueToSet & bitValueIfSet;
                int bitIsSet = bitValue >> bitShift;
                Bits[i] = bitIsSet == 1;
                bitShift--;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"PackedField [ Byte={this.Byte} ]";
        }
    }
}