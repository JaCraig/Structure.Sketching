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

using Structure.Sketching.Filters.Convolution.Enums;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System.Numerics;
using System.Threading.Tasks;

namespace Structure.Sketching.Filters.Pipelines
{
    /// <summary>
    /// Normal map processing pipeline
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Interfaces.IFilter"/>
    public class NormalMap : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalMap"/> class.
        /// </summary>
        /// <param name="xDirection">The x direction.</param>
        /// <param name="yDirection">The y direction.</param>
        public NormalMap(XDirection xDirection, YDirection yDirection)
        {
            YDirection = yDirection;
            XDirection = xDirection;
        }

        /// <summary>
        /// Gets or sets the x direction.
        /// </summary>
        /// <value>The x direction.</value>
        public XDirection XDirection { get; set; }

        /// <summary>
        /// Gets or sets the y direction.
        /// </summary>
        /// <value>The y direction.</value>
        public YDirection YDirection { get; set; }

        /// <summary>
        /// Applies the filter to the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The image</returns>
        public unsafe Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            targetLocation = targetLocation == default(Rectangle) ? new Rectangle(0, 0, image.Width, image.Height) : targetLocation.Clamp(image);
            Image TempImageX = new BumpMap(XDirection == XDirection.LeftToRight ? Direction.LeftToRight : Direction.RightToLeft).Apply(image.Copy(), targetLocation);
            Image TempImageY = new BumpMap(YDirection == YDirection.TopToBottom ? Direction.TopToBottom : Direction.BottomToTop).Apply(image.Copy(), targetLocation);
            Parallel.For(targetLocation.Bottom, targetLocation.Top, y =>
            {
                fixed (byte* TargetPointer = &image.Pixels[((y * image.Width) + targetLocation.Left) * 4])
                {
                    byte* TargetPointer2 = TargetPointer;
                    for (int x = targetLocation.Left; x < targetLocation.Right; ++x)
                    {
                        var TempVector = new Vector3(TempImageX.Pixels[((y * image.Width) + x) * 4] / 255f,
                            TempImageY.Pixels[((y * image.Width) + x) * 4] / 255f,
                            1f);
                        TempVector = Vector3.Normalize(TempVector);
                        TempVector = new Vector3(TempVector.X + 1.0f, TempVector.Y + 1f, TempVector.Z + 1f);
                        TempVector /= 2.0f;
                        image.Pixels[((y * image.Width) + x) * 4] = (byte)(TempVector.X * 255);
                        image.Pixels[(((y * image.Width) + x) * 4) + 1] = (byte)(TempVector.Y * 255);
                        image.Pixels[(((y * image.Width) + x) * 4) + 2] = (byte)(TempVector.Z * 255);
                    }
                }
            });
            return image;
        }
    }
}