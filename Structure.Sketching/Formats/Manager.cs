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

using Structure.Sketching.Formats.Bmp;
using Structure.Sketching.Formats.Gif;
using Structure.Sketching.Formats.Interfaces;
using Structure.Sketching.Formats.Jpeg;
using Structure.Sketching.Formats.Png;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats
{
    /// <summary>
    /// Format manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Manager"/> class.
        /// </summary>
        public Manager()
            : this(new IFormat[] { new BmpFormat(), new PngFormat(), new JpegFormat(), new GifFormat() },
                   new IAnimationFormat[] { new GifFormat() })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Manager" /> class.
        /// </summary>
        /// <param name="formats">The formats.</param>
        /// <param name="animationFormats">The animation formats.</param>
        public Manager(IEnumerable<IFormat> formats, IEnumerable<IAnimationFormat> animationFormats)
        {
            Formats = formats.ToList();
            AnimationFormats = animationFormats.ToList();
        }

        /// <summary>
        /// Gets or sets the animation formats.
        /// </summary>
        /// <value>
        /// The animation formats.
        /// </value>
        private List<IAnimationFormat> AnimationFormats { get; set; }

        /// <summary>
        /// Gets or sets the formats.
        /// </summary>
        /// <value>The formats.</value>
        private List<IFormat> Formats { get; set; }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An image object of the stream</returns>
        public Image Decode(Stream stream)
        {
            foreach (IFormat Format in Formats)
            {
                if (Format.CanDecode(stream))
                {
                    return Format.Decode(stream);
                }
            }
            return null;
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An animation object of the stream</returns>
        public Animation DecodeAnimation(Stream stream)
        {
            foreach (IAnimationFormat Format in AnimationFormats)
            {
                if (Format.CanDecode(stream))
                {
                    return Format.DecodeAnimation(stream);
                }
            }
            return null;
        }

        /// <summary>
        /// Encodes the image and saves it to the specified file name
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="image">The image.</param>
        /// <returns>True if it is encoded successfully, false otherwise</returns>
        public bool Encode(string fileName, Image image)
        {
            foreach (IFormat Format in Formats)
            {
                if (Format.CanEncode(fileName))
                {
                    new FileInfo(fileName).Directory.Create();
                    using (var ImageFile = File.OpenWrite(fileName))
                    {
                        return Encode(ImageFile, image, Format.Format);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Encodes the animation and saves it to the specified file name
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="animation">The animation.</param>
        /// <returns>
        /// True if it is encoded successfully, false otherwise
        /// </returns>
        public bool Encode(string fileName, Animation animation)
        {
            foreach (IAnimationFormat Format in AnimationFormats)
            {
                if (Format.CanEncode(fileName))
                {
                    new FileInfo(fileName).Directory.Create();
                    using (var ImageFile = File.OpenWrite(fileName))
                    {
                        return Encode(ImageFile, animation, Format.Format);
                    }
                }
            }
            return Encode(fileName, animation[0]);
        }

        /// <summary>
        /// Encodes the animation and saves it to the specified file name
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="animation">The animation.</param>
        /// <param name="format">The format.</param>
        /// <returns>
        /// True if it is encoded successfully, false otherwise
        /// </returns>
        public bool Encode(Stream stream, Animation animation, FileFormats format)
        {
            using (var TempWriter = new BinaryWriter(stream))
            {
                return AnimationFormats.First(x => x.Format == format).Encode(TempWriter, animation);
            }
        }

        /// <summary>
        /// Encodes the image and saves it to the specified stream
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns>True if it is encoded successfully, false otherwise</returns>
        public bool Encode(Stream stream, Image image, FileFormats format)
        {
            using (var TempWriter = new BinaryWriter(stream))
            {
                return Formats.First(x => x.Format == format).Encode(TempWriter, image);
            }
        }
    }
}