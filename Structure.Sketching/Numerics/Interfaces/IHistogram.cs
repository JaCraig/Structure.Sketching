using Structure.Sketching.Colors;

namespace Structure.Sketching.Numerics.Interfaces
{
    /// <summary>
    /// Histogram interface
    /// </summary>
    public interface IHistogram
    {
        /// <summary>
        /// Equalizes the histogram
        /// </summary>
        /// <returns>this</returns>
        IHistogram Equalize();

        /// <summary>
        /// Equalizes the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The resulting color</returns>
        Color EqualizeColor(Color color);

        /// <summary>
        /// Loads an image
        /// </summary>
        /// <param name="image">Image to load</param>
        /// <returns>this</returns>
        IHistogram LoadImage(Image image);

        /// <summary>
        /// Normalizes the histogram
        /// </summary>
        /// <returns>this</returns>
        IHistogram Normalize();
    }
}