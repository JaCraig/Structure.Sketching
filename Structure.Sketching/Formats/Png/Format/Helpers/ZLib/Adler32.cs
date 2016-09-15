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

namespace Structure.Sketching.Formats.Png.Format.Helpers.ZLib
{
    /// <summary>
    /// Adler32 check sum
    /// </summary>
    public class Adler32
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Adler32"/> class.
        /// </summary>
        public Adler32()
        {
            Value = 1;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public uint Value { get; set; }

        /// <summary>
        /// largest prime smaller than 65536
        /// </summary>
        private const uint Base = 65521;

        /// <summary>
        /// Updates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The updated value.</returns>
        public long Update(int value)
        {
            uint s1 = Value & 0xFFFF;
            uint s2 = Value >> 16;
            s1 = (s1 + ((uint)value & 0xFF)) % Base;
            s2 = (s1 + s2) % Base;
            Value = (s2 << 16) + s1;
            return Value;
        }

        /// <summary>
        /// Updates the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>The resulting value.</returns>
        public long Update(byte[] buffer, int offset = -1, int count = int.MaxValue)
        {
            buffer = buffer ?? new byte[0];
            count = count.Clamp(0, buffer.Length);
            offset = offset.Clamp(0, buffer.Length);
            uint s1 = Value & 0xFFFF;
            uint s2 = Value >> 16;
            while (count > 0)
            {
                int n = 3800;
                if (n > count)
                {
                    n = count;
                }
                count -= n;
                while (--n >= 0)
                {
                    s1 = s1 + (uint)(buffer[offset++] & 0xff);
                    s2 = s2 + s1;
                }
                s1 %= Base;
                s2 %= Base;
            }
            Value = (s2 << 16) | s1;
            return Value;
        }
    }
}