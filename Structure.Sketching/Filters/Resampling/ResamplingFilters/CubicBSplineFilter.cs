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
using System;

namespace Structure.Sketching.Filters.Resampling.ResamplingFilters
{
    /// <summary>
    /// Cubic B Spline filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces.IResamplingFilter"/>
    public class CubicBSplineFilter : IResamplingFilter
    {
        /// <summary>
        /// Gets the filter radius.
        /// </summary>
        /// <value>The filter radius.</value>
        public float FilterRadius => 2f;

        /// <summary>
        /// Gets the value based on the resampling filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new value based on the input.</returns>
        public double GetValue(double value)
        {
            if (value < 0) value = -value;
            if (value < 1)
            {
                var temp = value * value;
                return 0.5 * temp * value - temp + 2f / 3f;
            }
            if (value < 2)
            {
                value = 2f - value;
                return Math.Pow(value, 3) / 6f;
            }
            return 0;
        }
    }
}