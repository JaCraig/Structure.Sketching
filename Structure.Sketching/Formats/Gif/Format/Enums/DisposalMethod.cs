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

namespace Structure.Sketching.Formats.Gif.Format.Enums
{
    /// <summary>
    /// Disposal method used when GIF hits the end of the animation.
    /// </summary>
    public enum DisposalMethod
    {
        /// <summary>
        /// The undefined method... Do nothing.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The do not dispose method.
        /// </summary>
        DoNotDispose = 1,

        /// <summary>
        /// Restore to background color.
        /// </summary>
        RestoreToBackground = 2,

        /// <summary>
        /// Restore to previous.
        /// </summary>
        RestoreToPrevious = 3
    }
}