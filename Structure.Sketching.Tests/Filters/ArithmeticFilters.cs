using Structure.Sketching.Filters.Arithmetic;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class ArithmeticFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Sketching.Numerics.Rectangle> Filters = new TheoryData<string, IFilter, Sketching.Numerics.Rectangle>
        {
            { "XOr", new XOr(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "Or", new Or(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "And", new And(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "Subtract", new Subtract(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "Add", new Add(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "Division", new Division(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "Multiplication", new Multiplication(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            { "Modulo", new Modulo(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)}
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Sketching.Numerics.Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}