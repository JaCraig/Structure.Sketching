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
using Structure.Sketching.ExtensionMethods;
using Structure.Sketching.Filters.Drawing;
using Structure.Sketching.Filters.Interfaces;
using System;
using System.Numerics;

namespace Structure.Sketching.Filters.Effects
{
    /// <summary>
    /// Pointillism filter
    /// </summary>
    /// <seealso cref="IFilter"/>
    public class Pointillism : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pointillism"/> class.
        /// </summary>
        /// <param name="pointSize">Size of the points.</param>
        public Pointillism(int pointSize)
        {
            PointSize = pointSize;
        }

        /// <summary>
        /// Gets or sets the size of the points.
        /// </summary>
        /// <value>The size of the points.</value>
        public int PointSize { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Numerics.Rectangle targetLocation = default(Numerics.Rectangle))
        {
            targetLocation = targetLocation == default(Numerics.Rectangle) ? new Numerics.Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            var PointSize2 = PointSize * 2;
            var Copy = new Color[image.Pixels.Length];
            Array.Copy(image.Pixels, Copy, Copy.Length);
            var EllipseDrawer = new Ellipse(Color.AliceBlue, true, PointSize, PointSize, new Vector2(0, 0));

            for (int y = targetLocation.Bottom; y < targetLocation.Top; y += PointSize2)
            {
                var MinY = (y - PointSize).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                var MaxY = (y + PointSize).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                fixed (Color* TargetPointer = &Copy[(y * image.Width) + targetLocation.Left])
                {
                    Color* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; x += PointSize2)
                    {
                        uint RValue = 0;
                        uint GValue = 0;
                        uint BValue = 0;
                        var MinX = (x - PointSize).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        var MaxX = (x + PointSize).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        int NumberPixels = 0;
                        for (int x2 = MinX; x2 < MaxX; ++x2)
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                var Offset = ((y * image.Width) + x);
                                RValue += Copy[Offset].Red;
                                GValue += Copy[Offset].Green;
                                BValue += Copy[Offset].Blue;
                                ++NumberPixels;
                            }
                        }
                        RValue /= (uint)NumberPixels;
                        GValue /= (uint)NumberPixels;
                        BValue /= (uint)NumberPixels;
                        EllipseDrawer.Center = new Vector2(x, y);
                        EllipseDrawer.Color = new Color((byte)RValue, (byte)GValue, (byte)BValue);
                        EllipseDrawer.Apply(image, targetLocation);
                    }
                }
            }
            for (int y = targetLocation.Bottom + PointSize; y < targetLocation.Top; y += PointSize2)
            {
                var MinY = (y - PointSize).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                var MaxY = (y + PointSize).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                fixed (Color* TargetPointer = &Copy[(y * image.Width) + targetLocation.Left])
                {
                    Color* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left + PointSize; x < targetLocation.Right; x += PointSize2)
                    {
                        uint RValue = 0;
                        uint GValue = 0;
                        uint BValue = 0;
                        var MinX = (x - PointSize).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        var MaxX = (x + PointSize).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        int NumberPixels = 0;
                        for (int x2 = MinX; x2 < MaxX; ++x2)
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                var Offset = ((y * image.Width) + x);
                                RValue += Copy[Offset].Red;
                                GValue += Copy[Offset].Green;
                                BValue += Copy[Offset].Blue;
                                ++NumberPixels;
                            }
                        }
                        RValue /= (uint)NumberPixels;
                        GValue /= (uint)NumberPixels;
                        BValue /= (uint)NumberPixels;
                        EllipseDrawer.Center = new Vector2(x, y);
                        EllipseDrawer.Color = new Color((byte)RValue, (byte)GValue, (byte)BValue);
                        EllipseDrawer.Apply(image, targetLocation);
                    }
                }
            }
            for (int y = targetLocation.Bottom; y < targetLocation.Top; y += PointSize2)
            {
                var TempY = y + new Random(y).Next(-PointSize, PointSize);
                var MinY = (TempY - PointSize).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                var MaxY = (TempY + PointSize).Clamp(targetLocation.Bottom, targetLocation.Top - 1);
                fixed (Color* TargetPointer = &Copy[(y * image.Width) + targetLocation.Left])
                {
                    Color* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left + PointSize; x < targetLocation.Right; x += PointSize2)
                    {
                        uint RValue = 0;
                        uint GValue = 0;
                        uint BValue = 0;
                        var TempX = x + new Random(x).Next(-PointSize, PointSize);
                        var MinX = (TempX - PointSize).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        var MaxX = (TempX + PointSize).Clamp(targetLocation.Left, targetLocation.Right - 1);
                        int NumberPixels = 0;
                        for (int x2 = MinX; x2 < MaxX; ++x2)
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                var Offset = ((y * image.Width) + x);
                                RValue += Copy[Offset].Red;
                                GValue += Copy[Offset].Green;
                                BValue += Copy[Offset].Blue;
                                ++NumberPixels;
                            }
                        }
                        RValue /= (uint)NumberPixels;
                        GValue /= (uint)NumberPixels;
                        BValue /= (uint)NumberPixels;
                        EllipseDrawer.Center = new Vector2(TempX, TempY);
                        EllipseDrawer.Color = new Color((byte)RValue, (byte)GValue, (byte)BValue);
                        EllipseDrawer.Apply(image, targetLocation);
                    }
                }
            }
            return image;
        }
    }
}