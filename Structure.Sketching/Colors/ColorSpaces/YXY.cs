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
    /// YXY color space
    /// </summary>
    public struct YXY : IEquatable<YXY>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YXY"/> class.
        /// </summary>
        /// <param name="y1">The y1.</param>
        /// <param name="x">The x.</param>
        /// <param name="y2">The y2.</param>
        public YXY(double y1, double x, double y2)
        {
            Y2 = y2;
            X = x;
            Y1 = y1;
        }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y1.
        /// </summary>
        /// <value>The y1.</value>
        public double Y1 { get; set; }

        /// <summary>
        /// Gets or sets the y2.
        /// </summary>
        /// <value>The y2.</value>
        public double Y2 { get; set; }

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float EPSILON = 0.001f;

        /// <summary>
        /// Performs an implicit conversion from <see cref="YXY"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(YXY color)
        {
            return (XYZ)color;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="YXY"/> to <see cref="XYZ"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator XYZ(YXY color)
        {
            return new XYZ(color.X * (color.Y1 / color.Y2),
                color.Y1,
                (1.0 - color.X - color.Y2) * (color.Y1 / color.Y2));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="XYZ"/> to <see cref="YXY"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator YXY(XYZ color)
        {
            var xyz = color;
            var xDividend = xyz.X + xyz.Y + xyz.Z;
            var y2Dividend = xyz.X + xyz.Y + xyz.Z;
            return new YXY(xyz.Y,
                Math.Abs(xDividend) < EPSILON ? 0.0 : xyz.X / xDividend,
                Math.Abs(y2Dividend) < EPSILON ? 0.0 : xyz.Y / (xyz.X + xyz.Y + xyz.Z));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="YXY"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator YXY(Color color)
        {
            return (XYZ)color;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(YXY color1, YXY color2)
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
        public static bool operator ==(YXY color1, YXY color2)
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
            return obj is YXY && Equals((YXY)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(YXY other)
        {
            return Math.Abs(other.X - X) < EPSILON
                && Math.Abs(other.Y1 - Y1) < EPSILON
                && Math.Abs(other.Y2 - Y2) < EPSILON;
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
            hash = ComputeHash(hash, Y1);
            return ComputeHash(hash, Y2);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({Y1:#0.##},{X:#0.##},{Y2:#0.##})";

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