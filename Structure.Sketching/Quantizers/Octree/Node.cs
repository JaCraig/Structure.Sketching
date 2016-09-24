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
using Structure.Sketching.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace Structure.Sketching.Quantizers.Octree
{
    /// <summary>
    /// Node class
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="level">
        /// The level in the tree = 0 - 7
        /// </param>
        /// <param name="colorBits">
        /// The number of significant color bits in the image
        /// </param>
        /// <param name="octree">
        /// The tree to which this node belongs
        /// </param>
        public Node(int level, int colorBits, Octree octree)
        {
            // Construct the new node
            this.leaf = level == colorBits;

            this.red = this.green = this.blue = 0;
            this.pixelCount = 0;

            // If a leaf, increment the leaf count
            if (this.leaf)
            {
                octree.Leaves++;
                this.NextReducible = null;
                this.children = null;
            }
            else
            {
                // Otherwise add this to the reducible nodes
                this.NextReducible = octree.ReducibleNodes[level];
                octree.ReducibleNodes[level] = this;
                this.children = new Node[8];
            }
        }

        /// <summary>
        /// Gets the next reducible node
        /// </summary>
        public Node NextReducible { get; }

        private static readonly int[] Mask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

        /// <summary>
        /// Pointers to any child nodes
        /// </summary>
        private readonly Node[] children;

        /// <summary>
        /// Blue component
        /// </summary>
        private int blue;

        /// <summary>
        /// Green Component
        /// </summary>
        private int green;

        /// <summary>
        /// Flag indicating that this is a leaf node
        /// </summary>
        private bool leaf;

        /// <summary>
        /// The index of this node in the palette
        /// </summary>
        private int paletteIndex;

        /// <summary>
        /// Number of pixels in this node
        /// </summary>
        private int pixelCount;

        /// <summary>
        /// Red component
        /// </summary>
        private int red;

        /// <summary>
        /// Add a color into the tree
        /// </summary>
        /// <param name="pixel">
        /// The color
        /// </param>
        /// <param name="colorBits">
        /// The number of significant color bits
        /// </param>
        /// <param name="level">
        /// The level in the tree
        /// </param>
        /// <param name="octree">
        /// The tree to which this node belongs
        /// </param>
        public void AddColor(Bgra pixel, int colorBits, int level, Octree octree)
        {
            // Update the color information if this is a leaf
            if (this.leaf)
            {
                this.Increment(pixel);

                // Setup the previous node
                octree.TrackPrevious(this);
            }
            else
            {
                // Go to the next level down in the tree
                int shift = 7 - level;
                int index = ((pixel.Red & Mask[level]) >> (shift - 2)) |
                            ((pixel.Green & Mask[level]) >> (shift - 1)) |
                            ((pixel.Blue & Mask[level]) >> shift);

                Node child = this.children[index];

                if (child == null)
                {
                    // Create a new child node and store it in the array
                    child = new Node(level + 1, colorBits, octree);
                    this.children[index] = child;
                }

                // Add the color to the child node
                child.AddColor(pixel, colorBits, level + 1, octree);
            }
        }

        /// <summary>
        /// Traverse the tree, building up the color palette
        /// </summary>
        /// <param name="palette">
        /// The palette
        /// </param>
        /// <param name="index">
        /// The current palette index
        /// </param>
        public void ConstructPalette(List<Bgra> palette, ref int index)
        {
            if (this.leaf)
            {
                // Consume the next palette index
                this.paletteIndex = index++;

                var r = (this.red / this.pixelCount).ToByte();
                var g = (this.green / this.pixelCount).ToByte();
                var b = (this.blue / this.pixelCount).ToByte();

                // And set the color of the palette entry
                palette.Add(new Bgra(b, g, r));
            }
            else
            {
                // Loop through children looking for leaves
                for (int i = 0; i < 8; i++)
                {
                    if (this.children[i] != null)
                    {
                        this.children[i].ConstructPalette(palette, ref index);
                    }
                }
            }
        }

        /// <summary>
        /// Return the palette index for the passed color
        /// </summary>
        /// <param name="pixel">
        /// The <see cref="Bgra"/> representing the pixel.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <returns>
        /// The <see cref="int"/> representing the index of the pixel in the palette.
        /// </returns>
        public int GetPaletteIndex(Bgra pixel, int level)
        {
            int index = this.paletteIndex;

            if (!this.leaf)
            {
                int shift = 7 - level;
                int pixelIndex = ((pixel.Red & Mask[level]) >> (shift - 2)) |
                                 ((pixel.Green & Mask[level]) >> (shift - 1)) |
                                 ((pixel.Blue & Mask[level]) >> shift);

                if (this.children[pixelIndex] != null)
                {
                    index = this.children[pixelIndex].GetPaletteIndex(pixel, level + 1);
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
            this.pixelCount++;
            this.red += pixel.Red;
            this.green += pixel.Green;
            this.blue += pixel.Blue;
        }

        /// <summary>
        /// Reduce this node by removing all of its children
        /// </summary>
        /// <returns>The number of leaves removed</returns>
        public int Reduce()
        {
            this.red = this.green = this.blue = 0;
            int childNodes = 0;

            // Loop through all children and add their information to this node
            for (int index = 0; index < 8; index++)
            {
                if (this.children[index] != null)
                {
                    this.red += this.children[index].red;
                    this.green += this.children[index].green;
                    this.blue += this.children[index].blue;
                    this.pixelCount += this.children[index].pixelCount;
                    ++childNodes;
                    this.children[index] = null;
                }
            }

            // Now change this to a leaf node
            this.leaf = true;

            // Return the number of nodes to decrement the leaf count by
            return childNodes - 1;
        }
    }
}