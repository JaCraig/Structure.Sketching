using Structure.Sketching.Formats.Interfaces;
using System.IO;

namespace Structure.Sketching.Formats.BaseClasses
{
    /// <summary>
    /// Animation format base class
    /// </summary>
    /// <seealso cref="FormatBase" />
    /// <seealso cref="IAnimationFormat" />
    public abstract class AnimationFormatBase : FormatBase, IAnimationFormat
    {
        /// <summary>
        /// Decodes the specified stream and returns an image
        /// </summary>
        /// <param name="stream">The stream containing the image data.</param>
        /// <returns>
        /// The resulting image
        /// </returns>
        public Animation DecodeAnimation(Stream stream)
        {
            return Decoder.DecodeAnimation(stream);
        }

        /// <summary>
        /// Encodes an image and places it in the specified writer.
        /// </summary>
        /// <param name="writer">The binary writer.</param>
        /// <param name="image">The image to encode.</param>
        /// <returns>True if it is encoded, false otherwise.</returns>
        public bool Encode(BinaryWriter writer, Animation image)
        {
            return Encoder.Encode(writer, image);
        }
    }
}