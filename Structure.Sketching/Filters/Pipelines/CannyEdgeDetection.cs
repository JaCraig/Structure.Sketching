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

using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Binary;
using Structure.Sketching.Filters.Convolution;
using Structure.Sketching.Filters.Pipelines.BaseClasses;

namespace Structure.Sketching.Filters.Pipelines
{
    /// <summary>
    /// Canny edge detection processing pipeline
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Pipelines.BaseClasses.ProcessingPipelineBaseClass" />
    public class CannyEdgeDetection : ProcessingPipelineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CannyEdgeDetection" /> class.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <param name="threshold1">The threshold1.</param>
        /// <param name="threshold2">The threshold2.</param>
        public CannyEdgeDetection(Color color1, Color color2, float threshold1, float threshold2)
            : base(false)
        {
            AddFilter(new RobertsCross())
            .AddFilter(new NonMaximalSuppression(color1, color2, threshold1, threshold2));
        }
    }
}