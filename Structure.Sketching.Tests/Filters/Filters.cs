using Structure.Sketching.Colors;
using Structure.Sketching.Filters;

//using Structure.Sketching.Filters;
//using Structure.Sketching.Filters.Arithmetic;
//using Structure.Sketching.Filters.Binary;
//using Structure.Sketching.Filters.ColorMatrix;
//using Structure.Sketching.Filters.ColorMatrix.ColorBlindness;
//using Structure.Sketching.Filters.Convolution;
//using Structure.Sketching.Filters.Convolution.Enums;
using Structure.Sketching.Filters.Drawing;
using Structure.Sketching.Filters.Interfaces;

//using Structure.Sketching.Filters.Morphology;
//using Structure.Sketching.Filters.Overlays;
//using Structure.Sketching.Filters.Pipelines;
//using Structure.Sketching.Filters.Resampling;
//using Structure.Sketching.Filters.Resampling.Enums;
//using Structure.Sketching.Filters.Smoothing;
using Structure.Sketching.Tests.BaseClasses;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.Filters.ColorMatrix
{
    public class Filters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Sketching.Numerics.Rectangle> ColorMatrixFilters = new TheoryData<string, IFilter, Sketching.Numerics.Rectangle>
        {
            { "DrawingLine", new Line(Color.Fuchsia,0,0,500,1000),default(Sketching.Numerics.Rectangle) },
            { "DrawingRectangle", new Rectangle(Color.Fuchsia,false,new Sketching.Numerics.Rectangle(0,0,500,1000)),default(Sketching.Numerics.Rectangle) },
            { "DrawingEllipse", new Ellipse(Color.Fuchsia,false,100,100,new System.Numerics.Vector2(500,500)),default(Sketching.Numerics.Rectangle) },
            { "DrawingFilledEllipse", new Ellipse(Color.Fuchsia,true,100,100,new System.Numerics.Vector2(500,500)),default(Sketching.Numerics.Rectangle) },
            { "Pointillism", new Pointillism(5),default(Sketching.Numerics.Rectangle) },
            //{ "Logarithm", new Logarithm(),default(Sketching.Numerics.Rectangle) },
            //{ "Posterize", new Posterize(10),default(Sketching.Numerics.Rectangle) },
            //{ "Solarize", new Solarize(1f),default(Sketching.Numerics.Rectangle) },
            //{ "NonMaximalSuppression", new NonMaximalSuppression(Color.White,Color.Black,0.8f,0.5f),default(Sketching.Numerics.Rectangle) },
            //{ "CannyEdgeDetection", new CannyEdgeDetection(Color.White,Color.Black,0.8f,0.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Scale", new Scale(2000,2000,ResamplingFiltersAvailable.Hermite),default(Sketching.Numerics.Rectangle) },
            //{ "Translate", new Translate(50,50),default(Sketching.Numerics.Rectangle) },
            //{ "Alpha-50", new Alpha(.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Blend-50", new Blend(new Resize(500,500,ResamplingFiltersAvailable.Bilinear).Apply(new Image("./TestImages/EncodingTest.bmp")),0.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Replace-Black-For-White", new Replace(Color.Black,Color.White,0.2f),default(Sketching.Numerics.Rectangle) },
            //{ "Invert", new Invert(),default(Sketching.Numerics.Rectangle) },
            //{ "Crop", new Crop(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Flip-Vertical", new Flip(FlipDirection.Vertical),default(Sketching.Numerics.Rectangle) },
            //{ "Flip-Horizontal", new Flip(FlipDirection.Horizontal),default(Sketching.Numerics.Rectangle) },
            //{ "Flip-Both", new Flip(FlipDirection.Horizontal|FlipDirection.Vertical),default(Sketching.Numerics.Rectangle) },
            //{ "Rotate-45", new Rotate(45f),default(Sketching.Numerics.Rectangle) },
            //{ "Skew-45-80", new Skew(45f,80f,ResamplingFiltersAvailable.Hermite),default(Sketching.Numerics.Rectangle) },
            //{ "ResizeCanvas-UpperLeft-100x100", new ResizeCanvas(100,100),default(Sketching.Numerics.Rectangle) },
            //{ "ResizeCanvas-Center-100x100", new ResizeCanvas(100,100,ResizeOptions.Center),default(Sketching.Numerics.Rectangle) },
            //{ "ResizeCanvas-UpperLeft-2000x2000", new ResizeCanvas(2000,2000),default(Sketching.Numerics.Rectangle) },
            //{ "ResizeCanvas-Center-2000x2000", new ResizeCanvas(2000,2000,ResizeOptions.Center),default(Sketching.Numerics.Rectangle) },
            //{ "Fill-Purple", new Fill(new Color(127,0,127,255)),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "AdaptiveThreshold", new AdaptiveThreshold(10,Color.White,Color.Black,.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Threshold", new Threshold(Color.White,Color.Black,.5f),default(Sketching.Numerics.Rectangle) },
            //{ "BlackWhite", new BlackWhite(),default(Sketching.Numerics.Rectangle) },
            //{ "BlueFilter", new BlueFilter(),default(Sketching.Numerics.Rectangle) },
            //{ "Tritanopia", new Tritanopia(),default(Sketching.Numerics.Rectangle) },
            //{ "Tritanomaly", new Tritanomaly(),default(Sketching.Numerics.Rectangle) },
            //{ "Protanopia", new Protanopia(),default(Sketching.Numerics.Rectangle) },
            //{ "Protanomaly", new Protanomaly(),default(Sketching.Numerics.Rectangle) },
            //{ "Deuteranopia", new Deuteranopia(),default(Sketching.Numerics.Rectangle) },
            //{ "Achromatomaly", new Achromatomaly(),default(Sketching.Numerics.Rectangle) },
            //{ "Deuteranomaly", new Deuteranomaly(),default(Sketching.Numerics.Rectangle) },
            //{ "Achromatopsia", new Achromatopsia(),default(Sketching.Numerics.Rectangle) },
            //{ "Gamma", new Gamma(.2f),default(Sketching.Numerics.Rectangle) },
            //{ "SNNBlur-5", new SNNBlur(5),default(Sketching.Numerics.Rectangle) },
            //{ "Kuwahara-7", new Kuwahara(7),default(Sketching.Numerics.Rectangle) },
            //{ "Median-5", new Median(5),default(Sketching.Numerics.Rectangle) },
            //{ "NormalMap", new NormalMap(XDirection.LeftToRight,YDirection.BottomToTop),default(Sketching.Numerics.Rectangle) },
            //{ "Dilate", new Dilate(1),default(Sketching.Numerics.Rectangle) },
            //{ "AdaptiveEqualize", new AdaptiveEqualize(5),default(Sketching.Numerics.Rectangle) },
            //{ "AdaptiveHSVEqualize", new AdaptiveEqualize(5,()=>new HSVHistogram()),default(Sketching.Numerics.Rectangle) },
            //{ "HSVEqualize", new Equalize(new HSVHistogram()),default(Sketching.Numerics.Rectangle) },
            //{ "Equalize", new Equalize(),default(Sketching.Numerics.Rectangle) },
            //{ "Constrict", new Constrict(1),default(Sketching.Numerics.Rectangle) },
            //{ "Brightness-50", new Brightness(.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Brightness--50", new Brightness(-.5f),default(Sketching.Numerics.Rectangle) },
            //{ "StretchContrast", new StretchContrast(),default(Sketching.Numerics.Rectangle) },
            //{ "Turbulence", new Turbulence(),default(Sketching.Numerics.Rectangle) },
            //{ "GreenFilter", new GreenFilter(),default(Sketching.Numerics.Rectangle) },
            //{ "Greyscale601", new Greyscale601(),default(Sketching.Numerics.Rectangle) },
            //{ "Greyscale709", new Greyscale709(),default(Sketching.Numerics.Rectangle) },
            //{ "Hue-90", new Hue(90),default(Sketching.Numerics.Rectangle) },
            //{ "Hue-180", new Hue(180),default(Sketching.Numerics.Rectangle) },
            //{ "Hue-270", new Hue(270),default(Sketching.Numerics.Rectangle) },
            //{ "Kodachrome", new Kodachrome(),default(Sketching.Numerics.Rectangle) },
            //{ "LomographColorMatrix", new LomographColorMatrix(),default(Sketching.Numerics.Rectangle) },
            //{ "PolaroidColorMatrix", new PolaroidColorMatrix(),default(Sketching.Numerics.Rectangle) },
            //{ "RedFilter", new RedFilter(),default(Sketching.Numerics.Rectangle) },
            //{ "Saturation-50", new Saturation(.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Saturation--50", new Saturation(-.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Sepiatone", new Sepiatone(),default(Sketching.Numerics.Rectangle) },
            //{ "SobelEmboss", new SobelEmboss(),default(Sketching.Numerics.Rectangle) },
            //{ "BoxBlur-3", new BoxBlur(3),default(Sketching.Numerics.Rectangle) },
            //{ "BoxBlur-5", new BoxBlur(5),default(Sketching.Numerics.Rectangle) },
            //{ "Robinson", new Robinson(),default(Sketching.Numerics.Rectangle) },
            //{ "Unsharp", new Unsharp(3,0.2f),default(Sketching.Numerics.Rectangle) },
            //{ "Emboss", new Emboss(),default(Sketching.Numerics.Rectangle) },
            //{ "LaplaceEdgeDetection", new LaplaceEdgeDetection(),default(Sketching.Numerics.Rectangle) },
            //{ "Sharpen", new Sharpen(),default(Sketching.Numerics.Rectangle) },
            //{ "SharpenLess", new SharpenLess(),default(Sketching.Numerics.Rectangle) },
            //{ "LaplacianOfGaussianEdgeDetector", new LaplacianOfGaussianEdgeDetector(),default(Sketching.Numerics.Rectangle) },
            //{ "BumpMap", new BumpMap(Direction.LeftToRight),default(Sketching.Numerics.Rectangle) },
            //{ "GaussianBlur-7", new GaussianBlur(7) ,default(Sketching.Numerics.Rectangle)},
            //{ "Pixellate-10", new Pixellate(10) ,default(Sketching.Numerics.Rectangle)},
            //{ "Kayyali", new Kayyali(),default(Sketching.Numerics.Rectangle) },
            //{ "Kirsch", new Kirsch(),default(Sketching.Numerics.Rectangle) },
            //{ "SinWave", new SinWave(10f,10f,Direction.LeftToRight),default(Sketching.Numerics.Rectangle) },
            //{ "Prewitt", new Prewitt() ,default(Sketching.Numerics.Rectangle)},
            //{ "RobertsCross", new RobertsCross(),default(Sketching.Numerics.Rectangle) },
            //{ "Scharr", new Scharr(),default(Sketching.Numerics.Rectangle) },
            //{ "ColorMatrix", new Sketching.Filters.ColorMatrix.ColorMatrix(new Matrix5x5(1,0,1,0,0,0,.5f,0,0,0,2.5f,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0)),default(Sketching.Numerics.Rectangle) },
            //{ "MatrixMultiplication", new PolaroidColorMatrix() * new Brightness(.1f) ,default(Sketching.Numerics.Rectangle)},
            //{ "Contrast-50", new Contrast(1.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Contrast--50", new Contrast(0.5f),default(Sketching.Numerics.Rectangle) },
            //{ "Pipeline-Polaroid-Brightness", new ProcessingPipeline(true).AddFilter(new PolaroidColorMatrix()).AddFilter(new Brightness(.1f)),default(Sketching.Numerics.Rectangle) },
            //{ "Pipeline-RC-Greyscale", new ProcessingPipeline(true).AddFilter(new RobertsCross()).AddFilter(new Greyscale601()),default(Sketching.Numerics.Rectangle) },
            //{ "Poloroid", new Polaroid(),default(Sketching.Numerics.Rectangle)},
            //{ "Lomograph", new Lomograph() ,default(Sketching.Numerics.Rectangle)},

            //{ "Logarithm-Partial", new Logarithm(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Posterize-Partial", new Posterize(10),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Solarize-Partial", new Solarize(1f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "NonMaximalSuppression-Partial", new NonMaximalSuppression(Color.White,Color.Black,0.8f,0.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "CannyEdgeDetection-Partial", new CannyEdgeDetection(Color.White,Color.Black,0.8f,0.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Scale-Partial", new Scale(2000,2000),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Translate-Partial", new Translate(50,50),new Sketching.Numerics.Rectangle(150,150,500,500) },
            //{ "Alpha-50-Partial", new Alpha(.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Replace-Black-For-White-Partial", new Replace(Color.Black,Color.White,0.2f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Invert-Partial", new Invert(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Flip-Vertical-Partial", new Flip(FlipDirection.Vertical),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Flip-Horizontal-Partial", new Flip(FlipDirection.Horizontal),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Flip-Both-Partial", new Flip(FlipDirection.Horizontal|FlipDirection.Vertical),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Rotate-45-Partial", new Rotate(45f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Skew-45-80-Partial", new Skew(45f,80f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "AdaptiveThreshold-Partial", new AdaptiveThreshold(10,Color.White,Color.Black,.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Threshold-Partial", new Threshold(Color.White,Color.Black,.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "BlackWhite-Partial", new BlackWhite(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "BlueFilter-Partial", new BlueFilter(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Tritanopia-Partial", new Tritanopia(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Tritanomaly-Partial", new Tritanomaly(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Protanopia-Partial", new Protanopia(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Protanomaly-Partial", new Protanomaly(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Deuteranopia-Partial", new Deuteranopia(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Achromatomaly-Partial", new Achromatomaly(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Deuteranomaly-Partial", new Deuteranomaly(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Achromatopsia-Partial", new Achromatopsia(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Gamma-Partial", new Gamma(.2f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "SNNBlur-5-Partial", new SNNBlur(5),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Kuwahara-7-Partial", new Kuwahara(7),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Median-5-Partial", new Median(5),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "NormalMap-Partial", new NormalMap(XDirection.LeftToRight,YDirection.BottomToTop),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Dilate-Partial", new Dilate(1),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "AdaptiveEqualize-Partial", new AdaptiveEqualize(5),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "AdaptiveHSVEqualize-Partial", new AdaptiveEqualize(5,()=>new HSVHistogram()),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "HSVEqualize-Partial", new Equalize(new HSVHistogram()),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Equalize-Partial", new Equalize(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Constrict-Partial", new Constrict(1),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Brightness-50-Partial", new Brightness(.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Brightness--50-Partial", new Brightness(-.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Robinson-Partial", new Robinson(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Unsharp-Partial", new Unsharp(3,0.2f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "StretchContrast-Partial", new StretchContrast(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Turbulence-Partial", new Turbulence(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "GreenFilter-Partial", new GreenFilter(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Greyscale601-Partial", new Greyscale601(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Greyscale709-Partial", new Greyscale709(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Hue-90-Partial", new Hue(90),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Hue-180-Partial", new Hue(180),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Hue-270-Partial", new Hue(270),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Kodachrome-Partial", new Kodachrome(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "LomographColorMatrix-Partial", new LomographColorMatrix(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "PolaroidColorMatrix-Partial", new PolaroidColorMatrix(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "RedFilter-Partial", new RedFilter(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Saturation-50-Partial", new Saturation(.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Saturation--50-Partial", new Saturation(-.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Sepiatone-Partial", new Sepiatone(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "SobelEmboss-Partial", new SobelEmboss(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "BoxBlur-3-Partial", new BoxBlur(3),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "BoxBlur-5-Partial", new BoxBlur(5),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Emboss-Partial", new Emboss(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "LaplaceEdgeDetection-Partial", new LaplaceEdgeDetection(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Sharpen-Partial", new Sharpen(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "SharpenLess-Partial", new SharpenLess(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "LaplacianOfGaussianEdgeDetector-Partial", new LaplacianOfGaussianEdgeDetector(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "BumpMap-Partial", new BumpMap(Direction.LeftToRight),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "GaussianBlur-7-Partial", new GaussianBlur(7) ,new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Pixellate-10-Partial", new Pixellate(10) ,new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Kayyali-Partial", new Kayyali(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Kirsch-Partial", new Kirsch(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "SinWave-Partial", new SinWave(10f,10f,Direction.LeftToRight),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Prewitt-Partial", new Prewitt() ,new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "RobertsCross-Partial", new RobertsCross(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Scharr-Partial", new Scharr(),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "ColorMatrix-Partial", new Sketching.Filters.ColorMatrix.ColorMatrix(new Matrix5x5(1,0,1,0,0,0,.5f,0,0,0,2.5f,-1,-1,0,0,0,0,0,0,0,0,0,0,0,0)),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "MatrixMultiplication-Partial", new PolaroidColorMatrix() * new Brightness(.1f) ,new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Contrast-50-Partial", new Contrast(1.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Contrast--50-Partial", new Contrast(0.5f),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Pipeline-Polaroid-Brightness-Partial", new ProcessingPipeline(true).AddFilter(new PolaroidColorMatrix()).AddFilter(new Brightness(.1f)),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Pipeline-RC-Greyscale-Partial", new ProcessingPipeline(true).AddFilter(new RobertsCross()).AddFilter(new Greyscale601()),new Sketching.Numerics.Rectangle(100,100,500,500) },
            //{ "Poloroid-Partial", new Polaroid(),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Lomograph-Partial", new Lomograph() ,new Sketching.Numerics.Rectangle(100,100,500,500)},

            //{ "XOr", new XOr(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Or", new Or(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "And", new And(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Subtract", new Subtract(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Add", new Add(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Division", new Division(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Multiplication", new Multiplication(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},
            //{ "Modulo", new Modulo(new Image("./TestImages/Formats/Bmp/Car.bmp")),new Sketching.Numerics.Rectangle(100,100,500,500)},

            //{ "Noise-20", new Noise(20),default(Sketching.Numerics.Rectangle) },                    //Will never be correct as final result contains random noise
            //{ "Jitter-5", new Jitter(5),default(Sketching.Numerics.Rectangle) },                       //Will never be correct as final result contains random noise
            //{ "Jitter-10", new Jitter(10),default(Sketching.Numerics.Rectangle) },                     //Will never be correct as final result contains random noise
            //{"Resize-Bilinear-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bilinear),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Bilinear-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bilinear),default(Sketching.Numerics.Rectangle) },
            //{"Resize-NearestNeighbor-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.NearestNeighbor),default(Sketching.Numerics.Rectangle) },
            //{"Resize-NearestNeighbor-100x100",new Resize(100,100,ResamplingFiltersAvailable.NearestNeighbor) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-Robidoux-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Robidoux),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Robidoux-100x100",new Resize(100,100,ResamplingFiltersAvailable.Robidoux),default(Sketching.Numerics.Rectangle) },
            //{"Resize-RobidouxSharp-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.RobidouxSharp) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-RobidouxSharp-100x100",new Resize(100,100,ResamplingFiltersAvailable.RobidouxSharp) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-RobidouxSoft-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.RobidouxSoft) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-RobidouxSoft-100x100",new Resize(100,100,ResamplingFiltersAvailable.RobidouxSoft),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Bicubic-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bicubic) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-Bicubic-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bicubic),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Bell-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bell),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Bell-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bell),default(Sketching.Numerics.Rectangle) },
            //{"Resize-CatmullRom-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CatmullRom),default(Sketching.Numerics.Rectangle) },
            //{"Resize-CatmullRom-100x100",new Resize(100,100,ResamplingFiltersAvailable.CatmullRom),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Cosine-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Cosine),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Cosine-100x100",new Resize(100,100,ResamplingFiltersAvailable.Cosine),default(Sketching.Numerics.Rectangle) },
            //{"Resize-CubicBSpline-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CubicBSpline) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-CubicBSpline-100x100",new Resize(100,100,ResamplingFiltersAvailable.CubicBSpline),default(Sketching.Numerics.Rectangle) },
            //{"Resize-CubicConvolution-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CubicConvolution),default(Sketching.Numerics.Rectangle) },
            //{"Resize-CubicConvolution-100x100",new Resize(100,100,ResamplingFiltersAvailable.CubicConvolution),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Hermite-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Hermite),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Hermite-100x100",new Resize(100,100,ResamplingFiltersAvailable.Hermite),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Lanczos3-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Lanczos3),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Lanczos3-100x100",new Resize(100,100,ResamplingFiltersAvailable.Lanczos3) ,default(Sketching.Numerics.Rectangle)},
            //{"Resize-Lanczos8-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Lanczos8),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Lanczos8-100x100",new Resize(100,100,ResamplingFiltersAvailable.Lanczos8),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Mitchell-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Mitchell),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Mitchell-100x100",new Resize(100,100,ResamplingFiltersAvailable.Mitchell),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Quadratic-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Quadratic),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Quadratic-100x100",new Resize(100,100,ResamplingFiltersAvailable.Quadratic),default(Sketching.Numerics.Rectangle) },
            //{"Resize-QuadraticBSpline-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.QuadraticBSpline),default(Sketching.Numerics.Rectangle) },
            //{"Resize-QuadraticBSpline-100x100",new Resize(100,100,ResamplingFiltersAvailable.QuadraticBSpline),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Triangle-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Triangle),default(Sketching.Numerics.Rectangle) },
            //{"Resize-Triangle-100x100",new Resize(100,100,ResamplingFiltersAvailable.Triangle),default(Sketching.Numerics.Rectangle) },
        };

        [Theory]
        [MemberData("ColorMatrixFilters")]
        public void CheckCorrect(string name, IFilter filter, Sketching.Numerics.Rectangle target)
        {
            foreach (var file in Files)
            {
                string outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);
                new Image(file)
                    .Apply(filter, target)
                    .Save(OutputDirectory + outputFileName);
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