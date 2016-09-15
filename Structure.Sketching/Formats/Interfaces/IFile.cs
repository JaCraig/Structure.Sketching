using System.IO;

namespace Structure.Sketching.Formats.Interfaces
{
    /// <summary>
    /// File format interface
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="image">The image.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise.
        /// </returns>
        bool Write(BinaryWriter stream, Image image);
    }
}