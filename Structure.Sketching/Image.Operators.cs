using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Arithmetic;
using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.Effects;
using System;

namespace Structure.Sketching
{
    public partial class Image
    {
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator -(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Subtract(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator !.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator !(Image image)
        {
            var TempArray = new Color[image.Pixels.Length];
            Array.Copy(image.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image.Width, image.Height, TempArray);
            return new Invert().Apply(Result);
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator %(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Modulo(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator &(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new And(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator *(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Multiplication(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator /(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Division(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator ^(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new XOr(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator |(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Or(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator +(Image image1, Image image2)
        {
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Add(image2).Apply(Result);
        }

        /// <summary>
        /// Implements the operator &lt;&lt;.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="value">The value (should be between 0 and 255).</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator <<(Image image1, int value)
        {
            value = Math.Abs(value);
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Brightness(value / 255f).Apply(Result);
        }

        /// <summary>
        /// Implements the operator &gt;&gt;.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="value">The value.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator >>(Image image1, int value)
        {
            value = -Math.Abs(value);
            var TempArray = new Color[image1.Pixels.Length];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Brightness(value / 255f).Apply(Result);
        }
    }
}