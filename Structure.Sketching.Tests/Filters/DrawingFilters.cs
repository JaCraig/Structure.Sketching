using Structure.Sketching.Colors;
using Structure.Sketching.Filters.Drawing;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class DrawingFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Sketching.Numerics.Rectangle> Filters = new TheoryData<string, IFilter, Sketching.Numerics.Rectangle>
        {
            { "DrawingLine", new Line(Color.Fuchsia,0,0,500,1000),default(Sketching.Numerics.Rectangle) },
            { "DrawingRectangle", new Rectangle(Color.Fuchsia,false,new Sketching.Numerics.Rectangle(0,0,500,1000)),default(Sketching.Numerics.Rectangle) },
            { "DrawingEllipse", new Ellipse(Color.Fuchsia,false,100,100,new System.Numerics.Vector2(500,500)),default(Sketching.Numerics.Rectangle) },
            { "DrawingFilledEllipse", new Ellipse(Color.Fuchsia,true,100,100,new System.Numerics.Vector2(500,500)),default(Sketching.Numerics.Rectangle) },
            { "Fill-Purple", new Rectangle(new Color(127,0,127,255),true,new Sketching.Numerics.Rectangle(100,100,500,500)),default(Sketching.Numerics.Rectangle) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Sketching.Numerics.Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}