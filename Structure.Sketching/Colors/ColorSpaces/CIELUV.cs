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
    /// LUV color space
    /// </summary>
    public struct CIELUV : IEquatable<CIELUV>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CIELUV"/> class.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <param name="u">The u.</param>
        /// <param name="v">The v.</param>
        public CIELUV(double l, double u, double v)
        {
            L = l;
            U = u;
            V = v;
        }

        /// <summary>
        /// Gets or sets the l.
        /// </summary>
        /// <value>The l.</value>
        public double L { get; set; }

        /// <summary>
        /// Gets or sets the u.
        /// </summary>
        /// <value>The u.</value>
        public double U { get; set; }

        /// <summary>
        /// Gets or sets the v.
        /// </summary>
        /// <value>The v.</value>
        public double V { get; set; }

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float EPSILON = 0.001f;

        /// <summary>
        /// Intent is 24389/27
        /// </summary>
        private const double Kappa = 903.3;

        /// <summary>
        /// Intent is 216/24389
        /// </summary>
        private const double XYZEpsilon = 0.008856;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="CIELUV"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CIELUV(Color color)
        {
            return (XYZ)color;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="XYZ"/> to <see cref="CIELUV"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CIELUV(XYZ color)
        {
            var White = XYZ.WhiteReference;

            var y = color.Y / White.Y;
            var L = y > XYZEpsilon ? 116.0 * y.CubicRoot() - 16.0 : Kappa * y;

            var targetDenominator = GetDenominator(color);
            var referenceDenominator = GetDenominator(White);
            var xTarget = targetDenominator == 0 ? 0 : ((4.0 * color.X / targetDenominator) - (4.0 * White.X / referenceDenominator));
            var yTarget = targetDenominator == 0 ? 0 : ((9.0 * color.Y / targetDenominator) - (9.0 * White.Y / referenceDenominator));
            var U = 13.0 * L * xTarget;
            var V = 13.0 * L * yTarget;
            return new CIELUV(L, U, V);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CIELUV"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(CIELUV color)
        {
            return (XYZ)color;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CIELUV"/> to <see cref="XYZ"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator XYZ(CIELUV color)
        {
            var White = XYZ.WhiteReference;
            var C = -1.0 / 3.0;
            var UPrime = (4.0 * White.X) / GetDenominator(White);
            var VPrime = (9.0 * White.Y) / GetDenominator(White);
            var A = (1.0 / 3.0) * ((52.0 * color.L) / (color.U + 13 * color.L * UPrime) - 1.0);
            var ImteL_16_116 = (color.L + 16.0) / 116.0;
            var Y = color.L > Kappa * XYZEpsilon
                        ? ImteL_16_116 * ImteL_16_116 * ImteL_16_116
                        : color.L / Kappa;
            var B = -5.0 * Y;
            var D = Y * ((39.0 * color.L) / (color.V + 13.0 * color.L * VPrime) - 5.0);
            var X = (D - B) / (A - C);
            var Z = X * A + B;
            return new XYZ(100 * X, 100 * Y, 100 * Z);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CIELUV color1, CIELUV color2)
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
        public static bool operator ==(CIELUV color1, CIELUV color2)
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
            return obj is CIELUV && Equals((CIELUV)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CIELUV other)
        {
            return Math.Abs(other.L - L) < EPSILON
                && Math.Abs(other.U - U) < EPSILON
                && Math.Abs(other.V - V) < EPSILON;
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
            hash = ComputeHash(hash, U);
            return ComputeHash(hash, V);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({L:#0.##},{U:#0.##},{V:#0.##})";

        /// <summary>
        /// Gets the denominator.
        /// </summary>
        /// <param name="xyz">The xyz.</param>
        /// <returns>The denominator</returns>
        private static double GetDenominator(XYZ xyz)
        {
            return xyz.X + 15.0 * xyz.Y + 3.0 * xyz.Z;
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