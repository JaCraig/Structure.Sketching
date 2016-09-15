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

using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.Overlays;
using Structure.Sketching.Filters.Pipelines.BaseClasses;
using System.Numerics;

namespace Structure.Sketching.Filters.Pipelines
{
    /// <summary>
    /// Polaroid processing pipeline
    /// </summary>
    /// <seealso cref="Structure.Sketching.Filters.Pipelines.BaseClasses.ProcessingPipelineBaseClass"/>
    public class Polaroid : ProcessingPipelineBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Polaroid"/> class.
        /// </summary>
        public Polaroid()
            : base(true)
        {
            AddFilter(new PolaroidColorMatrix())
                .AddFilter(new Vignette(new Vector4(.4f, 0.1333333333333f, 0, 1), .75f, .75f))
                .AddFilter(new Glow(new Vector4(1, .6f, .4f, .7f), .25f, .25f));
        }
    }
}