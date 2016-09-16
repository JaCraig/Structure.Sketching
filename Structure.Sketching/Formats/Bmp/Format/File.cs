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

using Structure.Sketching.Formats.BaseClasses;
using System.IO;

namespace Structure.Sketching.Formats.Bmp.Format
{
    /// <summary>
    /// BMP file
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Interfaces.IFile"/>
    public class File : FileBase
    {
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public Body Body { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public FileHeader FileHeader { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public Header Header { get; set; }

        /// <summary>
        /// Gets or sets the palette.
        /// </summary>
        /// <value>The palette.</value>
        public Palette Palette { get; set; }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>This.</returns>
        public override FileBase Decode(Stream stream)
        {
            FileHeader = FileHeader.Read(stream);
            Header = Header.Read(stream);
            Palette = Palette.Read(Header, stream);
            Body = Body.Read(Header, Palette, stream);
            return this;
        }

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="image">The image.</param>
        /// <returns>True if it writes successfully, false otherwise.</returns>
        public override bool Write(BinaryWriter stream, Image image)
        {
            LoadImage(image);
            FileHeader.Write(stream);
            Header.Write(stream);
            Body.Write(stream);
            return true;
        }

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="animation">The animation.</param>
        /// <returns>True if it writes successfully, false otherwise.</returns>
        public override bool Write(BinaryWriter writer, Animation animation)
        {
            return Write(writer, animation[0]);
        }

        /// <summary>
        /// Converts the file to an animation.
        /// </summary>
        /// <returns>The animation version of the file.</returns>
        protected override Animation ToAnimation()
        {
            return new Animation(new Image[] { ToImage() }, 0);
        }

        /// <summary>
        /// Converts the file to an image.
        /// </summary>
        /// <returns>The image version of the file.</returns>
        protected override Image ToImage()
        {
            return new Image(Header.Width, Header.Height, Body.Data);
        }

        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="image">The image.</param>
        private void LoadImage(Image image)
        {
            var ImageSize = image.Pixels.Length;
            FileHeader = new FileHeader(54 + ImageSize, 54);
            Header = new Header(image.Width, image.Height, 24, ImageSize, 0, 0, 0, 0, Compression.RGB);
            Palette = new Palette(0, new byte[0]);
            Body = new Body(image);
        }
    }
}