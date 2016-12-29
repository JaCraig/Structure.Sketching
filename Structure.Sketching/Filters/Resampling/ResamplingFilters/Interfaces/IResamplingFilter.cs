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

using Structure.Sketching.Filters.Resampling.ResamplingFilters.HelperClasses;

namespace Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces
{
    /// <summary>
    /// Resampling filter interface
    /// </summary>
    public interface IResamplingFilter
    {
        /// <summary>
        /// Gets the filter radius.
        /// </summary>
        /// <value>The filter radius.</value>
        float FilterRadius { get; }

        /// <summary>
        /// Gets the precomputed x axis weights.
        /// </summary>
        /// <value>The precomputed x axis weights.</value>
        Weights[] XWeights { get; }

        /// <summary>
        /// Gets the precomputed y axis weights.
        /// </summary>
        /// <value>The precomputed y axis weights.</value>
        Weights[] YWeights { get; }

        /// <summary>
        /// Gets the value based on the resampling filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new value based on the input.</returns>
        double GetValue(double value);

        /// <summary>
        /// Precomputes the weights based on the values passed in.
        /// </summary>
        /// <param name="oldWidth">The old width.</param>
        /// <param name="oldHeight">The old height.</param>
        /// <param name="newWidth">The new width.</param>
        /// <param name="newHeight">The new height.</param>
        void Precompute(int oldWidth, int oldHeight, int newWidth, int newHeight);
    }
}