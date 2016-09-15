using Structure.Sketching.Formats.Bmp.Format.PixelFormats.Interfaces;

namespace Structure.Sketching.Tests.Formats.Bmp.Format.PixelFormats.BaseClasses
{
    public abstract class FormatBaseFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBaseFixture"/> class.
        /// </summary>
        protected FormatBaseFixture()
        {
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public abstract string FileName { get; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>The format.</value>
        public abstract IPixelFormat Format { get; }
    }
}