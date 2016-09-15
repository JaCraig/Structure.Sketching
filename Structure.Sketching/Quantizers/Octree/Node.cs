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

using Structure.Sketching.Colors.ColorSpaces;
using System;
using System.Collections.Generic;

namespace Structure.Sketching.Quantizers.Octree
{
    /// <summary>
    /// Octree node class
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="colorBits">The color bits.</param>
        /// <param name="octree">The octree.</param>
        public Node(int level, int colorBits, Octree octree)
        {
            ColorBits = colorBits;
            Level = level;
            Color = new Bgra(0, 0, 0);
            IsLeafNode = ColorBits == Level;
            if (IsLeafNode)
            {
                ++octree.Leaves;
                NextReducible = null;
                Children = null;
            }
            else
            {
                NextReducible = octree.ReducibleNodes[level];
                octree.ReducibleNodes[level] = this;
                Children = new Node[8];
            }
        }

        /// <summary>
        /// The children
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public Node[] Children { get; set; }

        /// <summary>
        /// Gets or sets the color bits.
        /// </summary>
        /// <value>
        /// The color bits.
        /// </value>
        public int ColorBits { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is leaf node.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is leaf node; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeafNode { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public int Level { get; set; }

        /// <summary>
        /// Gets the next reducible node
        /// </summary>
        /// <value>
        /// The next reducible.
        /// </value>
        public Node NextReducible { get; }

        /// <summary>
        /// The index of this node in the palette
        /// </summary>
        /// <value>
        /// The index of the palette.
        /// </value>
        public int PaletteIndex { get; set; }

        /// <summary>
        /// The pixel count
        /// </summary>
        /// <value>
        /// The pixel count.
        /// </value>
        public int PixelCount { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Bgra Color;

        /// <summary>
        /// The mask
        /// </summary>
        private static readonly int[] Mask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="pixel">The pixel.</param>
        /// <param name="colorBits">The color bits.</param>
        /// <param name="level">The level.</param>
        /// <param name="octree">The octree.</param>
        public void AddColor(Bgra pixel, int colorBits, int level, Octree octree)
        {
            if (IsLeafNode)
            {
                this.Increment(pixel);
                octree.TrackPrevious(this);
            }
            else
            {
                int shift = 7 - level;
                int index = (((int)pixel.Red & Mask[level]) >> (shift - 2)) |
                            (((int)pixel.Green & Mask[level]) >> (shift - 1)) |
                            (((int)pixel.Blue & Mask[level]) >> shift);

                Node child = Children[index];

                if (child == null)
                {
                    child = new Node(level + 1, colorBits, octree);
                    Children[index] = child;
                }
                child.AddColor(pixel, colorBits, level + 1, octree);
            }
        }

        /// <summary>
        /// Constructs the palette.
        /// </summary>
        /// <param name="palette">The palette.</param>
        /// <param name="index">The index.</param>
        public void ConstructPalette(List<Bgra> palette, ref int index)
        {
            if (IsLeafNode)
            {
                PaletteIndex = index++;

                var red = (byte)(Color.Red / PixelCount);
                var green = (byte)(Color.Green / PixelCount);
                var blue = (byte)(Color.Blue / PixelCount);

                palette.Add(new Bgra(red, green, blue));
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Children[i] != null)
                    {
                        Children[i].ConstructPalette(palette, ref index);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the index of the palette.
        /// </summary>
        /// <param name="pixel">The pixel.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Didn't expect this!</exception>
        public int GetPaletteIndex(Bgra pixel, int level)
        {
            int index = PaletteIndex;

            if (!IsLeafNode)
            {
                int shift = 7 - level;
                int pixelIndex = (((byte)pixel.Red & Mask[level]) >> (shift - 2)) |
                                 (((byte)pixel.Green & Mask[level]) >> (shift - 1)) |
                                 (((byte)pixel.Blue & Mask[level]) >> shift);

                if (Children[pixelIndex] != null)
                {
                    index = Children[pixelIndex].GetPaletteIndex(pixel, level + 1);
                }
                else
                {
                    throw new Exception("Didn't expect this!");
                }
            }

            return index;
        }

        /// <summary>
        /// Increment the pixel count and add to the color information
        /// </summary>
        /// <param name="pixel">
        /// The pixel to add.
        /// </param>
        public void Increment(Bgra pixel)
        {
            PixelCount++;
            Color.Red += pixel.Red;
            Color.Green += pixel.Green;
            Color.Blue += pixel.Blue;
        }

        /// <summary>
        /// Reduce this node by removing all of its children
        /// </summary>
        /// <returns>The number of leaves removed</returns>
        public int Reduce()
        {
            Color = new Bgra(0, 0, 0);
            int childNodes = 0;
            for (int index = 0; index < 8; index++)
            {
                if (Children[index] != null)
                {
                    Color.Red += Children[index].Color.Red;
                    Color.Green += Children[index].Color.Green;
                    Color.Blue += Children[index].Color.Blue;
                    PixelCount += Children[index].PixelCount;
                    ++childNodes;
                    Children[index] = null;
                }
            }

            IsLeafNode = true;
            return childNodes - 1;
        }
    }
}