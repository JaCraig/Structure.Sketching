using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Filters.Resampling;
using Structure.Sketching.Filters.Resampling.Enums;
using Structure.Sketching.Numerics;
using Structure.Sketching.Tests.BaseClasses;
using Xunit;

namespace Structure.Sketching.Tests.Filters
{
    public class ResamplingFilters : FilterTestBaseClass
    {
        public override string ExpectedDirectory => "./ExpectedResults/Filters/";

        public override string OutputDirectory => "./TestOutput/Filters/";

        public static readonly TheoryData<string, IFilter, Rectangle> Filters = new TheoryData<string, IFilter, Rectangle>
        {
            { "Scale", new Scale(2000,2000,ResamplingFiltersAvailable.Hermite),default(Rectangle) },
            { "Translate", new Translate(50,50),default(Rectangle) },
            { "Crop", new Crop(),new Rectangle(100,100,500,500) },
            { "Flip-Vertical", new Flip(FlipDirection.Vertical),default(Rectangle) },
            { "Flip-Horizontal", new Flip(FlipDirection.Horizontal),default(Rectangle) },
            { "Flip-Both", new Flip(FlipDirection.Horizontal|FlipDirection.Vertical),default(Rectangle) },
            { "Rotate-45", new Rotate(45f),default(Rectangle) },
            { "Skew-45-80", new Skew(45f,80f,ResamplingFiltersAvailable.Hermite),default(Rectangle) },
            { "ResizeCanvas-UpperLeft-100x100", new ResizeCanvas(100,100),default(Rectangle) },
            { "ResizeCanvas-Center-100x100", new ResizeCanvas(100,100,ResizeOptions.Center),default(Rectangle) },
            { "ResizeCanvas-UpperLeft-2000x2000", new ResizeCanvas(2000,2000),default(Rectangle) },
            { "ResizeCanvas-Center-2000x2000", new ResizeCanvas(2000,2000,ResizeOptions.Center),default(Rectangle) },

            { "Scale-Partial", new Scale(2000,2000),new Rectangle(100,100,500,500) },
            { "Translate-Partial", new Translate(50,50),new Rectangle(150,150,500,500) },
            { "Flip-Vertical-Partial", new Flip(FlipDirection.Vertical),new Rectangle(100,100,500,500) },
            { "Flip-Horizontal-Partial", new Flip(FlipDirection.Horizontal),new Rectangle(100,100,500,500) },
            { "Flip-Both-Partial", new Flip(FlipDirection.Horizontal|FlipDirection.Vertical),new Rectangle(100,100,500,500) },
            { "Rotate-45-Partial", new Rotate(45f),new Rectangle(100,100,500,500) },
            { "Skew-45-80-Partial", new Skew(45f,80f),new Rectangle(100,100,500,500) },

            {"Resize-Bilinear-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bilinear),default(Rectangle) },
            {"Resize-Bilinear-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bilinear),default(Rectangle) },
            {"Resize-NearestNeighbor-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.NearestNeighbor),default(Rectangle) },
            {"Resize-NearestNeighbor-100x100",new Resize(100,100,ResamplingFiltersAvailable.NearestNeighbor) ,default(Rectangle)},
            {"Resize-Robidoux-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Robidoux),default(Rectangle) },
            {"Resize-Robidoux-100x100",new Resize(100,100,ResamplingFiltersAvailable.Robidoux),default(Rectangle) },
            {"Resize-RobidouxSharp-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.RobidouxSharp) ,default(Rectangle)},
            {"Resize-RobidouxSharp-100x100",new Resize(100,100,ResamplingFiltersAvailable.RobidouxSharp) ,default(Rectangle)},
            {"Resize-RobidouxSoft-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.RobidouxSoft) ,default(Rectangle)},
            {"Resize-RobidouxSoft-100x100",new Resize(100,100,ResamplingFiltersAvailable.RobidouxSoft),default(Rectangle) },
            {"Resize-Bicubic-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bicubic) ,default(Rectangle)},
            {"Resize-Bicubic-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bicubic),default(Rectangle) },
            {"Resize-Bell-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Bell),default(Rectangle) },
            {"Resize-Bell-100x100",new Resize(100,100,ResamplingFiltersAvailable.Bell),default(Rectangle) },
            {"Resize-CatmullRom-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CatmullRom),default(Rectangle) },
            {"Resize-CatmullRom-100x100",new Resize(100,100,ResamplingFiltersAvailable.CatmullRom),default(Rectangle) },
            {"Resize-Cosine-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Cosine),default(Rectangle) },
            {"Resize-Cosine-100x100",new Resize(100,100,ResamplingFiltersAvailable.Cosine),default(Rectangle) },
            {"Resize-CubicBSpline-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CubicBSpline) ,default(Rectangle)},
            {"Resize-CubicBSpline-100x100",new Resize(100,100,ResamplingFiltersAvailable.CubicBSpline),default(Rectangle) },
            {"Resize-CubicConvolution-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.CubicConvolution),default(Rectangle) },
            {"Resize-CubicConvolution-100x100",new Resize(100,100,ResamplingFiltersAvailable.CubicConvolution),default(Rectangle) },
            {"Resize-Hermite-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Hermite),default(Rectangle) },
            {"Resize-Hermite-100x100",new Resize(100,100,ResamplingFiltersAvailable.Hermite),default(Rectangle) },
            {"Resize-Lanczos3-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Lanczos3),default(Rectangle) },
            {"Resize-Lanczos3-100x100",new Resize(100,100,ResamplingFiltersAvailable.Lanczos3) ,default(Rectangle)},
            {"Resize-Lanczos8-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Lanczos8),default(Rectangle) },
            {"Resize-Lanczos8-100x100",new Resize(100,100,ResamplingFiltersAvailable.Lanczos8),default(Rectangle) },
            {"Resize-Mitchell-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Mitchell),default(Rectangle) },
            {"Resize-Mitchell-100x100",new Resize(100,100,ResamplingFiltersAvailable.Mitchell),default(Rectangle) },
            {"Resize-Quadratic-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Quadratic),default(Rectangle) },
            {"Resize-Quadratic-100x100",new Resize(100,100,ResamplingFiltersAvailable.Quadratic),default(Rectangle) },
            {"Resize-QuadraticBSpline-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.QuadraticBSpline),default(Rectangle) },
            {"Resize-QuadraticBSpline-100x100",new Resize(100,100,ResamplingFiltersAvailable.QuadraticBSpline),default(Rectangle) },
            {"Resize-Triangle-2000x2000",new Resize(2000,2000,ResamplingFiltersAvailable.Triangle),default(Rectangle) },
            {"Resize-Triangle-100x100",new Resize(100,100,ResamplingFiltersAvailable.Triangle),default(Rectangle) }
        };

        [Theory]
        [MemberData("Filters")]
        public void Run(string name, IFilter filter, Rectangle target)
        {
            CheckCorrect(name, filter, target);
        }
    }
}