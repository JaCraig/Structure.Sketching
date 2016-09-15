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
using System.Collections.Generic;
using System.Linq;

namespace Structure.Sketching.Quantizers.Octree
{
    /// <summary>
    /// Octree
    /// </summary>
    public class Octree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Octree"/> class.
        /// </summary>
        /// <param name="maxColorBits">The maximum color bits.</param>
        public Octree(int maxColorBits)
        {
            MaxColorBits = maxColorBits;
            Leaves = 0;
            ReducibleNodes = new Node[9];
            Root = new Node(0, MaxColorBits, this);
            PreviousColor = 0;
            PreviousNode = null;
        }

        /// <summary>
        /// Gets or sets the number of leaves in the tree
        /// </summary>
        public int Leaves { get; set; }

        /// <summary>
        /// Gets the maximum color bits.
        /// </summary>
        /// <value>
        /// The maximum color bits.
        /// </value>
        public int MaxColorBits { get; private set; }

        /// <summary>
        /// Gets the color of the previous.
        /// </summary>
        /// <value>
        /// The color of the previous.
        /// </value>
        public int PreviousColor { get; private set; }

        /// <summary>
        /// Gets the previous node.
        /// </summary>
        /// <value>
        /// The previous node.
        /// </value>
        public Node PreviousNode { get; private set; }

        /// <summary>
        /// Gets the reducible nodes.
        /// </summary>
        /// <value>
        /// The reducible nodes.
        /// </value>
        public Node[] ReducibleNodes { get; private set; }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public Node Root { get; private set; }

        /// <summary>
        /// The mask
        /// </summary>
        private static readonly int[] Mask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

        /// <summary>
        /// Adds the color.
        /// </summary>
        /// <param name="pixel">The pixel.</param>
        public void AddColor(Bgra pixel)
        {
            if (PreviousColor == pixel)
            {
                if (PreviousNode == null)
                {
                    PreviousColor = pixel;
                    Root.AddColor(pixel, MaxColorBits, 0, this);
                }
                else
                {
                    PreviousNode.Increment(pixel);
                }
            }
            else
            {
                PreviousColor = pixel;
                Root.AddColor(pixel, MaxColorBits, 0, this);
            }
        }

        /// <summary>
        /// Gets the index of the palette.
        /// </summary>
        /// <param name="pixel">The pixel.</param>
        /// <returns>The palette index</returns>
        public int GetPaletteIndex(Bgra pixel)
        {
            return Root.GetPaletteIndex(pixel, 0);
        }

        /// <summary>
        /// Palletizes the specified color count.
        /// </summary>
        /// <param name="colorCount">The color count.</param>
        /// <returns>The resulting palette</returns>
        public List<Bgra> Palletize(int colorCount)
        {
            while (Leaves > colorCount)
            {
                Reduce();
            }
            var palette = new List<Bgra>(Leaves);
            int paletteIndex = 0;
            Root.ConstructPalette(palette, ref paletteIndex);
            return palette.ToList();
        }

        /// <summary>
        /// Tracks the previous.
        /// </summary>
        /// <param name="node">The node.</param>
        public void TrackPrevious(Node node)
        {
            PreviousNode = node;
        }

        /// <summary>
        /// Reduces this instance.
        /// </summary>
        private void Reduce()
        {
            int index = MaxColorBits - 1;
            while ((index > 0) && (ReducibleNodes[index] == null))
            {
                index--;
            }
            Node node = ReducibleNodes[index];
            ReducibleNodes[index] = node.NextReducible;
            Leaves -= node.Reduce();
            PreviousNode = null;
        }
    }
}