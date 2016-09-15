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

namespace Structure.Sketching.Formats.Jpeg.Format.HelperClasses
{
    /// <summary>
    /// Component data holder
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public byte ComponentIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the horizontal sampling factor.
        /// </summary>
        /// <value>
        /// The horizontal sampling factor.
        /// </value>
        public int HorizontalSamplingFactor { get; set; }

        /// <summary>
        /// Gets or sets the quatization table dest selector.
        /// </summary>
        /// <value>
        /// The quatization table dest selector.
        /// </value>
        public byte QuatizationTableDestSelector { get; set; }

        /// <summary>
        /// Gets or sets the vertical sampling factor.
        /// </summary>
        /// <value>
        /// The vertical sampling factor.
        /// </value>
        public int VerticalSamplingFactor { get; set; }
    }
}