using Structure.Sketching.Formats.Png.Format.Enums;
using Structure.Sketching.Formats.Png.Format.Filters.BaseClasses;
using System;

namespace Structure.Sketching.Formats.Png.Format.Filters
{
    /// <summary>
    /// Paeth filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Png.Format.Filters.BaseClasses.FilterBaseClass"/>
    public class PaethFilter : FilterBaseClass
    {
        /// <summary>
        /// Gets the filter value.
        /// </summary>
        /// <value>The filter value.</value>
        public override byte FilterValue => (byte)FilterType.Paeth;

        /// <summary>
        /// Calculates the value to add based on the left, up, and upper left bytes.
        /// </summary>
        /// <param name="left">The left byte.</param>
        /// <param name="above">The above byte.</param>
        /// <param name="upperLeft">The upper left byte.</param>
        /// <returns>The resulting byte.</returns>
        protected override byte Calculate(byte left, byte above, byte upperLeft)
        {
            int p = left + above - upperLeft;
            var pa = Math.Abs(p - left);
            var pb = Math.Abs(p - above);
            var pc = Math.Abs(p - upperLeft);
            if (pa <= pb && pa <= pc)
            {
                return left;
            }
            if (pb <= pc)
            {
                return above;
            }
            return upperLeft;
        }
    }
}