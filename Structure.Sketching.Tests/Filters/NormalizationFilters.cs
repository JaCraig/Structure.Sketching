using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Filters.Normalization;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class NormalizationFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> Filters = new TheoryData<string, IFilter, Rectangle>
        {
            { "Logarithm", new Logarithm(),default(Rectangle) },
            { "Gamma", new Gamma(.2f),default(Rectangle) },
            { "AdaptiveEqualize", new AdaptiveEqualize(5),default(Rectangle) },
            { "AdaptiveHSVEqualize", new AdaptiveEqualize(5,()=>new HSVHistogram()),default(Rectangle) },
            { "HSVEqualize", new Equalize(new HSVHistogram()),default(Rectangle) },
            { "Equalize", new Equalize(),default(Rectangle) },
            { "StretchContrast", new StretchContrast(),default(Rectangle) },

            { "Logarithm-Partial", new Logarithm(),new Rectangle(100,100,500,500) },
            { "Gamma-Partial", new Gamma(.2f),new Rectangle(100,100,500,500) },
            { "AdaptiveEqualize-Partial", new AdaptiveEqualize(5),new Rectangle(100,100,500,500) },
            { "AdaptiveHSVEqualize-Partial", new AdaptiveEqualize(5,()=>new HSVHistogram()),new Rectangle(100,100,500,500) },
            { "HSVEqualize-Partial", new Equalize(new HSVHistogram()),new Rectangle(100,100,500,500) },
            { "Equalize-Partial", new Equalize(),new Rectangle(100,100,500,500) },
            { "StretchContrast-Partial", new StretchContrast(),new Rectangle(100,100,500,500) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}