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
using Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces;

namespace Structure.Sketching.Filters.Resampling.ResamplingFilters.BaseClasses
{
    /// <summary>
    /// Resampling filter base class
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Resampling.ResamplingFilters.Interfaces.IResamplingFilter"/>
    public abstract class ResamplingFilterBase : IResamplingFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResamplingFilterBase"/> class.
        /// </summary>
        protected ResamplingFilterBase()
        {
        }

        /// <summary>
        /// Gets the filter radius.
        /// </summary>
        /// <value>The filter radius.</value>
        public abstract float FilterRadius { get; }

        /// <summary>
        /// Gets the precomputed weights.
        /// </summary>
        /// <value>The precomputed weights.</value>
        public Weights[] XWeights { get; private set; }

        /// <summary>
        /// Gets the precomputed y axis weights.
        /// </summary>
        /// <value>The precomputed y axis weights.</value>
        public Weights[] YWeights { get; private set; }

        /// <summary>
        /// Gets the value based on the resampling filter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new value based on the input.</returns>
        public abstract double GetValue(double value);

        /// <summary>
        /// Precomputes the weights based on the values passed in.
        /// </summary>
        /// <param name="oldWidth">The old width.</param>
        /// <param name="oldHeight">The old height.</param>
        /// <param name="newWidth">The new width.</param>
        /// <param name="newHeight">The new height.</param>
        public void Precompute(int oldWidth, int oldHeight, int newWidth, int newHeight)
        {
            int DestinationWidth = newWidth < 0 ? oldWidth : newWidth;
            int DestinationHeight = newHeight < 0 ? oldWidth : newHeight;
            XWeights = PrecomputeWeights(DestinationWidth, oldWidth);
            YWeights = PrecomputeWeights(DestinationHeight, oldHeight);
        }

        private Weights[] PrecomputeWeights(int destinationSize, int sourceSize)
        {
            double Scale = (double)destinationSize / (double)sourceSize;
            double Radius = Scale < 1f ? (FilterRadius / Scale) : FilterRadius;
            Weights[] Result = new Weights[sourceSize];

            for (int x = 0; x < sourceSize; ++x)
            {
                var Left = (int)(x - Radius);
                var Right = (int)(x + Radius);
                if (Left < 0)
                    Left = 0;
                if (Right >= sourceSize)
                    Right = sourceSize - 1;
                Result[x] = new Weights();
                Result[x].Values = new double[(Right - Left) + 1];
                for (int y = Left, count = 0; y <= Right; ++y, ++count)
                {
                    Result[x].Values[count] = Scale < 1f ?
                        GetValue((x - y) * Scale) :
                        GetValue(x - y);
                    Result[x].TotalWeight += Result[x].Values[count];
                }

                if (Result[x].TotalWeight > 0)
                {
                    for (int y = 0; y < Result[x].Values.Length; ++y)
                    {
                        Result[x].Values[y] /= Result[x].TotalWeight;
                    }
                }
            }
            return Result;
        }
    }
}