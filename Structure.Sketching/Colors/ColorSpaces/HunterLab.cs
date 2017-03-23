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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Structure.Sketching.Colors.ColorSpaces
{
    /// <summary>
    /// LAB color space
    /// </summary>
    public struct HunterLAB : IEquatable<HunterLAB>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HunterLAB"/> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        public HunterLAB(double l, double a, double b)
        {
            L = l;
            A = a;
            B = b;
        }

        /// <summary>
        /// Gets or sets a.
        /// </summary>
        /// <value>a.</value>
        public double A { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        /// <value>The b.</value>
        public double B { get; set; }

        /// <summary>
        /// Gets or sets the l.
        /// </summary>
        /// <value>The l.</value>
        public double L { get; set; }

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float EPSILON = 0.001f;

        /// <summary>
        /// Performs an implicit conversion from <see cref="HunterLAB"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(HunterLAB color)
        {
            return (XYZ)color;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="HunterLAB"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HunterLAB(Color color)
        {
            return (XYZ)color;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="XYZ"/> to <see cref="HunterLAB"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HunterLAB(XYZ color)
        {
            var L = 10.0 * Math.Sqrt(color.Y);
            var A = color.Y != 0 ? 17.5 * (((1.02 * color.X) - color.Y) / Math.Sqrt(color.Y)) : 0;
            var B = color.Y != 0 ? 7.0 * ((color.Y - (.847 * color.Z)) / Math.Sqrt(color.Y)) : 0;
            return new HunterLAB(L, A, B);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="HunterLAB"/> to <see cref="XYZ"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator XYZ(HunterLAB color)
        {
            var x = (color.A / 17.5) * (color.L / 10.0);
            var itemL_10 = color.L / 10.0;
            var y = itemL_10 * itemL_10;
            var z = color.B / 7.0 * color.L / 10.0;

            return new XYZ((x + y) / 1.02,
                    y,
                    -(z - y) / .847
                );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HunterLAB color1, HunterLAB color2)
        {
            return !(color1 == color2);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HunterLAB color1, HunterLAB color2)
        {
            return color1.Equals(color2);
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
            return obj is HunterLAB && Equals((HunterLAB)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HunterLAB other)
        {
            return Math.Abs(other.L - L) < EPSILON
                && Math.Abs(other.A - A) < EPSILON
                && Math.Abs(other.B - B) < EPSILON;
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
            var hash = L.GetHashCode();
            hash = ComputeHash(hash, A);
            return ComputeHash(hash, B);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({L:#0.##},{A:#0.##},{B:#0.##})";

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="hash">The existing hash.</param>
        /// <param name="component">The component.</param>
        /// <returns>The resulting hash</returns>
        private int ComputeHash(int hash, double component)
        {
            return ((hash << 5) + hash) ^ component.GetHashCode();
        }
    }
}