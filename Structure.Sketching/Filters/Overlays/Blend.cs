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
using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;

namespace Structure.Sketching.Filters.Overlays
{
    /// <summary>
    /// Blends two images together.
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class Blend : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Blend"/> class.
        /// </summary>
        /// <param name="image">The image to apply.</param>
        /// <param name="alpha">The alpha value for the image.</param>
        /// <param name="sourceLocation">The source location.</param>
        public Blend(Image image, float alpha, Rectangle sourceLocation = default(Rectangle))
        {
            Alpha = alpha;
            image = image.Copy();
            new Alpha(alpha).Apply(image);
            Image = image;
            SourceLocation = sourceLocation;
            SourceLocation = SourceLocation == default(Rectangle) ? new Rectangle(0, 0, Image.Width, Image.Height) : SourceLocation.Clamp(Image);
        }

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        public float Alpha { get; private set; }

        /// <summary>
        /// Gets or sets the image.;
        /// </summary>
        /// <value>The image.</value>
        public Image Image { get; private set; }

        /// <summary>
        /// Gets or sets the source location.
        /// </summary>
        /// <value>The source location.</value>
        public Rectangle SourceLocation { get; private set; }

        /// <summary>
        /// Applies the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            for (int y1 = targetLocation.Bottom, y2 = SourceLocation.Bottom; y1 < targetLocation.Top && y2 < SourceLocation.Top; ++y1, ++y2)
            {
                fixed (Color* TargetPointer = &image.Pixels[(y1 * image.Width) + targetLocation.Left])
                {
                    fixed (Color* SourcePointer = &Image.Pixels[(y2 * Image.Width) + SourceLocation.Left])
                    {
                        Color* TargetPointer2 = TargetPointer;
                        Color* SourcePointer2 = SourcePointer;
                        for (int x1 = targetLocation.Left, x2 = SourceLocation.Left; x1 < targetLocation.Right && x2 < SourceLocation.Right; ++x1, ++x2)
                        {
                            float TempAlpha = (*SourcePointer2).Alpha / 255f;
                            *TargetPointer2 = (*TargetPointer2 * (1f - TempAlpha)) + (*SourcePointer2 * TempAlpha);
                            ++TargetPointer2;
                            ++SourcePointer2;
                        }
                    }
                }
            }
            return image;
        }
    }
}