using Structure.Sketching.Colors.ColorSpaces;
using Structure.Sketching.Colors.ColorSpaces.Interfaces;
using System;
using Xunit;

namespace Structure.Sketching.Tests.Colors.ColorSpaces
{
    public class Conversion
    {
        public static readonly TheoryData<Func<Sketching.Colors.Color, IColorSpace>, IColorSpace> ConversionTestData = new TheoryData<Func<Sketching.Colors.Color, IColorSpace>, IColorSpace>
        {
            {x=>(Bgra)x,new Bgra(25,140,132,51) },
            {x=>(CIELab)x,new CIELab(76.66624f,-16.3309269f,52.36721f) },
            {x=>(YCbCr)x,new YCbCr(124.498f,71.849884f,133.350876f) },
            {x=>(HSV)x,new HSV(64.17f,0.82f,0.55f) },
            {x=>(XYZ)x,new XYZ(19.069271087036391d,23.731887647203653d,4.4953578376945806d) },
            {x=>(YXY)x,new YXY(23.731887647203653d,0.40318552970034038d,0.50176819282471141d) }
        };

        public static readonly TheoryData<Func<IColorSpace, Sketching.Colors.Color>, IColorSpace> ConversionTestData2 = new TheoryData<Func<IColorSpace, Sketching.Colors.Color>, IColorSpace>
        {
            {x=>(Bgra)x,new Bgra(25, 140, 132, 255) },
            {x=>(CIELab)x,new CIELab(76.74753f, -16.1817074f, 52.0789261f) },
            {x=>(YCbCr)x,new YCbCr(124.881149f, 71.91583f, 133.505554f) },
            {x=>(HSV)x,new HSV(64.17f,0.82f,0.55f) },
            {x=>(XYZ)x,new XYZ(19.069271087036391d,23.731887647203653d,4.4953578376945806d) },
            {x=>(YXY)x,new YXY(23.731887647203653d,0.40318552970034038d,0.50176819282471141d) }
        };

        [Theory]
        [MemberData("ConversionTestData")]
        public void CheckCorrect(Func<Sketching.Colors.Color, IColorSpace> conversionFunc, IColorSpace expected)
        {
            var Actual = conversionFunc(new Sketching.Colors.Color(132, 140, 25, 51));
            Assert.Equal(expected, Actual);
        }

        [Theory]
        [MemberData("ConversionTestData2")]
        public void CheckCorrectToRGB(Func<IColorSpace, Sketching.Colors.Color> conversionFunction, IColorSpace value)
        {
            var Actual = conversionFunction(value);
            Assert.Equal(new Sketching.Colors.Color(132, 140, 25, 255), Actual);
        }
    }
}