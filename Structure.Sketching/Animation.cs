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

using Structure.Sketching.Formats;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structure.Sketching
{
    /// <summary>
    /// A series of images used in an animation
    /// </summary>
    /// <seealso cref="IList{Image}" />
    public class Animation : IList<Image>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Animation" /> class.
        /// </summary>
        /// <param name="images">The images.</param>
        /// <param name="delay">The delay.</param>
        public Animation(IEnumerable<Image> images, short delay)
        {
            Frames = images.ToList();
            Delay = delay;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public Animation(string fileName)
            : this(File.Open(fileName, FileMode.Open, FileAccess.Read))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        public Animation()
            : this(new List<Image>(), 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="animation">The animation.</param>
        public Animation(Animation animation)
            : this(animation, animation.Delay)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public Animation(Stream stream)
            : this(new Manager().DecodeAnimation(stream))
        {
            stream.Dispose();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count => ((IList<Image>)Frames).Count;

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        public short Delay { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly => ((IList<Image>)Frames).IsReadOnly;

        /// <summary>
        /// Gets or sets the repeat count. (0 means loop indefinitely, otherwise loop N+1 times)
        /// </summary>
        /// <value>
        /// The repeat count.
        /// </value>
        public ushort RepeatCount { get; set; }

        /// <summary>
        /// Gets or sets the frames.
        /// </summary>
        /// <value>
        /// The frames.
        /// </value>
        private List<Image> Frames { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Image"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Image"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>The image at the index specified</returns>
        public Image this[int index]
        {
            get
            {
                return ((IList<Image>)Frames)[index];
            }

            set
            {
                ((IList<Image>)Frames)[index] = value;
            }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(Image item)
        {
            ((IList<Image>)Frames).Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            ((IList<Image>)Frames).Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(Image item)
        {
            return ((IList<Image>)Frames).Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(Image[] array, int arrayIndex)
        {
            ((IList<Image>)Frames).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Image> GetEnumerator()
        {
            return ((IList<Image>)Frames).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Image>)Frames).GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(Image item)
        {
            return ((IList<Image>)Frames).IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        public void Insert(int index, Image item)
        {
            ((IList<Image>)Frames).Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(Image item)
        {
            return ((IList<Image>)Frames).Remove(item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            ((IList<Image>)Frames).RemoveAt(index);
        }

        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>True if it is saved, false otherwise</returns>
        public bool Save(string fileName)
        {
            return new Manager().Encode(fileName, this);
        }
    }
}