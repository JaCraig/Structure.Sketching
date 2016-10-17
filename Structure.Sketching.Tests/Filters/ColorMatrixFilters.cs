using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.ColorMatrix.ColorBlindness;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class ColorMatrixFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> Filters = new TheoryData<string, IFilter, Rectangle>
        {
            { "Alpha-50", new Alpha(.5f),default(Rectangle) },
            { "BlackWhite", new BlackWhite(),default(Rectangle) },
            { "BlueFilter", new BlueFilter(),default(Rectangle) },
            { "Tritanopia", new Tritanopia(),default(Rectangle) },
            { "Tritanomaly", new Tritanomaly(),default(Rectangle) },
            { "Protanopia", new Protanopia(),default(Rectangle) },
            { "Protanomaly", new Protanomaly(),default(Rectangle) },
            { "Deuteranopia", new Deuteranopia(),default(Rectangle) },
            { "Achromatomaly", new Achromatomaly(),default(Rectangle) },
            { "Deuteranomaly", new Deuteranomaly(),default(Rectangle) },
            { "Achromatopsia", new Achromatopsia(),default(Rectangle) },
            { "Brightness-50", new Brightness(.5f),default(Rectangle) },
            { "Brightness--50", new Brightness(-.5f),default(Rectangle) },
            { "GreenFilter", new GreenFilter(),default(Rectangle) },
            { "Greyscale601", new Greyscale601(),default(Rectangle) },
            { "Greyscale709", new Greyscale709(),default(Rectangle) },
            { "LomographColorMatrix", new LomographColorMatrix(),default(Rectangle) },
            { "PolaroidColorMatrix", new PolaroidColorMatrix(),default(Rectangle) },
            { "RedFilter", new RedFilter(),default(Rectangle) },
            { "Contrast-50", new Contrast(1.5f),default(Rectangle) },
            { "Contrast--50", new Contrast(0.5f),default(Rectangle) },
            { "Hue-90", new Hue(90),default(Rectangle) },
            { "Hue-180", new Hue(180),default(Rectangle) },
            { "Hue-270", new Hue(270),default(Rectangle) },
            { "Kodachrome", new Kodachrome(),default(Rectangle) },
            { "Saturation-50", new Saturation(.5f),default(Rectangle) },
            { "Saturation--50", new Saturation(-.5f),default(Rectangle) },
            { "Sepiatone", new Sepiatone(),default(Rectangle) },
            { "MatrixMultiplication", new PolaroidColorMatrix() * new Brightness(.1f) ,default(Rectangle)},
            { "ColorMatrix", new Sketching.Filters.ColorMatrix.ColorMatrix(new Matrix5x5(1,0,1,0,0,0,.5f,0,0,0,2.5f,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0)),default(Rectangle) },

            { "Alpha-50-Partial", new Alpha(.5f),new Rectangle(100,100,500,500) },
            { "BlackWhite-Partial", new BlackWhite(),new Rectangle(100,100,500,500) },
            { "BlueFilter-Partial", new BlueFilter(),new Rectangle(100,100,500,500) },
            { "Tritanopia-Partial", new Tritanopia(),new Rectangle(100,100,500,500) },
            { "Tritanomaly-Partial", new Tritanomaly(),new Rectangle(100,100,500,500) },
            { "Protanopia-Partial", new Protanopia(),new Rectangle(100,100,500,500) },
            { "Protanomaly-Partial", new Protanomaly(),new Rectangle(100,100,500,500) },
            { "Deuteranopia-Partial", new Deuteranopia(),new Rectangle(100,100,500,500) },
            { "Achromatomaly-Partial", new Achromatomaly(),new Rectangle(100,100,500,500) },
            { "Deuteranomaly-Partial", new Deuteranomaly(),new Rectangle(100,100,500,500) },
            { "Achromatopsia-Partial", new Achromatopsia(),new Rectangle(100,100,500,500) },
            { "Brightness-50-Partial", new Brightness(.5f),new Rectangle(100,100,500,500) },
            { "Brightness--50-Partial", new Brightness(-.5f),new Rectangle(100,100,500,500) },
            { "GreenFilter-Partial", new GreenFilter(),new Rectangle(100,100,500,500) },
            { "Greyscale601-Partial", new Greyscale601(),new Rectangle(100,100,500,500) },
            { "Greyscale709-Partial", new Greyscale709(),new Rectangle(100,100,500,500) },
            { "Hue-90-Partial", new Hue(90),new Rectangle(100,100,500,500) },
            { "Hue-180-Partial", new Hue(180),new Rectangle(100,100,500,500) },
            { "Hue-270-Partial", new Hue(270),new Rectangle(100,100,500,500) },
            { "Kodachrome-Partial", new Kodachrome(),new Rectangle(100,100,500,500) },
            { "LomographColorMatrix-Partial", new LomographColorMatrix(),new Rectangle(100,100,500,500) },
            { "PolaroidColorMatrix-Partial", new PolaroidColorMatrix(),new Rectangle(100,100,500,500) },
            { "RedFilter-Partial", new RedFilter(),new Rectangle(100,100,500,500) },
            { "Saturation-50-Partial", new Saturation(.5f),new Rectangle(100,100,500,500) },
            { "Saturation--50-Partial", new Saturation(-.5f),new Rectangle(100,100,500,500) },
            { "Sepiatone-Partial", new Sepiatone(),new Rectangle(100,100,500,500) },
            { "MatrixMultiplication-Partial", new PolaroidColorMatrix() * new Brightness(.1f) ,new Rectangle(100,100,500,500)},
            { "Contrast-50-Partial", new Contrast(1.5f),new Rectangle(100,100,500,500) },
            { "Contrast--50-Partial", new Contrast(0.5f),new Rectangle(100,100,500,500) },
            { "ColorMatrix-Partial", new Sketching.Filters.ColorMatrix.ColorMatrix(new Matrix5x5(1,0,1,0,0,0,.5f,0,0,0,2.5f,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0)),new Rectangle(100,100,500,500) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}