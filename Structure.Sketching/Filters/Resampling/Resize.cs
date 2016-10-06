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

using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Filters.Resampling.ResamplingFilters;
using Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces;
using Structure.Sketching.Numerics;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Resampling
{
    /// <summary>
    /// Resizes an image
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Resize : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resize"/> class.
        /// </summary>
        /// <param name="height">The new height.</param>
        /// <param name="width">The new width.</param>
        /// <param name="filter">The filter.</param>
        public Resize(int width, int height, ResamplingFiltersAvailable filter)
        {
            Height = height;
            Width = width;
            ResamplingFilter = new Dictionary<ResamplingFiltersAvailable, IResamplingFilter>
            {
                { ResamplingFiltersAvailable.Bell, new BellFilter() },
                { ResamplingFiltersAvailable.CatmullRom, new CatmullRomFilter() },
                { ResamplingFiltersAvailable.Cosine, new CosineFilter() },
                { ResamplingFiltersAvailable.CubicBSpline, new CubicBSplineFilter() },
                { ResamplingFiltersAvailable.CubicConvolution, new CubicConvolutionFilter() },
                { ResamplingFiltersAvailable.Hermite, new HermiteFilter() },
                { ResamplingFiltersAvailable.Lanczos3, new Lanczos3Filter() },
                { ResamplingFiltersAvailable.Lanczos8, new Lanczos8Filter() },
                { ResamplingFiltersAvailable.Mitchell, new MitchellFilter() },
                { ResamplingFiltersAvailable.Quadratic, new QuadraticFilter() },
                { ResamplingFiltersAvailable.QuadraticBSpline, new QuadraticBSplineFilter() },
                { ResamplingFiltersAvailable.Triangle, new TriangleFilter() },
                { ResamplingFiltersAvailable.Bilinear, new BilinearFilter() },
                { ResamplingFiltersAvailable.NearestNeighbor, new NearestNeighborFilter() },
                { ResamplingFiltersAvailable.Robidoux, new RobidouxFilter() },
                { ResamplingFiltersAvailable.RobidouxSharp, new RobidouxSharpFilter() },
                { ResamplingFiltersAvailable.RobidouxSoft, new RobidouxSoftFilter() },
                { ResamplingFiltersAvailable.Bicubic, new BicubicFilter() }
            };
            Filter = ResamplingFilter[filter];
            FilterKey = filter;
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public IResamplingFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the filter key.
        /// </summary>
        /// <value>The filter key.</value>
        private ResamplingFiltersAvailable FilterKey { get; set; }

        /// <summary>
        /// Gets or sets the resampling filter dictionary.
        /// </summary>
        /// <value>The resampling filter.</value>
        private Dictionary<ResamplingFiltersAvailable, IResamplingFilter> ResamplingFilter { get; set; }

        /// <summary>
        /// Applies the resizing filter to the specified image.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            var Output = new byte[Width * Height * 4];

            double XScale = (double)Width / image.Width;
            double YScale = (double)Height / image.Height;

            Output = Sample(image, XScale, YScale, image.Width, image.Height);
            image.ReCreate(Width, Height, Output);
            return image;
        }

        private unsafe byte[] Sample(Image image, double xScale, double yScale, int oldWidth, int oldHeight)
        {
            var Output = new byte[Width * Height * 4];
            double YRadius = yScale < 1f ? (Filter.FilterRadius / yScale) : Filter.FilterRadius;
            double XRadius = xScale < 1f ? (Filter.FilterRadius / xScale) : Filter.FilterRadius;

            Parallel.For(0, Height, y =>
            {
                fixed (byte* OutputPointer = &Output[y * Width * 4])
                {
                    byte* OutputPointer2 = OutputPointer;
                    var YCenter = (y + 0.5) / yScale;
                    var Top = (int)(YCenter - YRadius);
                    var Bottom = (int)(YCenter + YRadius);

                    for (int x = 0; x < Width; ++x)
                    {
                        var Values = new Vector4(0, 0, 0, 0);
                        float Weight = 0;

                        var XCenter = (x + 0.5) / xScale;
                        var Left = (int)(XCenter - XRadius);
                        var Right = (int)(XCenter + XRadius);

                        for (int i = Top; i <= Bottom; ++i)
                        {
                            if (i < 0 || i >= oldHeight)
                                continue;
                            fixed (byte* PixelPointer = &image.Pixels[i * oldWidth * 4])
                            {
                                for (int j = Left; j <= Right; ++j)
                                {
                                    if (j < 0 || j >= oldWidth)
                                        continue;
                                    byte* PixelPointer2 = PixelPointer + (j * 4);
                                    var TempYWeight = yScale < 1f ?
                                        Filter.GetValue((YCenter - i - 0.5) * yScale) :
                                        Filter.GetValue(YCenter - i - 0.5);
                                    var TempXWeight = xScale < 1f ?
                                        Filter.GetValue((XCenter - j - 0.5) * xScale) :
                                        Filter.GetValue(XCenter - j - 0.5);
                                    var TempWeight = TempYWeight * TempXWeight;

                                    if (YRadius == 0 && XRadius == 0)
                                        TempWeight = 1;

                                    if (TempWeight == 0)
                                        continue;
                                    Values.X = Values.X + (*PixelPointer2 * (float)TempWeight);
                                    ++PixelPointer2;
                                    Values.Y = Values.Y + (*PixelPointer2 * (float)TempWeight);
                                    ++PixelPointer2;
                                    Values.Z = Values.Z + (*PixelPointer2 * (float)TempWeight);
                                    ++PixelPointer2;
                                    Values.W = Values.W + (*PixelPointer2 * (float)TempWeight);
                                    Weight += (float)TempWeight;
                                }
                            }
                        }
                        if (Weight == 0)
                            Weight = 1;
                        if (Weight > 0)
                        {
                            Values /= Weight;
                            Values = Vector4.Clamp(Values, Vector4.Zero, new Vector4(255, 255, 255, 255));
                            *OutputPointer2 = (byte)Values.X;
                            ++OutputPointer2;
                            *OutputPointer2 = (byte)Values.Y;
                            ++OutputPointer2;
                            *OutputPointer2 = (byte)Values.Z;
                            ++OutputPointer2;
                            *OutputPointer2 = (byte)Values.W;
                            ++OutputPointer2;
                        }
                        else
                            OutputPointer2 += 4;
                    }
                }
            });
            return Output;
        }
    }
}