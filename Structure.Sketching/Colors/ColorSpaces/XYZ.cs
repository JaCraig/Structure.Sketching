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
    /// XYZ color space
    /// </summary>
    public struct XYZ : IEquatable<XYZ>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XYZ"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public XYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets the white reference.
        /// </summary>
        /// <value>The white reference.</value>
        public static XYZ WhiteReference => new XYZ(95.047, 100, 108.883);

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        /// <value>The z.</value>
        public double Z { get; set; }

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float EPSILON = 0.001f;

        /// <summary>
        /// Performs an implicit conversion from <see cref="XYZ"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(XYZ color)
        {
            var x = color.X / 100.0;
            var y = color.Y / 100.0;
            var z = color.Z / 100.0;

            var r = x * 3.2406 + y * -1.5372 + z * -0.4986;
            var g = x * -0.9689 + y * 1.8758 + z * 0.0415;
            var b = x * 0.0557 + y * -0.2040 + z * 1.0570;

            return new Colors.Color(ToRgb(r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r),
            ToRgb(g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g),
            ToRgb(b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="XYZ"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator XYZ(Color color)
        {
            var r = PivotRgb(color.Red / 255.0);
            var g = PivotRgb(color.Green / 255.0);
            var b = PivotRgb(color.Blue / 255.0);
            return new XYZ(r * 0.4124 + g * 0.3576 + b * 0.1805,
                            r * 0.2126 + g * 0.7152 + b * 0.0722,
                            r * 0.0193 + g * 0.1192 + b * 0.9505);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(XYZ color1, XYZ color2)
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
        public static bool operator ==(XYZ color1, XYZ color2)
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
            return obj is XYZ && Equals((XYZ)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(XYZ other)
        {
            return Math.Abs(other.X - X) < EPSILON
                && Math.Abs(other.Y - Y) < EPSILON
                && Math.Abs(other.Z - Z) < EPSILON;
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
            var hash = X.GetHashCode();
            hash = ComputeHash(hash, Y);
            return ComputeHash(hash, Z);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({X:#0.##},{Y:#0.##},{Z:#0.##})";

        /// <summary>
        /// Pivots the RGB.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        private static double PivotRgb(double n)
        {
            return (n > 0.04045 ? Math.Pow((n + 0.055) / 1.055, 2.4) : n / 12.92) * 100.0;
        }

        /// <summary>
        /// To the RGB.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        private static byte ToRgb(double n)
        {
            return (byte)Math.Round((255.0 * n).Clamp(0, 255), MidpointRounding.AwayFromZero);
        }

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