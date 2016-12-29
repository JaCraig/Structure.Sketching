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
    /// Robidoux filter
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces.IResamplingFilter"/>
    public class RobidouxFilter : ResamplingFilterBase
    {
        /// <summary>
        /// Gets the filter radius.
        /// </summary>
        /// <value>The filter radius.</value>
        public override float FilterRadius => 2f;

        /// <summary>
        /// Gets the value based on the resampling filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new value based on the input.</returns>
        public override double GetValue(double value)
        {
            const float B = 0.3782158F;
            const float C = 0.3108921F;

            if (value < 0) value = -value;
            var temp = value * value;
            if (value < 1)
                return (((12 - (9 * B) - (6 * C)) * (value * temp)) + ((-18 + (12 * B) + (6 * C)) * temp) + (6 - (2 * B))) / 6;
            if (value < 2)
                return (((-B - (6 * C)) * (value * temp)) + (((6 * B) + (30 * C)) * temp) + (((-12 * B) - (48 * C)) * value) + ((8 * B) + (24 * C))) / 6;
            return 0;
        }
    }
}