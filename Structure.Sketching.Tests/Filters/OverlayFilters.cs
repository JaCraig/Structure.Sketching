using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Filters.Overlays;
using Structure.Sketching.Filters.Resampling;
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class OverlayFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> Filters = new TheoryData<string, IFilter, Rectangle>
        {
            { "Blend-50", new Blend(new Resize(500,500,ResamplingFiltersAvailable.Bilinear).Apply(new Image("./TestImages/Formats/Bmp/EncodingTest.bmp")),0.5f),new Rectangle(100,100,500,500) },
            { "Glow",new Glow(Color.Aqua,0.4f,0.4f),default(Rectangle) },
            { "Vignette",new Vignette(Color.Aqua,0.4f,0.4f),default(Rectangle) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}