using Structure.Sketching.Filters.Convolution;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class ConvolutionFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> Filters = new TheoryData<string, IFilter, Rectangle>
        {
            { "SobelEmboss", new SobelEmboss(),default(Rectangle) },
            { "BoxBlur-3", new BoxBlur(3),default(Rectangle) },
            { "BoxBlur-5", new BoxBlur(5),default(Rectangle) },
            { "Robinson", new Robinson(),default(Rectangle) },
            { "Emboss", new Emboss(),default(Rectangle) },
            { "LaplaceEdgeDetection", new LaplaceEdgeDetection(),default(Rectangle) },
            { "Sharpen", new Sharpen(),default(Rectangle) },
            { "SharpenLess", new SharpenLess(),default(Rectangle) },
            { "LaplacianOfGaussianEdgeDetector", new LaplacianOfGaussianEdgeDetector(),default(Rectangle) },
            { "Kayyali", new Kayyali(),default(Rectangle) },
            { "Kirsch", new Kirsch(),default(Rectangle) },
            { "Prewitt", new Prewitt() ,default(Rectangle)},
            { "RobertsCross", new RobertsCross(),default(Rectangle) },
            { "Scharr", new Scharr(),default(Rectangle) },

            { "SobelEmboss-Partial", new SobelEmboss(),new Rectangle(100,100,500,500) },
            { "BoxBlur-3-Partial", new BoxBlur(3),new Rectangle(100,100,500,500) },
            { "BoxBlur-5-Partial", new BoxBlur(5),new Rectangle(100,100,500,500) },
            { "Emboss-Partial", new Emboss(),new Rectangle(100,100,500,500) },
            { "LaplaceEdgeDetection-Partial", new LaplaceEdgeDetection(),new Rectangle(100,100,500,500) },
            { "Sharpen-Partial", new Sharpen(),new Rectangle(100,100,500,500) },
            { "SharpenLess-Partial", new SharpenLess(),new Rectangle(100,100,500,500) },
            { "LaplacianOfGaussianEdgeDetector-Partial", new LaplacianOfGaussianEdgeDetector(),new Rectangle(100,100,500,500) },
            { "Kayyali-Partial", new Kayyali(),new Rectangle(100,100,500,500) },
            { "Kirsch-Partial", new Kirsch(),new Rectangle(100,100,500,500) },
            { "Prewitt-Partial", new Prewitt() ,new Rectangle(100,100,500,500)},
            { "RobertsCross-Partial", new RobertsCross(),new Rectangle(100,100,500,500) },
            { "Robinson-Partial", new Robinson(),new Rectangle(100,100,500,500) },
            { "Scharr-Partial", new Scharr(),new Rectangle(100,100,500,500) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}