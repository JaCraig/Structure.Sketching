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
    /// CIELAB
    /// </summary>
    /// <seealso cref="IColorSpace"/>
    /// <seealso cref="IEquatable{CIELab}"/>
    public struct CIELab : IEquatable<CIELab>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CIELab"/> struct.
        /// </summary>
        /// <param name="lightness">The lightness.</param>
        /// <param name="aComponent">The a component.</param>
        /// <param name="bComponent">The b component.</param>
        public CIELab(float lightness, float aComponent, float bComponent)
        {
            l = lightness.Clamp(0, 100);
            a = aComponent.Clamp(-100, 100);
            b = bComponent.Clamp(-100, 100);
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        /// <value>a component.</value>
        public float AComponent
        {
            get { return a; }
            set { a = value; }
        }

        /// <summary>
        /// Gets or sets the b component.
        /// </summary>
        /// <value>The b component.</value>
        public float BComponent
        {
            get { return b; }
            set { b = value; }
        }

        /// <summary>
        /// Gets or sets the lightness.
        /// </summary>
        /// <value>The lightness.</value>
        public float Lightness
        {
            get { return l; }
            set { l = value; }
        }

        /// <summary>
        /// The epsilon
        /// </summary>
        private const float EPSILON = 0.001f;

        /// <summary>
        /// a component
        /// </summary>
        private float a;

        /// <summary>
        /// b component
        /// </summary>
        private float b;

        /// <summary>
        /// lightness
        /// </summary>
        private float l;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="CIELab"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator CIELab(Color color)
        {
            float x = (((color.Red / 255f) * 0.4124F) + ((color.Green / 255f) * 0.3576F) + ((color.Blue / 255f) * 0.1805F)) / 0.95047f;
            float y = ((color.Red / 255f) * 0.2126F) + ((color.Green / 255f) * 0.7152F) + ((color.Blue / 255f) * 0.0722F);
            float z = (((color.Red / 255f) * 0.0193F) + ((color.Green / 255f) * 0.1192F) + ((color.Blue / 255f) * 0.9505F)) / 1.08883f;

            x = x > 0.008856f ? (float)Math.Pow(x, 0.3333333f) : (903.3f * x + 16f) / 116f;
            y = y > 0.008856f ? (float)Math.Pow(y, 0.3333333f) : (903.3f * y + 16f) / 116f;
            z = z > 0.008856f ? (float)Math.Pow(z, 0.3333333f) : (903.3f * z + 16f) / 116f;

            var l = Math.Max(0, (116f * y) - 16f);
            float a = 500f * (x - y);
            float b = 200f * (y - z);

            return new CIELab(l, a, b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CIELab"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Color(CIELab color)
        {
            float y = (color.Lightness + 16f) / 116f;
            float x = (color.AComponent / 500f) + y;
            float z = y - (color.BComponent / 200f);

            float x3 = x * x * x;
            float y3 = y * y * y;
            float z3 = z * z * z;

            x = (x3 > 0.008856f ? x3 : (x - 0.137931f) / 7.787f) * 0.95047f;
            y = (color.Lightness > 7.999625f) ? y3 : (color.Lightness / 903.3f);
            z = ((z3 > 0.008856f) ? z3 : (z - 0.137931f) / 7.787f) * 1.08883f;

            float r = (x * 3.2406f) + (y * -1.5372f) + (z * -0.4986f);
            float g = (x * -0.9689f) + (y * 1.8758f) + (z * 0.0415f);
            float b = (x * 0.0557f) + (y * -0.2040f) + (z * 1.0570f);

            return new Color((byte)(r * 255f), (byte)(g * 255f), (byte)(b * 255f));
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(CIELab left, CIELab right)
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
        public static bool operator ==(CIELab left, CIELab right)
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
            return obj is CIELab && Equals((CIELab)obj);
        }

        /// <summary>
        /// Determines if the items are equal
        /// </summary>
        /// <param name="other">The other CIELab color.</param>
        /// <returns>True if they are, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CIELab other)
        {
            return Math.Abs(other.l - l) < EPSILON
                && Math.Abs(other.a - a) < EPSILON
                && Math.Abs(other.b - b) < EPSILON;
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
            var hash = l.GetHashCode();
            hash = ComputeHash(hash, a);
            return ComputeHash(hash, b);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({l:#0.##},{a:#0.##},{b:#0.##})";

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="hash">The existing hash.</param>
        /// <param name="component">The component.</param>
        /// <returns>The resulting hash</returns>
        private int ComputeHash(int hash, float component)
        {
            return ((hash << 5) + hash) ^ component.GetHashCode();
        }
    }
}