﻿/*
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
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Resampling.BaseClasses
{
    /// <summary>
    /// Affine transformation base class
    /// </summary>
    /// <seealso cref="IFilter"/>
    public abstract class AffineBaseClass : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AffineBaseClass"/> class.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        /// <param name="filter">The filter to use (defaults to nearest neighbor).</param>
        protected AffineBaseClass(int width = -1, int height = -1, ResamplingFiltersAvailable filter = ResamplingFiltersAvailable.NearestNeighbor)
        {
            Width = width;
            Height = height;
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
        protected int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        protected int Width { get; set; }

        /// <summary>
        /// Gets or sets the resampling filter dictionary.
        /// </summary>
        /// <value>The resampling filter.</value>
        private Dictionary<ResamplingFiltersAvailable, IResamplingFilter> ResamplingFilter { get; set; }

        /// <summary>
        /// Gets the transformation matrix.
        /// </summary>
        /// <value>The transformation matrix.</value>
        private Matrix3x2 TransformationMatrix { get; set; }

        /// <summary>
        /// Gets or sets the x radius for the sampling filter.
        /// </summary>
        /// <value>The x radius.</value>
        private double XRadius { get; set; }

        /// <summary>
        /// Gets or sets the y radius for the sampling filter.
        /// </summary>
        /// <value>The y radius.</value>
        private double YRadius { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var Copy = new Color[image.Pixels.Length];
            Array.Copy(image.Pixels, Copy, Copy.Length);
            TransformationMatrix = GetMatrix(image, targetLocation);
            double TempWidth = Width < 0 ? image.Width : Width;
            double TempHeight = Height < 0 ? image.Width : Height;
            double XScale = TempWidth / image.Width;
            double YScale = TempHeight / image.Height;
            YRadius = YScale < 1f ? (Filter.FilterRadius / YScale) : Filter.FilterRadius;
            XRadius = XScale < 1f ? (Filter.FilterRadius / XScale) : Filter.FilterRadius;

            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (Color* OutputPointer = &image.Pixels[(y * image.Width) + targetLocation.Left])
                {
                    Color* OutputPointer2 = OutputPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
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
                        for (int i = Top; i <= Bottom; ++i)
                        {
                            if (i < 0 || i >= image.Height)
                                continue;
                            fixed (Color* PixelPointer = &Copy[i * image.Width])
                            {
                                for (int j = Left; j <= Right; ++j)
                                {
                                    if (j < 0 || j >= image.Width)
                                        continue;
                                    Color* PixelPointer2 = PixelPointer + j;
                                    var TempYWeight = YScale < 1f ?
                                        Filter.GetValue((rotatedY - i) * YScale) :
                                        Filter.GetValue(rotatedY - i);
                                    var TempXWeight = XScale < 1f ?
                                        Filter.GetValue((rotatedX - j) * XScale) :
                                        Filter.GetValue(rotatedX - j);
                                    var TempWeight = TempYWeight * TempXWeight;

                                    if (YRadius == 0 && XRadius == 0)
                                        TempWeight = 1;

                                    if (TempWeight == 0)
                                        continue;
                                    Values.X = Values.X + ((*PixelPointer2).Red * (float)TempWeight);
                                    Values.Y = Values.Y + ((*PixelPointer2).Green * (float)TempWeight);
                                    Values.Z = Values.Z + ((*PixelPointer2).Blue * (float)TempWeight);
                                    Values.W = Values.W + ((*PixelPointer2).Alpha * (float)TempWeight);
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
            return image;
        }

        /// <summary>
        /// Gets the matrix.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The matrix used for the transformation</returns>
        protected abstract Matrix3x2 GetMatrix(Image image, Rectangle targetLocation);
    }
}