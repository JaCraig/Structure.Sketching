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
    /// LCH color space
    /// </summary>
    public struct CIELCH : IEquatable<CIELCH>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CIELCH"/> struct.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="c">The c.</param>
        /// <param name="h">The h.</param>
        public CIELCH(double l, double c, double h)
        {
            L = l;
            C = c;
            H = h;
        }

        /// <summary>
        /// Gets or sets the c.
        /// </summary>
        /// <value>The c.</value>
        public double C { get; set; }

        /// <summary>
        /// Gets or sets the h.
        /// </summary>
        /// <value>The h.</value>
        public double H { get; set; }

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
        /// Performs an implicit conversion from <see cref="CIELCH"/> to <see cref="XYZ"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CIELab(CIELCH color)
        {
            var hRadians = color.H * Math.PI / 180.0;
            return new CIELab(color.L, Math.Cos(hRadians) * color.C, Math.Sin(hRadians) * color.C);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="CIELCH"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CIELCH(Color color)
        {
            return (CIELab)color;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CIELab"/> to <see cref="CIELCH"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CIELCH(CIELab color)
        {
            var h = Math.Atan2(color.B, color.A);
            h = h > 0 ?
                (h / Math.PI) * 180.0 :
                360 - (Math.Abs(h) / Math.PI) * 180.0;
            if (h < 0)
            {
                h += 360.0;
            }
            else if (h >= 360)
            {
                h -= 360.0;
            }

            return new CIELCH(color.L, Math.Sqrt(color.A * color.A + color.B * color.B), h);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CIELCH"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(CIELCH color)
        {
            return (CIELab)color;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CIELCH color1, CIELCH color2)
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
        public static bool operator ==(CIELCH color1, CIELCH color2)
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
            return obj is CIELCH && Equals((CIELCH)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CIELCH other)
        {
            return Math.Abs(other.L - L) < EPSILON
                && Math.Abs(other.C - C) < EPSILON
                && Math.Abs(other.H - H) < EPSILON;
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
            hash = ComputeHash(hash, C);
            return ComputeHash(hash, H);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({L:#0.##},{C:#0.##},{H:#0.##})";

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