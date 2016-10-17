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
using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.Overlays;
using Structure.Sketching.Filters.Pipelines.BaseClasses;

namespace Structure.Sketching.Filters.Pipelines
{
    /// <summary>
    /// Lomograph processing pipeline
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Pipelines.BaseClasses.ProcessingPipelineBaseClass"/>
    public class Lomograph : ProcessingPipelineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lomograph"/> class.
        /// </summary>
        public Lomograph()
            : base(true)
        {
            AddFilter(new LomographColorMatrix())
                .AddFilter(new Vignette(new Color(0, 10, 0, 255), 0, 0));
        }
    }
}