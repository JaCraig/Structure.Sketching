using Structure.Sketching.Colors.ColorSpaces.Interfaces;
using Structure.Sketching.ExtensionMethods;
using System;
using System.Runtime.CompilerServices;

namespace Structure.Sketching.Colors.ColorSpaces
{
    /// <summary>
    /// HSV color space
    /// </summary>
    /// <seealso cref="System.IEquatable{HSV}" />
    /// <seealso cref="IColorSpace" />
    public struct HSV : IEquatable<HSV>, IColorSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HSV"/> struct.
        /// </summary>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="value">The value.</param>
        public HSV(double hue, double saturation, double value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the hue.
        /// </summary>
        /// <value>
        /// The hue.
        /// </value>
        public double Hue { get; set; }

        /// <summary>
        /// Gets or sets the saturation.
        /// </summary>
        /// <value>
        /// The saturation.
        /// </value>
        public double Saturation { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// The epsilon value for double comparison
        /// </summary>
        private static double EPSILON = 0.01d;

        /// <summary>
        /// Performs an implicit conversion from <see cref="HSV"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Color(HSV color)
        {
            double Red = 0;
            double Green = 0;
            double Blue = 0;
            if (Math.Abs(color.Saturation) < EPSILON)
            {
                Red = color.Value;
                Green = color.Value;
                Blue = color.Value;
            }
            else
            {
                double a = (color.Hue / 360d) * 6;
                double b = Math.Floor(a);
                double e = color.Value * (1 - color.Saturation);
                double f = color.Value * (1 - color.Saturation * (a - b));
                double g = color.Value * (1 - color.Saturation * (1 - (a - b)));

                switch ((int)b)
                {
                    case 6:
                    case 0: Red = color.Value; Green = g; Blue = e; break;
                    case 1: Red = f; Green = color.Value; Blue = e; break;
                    case 2: Red = e; Green = color.Value; Blue = g; break;
                    case 3: Red = e; Green = f; Blue = color.Value; break;
                    case 4: Red = g; Green = e; Blue = color.Value; break;
                    default: Red = color.Value; Green = e; Blue = f; break;
                }
            }

            return new Color(((byte)(Red * 255)).Clamp(0, 255),
                             ((byte)(Green * 255)).Clamp(0, 255),
                             ((byte)(Blue * 255)).Clamp(0, 255));
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Color"/> to <see cref="HSV"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator HSV(Color color)
        {
            double Hue = 0;
            double Saturation = 0;
            double Value = 0;
            double max = Math.Max(color.Red, Math.Max(color.Green, color.Blue));
            double min = Math.Min(color.Red, Math.Min(color.Green, color.Blue));
            if (Math.Abs(min - max) < EPSILON)
            {
                Value = min;
            }
            else
            {
                double c;
                if (Math.Abs(color.Red - min) < EPSILON) c = color.Green - color.Blue;
                else if (Math.Abs(color.Blue - min) < EPSILON) c = color.Red - color.Green;
                else c = color.Blue - color.Red;
                double d;
                if (Math.Abs(color.Red - min) < EPSILON) d = 3;
                else if (Math.Abs(color.Blue - min) < EPSILON) d = 1;
                else d = 5;
                Hue = 60d * (d - c / (max - min));
                Value = max;
            }
            Saturation = Math.Abs(max) < EPSILON ? 0 : (max - min) / max;
            return new HSV(Hue, Saturation, Value / 255d);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HSV left, HSV right)
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
        public static bool operator ==(HSV left, HSV right)
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
            return obj is HSV && Equals((HSV)obj);
        }

        /// <summary>
        /// Determines if the items are equal
        /// </summary>
        /// <param name="other">The other HSV color.</param>
        /// <returns>True if they are, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HSV other)
        {
            return Math.Abs(other.Hue - Hue) < EPSILON
                && Math.Abs(other.Saturation - Saturation) < EPSILON
                && Math.Abs(other.Value - Value) < EPSILON;
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
            var hash = Hue.GetHashCode();
            hash = ComputeHash(hash, Saturation);
            return ComputeHash(hash, Value);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => $"({Hue:#0.##},{Saturation:#0.##},{Value:#0.##})";

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