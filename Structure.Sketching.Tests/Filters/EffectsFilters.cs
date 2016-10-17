using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Convolution.Enums;
using Structure.Sketching.Filters.Effects;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class EffectsFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> Filters = new TheoryData<string, IFilter, Rectangle>
        {
            { "Pointillism", new Pointillism(5),default(Rectangle) },
            { "Posterize", new Posterize(10),default(Rectangle) },
            { "Solarize", new Solarize(1f),default(Rectangle) },
            { "Replace-Black-For-White", new Replace(Color.Black,Color.White,0.2f),default(Rectangle) },
            { "Invert", new Invert(),default(Rectangle) },
            { "Turbulence", new Turbulence(),default(Rectangle) },
            { "Pixellate-10", new Pixellate(10) ,default(Rectangle)},
            { "SinWave", new SinWave(10f,10f,Direction.LeftToRight),default(Rectangle) },

            { "Posterize-Partial", new Posterize(10),new Rectangle(100,100,500,500) },
            { "Solarize-Partial", new Solarize(1f),new Rectangle(100,100,500,500) },
            { "Replace-Black-For-White-Partial", new Replace(Color.Black,Color.White,0.2f),new Rectangle(100,100,500,500) },
            { "Invert-Partial", new Invert(),new Rectangle(100,100,500,500) },
            { "Turbulence-Partial", new Turbulence(),new Rectangle(100,100,500,500) },
            { "Pixellate-10-Partial", new Pixellate(10) ,new Rectangle(100,100,500,500)},
            { "SinWave-Partial", new SinWave(10f,10f,Direction.LeftToRight),new Rectangle(100,100,500,500) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}