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

using Structure.Sketching.Filters.ColorMatrix.BaseClasses;
using Structure.Sketching.Filters.Convolution.BaseClasses;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using System.Collections.Generic;

namespace Structure.Sketching.Filters.Pipelines.BaseClasses
{
    /// <summary>
    /// Processing pipeline base class
    /// </summary>
    public abstract class ProcessingPipelineBaseClass : IFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingPipelineBaseClass"/> class.
        /// </summary>
        /// <param name="combine">
        /// if set to <c>true</c> [combine] the convolution filters when possible.
        /// </param>
        protected ProcessingPipelineBaseClass(bool combine)
        {
            Combine = combine;
            Filters = new List<IFilter>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProcessingPipelineBaseClass"/>
        /// should combine the filters or not.
        /// </summary>
        /// <value><c>true</c> if combine; otherwise, <c>false</c>.</value>
        public bool Combine { get; private set; }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <value>The filters.</value>
        public List<IFilter> Filters { get; private set; }

        /// <summary>
        /// Adds the filter to the pipeline
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>This</returns>
        public ProcessingPipelineBaseClass AddFilter(IFilter filter)
        {
            if (filter as MatrixBaseClass != null && Filters.Count > 0)
            {
                var LastFilter = Filters[Filters.Count - 1];
                if (LastFilter as MatrixBaseClass != null)
                {
                    Filters.Remove(LastFilter);
                    filter = (MatrixBaseClass)LastFilter * (MatrixBaseClass)filter;
                }
            }
            else if (Combine && filter as ConvolutionBaseClass != null && Filters.Count > 0)
            {
                var LastFilter = Filters[Filters.Count - 1];
                if (LastFilter as ConvolutionBaseClass != null)
                {
                    Filters.Remove(LastFilter);
                    filter = (ConvolutionBaseClass)LastFilter * (ConvolutionBaseClass)filter;
                }
            }
            Filters.Add(filter);
            return this;
        }

        /// <summary>
        /// Executes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The resulting image</returns>
        public Image Apply(Image image, Rectangle targetLocation = default(Rectangle))
        {
            for (int x = 0; x < Filters.Count; ++x)
            {
                Filters[x].Apply(image, targetLocation);
            }
            return image;
        }
    }
}