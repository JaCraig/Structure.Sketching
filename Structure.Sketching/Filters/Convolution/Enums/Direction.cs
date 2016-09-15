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

namespace Structure.Sketching.Filters.Convolution.Enums
{
    /// <summary>
    /// Direction
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Top to bottom
        /// </summary>
        TopToBottom = 0,

        /// <summary>
        /// Bottom to top
        /// </summary>
        BottomToTop = 1,

        /// <summary>
        /// Left to right
        /// </summary>
        LeftToRight = 2,

        /// <summary>
        /// Right to left
        /// </summary>
        RightToLeft = 3
    }

    /// <summary>
    /// X Direction
    /// </summary>
    public enum XDirection
    {
        /// <summary>
        /// Left to right
        /// </summary>
        LeftToRight = 0,

        /// <summary>
        /// Right to left
        /// </summary>
        RightToLeft = 1
    }

    /// <summary>
    /// Y Direction
    /// </summary>
    public enum YDirection
    {
        /// <summary>
        /// Top to bottom
        /// </summary>
        TopToBottom = 0,

        /// <summary>
        /// Bottom to top
        /// </summary>
        BottomToTop = 1
    }
}