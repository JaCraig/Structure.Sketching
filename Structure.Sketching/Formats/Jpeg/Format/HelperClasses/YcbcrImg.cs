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

using Structure.Sketching.Colors;
using Structure.Sketching.Colors.ColorSpaces;
using Structure.Sketching.Formats.Jpeg.Format.HelperClasses.Enums;
using Structure.Sketching.Formats.Jpeg.Format.Segments;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structure.Sketching.Formats.Jpeg.Format.HelperClasses
{
    /// <summary>
    /// YCbCr image
    /// </summary>
    public class YcbcrImg
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YcbcrImg"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="ratio">The ratio.</param>
        public YcbcrImg(int width, int height, YCbCrSubsampleRatio ratio)
        {
            int cw, ch;
            yCbCrSize(width, height, ratio, out cw, out ch);
            YPixels = new byte[width * height];
            CBPixels = new byte[cw * ch];
            CRPixels = new byte[cw * ch];
            Ratio = ratio;
            YStride = width;
            CStride = cw;
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
        }

        private YcbcrImg()
        {
        }

        /// <summary>
        /// Gets or sets the cb pixels.
        /// </summary>
        /// <value>The cb pixels.</value>
        public byte[] CBPixels { get; set; }

        /// <summary>
        /// Gets or sets the c offset.
        /// </summary>
        /// <value>The c offset.</value>
        public int COffset { get; set; }

        /// <summary>
        /// Gets or sets the cr pixels.
        /// </summary>
        /// <value>The cr pixels.</value>
        public byte[] CRPixels { get; set; }

        /// <summary>
        /// Gets or sets the c stride.
        /// </summary>
        /// <value>The c stride.</value>
        public int CStride { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// The ratio
        /// </summary>
        /// <value>The ratio.</value>
        public YCbCrSubsampleRatio Ratio { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// X value
        /// </summary>
        /// <value>The x value.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the y offset.
        /// </summary>
        /// <value>The y offset.</value>
        public int YOffset { get; set; }

        /// <summary>
        /// Gets or sets the y pixels.
        /// </summary>
        /// <value>The y pixels.</value>
        public byte[] YPixels { get; set; }

        /// <summary>
        /// Gets or sets the y stride.
        /// </summary>
        /// <value>The y stride.</value>
        public int YStride { get; set; }

        private const byte AdobeTransformUnknown = 0;

        /// <summary>
        /// Converts this to an image.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="image">The image.</param>
        /// <param name="segments">The segments.</param>
        /// <returns>Converts to an image</returns>
        public Image Convert(int width, int height, Image image, IEnumerable<SegmentBase> segments)
        {
            if (this.IsRGB(segments))
            {
                this.ConvertDirectToRGB(width, height, image, segments);
            }
            else
            {
                this.ConvertToRGB(width, height, image, segments);
            }
            return image;
        }

        /// <summary>
        /// Determines whether the specified segments are RGB.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool IsRGB(IEnumerable<SegmentBase> segments)
        {
            var Application0Segment = segments.OfType<Application0>().FirstOrDefault() ?? new Application0(null);
            var Application14Segment = segments.OfType<Application14>().FirstOrDefault() ?? new Application14(null);
            var StartFrameSegment = segments.OfType<StartOfFrame>().FirstOrDefault();
            if (Application0Segment.IsJFIF)
            {
                return false;
            }

            if (Application14Segment.IsAdobeTransformValid && Application14Segment.AdobeTransform == AdobeTransformUnknown)
            {
                return true;
            }

            return StartFrameSegment.Components[0].ComponentIdentifier == 'R'
                && StartFrameSegment.Components[1].ComponentIdentifier == 'G'
                && StartFrameSegment.Components[2].ComponentIdentifier == 'B';
        }

        /// <summary>
        /// Rows the c offset.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>The row offset</returns>
        public int RowCOffset(int y)
        {
            switch (Ratio)
            {
                case YCbCrSubsampleRatio.YCbCrSubsampleRatio422:
                    return y * CStride;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio420:
                    return (y / 2) * CStride;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio440:
                    return (y / 2) * CStride;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio411:
                    return y * CStride;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio410:
                    return (y / 2) * CStride;
            }
            return y * CStride;
        }

        /// <summary>
        /// The row y offset.
        /// </summary>
        /// <param name="y">The y.</param>
        /// <returns>The y row offset</returns>
        public int RowYOffset(int y)
        {
            return y * YStride;
        }

        /// <summary>
        /// Gets a sub image starting at (X,Y) with width x height dimensions.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The resulting sub image</returns>
        public YcbcrImg SubImage(int x, int y, int width, int height)
        {
            return new YcbcrImg
            {
                Width = width,
                Height = height,
                YPixels = YPixels,
                CBPixels = CBPixels,
                CRPixels = CRPixels,
                Ratio = Ratio,
                YStride = YStride,
                CStride = CStride,
                YOffset = y * YStride + x,
                COffset = y * CStride + x
            };
        }

        /// <summary>
        /// gets the size of the cb cr.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="ratio">The ratio.</param>
        /// <param name="cw">The cw.</param>
        /// <param name="ch">The ch.</param>
        private static void yCbCrSize(int width, int height, YCbCrSubsampleRatio ratio, out int cw, out int ch)
        {
            switch (ratio)
            {
                case YCbCrSubsampleRatio.YCbCrSubsampleRatio422:
                    cw = (width + 1) / 2;
                    ch = height;
                    break;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio420:
                    cw = (width + 1) / 2;
                    ch = (height + 1) / 2;
                    break;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio440:
                    cw = width;
                    ch = (height + 1) / 2;
                    break;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio411:
                    cw = (width + 3) / 4;
                    ch = height;
                    break;

                case YCbCrSubsampleRatio.YCbCrSubsampleRatio410:
                    cw = (width + 3) / 4;
                    ch = (height + 1) / 2;
                    break;

                default:
                    cw = width;
                    ch = height;
                    break;
            }
        }

        private void ConvertDirectToRGB(int width, int height, Image image, IEnumerable<SegmentBase> segments)
        {
            var StartFrameSegment = segments.OfType<StartOfFrame>().FirstOrDefault();
            int cScale = StartFrameSegment.Components[0].HorizontalSamplingFactor / StartFrameSegment.Components[1].HorizontalSamplingFactor;
            var pixels = new Color[width * height];

            Parallel.For(
               0,
               height,
               y =>
               {
                   var yo = RowYOffset(y);
                   var co = RowCOffset(y);

                   for (int x = 0; x < width; x++)
                   {
                       byte red = YPixels[yo + x];
                       byte green = CBPixels[co + x / cScale];
                       byte blue = CRPixels[co + x / cScale];

                       pixels[(y * width) + x] = new Bgra(red, green, blue);
                   }
               });

            image.ReCreate(width, height, pixels);
        }

        private void ConvertToRGB(int width, int height, Image image, IEnumerable<SegmentBase> segments)
        {
            var StartFrameSegment = segments.OfType<StartOfFrame>().FirstOrDefault();
            int cScale = StartFrameSegment.Components[0].HorizontalSamplingFactor / StartFrameSegment.Components[1].HorizontalSamplingFactor;

            Color[] pixels = new Color[width * height];

            Parallel.For(
                0,
                height,
                y =>
                {
                    var yo = RowYOffset(y);
                    var co = RowCOffset(y);

                    for (int x = 0; x < width; x++)
                    {
                        byte yy = YPixels[yo + x];
                        byte cb = CBPixels[co + (x / cScale)];
                        byte cr = CRPixels[co + (x / cScale)];

                        pixels[(y * width) + x] = new YCbCr(yy, cb, cr);
                    }
                });

            image.ReCreate(width, height, pixels);
        }
    }
}