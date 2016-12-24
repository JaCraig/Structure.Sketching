using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Drawing.Interfaces;

namespace Structure.Sketching.Filters.Drawing.BaseClasses
{
    /// <summary>
    /// Shape base class
    /// </summary>
    /// <seealso cref="IShape"/>
    public abstract class ShapeBaseClass : IShape
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeBaseClass"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        protected ShapeBaseClass(Color color)
        {
            Color = color;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color { get; set; }

        /// <summary>
        /// Applies the shape to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The resulting image</returns>
        public abstract Image Apply(Image image, Numerics.Rectangle targetLocation = default(Numerics.Rectangle));

        /// <summary>
        /// Gets the fractional part of a number
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The fractional portion of the number</returns>
        protected double FractionalPart(double value)
        {
            return value - (int)value;
        }

        /// <summary>
        /// Plots the pixel to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="brightness">The brightness of the pixel.</param>
        /// <param name="targetLocation">The target location.</param>
        protected void Plot(Image image, int x, int y, float brightness, Numerics.Rectangle targetLocation)
        {
            if (!targetLocation.Contains(x, y))
                return;
            var Offset = (y * image.Width) + x;
            var TempColor = Color * brightness;
            //float TempAlpha = TempColor.Alpha / 255f;
            image.Pixels[Offset] = TempColor;
        }

        /// <summary>
        /// Gets the RF part of the value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>One minus the fractional portion of the value</returns>
        protected double RFPart(double value)
        {
            return 1 - FractionalPart(value);
        }

        /// <summary>
        /// Rounds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Rounds the specified value</returns>
        protected double Round(double value)
        {
            return (int)(value + 0.5);
        }
    }
}