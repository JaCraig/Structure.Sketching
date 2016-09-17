using Structure.Sketching.Colors;
using Structure.Sketching.Filters;
using Structure.Sketching.Filters.Arithmetic;
using Structure.Sketching.Filters.Binary;
using Structure.Sketching.Filters.ColorMatrix;
using Structure.Sketching.Filters.ColorMatrix.ColorBlindness;
using Structure.Sketching.Filters.Convolution;
using Structure.Sketching.Filters.Convolution.Enums;
using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Filters.Morphology;
using Structure.Sketching.Filters.Overlays;
using Structure.Sketching.Filters.Pipelines;
using Structure.Sketching.Filters.Resampling;
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Filters.Smoothing;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Filters.ColorMatrix
{
    public class Filters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> ColorMatrixFilters = new TheoryData<string, IFilter, Rectangle>
        {
            { "Logarithm", new Logarithm(),default(Rectangle) },
            { "Posterize", new Posterize(10),default(Rectangle) },
            { "Solarize", new Solarize(1.5f),default(Rectangle) },
            { "NonMaximalSuppression", new NonMaximalSuppression(Color.White,Color.Black,0.8f,0.5f),default(Rectangle) },
            { "CannyEdgeDetection", new CannyEdgeDetection(Color.White,Color.Black,0.8f,0.5f),default(Rectangle) },
            { "Translate", new Translate(50,50),default(Rectangle) },
            { "Alpha-50", new Alpha(.5f),default(Rectangle) },
            { "Blend-50", new Blend(new Resize(500,500,ResamplingFiltersAvailable.Bilinear).Apply(new Image("./TestImages/EncodingTest.bmp")),0.5f),new Rectangle(100,100,500,500) },
            { "Replace-Black-For-White", new Replace(Color.Black,Color.White,0.2f),default(Rectangle) },
            { "Invert", new Invert(),default(Rectangle) },
            { "Crop", new Crop(),new Rectangle(100,100,500,500) },
            { "Flip-Vertical", new Flip(FlipDirection.Vertical),default(Rectangle) },
            { "Flip-Horizontal", new Flip(FlipDirection.Horizontal),default(Rectangle) },
            { "Flip-Both", new Flip(FlipDirection.Horizontal|FlipDirection.Vertical),default(Rectangle) },
            { "Rotate-45", new Rotate(45f),default(Rectangle) },
            { "Fill-Purple", new Fill(new Color(127,0,127,255)),new Rectangle(100,100,500,500) },
            //{ "Noise-20", new Noise(.2f),default(Rectangle) },                    //Will never be correct as final result contains random noise
            { "AdaptiveThreshold", new AdaptiveThreshold(10,Color.White,Color.Black,.5f),default(Rectangle) },
            { "Threshold", new Threshold(Color.White,Color.Black,.5f),default(Rectangle) },
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
            { "Gamma", new Gamma(.2f),default(Rectangle) },
            { "SNNBlur-5", new SNNBlur(5),default(Rectangle) },
            { "Kuwahara-7", new Kuwahara(7),default(Rectangle) },
            { "Median-5", new Median(5),default(Rectangle) },
            { "NormalMap", new NormalMap(XDirection.LeftToRight,YDirection.BottomToTop),default(Rectangle) },
            { "Dilate", new Dilate(1),default(Rectangle) },
            //{ "Jitter-5", new Jitter(5),default(Rectangle) },                       //Will never be correct as final result contains random noise
            //{ "Jitter-10", new Jitter(10),default(Rectangle) },                     //Will never be correct as final result contains random noise
            { "Equalize", new Equalize(),default(Rectangle) },
            { "Constrict", new Constrict(1),default(Rectangle) },
            { "Brightness-50", new Brightness(.5f),default(Rectangle) },
            { "Brightness--50", new Brightness(-.5f),default(Rectangle) },
            { "StretchContrast", new StretchContrast(),default(Rectangle) },
            { "Turbulence", new Turbulence(),default(Rectangle) },
            { "GreenFilter", new GreenFilter(),default(Rectangle) },
            { "Greyscale601", new Greyscale601(),default(Rectangle) },
            { "Greyscale709", new Greyscale709(),default(Rectangle) },
            { "Hue-90", new Hue(90),default(Rectangle) },
            { "Hue-180", new Hue(180),default(Rectangle) },
            { "Hue-270", new Hue(270),default(Rectangle) },
            { "Kodachrome", new Kodachrome(),default(Rectangle) },
            { "LomographColorMatrix", new LomographColorMatrix(),default(Rectangle) },
            { "PolaroidColorMatrix", new PolaroidColorMatrix(),default(Rectangle) },
            { "RedFilter", new RedFilter(),default(Rectangle) },
            { "Saturation-50", new Saturation(.5f),default(Rectangle) },
            { "Saturation--50", new Saturation(-.5f),default(Rectangle) },
            { "Sepiatone", new Sepiatone(),default(Rectangle) },
            { "SobelEmboss", new SobelEmboss(),default(Rectangle) },
            { "BoxBlur-3", new BoxBlur(3),default(Rectangle) },
            { "BoxBlur-5", new BoxBlur(5),default(Rectangle) },
            { "Robinson", new Robinson(),default(Rectangle) },
            { "Unsharp", new Unsharp(3,0.2f),default(Rectangle) },
            { "Emboss", new Emboss(),default(Rectangle) },
            { "LaplaceEdgeDetection", new LaplaceEdgeDetection(),default(Rectangle) },
            { "Sharpen", new Sharpen(),default(Rectangle) },
            { "SharpenLess", new SharpenLess(),default(Rectangle) },
            { "LaplacianOfGaussianEdgeDetector", new LaplacianOfGaussianEdgeDetector(),default(Rectangle) },
            { "BumpMap", new BumpMap(Direction.LeftToRight),default(Rectangle) },
            { "GaussianBlur-7", new GaussianBlur(7) ,default(Rectangle)},
            { "Pixellate-10", new Pixellate(10) ,default(Rectangle)},
            { "Kayyali", new Kayyali(),default(Rectangle) },
            { "Kirsch", new Kirsch(),default(Rectangle) },
            { "SinWave", new SinWave(10f,10f,Direction.LeftToRight),default(Rectangle) },
            { "Prewitt", new Prewitt() ,default(Rectangle)},
            { "RobertsCross", new RobertsCross(),default(Rectangle) },
            { "Scharr", new Scharr(),default(Rectangle) },
            { "ColorMatrix", new Sketching.Filters.ColorMatrix.ColorMatrix(new Matrix5x5(1,0,1,0,0,0,.5f,0,0,0,2.5f,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0)),default(Rectangle) },
            { "MatrixMultiplication", new PolaroidColorMatrix() * new Brightness(.1f) ,default(Rectangle)},
            { "Contrast-50", new Contrast(1.5f),default(Rectangle) },
            { "Contrast--50", new Contrast(0.5f),default(Rectangle) },
            { "Pipeline-Polaroid-Brightness", new ProcessingPipeline(true).AddFilter(new PolaroidColorMatrix()).AddFilter(new Brightness(.1f)),default(Rectangle) },
            { "Pipeline-RC-Greyscale", new ProcessingPipeline(true).AddFilter(new RobertsCross()).AddFilter(new Greyscale601()),default(Rectangle) },
            { "Poloroid", new Polaroid(),default(Rectangle)},
            { "Lomograph", new Lomograph() ,default(Rectangle)},

            { "Logarithm-Partial", new Logarithm(),new Rectangle(100,100,500,500) },
            { "Posterize-Partial", new Posterize(10),new Rectangle(100,100,500,500) },
            { "Solarize-Partial", new Solarize(1.5f),new Rectangle(100,100,500,500) },
            { "NonMaximalSuppression-Partial", new NonMaximalSuppression(Color.White,Color.Black,0.8f,0.5f),new Rectangle(100,100,500,500) },
            { "CannyEdgeDetection-Partial", new CannyEdgeDetection(Color.White,Color.Black,0.8f,0.5f),new Rectangle(100,100,500,500) },
            { "Translate-Partial", new Translate(50,50),new Rectangle(100,100,500,500) },
            { "Alpha-50-Partial", new Alpha(.5f),new Rectangle(100,100,500,500) },
            { "Replace-Black-For-White-Partial", new Replace(Color.Black,Color.White,0.2f),new Rectangle(100,100,500,500) },
            { "Invert-Partial", new Invert(),new Rectangle(100,100,500,500) },
            { "Flip-Vertical-Partial", new Flip(FlipDirection.Vertical),new Rectangle(100,100,500,500) },
            { "Flip-Horizontal-Partial", new Flip(FlipDirection.Horizontal),new Rectangle(100,100,500,500) },
            { "Flip-Both-Partial", new Flip(FlipDirection.Horizontal|FlipDirection.Vertical),new Rectangle(100,100,500,500) },
            { "Rotate-45-Partial", new Rotate(45f),new Rectangle(100,100,500,500) },
            { "AdaptiveThreshold-Partial", new AdaptiveThreshold(10,Color.White,Color.Black,.5f),new Rectangle(100,100,500,500) },
            { "Threshold-Partial", new Threshold(Color.White,Color.Black,.5f),new Rectangle(100,100,500,500) },
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
            { "Gamma-Partial", new Gamma(.2f),new Rectangle(100,100,500,500) },
            { "SNNBlur-5-Partial", new SNNBlur(5),new Rectangle(100,100,500,500) },
            { "Kuwahara-7-Partial", new Kuwahara(7),new Rectangle(100,100,500,500) },
            { "Median-5-Partial", new Median(5),new Rectangle(100,100,500,500) },
            { "NormalMap-Partial", new NormalMap(XDirection.LeftToRight,YDirection.BottomToTop),new Rectangle(100,100,500,500) },
            { "Dilate-Partial", new Dilate(1),new Rectangle(100,100,500,500) },
            { "Equalize-Partial", new Equalize(),new Rectangle(100,100,500,500) },
            { "Constrict-Partial", new Constrict(1),new Rectangle(100,100,500,500) },
            { "Brightness-50-Partial", new Brightness(.5f),new Rectangle(100,100,500,500) },
            { "Brightness--50-Partial", new Brightness(-.5f),new Rectangle(100,100,500,500) },
            { "Robinson-Partial", new Robinson(),new Rectangle(100,100,500,500) },
            { "Unsharp-Partial", new Unsharp(3,0.2f),new Rectangle(100,100,500,500) },
            { "StretchContrast-Partial", new StretchContrast(),new Rectangle(100,100,500,500) },
            { "Turbulence-Partial", new Turbulence(),new Rectangle(100,100,500,500) },
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
            { "SobelEmboss-Partial", new SobelEmboss(),new Rectangle(100,100,500,500) },
            { "BoxBlur-3-Partial", new BoxBlur(3),new Rectangle(100,100,500,500) },
            { "BoxBlur-5-Partial", new BoxBlur(5),new Rectangle(100,100,500,500) },
            { "Emboss-Partial", new Emboss(),new Rectangle(100,100,500,500) },
            { "LaplaceEdgeDetection-Partial", new LaplaceEdgeDetection(),new Rectangle(100,100,500,500) },
            { "Sharpen-Partial", new Sharpen(),new Rectangle(100,100,500,500) },
            { "SharpenLess-Partial", new SharpenLess(),new Rectangle(100,100,500,500) },
            { "LaplacianOfGaussianEdgeDetector-Partial", new LaplacianOfGaussianEdgeDetector(),new Rectangle(100,100,500,500) },
            { "BumpMap-Partial", new BumpMap(Direction.LeftToRight),new Rectangle(100,100,500,500) },
            { "GaussianBlur-7-Partial", new GaussianBlur(7) ,new Rectangle(100,100,500,500)},
            { "Pixellate-10-Partial", new Pixellate(10) ,new Rectangle(100,100,500,500)},
            { "Kayyali-Partial", new Kayyali(),new Rectangle(100,100,500,500) },
            { "Kirsch-Partial", new Kirsch(),new Rectangle(100,100,500,500) },
            { "SinWave-Partial", new SinWave(10f,10f,Direction.LeftToRight),new Rectangle(100,100,500,500) },
            { "Prewitt-Partial", new Prewitt() ,new Rectangle(100,100,500,500)},
            { "RobertsCross-Partial", new RobertsCross(),new Rectangle(100,100,500,500) },
            { "Scharr-Partial", new Scharr(),new Rectangle(100,100,500,500) },
            { "ColorMatrix-Partial", new Sketching.Filters.ColorMatrix.ColorMatrix(new Matrix5x5(1,0,1,0,0,0,.5f,0,0,0,2.5f,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0)),new Rectangle(100,100,500,500) },
            { "MatrixMultiplication-Partial", new PolaroidColorMatrix() * new Brightness(.1f) ,new Rectangle(100,100,500,500)},
            { "Contrast-50-Partial", new Contrast(1.5f),new Rectangle(100,100,500,500) },
            { "Contrast--50-Partial", new Contrast(0.5f),new Rectangle(100,100,500,500) },
            { "Pipeline-Polaroid-Brightness-Partial", new ProcessingPipeline(true).AddFilter(new PolaroidColorMatrix()).AddFilter(new Brightness(.1f)),new Rectangle(100,100,500,500) },
            { "Pipeline-RC-Greyscale-Partial", new ProcessingPipeline(true).AddFilter(new RobertsCross()).AddFilter(new Greyscale601()),new Rectangle(100,100,500,500) },
            { "Poloroid-Partial", new Polaroid(),new Rectangle(100,100,500,500)},
            { "Lomograph-Partial", new Lomograph() ,new Rectangle(100,100,500,500)},

            { "XOr", new XOr(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "Or", new Or(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "And", new And(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "Subtract", new Subtract(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "Add", new Add(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "Division", new Division(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "Multiplication", new Multiplication(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},
            { "Modulo", new Modulo(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Rectangle(100,100,500,500)},

            //{"Resize-Bilinear-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bilinear),default(Rectangle) },
            //{"Resize-Bilinear-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bilinear),default(Rectangle) },
            //{"Resize-NearestNeighbor-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.NearestNeighbor),default(Rectangle) },
            //{"Resize-NearestNeighbor-100x100",new Resize(100,100,ResamplingFiltersAvailable.NearestNeighbor) ,default(Rectangle)},
            //{"Resize-Robidoux-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Robidoux),default(Rectangle) },
            //{"Resize-Robidoux-100x100",new Resize(100,100,ResamplingFiltersAvailable.Robidoux),default(Rectangle) },
            //{"Resize-RobidouxSharp-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.RobidouxSharp) ,default(Rectangle)},
            //{"Resize-RobidouxSharp-100x100",new Resize(100,100,ResamplingFiltersAvailable.RobidouxSharp) ,default(Rectangle)},
            //{"Resize-RobidouxSoft-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.RobidouxSoft) ,default(Rectangle)},
            //{"Resize-RobidouxSoft-100x100",new Resize(100,100,ResamplingFiltersAvailable.RobidouxSoft),default(Rectangle) },
            //{"Resize-Bicubic-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bicubic) ,default(Rectangle)},
            //{"Resize-Bicubic-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bicubic),default(Rectangle) },
            //{"Resize-Bell-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bell),default(Rectangle) },
            //{"Resize-Bell-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bell),default(Rectangle) },
            //{"Resize-CatmullRom-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CatmullRom),default(Rectangle) },
            //{"Resize-CatmullRom-100x100",new Resize(100,100,ResamplingFiltersAvailable.CatmullRom),default(Rectangle) },
            //{"Resize-Cosine-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Cosine),default(Rectangle) },
            //{"Resize-Cosine-100x100",new Resize(100,100,ResamplingFiltersAvailable.Cosine),default(Rectangle) },
            //{"Resize-CubicBSpline-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CubicBSpline) ,default(Rectangle)},
            //{"Resize-CubicBSpline-100x100",new Resize(100,100,ResamplingFiltersAvailable.CubicBSpline),default(Rectangle) },
            //{"Resize-CubicConvolution-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CubicConvolution),default(Rectangle) },
            //{"Resize-CubicConvolution-100x100",new Resize(100,100,ResamplingFiltersAvailable.CubicConvolution),default(Rectangle) },
            //{"Resize-Hermite-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Hermite),default(Rectangle) },
            //{"Resize-Hermite-100x100",new Resize(100,100,ResamplingFiltersAvailable.Hermite),default(Rectangle) },
            //{"Resize-Lanczos3-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Lanczos3),default(Rectangle) },
            //{"Resize-Lanczos3-100x100",new Resize(100,100,ResamplingFiltersAvailable.Lanczos3) ,default(Rectangle)},
            //{"Resize-Lanczos8-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Lanczos8),default(Rectangle) },
            //{"Resize-Lanczos8-100x100",new Resize(100,100,ResamplingFiltersAvailable.Lanczos8),default(Rectangle) },
            //{"Resize-Mitchell-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Mitchell),default(Rectangle) },
            //{"Resize-Mitchell-100x100",new Resize(100,100,ResamplingFiltersAvailable.Mitchell),default(Rectangle) },
            //{"Resize-Quadratic-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Quadratic),default(Rectangle) },
            //{"Resize-Quadratic-100x100",new Resize(100,100,ResamplingFiltersAvailable.Quadratic),default(Rectangle) },
            //{"Resize-QuadraticBSpline-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.QuadraticBSpline),default(Rectangle) },
            //{"Resize-QuadraticBSpline-100x100",new Resize(100,100,ResamplingFiltersAvailable.QuadraticBSpline),default(Rectangle) },
            //{"Resize-Triangle-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Triangle),default(Rectangle) },
            //{"Resize-Triangle-100x100",new Resize(100,100,ResamplingFiltersAvailable.Triangle),default(Rectangle) },
        };

        [Theory]
        [MemberData("ColorMatrixFilters")]
        public void CheckCorrect(string name, IFilter filter, Rectangle target)
        {
            foreach (var file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                var TestImage = new Image(file);
                filter.Apply(TestImage, target);
                TestImage.Save(OutputDirectory + outputFileName);
            }
            foreach (string file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                Assert.True(CheckFileCorrect(ExpectedDirectory + Path.GetFileName(outputFileName), OutputDirectory + Path.GetFileName(outputFileName)), outputFileName);
            }
        }

        [Fact]
        public void ConvolutionMultiplicationTest()
        {
            var Variable = new Structure.Sketching.Filters.Convolution.BoxBlur(3) * new Structure.Sketching.Filters.Convolution.BoxBlur(3);
            Assert.Equal(5, Variable.Width);
            Assert.Equal(5, Variable.Height);
            Assert.Equal(0, Variable.Offset);
            Assert.Equal(false, Variable.Absolute);
            Assert.Equal(new float[] { 1, 2, 3, 2, 1, 2, 4, 6, 4, 2, 3, 6, 9, 6, 3, 2, 4, 6, 4, 2, 1, 2, 3, 2, 1 }, Variable.Matrix);
        }
    }
}