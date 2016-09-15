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

using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Filters;
using Structure.Sketching.Filters.Arithmetic;
using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Formats;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace Structure.Sketching
{
    /// <summary>
    /// Represents an image
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Image(int width, int height)
            : this(width, height, (Vector4[])null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="data">The data.</param>
        public Image(int width, int height, Vector4[] data)
        {
            ReCreate(width, height, data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="data">The data.</param>
        public Image(int width, int height, float[] data)
            : this(width, height, data.ToVector4())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public Image(string fileName)
            : this(File.Open(fileName, FileMode.Open, FileAccess.Read))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="stream">The stream to copy the data from.</param>
        public Image(Stream stream)
            : this(new Manager().Decode(stream))
        {
            stream.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="image">The image to copy the data from.</param>
        public Image(Image image)
            : this(image.Width, image.Height, image.Pixels)
        {
        }

        /// <summary>
        /// Gets the center.
        /// </summary>
        /// <value>The center.</value>
        public Vector2 Center { get; private set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the pixel ratio.
        /// </summary>
        /// <value>The pixel ratio.</value>
        public double PixelRatio { get; private set; }

        /// <summary>
        /// Gets or sets the pixels.
        /// </summary>
        /// <value>The pixels.</value>
        public Vector4[] Pixels { get; private set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// The ASCII characters used
        /// </summary>
        private static readonly string[] _ASCIICharacters = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="image1">The image1.</param>
        /// <param name="image2">The image2.</param>
        /// <returns>The result of the operator.</returns>
        public static unsafe Image operator -(Image image1, Image image2)
        {
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image.Width * image.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
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
            var TempArray = new Vector4[image1.Width * image1.Height];
            Array.Copy(image1.Pixels, TempArray, TempArray.Length);
            var Result = new Image(image1.Width, image1.Height, TempArray);
            return new Brightness(value / 255f).Apply(Result);
        }

        /// <summary>
        /// Makes a copy of this image.
        /// </summary>
        /// <returns>A copy of this image.</returns>
        public Image Copy()
        {
            var data = new Vector4[Width * Height];
            Array.Copy(Pixels, data, data.Length);
            return new Image(Width, Height, data);
        }

        /// <summary>
        /// Recreates the image object using the new data.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        /// <param name="data">The new pixel data.</param>
        /// <returns>this</returns>
        public Image ReCreate(int width, int height, Vector4[] data)
        {
            Width = width < 1 ? 1 : width;
            Height = height < 1 ? 1 : height;
            PixelRatio = (double)Width / Height;
            Center = new Vector2(Width >> 1, Height >> 1);
            if (data == null)
                return this;
            Pixels = new Vector4[width * height];
            Array.Copy(data, Pixels, Pixels.Length);
            return this;
        }

        /// <summary>
        /// Saves the image to the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if it saves successfully, false otherwise</returns>
        public bool Save(string fileName)
        {
            return new Manager().Encode(fileName, this);
        }

        /// <summary>
        /// Saves the image to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="format">The format.</param>
        /// <returns>True if it saves successfully, false otherwise</returns>
        public bool Save(Stream stream, FileFormats format)
        {
            return new Manager().Encode(stream, this, format);
        }

        /// <summary>
        /// Converts the image to ASCII art.
        /// </summary>
        /// <returns>The image as ASCII art</returns>
        public string ToASCIIArt()
        {
            bool ShowLine = true;
            var TempImage = new Greyscale601().Apply(Copy());
            var Builder = new StringBuilder();
            for (int x = 0; x < TempImage.Height; ++x)
            {
                for (int y = 0; y < TempImage.Width; ++y)
                {
                    if (ShowLine)
                    {
                        var RValue = TempImage.Pixels[(y * TempImage.Width) + x].X;
                        Builder.Append(_ASCIICharacters[(int)(RValue * _ASCIICharacters.Length)]);
                    }
                }
                if (ShowLine)
                {
                    Builder.AppendLine();
                    ShowLine = false;
                }
                else
                {
                    ShowLine = true;
                }
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Returns a base64 <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="desiredFormat">The desired format.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance as a base64 instance.
        /// </returns>
        public string ToString(FileFormats desiredFormat)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                if (Save(Stream, desiredFormat))
                {
                    byte[] TempArray = Stream.ToArray();
                    return Convert.ToBase64String(TempArray, 0, TempArray.Length);
                }
                return string.Empty;
            }
        }
    }
}