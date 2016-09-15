﻿//// <copyright file="GifEncoderCore.cs" company="James Jackson-South">
//// Copyright (c) James Jackson-South and contributors.
//// Licensed under the Apache License, Version 2.0.
//// </copyright>

//namespace ImageProcessorCore.Formats
//{
//    using Structure.Sketching;
//    using Structure.Sketching.Quantizers.Interfaces;
//    using Structure.Sketching.Quantizers.Octree;
//    using System;
//    using System.IO;
//    using System.Linq;
//    using System.Threading.Tasks;

//    /// <summary>
//    /// Performs the gif encoding operation.
//    /// </summary>
//    internal sealed class GifEncoderCore
//    {
//        /// <summary>
//        /// Gets or sets the quality of output for images.
//        /// </summary>
//        /// <remarks>For gifs the value ranges from 1 to 256.</remarks>
//        public int Quality { get; set; } = 256;

//        /// <summary>
//        /// The quantizer for reducing the color count.
//        /// </summary>
//        public IQuantizer Quantizer { get; set; }

//        /// <summary>
//        /// Gets or sets the transparency threshold.
//        /// </summary>
//        public byte Threshold { get; set; } = 128;

//        /// <summary>
//        /// The number of bits requires to store the image palette.
//        /// </summary>
//        private int bitDepth;

//        /// <summary>
//        /// Encodes the image to the specified stream from the <see cref="ImageBase"/>.
//        /// </summary>
//        /// <param name="imageBase">The <see cref="ImageBase"/> to encode from.</param>
//        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
//        public void Encode(Image imageBase, Stream stream)
//        {
//            Image image = (Image)imageBase;

//            if (this.Quantizer == null)
//            {
//                this.Quantizer = new OctreeQuantizer { Threshold = this.Threshold };
//            }

//            using (EndianBinaryWriter writer = new EndianBinaryWriter(EndianBitConverter.Little, stream))
//            {
//                // Ensure that quality can be set but has a fallback.
//                int quality = this.Quality > 0 ? this.Quality : imageBase.Quality;
//                this.Quality = quality > 0 ? quality.Clamp(1, 256) : 256;

//                // Get the number of bits.
//                this.bitDepth = ImageMaths.GetBitsNeededForColorDepth(this.Quality);

//                // Quantize the image returning a palette.
//                QuantizedImage quantized = this.Quantizer.Quantize(image, this.Quality);

//                // Write the header.
//                this.WriteHeader(writer);

//                // Write the LSD. We'll use local color tables for now.
//                this.WriteLogicalScreenDescriptor(image, writer, quantized.TransparentIndex);

//                // Write the first frame.
//                this.WriteGraphicalControlExtension(imageBase, writer, quantized.TransparentIndex);
//                this.WriteImageDescriptor(image, writer);
//                this.WriteColorTable(quantized, writer);
//                this.WriteImageData(quantized, writer);

//                // Write additional frames.
//                if (image.Frames.Any())
//                {
//                    this.WriteApplicationExtension(writer, image.RepeatCount, image.Frames.Count);
//                    foreach (ImageFrame frame in image.Frames)
//                    {
//                        QuantizedImage quantizedFrame = this.Quantizer.Quantize(frame, this.Quality);
//                        this.WriteGraphicalControlExtension(frame, writer, quantizedFrame.TransparentIndex);
//                        this.WriteImageDescriptor(frame, writer);
//                        this.WriteColorTable(quantizedFrame, writer);
//                        this.WriteImageData(quantizedFrame, writer);
//                    }
//                }

//                // TODO: Write Comments extension etc
//                writer.Write(GifConstants.EndIntroducer);
//            }
//        }

//        /// <summary>
//        /// Writes the application exstension to the stream.
//        /// </summary>
//        /// <param name="writer">The writer to write to the stream with.</param>
//        /// <param name="repeatCount">The animated image repeat count.</param>
//        /// <param name="frames">Th number of image frames.</param>
//        private void WriteApplicationExtension(EndianBinaryWriter writer, ushort repeatCount, int frames)
//        {
//            // Application Extension Header
//            if (repeatCount != 1 && frames > 0)
//            {
//                byte[] ext =
//                {
//                    GifConstants.ExtensionIntroducer,
//                    GifConstants.ApplicationExtensionLabel,
//                    GifConstants.ApplicationBlockSize
//                };

//                writer.Write(ext);

//                writer.Write(GifConstants.ApplicationIdentification.ToCharArray()); // NETSCAPE2.0
//                writer.Write((byte)3); // Application block length
//                writer.Write((byte)1); // Data sub-block index (always 1)

//                // 0 means loop indefinitely. Count is set as play n + 1 times.
//                repeatCount = (ushort)(Math.Max((ushort)0, repeatCount) - 1);

//                writer.Write(repeatCount); // Repeat count for images.

//                writer.Write(GifConstants.Terminator); // Terminator
//            }
//        }

//        /// <summary>
//        /// Writes the color table to the stream.
//        /// </summary>
//        /// <param name="image">The <see cref="ImageBase"/> to encode.</param>
//        /// <param name="writer">The writer to write to the stream with.</param>
//        private void WriteColorTable(QuantizedImage image, EndianBinaryWriter writer)
//        {
//            // Grab the palette and write it to the stream.
//            Bgra32[] palette = image.Palette;
//            int pixelCount = palette.Length;

//            // Get max colors for bit depth.
//            int colorTableLength = (int)Math.Pow(2, this.bitDepth) * 3;
//            byte[] colorTable = new byte[colorTableLength];

//            Parallel.For(0, pixelCount,
//                i =>
//                {
//                    int offset = i * 3;
//                    Bgra32 color = palette[i];

//                    colorTable[offset] = color.R;
//                    colorTable[offset + 1] = color.G;
//                    colorTable[offset + 2] = color.B;
//                });

//            writer.Write(colorTable, 0, colorTableLength);
//        }

//        /// <summary>
//        /// Writes the graphics control extension to the stream.
//        /// </summary>
//        /// <param name="image">The <see cref="ImageBase"/> to encode.</param>
//        /// <param name="writer">The stream to write to.</param>
//        /// <param name="transparencyIndex">The index of the color in the color palette to make transparent.</param>
//        private void WriteGraphicalControlExtension(ImageBase image, EndianBinaryWriter writer, int transparencyIndex)
//        {
//            // TODO: Check transparency logic.
//            bool hasTransparent = transparencyIndex > -1;
//            DisposalMethod disposalMethod = hasTransparent
//                ? DisposalMethod.RestoreToBackground
//                : DisposalMethod.Unspecified;

//            GifGraphicsControlExtension extension = new GifGraphicsControlExtension()
//            {
//                DisposalMethod = disposalMethod,
//                TransparencyFlag = hasTransparent,
//                TransparencyIndex = transparencyIndex,
//                DelayTime = image.FrameDelay
//            };

//            // Reduce the number of writes.
//            byte[] intro = {
//                GifConstants.ExtensionIntroducer,
//                GifConstants.GraphicControlLabel,
//                4 // Size
//            };

//            writer.Write(intro);

//            PackedField field = new PackedField();
//            field.SetBits(3, 3, (int)extension.DisposalMethod); // 1-3 : Reserved, 4-6 : Disposal

//            // TODO: Allow this as an option.
//            field.SetBit(6, false); // 7 : User input - 0 = none
//            field.SetBit(7, extension.TransparencyFlag); // 8: Has transparent.

//            writer.Write(field.Byte);
//            writer.Write((ushort)extension.DelayTime);
//            writer.Write((byte)(extension.TransparencyIndex == -1 ? 255 : extension.TransparencyIndex));
//            writer.Write(GifConstants.Terminator);
//        }

//        /// <summary>
//        /// Writes the file header signature and version to the stream.
//        /// </summary>
//        /// <param name="writer">The writer to write to the stream with.</param>
//        private void WriteHeader(EndianBinaryWriter writer)
//        {
//            writer.Write((GifConstants.FileType + GifConstants.FileVersion).ToCharArray());
//        }

//        /// <summary>
//        /// Writes the image pixel data to the stream.
//        /// </summary>
//        /// <param name="image">The <see cref="QuantizedImage"/> containing indexed pixels.</param>
//        /// <param name="writer">The stream to write to.</param>
//        private void WriteImageData(QuantizedImage image, EndianBinaryWriter writer)
//        {
//            byte[] indexedPixels = image.Pixels;

//            LzwEncoder encoder = new LzwEncoder(indexedPixels, (byte)this.bitDepth);
//            encoder.Encode(writer.BaseStream);
//        }

//        /// <summary>
//        /// Writes the image descriptor to the stream.
//        /// </summary>
//        /// <param name="image">The <see cref="ImageBase"/> to be encoded.</param>
//        /// <param name="writer">The stream to write to.</param>
//        private void WriteImageDescriptor(ImageBase image, EndianBinaryWriter writer)
//        {
//            writer.Write(GifConstants.ImageDescriptorLabel); // 2c
//            // TODO: Can we capture this?
//            writer.Write((ushort)0); // Left position
//            writer.Write((ushort)0); // Top position
//            writer.Write((ushort)image.Width);
//            writer.Write((ushort)image.Height);

//            PackedField field = new PackedField();
//            field.SetBit(0, true); // 1: Local color table flag = 1 (LCT used)
//            field.SetBit(1, false); // 2: Interlace flag 0
//            field.SetBit(2, false); // 3: Sort flag 0
//            field.SetBits(5, 3, this.bitDepth - 1); // 4-5: Reserved, 6-8 : LCT size. 2^(N+1)

//            writer.Write(field.Byte);
//        }

//        /// <summary>
//        /// Writes the logical screen descriptor to the stream.
//        /// </summary>
//        /// <param name="image">The image to encode.</param>
//        /// <param name="writer">The writer to write to the stream with.</param>
//        /// <param name="tranparencyIndex">The transparency index to set the default backgound index to.</param>
//        private void WriteLogicalScreenDescriptor(Image image, EndianBinaryWriter writer, int tranparencyIndex)
//        {
//            GifLogicalScreenDescriptor descriptor = new GifLogicalScreenDescriptor
//            {
//                Width = (short)image.Width,
//                Height = (short)image.Height,
//                GlobalColorTableFlag = false, // Always false for now.
//                GlobalColorTableSize = this.bitDepth - 1,
//                BackgroundColorIndex = (byte)(tranparencyIndex > -1 ? tranparencyIndex : 255)
//            };

//            writer.Write((ushort)descriptor.Width);
//            writer.Write((ushort)descriptor.Height);

//            PackedField field = new PackedField();
//            field.SetBit(0, descriptor.GlobalColorTableFlag); // 1   : Global color table flag = 1 || 0 (GCT used/ not used)
//            field.SetBits(1, 3, descriptor.GlobalColorTableSize); // 2-4 : color resolution
//            field.SetBit(4, false); // 5   : GCT sort flag = 0
//            field.SetBits(5, 3, descriptor.GlobalColorTableSize); // 6-8 : GCT size. 2^(N+1)

//            // Reduce the number of writes
//            byte[] arr = {
//                field.Byte,
//                descriptor.BackgroundColorIndex, // Background Color Index
//                descriptor.PixelAspectRatio // Pixel aspect ratio. Assume 1:1
//            };

//            writer.Write(arr);
//        }
//    }
//}