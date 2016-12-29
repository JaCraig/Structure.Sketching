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
            double XScale = (double)Width / image.Width;
            double YScale = (double)Height / image.Height;

            var Output = Sample(image, XScale, YScale, image.Width, image.Height);
            image.ReCreate(Width, Height, Output);
            return image;
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The transformation matrix</returns>
        protected Matrix3x2 GetMatrix(Image image, Rectangle targetLocation)
        {
            float XScale = (float)image.Width / Width;
            float YScale = (float)image.Height / Height;
            return Matrix3x2.CreateScale(XScale, YScale);
        }

        private unsafe Color[] Sample(Image image, double xScale, double yScale, int oldWidth, int oldHeight)
        {
            Filter.Precompute(image.Width, image.Height, Width, Height);
            var targetLocation = new Rectangle(0, 0, image.Width, image.Height);
            var Output = new Color[Width * Height];
            var TransformationMatrix = GetMatrix(image, targetLocation);
            double TempWidth = Width < 0 ? image.Width : Width;
            double TempHeight = Height < 0 ? image.Width : Height;
            double XScale = TempWidth / image.Width;
            double YScale = TempHeight / image.Height;
            var YRadius = YScale < 1f ? (Filter.FilterRadius / YScale) : Filter.FilterRadius;
            var XRadius = XScale < 1f ? (Filter.FilterRadius / XScale) : Filter.FilterRadius;

            Parallel.For(0, Height, y =>
            {
                fixed (Color* OutputPointer = &Output[y * Width])
                {
                    Color* OutputPointer2 = OutputPointer;
                    for (int x = 0; x < Width; ++x)
                    {
                        var Values = new Vector4(0, 0, 0, 0);
                        float Weight = 0;

                        var rotated = Vector2.Transform(new Vector2(x, y), TransformationMatrix);
                        var rotatedY = (int)rotated.Y;
                        var rotatedX = (int)rotated.X;
                        var Left = (int)(rotatedX - XRadius);
                        var Right = (int)(rotatedX + XRadius);
                        var Top = (int)(rotatedY - YRadius);
                        var Bottom = (int)(rotatedY + YRadius);
                        if (Top < 0)
                            Top = 0;
                        if (Bottom >= image.Height)
                            Bottom = image.Height - 1;
                        if (Left < 0)
                            Left = 0;
                        if (Right >= image.Width)
                            Right = image.Width - 1;
                        for (int i = Top, YCount = 0; i <= Bottom; ++i, ++YCount)
                        {
                            fixed (Color* PixelPointer = &image.Pixels[i * image.Width])
                            {
                                Color* PixelPointer2 = PixelPointer + Left;
                                for (int j = Left, XCount = 0; j <= Right; ++j, ++XCount)
                                {
                                    var TempYWeight = Filter.YWeights[rotatedY].Values[YCount];
                                    var TempXWeight = Filter.XWeights[rotatedX].Values[XCount];
                                    var TempWeight = TempYWeight * TempXWeight;

                                    if (YRadius == 0 && XRadius == 0)
                                        TempWeight = 1;

                                    if (TempWeight == 0)
                                    {
                                        ++PixelPointer2;
                                        continue;
                                    }
                                    Values.X = Values.X + ((*PixelPointer2).Red * (float)TempWeight);
                                    Values.Y = Values.Y + ((*PixelPointer2).Green * (float)TempWeight);
                                    Values.Z = Values.Z + ((*PixelPointer2).Blue * (float)TempWeight);
                                    Values.W = Values.W + ((*PixelPointer2).Alpha * (float)TempWeight);
                                    ++PixelPointer2;
                                    Weight += (float)TempWeight;
                                }
                            }
                        }
                        if (Weight == 0)
                            Weight = 1;
                        if (Weight > 0)
                        {
                            Values = Vector4.Clamp(Values, Vector4.Zero, new Vector4(255, 255, 255, 255));
                            (*OutputPointer2).Red = (byte)Values.X;
                            (*OutputPointer2).Green = (byte)Values.Y;
                            (*OutputPointer2).Blue = (byte)Values.Z;
                            (*OutputPointer2).Alpha = (byte)Values.W;
                            ++OutputPointer2;
                        }
                        else
                            ++OutputPointer2;
                    }
                }
            });
            return Output;
        }
    }
}