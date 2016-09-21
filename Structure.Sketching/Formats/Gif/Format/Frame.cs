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

using Structure.Sketching.Formats.Gif.Format.BaseClasses;
using Structure.Sketching.Formats.Gif.Format.Enums;
using Structure.Sketching.IO;
using Structure.Sketching.Quantizers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Structure.Sketching.Formats.Gif.Format
{
    /// <summary>
    /// Frame data holder
    /// </summary>
    /// <seealso cref="Structure.Sketching.Formats.Gif.Format.BaseClasses.SectionBase" />
    public class Frame : SectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Frame" /> class.
        /// </summary>
        /// <param name="graphicsControl">The graphics control.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="localColorTable">The local color table.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="data">The data.</param>
        public Frame(GraphicsControl graphicsControl, ImageDescriptor descriptor, ColorTable localColorTable, FrameIndices indices, byte[] data)
        {
            Data = data;
            Indices = indices;
            Descriptor = descriptor;
            LocalColorTable = localColorTable;
            GraphicsControl = graphicsControl;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="quantizedImage">The quantized image.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="delay">The delay.</param>
        public Frame(Image image, QuantizedImage quantizedImage, int bitDepth, short delay)
            : this(new GraphicsControl(image, quantizedImage, delay),
                new ImageDescriptor(image, bitDepth),
                new ColorTable(quantizedImage, bitDepth),
                new FrameIndices(quantizedImage, bitDepth),
                null)
        {
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets or sets the descriptor.
        /// </summary>
        /// <value>
        /// The descriptor.
        /// </value>
        public ImageDescriptor Descriptor { get; set; }

        /// <summary>
        /// Gets the graphics control.
        /// </summary>
        /// <value>
        /// The graphics control.
        /// </value>
        public GraphicsControl GraphicsControl { get; private set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public FrameIndices Indices { get; set; }

        /// <summary>
        /// Gets or sets the color table using.
        /// </summary>
        /// <value>
        /// The color table using.
        /// </value>
        public ColorTable LocalColorTable { get; set; }

        /// <summary>
        /// Reads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="globalColorTable">The global color table.</param>
        /// <param name="graphicsControl">The graphics control.</param>
        /// <param name="screenDescriptor">The screen descriptor.</param>
        /// <param name="frames">The frames.</param>
        /// <returns>
        /// The resulting frame.
        /// </returns>
        public static Frame Read(Stream stream, ColorTable globalColorTable, GraphicsControl graphicsControl, LogicalScreenDescriptor screenDescriptor, List<Frame> frames)
        {
            var TempDescriptor = ImageDescriptor.Read(stream);
            ColorTable LocalColorTable = TempDescriptor.LocalColorTableExists ?
                                            ColorTable.Read(stream, TempDescriptor.LocalColorTableSize) :
                                            globalColorTable;
            var TempIndices = FrameIndices.Read(stream, TempDescriptor);

            byte[] Data = ReadFrameColors(TempIndices, LocalColorTable, graphicsControl, TempDescriptor, screenDescriptor, frames);

            Skip(stream, 0);

            return new Frame(graphicsControl, TempDescriptor, LocalColorTable, TempIndices, Data);
        }

        /// <summary>
        /// Writes to the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>
        /// True if it writes successfully, false otherwise
        /// </returns>
        public override bool Write(EndianBinaryWriter writer)
        {
            GraphicsControl.Write(writer);
            Descriptor.Write(writer);
            LocalColorTable.Write(writer);
            Indices.Write(writer);
            return true;
        }

        private static byte[] ReadFrameColors(FrameIndices indices, ColorTable colorTable, GraphicsControl graphicsControl, ImageDescriptor descriptor, LogicalScreenDescriptor screenDescriptor, List<Frame> frames)
        {
            int imageWidth = screenDescriptor.Width;
            int imageHeight = screenDescriptor.Height;
            byte[] CurrentFrame = null;
            byte[] LastFrame = null;
            if (frames.Count > 0 &&
                graphicsControl != null &&
                graphicsControl.DisposalMethod == DisposalMethod.RestoreToPrevious)
            {
                CurrentFrame = new byte[imageWidth * imageHeight * 4];
                Array.Copy(frames[frames.Count - 1].Data, CurrentFrame, CurrentFrame.Length);
                LastFrame = new byte[imageWidth * imageHeight * 4];
                Array.Copy(CurrentFrame, LastFrame, LastFrame.Length);
            }
            else
            {
                CurrentFrame = new byte[imageWidth * imageHeight * 4];
            }

            int offset, i = 0;
            int interlacePass = 0;
            int interlaceIncrement = 8;
            int interlaceY = 0;

            for (int y = descriptor.Top; y < descriptor.Top + descriptor.Height; y++)
            {
                int writeY;
                if (descriptor.Interlace)
                {
                    if (interlaceY >= descriptor.Height)
                    {
                        interlacePass++;
                        switch (interlacePass)
                        {
                            case 1:
                                interlaceY = 4;
                                break;

                            case 2:
                                interlaceY = 2;
                                interlaceIncrement = 4;
                                break;

                            case 3:
                                interlaceY = 1;
                                interlaceIncrement = 2;
                                break;
                        }
                    }

                    writeY = interlaceY + descriptor.Top;

                    interlaceY += interlaceIncrement;
                }
                else
                {
                    writeY = y;
                }

                for (int x = descriptor.Left; x < descriptor.Left + descriptor.Width; x++)
                {
                    offset = ((writeY * imageWidth) + x) * 4;
                    int index = indices.Indices[i];

                    if (graphicsControl == null ||
                        graphicsControl.TransparencyFlag == false ||
                        graphicsControl.TransparencyIndex != index)
                    {
                        int indexOffset = index * 3;
                        CurrentFrame[offset + 0] = colorTable.Data[indexOffset];
                        CurrentFrame[offset + 1] = colorTable.Data[indexOffset + 1];
                        CurrentFrame[offset + 2] = colorTable.Data[indexOffset + 2];
                        CurrentFrame[offset + 3] = 255;
                    }

                    i++;
                }
            }
            return CurrentFrame;
        }
    }
}