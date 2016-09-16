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

using Structure.Sketching.Colors.ColorSpaces.Interfaces;
using Structure.Sketching.ExtensionMethods;
using System;
using System.Runtime.CompilerServices;

namespace Structure.Sketching.Colors.ColorSpaces
{
    /// <summary>
    /// BGRA color space
    /// </summary>
    /// <seealso cref="IColorSpace"/>
    /// <seealso cref="IEquatable{Bgra}"/>
    public struct Bgra : IEquatable<Bgra>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bgra"/> struct.
        /// </summary>
        /// <param name="blue">The blue.</param>
        /// <param name="green">The green.</param>
        /// <param name="red">The red.</param>
        /// <param name="alpha">The alpha.</param>
        public Bgra(byte blue, byte green, byte red, byte alpha = 255)
        {
            r = red.Clamp(0, 255);
            g = green.Clamp(0, 255);
            b = blue.Clamp(0, 255);
            a = alpha.Clamp(0, 255);
        }

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        public byte Alpha
        {
            get { return a; }
            set { a = value; }
        }

        /// <summary>
        /// Gets or sets the blue.
        /// </summary>
        /// <value>The blue.</value>
        public byte Blue
        {
            get { return b; }
            set { b = value; }
        }

        /// <summary>
        /// Gets or sets the green.
        /// </summary>
        /// <value>The green.</value>
        public byte Green
        {
            get { return g; }
            set { g = value; }
        }

        /// <summary>
        /// Gets or sets the red.
        /// </summary>
        /// <value>The red.</value>
        public byte Red
        {
            get { return r; }
            set { r = value; }
        }

        /// <summary>
        /// alpha component
        /// </summary>
        private byte a;

        /// <summary>
        /// blue component
        /// </summary>
        private byte b;

        /// <summary>
        /// green component
        /// </summary>
        private byte g;

        /// <summary>
        /// red component
        /// </summary>
        private byte r;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="Bgra"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Bgra(Color color)
        {
            return new Bgra((byte)color.Blue, (byte)color.Green, (byte)color.Red, (byte)color.Alpha);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Bgra"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(Bgra color)
        {
            return new Color(color.Red, color.Green, color.Blue, color.Alpha);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Bgra"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(Bgra color)
        {
            return color.r << 16 | color.g << 8 | color.b << 0 | color.a << 24;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Bgra left, Bgra right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Bgra left, Bgra right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Bgra && Equals((Bgra)obj);
        }

        /// <summary>
        /// Determines if the items are equal
        /// </summary>
        /// <param name="other">The other Bgra color.</param>
        /// <returns>True if they are, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Bgra other)
        {
            return other.b == b
                && other.g == g
                && other.r == r
                && other.a == a;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures
        /// like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            int hash = b.GetHashCode();
            hash = ComputeHash(hash, g);
            hash = ComputeHash(hash, r);
            return ComputeHash(hash, a);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({b:#0.##},{g:#0.##},{r:#0.##},{a:#0.##})";

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="hash">The existing hash.</param>
        /// <param name="component">The component.</param>
        /// <returns>The resulting hash</returns>
        private int ComputeHash(int hash, byte component)
        {
            return ((hash << 5) + hash) ^ component.GetHashCode();
        }
    }
}