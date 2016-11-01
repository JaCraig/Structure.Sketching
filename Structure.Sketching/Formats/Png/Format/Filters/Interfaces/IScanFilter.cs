namespace Structure.Sketching.Formats.Png.Format.Filters.Interfaces
{
    /// <summary>
    /// Scan line filter interface for scan
    /// </summary>
    public interface IScanFilter
    {
        /// <summary>
        /// Decodes the specified scanline.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        /// <param name="previousScanline">The previous scanline.</param>
        /// <param name="scanlineStep">The scanline step.</param>
        /// <returns>The resulting byte array</returns>
        byte[] Decode(byte[] scanline, byte[] previousScanline, int scanlineStep);

        /// <summary>
        /// Encodes the specified scanline.
        /// </summary>
        /// <param name="scanline">The scanline.</param>
        /// <param name="previousScanline">The previous scanline.</param>
        /// <param name="scanlineStep">The scanline step.</param>
        /// <returns>The resulting byte array</returns>
        byte[] Encode(byte[] scanline, byte[] previousScanline, int scanlineStep);
    }
}