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
using Structure.Sketching.IO;
using Structure.Sketching.IO.Converters.BaseClasses;
using Structure.Sketching.Quantizers;
using Structure.Sketching.Quantizers.Interfaces;
using Structure.Sketching.Quantizers.Octree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Gif file class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.BaseClasses.FileBase"/>
    public class File : FileBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="File"/> class.
        /// </summary>
        public File()
        {
            Frames = new List<Frame>();
            Quantizer = new OctreeQuantizer { TransparencyThreshold = TransparencyThreshold };
            BitDepth = (int)Math.Ceiling(Math.Log(Quality, 2));
        }

        /// <summary>
        /// Gets the application extension.
        /// </summary>
        /// <value>The application extension.</value>
        public ApplicationExtension AppExtension { get; private set; }

        /// <summary>
        /// Gets the bit depth.
        /// </summary>
        /// <value>The bit depth.</value>
        public int BitDepth { get; private set; }

        /// <summary>
        /// Gets the color table.
        /// </summary>
        /// <value>The color table.</value>
        public ColorTable ColorTable { get; private set; }

        /// <summary>
        /// Gets the frames.
        /// </summary>
        /// <value>The frames.</value>
        public List<Frame> Frames { get; private set; }

        /// <summary>
        /// Gets the graphics control extension.
        /// </summary>
        /// <value>The graphics control extension.</value>
        public GraphicsControl GraphicsControlExtension { get; private set; }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <value>The header.</value>
        public FileHeader Header { get; private set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public int Quality { get; set; } = 256;

        /// <summary>
        /// Gets or sets the quantizer.
        /// </summary>
        /// <value>The quantizer.</value>
        public IQuantizer Quantizer { get; set; }

        /// <summary>
        /// Gets the screen descriptor.
        /// </summary>
        /// <value>The screen descriptor.</value>
        public LogicalScreenDescriptor ScreenDescriptor { get; private set; }

        /// <summary>
        /// Gets or sets the transparency threshold.
        /// </summary>
        /// <value>The transparency threshold.</value>
        public byte TransparencyThreshold { get; set; } = 128;

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>This.</returns>
        public override FileBase Decode(Stream stream)
        {
            Header = FileHeader.Read(stream);
            ScreenDescriptor = LogicalScreenDescriptor.Read(stream);
            if (ScreenDescriptor.GlobalColorTablePresent)
            {
                ColorTable = ColorTable.Read(stream, ScreenDescriptor.GlobalColorTableSize);
            }
            int Flag = stream.ReadByte();
            while (Flag != SectionTypes.Terminator)
            {
                if (Flag == SectionTypes.ImageLabel)
                {
                    Frames.Add(Frame.Read(stream, ColorTable, GraphicsControlExtension, ScreenDescriptor, Frames));
                }
                else if (Flag == SectionTypes.ExtensionIntroducer)
                {
                    var Label = (SectionTypes)stream.ReadByte();
                    if (Label == SectionTypes.GraphicControlLabel)
                    {
                        GraphicsControlExtension = GraphicsControl.Read(stream);
                    }
                    else if (Label == SectionTypes.CommentLabel)
                    {
                        Comment.Read(stream);
                    }
                    else if (Label == SectionTypes.ApplicationExtensionLabel)
                    {
                        ApplicationExtension.Read(stream);
                    }
                    else if (Label == SectionTypes.PlainTextLabel)
                    {
                        PlainText.Read(stream);
                    }
                }
                else if (Flag == SectionTypes.EndIntroducer)
                {
                    break;
                }

                Flag = stream.ReadByte();
            }
            return this;
        }

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="animation">The animation.</param>
        /// <returns>True if it writes successfully, false otherwise.</returns>
        public override bool Write(BinaryWriter writer, Animation animation)
        {
            QuantizedImage Quantized = Quantizer.Quantize(animation[0], Quality);
            LoadAnimation(animation, Quantized);
            WriteToFile(writer);
            return true;
        }

        /// <summary>
        /// Writes to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="image">The image.</param>
        /// <returns>True if it writes successfully, false otherwise.</returns>
        public override bool Write(BinaryWriter stream, Image image)
        {
            QuantizedImage Quantized = Quantizer.Quantize(image, Quality);
            LoadImage(image, Quantized);
            WriteToFile(stream);
            return true;
        }

        /// <summary>
        /// Converts the file to an animation
        /// </summary>
        /// <returns>The animation version of the file</returns>
        protected override Animation ToAnimation()
        {
            short Delay = (GraphicsControlExtension ?? new GraphicsControl(0, 0, false, Enums.DisposalMethod.Undefined)).Delay;
            return new Animation(Frames.Select(x => new Image(ScreenDescriptor.Width, ScreenDescriptor.Height, x.Data)), Delay);
        }

        /// <summary>
        /// Converts the file to an image.
        /// </summary>
        /// <returns>The image version of the file.</returns>
        protected override Image ToImage()
        {
            return new Image(ScreenDescriptor.Width, ScreenDescriptor.Height, Frames[0].Data);
        }

        /// <summary>
        /// Loads the animation.
        /// </summary>
        /// <param name="animation">The animation.</param>
        /// <param name="quantizedImage">The quantized image.</param>
        private void LoadAnimation(Animation animation, QuantizedImage quantizedImage)
        {
            var TempImage = animation[0];
            var TransparencyIndex = quantizedImage.TransparentIndex;

            Header = new FileHeader();
            ScreenDescriptor = new LogicalScreenDescriptor(TempImage, TransparencyIndex, BitDepth);
            Frames.Add(new Frame(TempImage, quantizedImage, BitDepth, animation.Delay));
            if (animation.Count > 1)
            {
                AppExtension = new ApplicationExtension(animation.RepeatCount, animation.Count);
                for (int x = 1; x < animation.Count; ++x)
                {
                    quantizedImage = Quantizer.Quantize(animation[x], Quality);
                    TempImage = animation[x];
                    Frames.Add(new Frame(TempImage, quantizedImage, BitDepth, animation.Delay));
                }
            }
        }

        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="quantizedImage">The quantized image.</param>
        private void LoadImage(Image image, QuantizedImage quantizedImage)
        {
            Header = new FileHeader();
            ScreenDescriptor = new LogicalScreenDescriptor(image, quantizedImage.TransparentIndex, BitDepth);
            Frames.Add(new Frame(image, quantizedImage, BitDepth, 150));
        }

        private void WriteToFile(BinaryWriter writer)
        {
            using (EndianBinaryWriter writer2 = new EndianBinaryWriter(EndianBitConverterBase.LittleEndian, writer.BaseStream))
            {
                Header.Write(writer2);
                ScreenDescriptor.Write(writer2);
                Frames[0].Write(writer2);
                if (Frames.Count > 1)
                {
                    AppExtension.Write(writer2);
                    for (int x = 0; x < Frames.Count; ++x)
                    {
                        Frames[x].Write(writer2);
                    }
                }
                writer.Write(SectionTypes.EndIntroducer);
            }
        }
    }
}