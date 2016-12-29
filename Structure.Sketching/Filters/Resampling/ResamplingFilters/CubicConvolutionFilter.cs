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

using Structure.Sketching.Filters.Resampling.ResamplingFilters.BaseClasses;

namespace Structure.Sketching.Filters.Resampling.ResamplingFilters
{
    /// <summary>
    /// Cubic Convolution filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces.IResamplingFilter"/>
    public class CubicConvolutionFilter : ResamplingFilterBase
    {
        /// <summary>
        /// Gets the filter radius.
        /// </summary>
        /// <value>The filter radius.</value>
        public override float FilterRadius => 3f;

        /// <summary>
        /// Gets the value based on the resampling filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new value based on the input.</returns>
        public override double GetValue(double value)
        {
            if (value < 0) value = -value;
            var temp = value * value;
            if (value <= 1) return (4f / 3f) * temp * value - (7f / 3f) * temp + 1;
            if (value <= 2) return -(7f / 12f) * temp * value + 3 * temp - (59f / 12f) * value + 2.5;
            if (value <= 3) return (1f / 12f) * temp * value - (2f / 3f) * temp + 1.75 * value - 1.5;
            return 0;
        }
    }
}