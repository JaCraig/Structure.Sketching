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

using Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces;

namespace Structure.Sketching.Filters.Resampling.ResamplingFilters
{
    /// <summary>
    /// Quadratic B Spline filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces.IResamplingFilter"/>
    public class QuadraticBSplineFilter : IResamplingFilter
    {
        /// <summary>
        /// Gets the filter radius.
        /// </summary>
        /// <value>The filter radius.</value>
        public float FilterRadius => 1.5f;

        /// <summary>
        /// Gets the value based on the resampling filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new value based on the input.</returns>
        public double GetValue(double value)
        {
            if (value < 0) value = -value;
            if (value <= 0.5) return -value * value + 0.75;
            if (value <= 1.5) return 0.5 * value * value - 1.5 * value + 1.125;
            return 0;
        }
    }
}