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
            {x=>(YCbCr)x,new YCbCr(124.498f,71.849884f,133.350876f) }
        };

        [Fact]
        public void BgraToColor()
        {
            Assert.Equal(new Sketching.Colors.Color(132, 140, 25, 51), (Sketching.Colors.Color)new Bgra(25, 140, 132, 51));
        }

        [Theory]
        [MemberData("ConversionTestData")]
        public void CheckCorrect(Func<Sketching.Colors.Color, IColorSpace> conversionFunc, IColorSpace expected)
        {
            var Actual = conversionFunc(new Sketching.Colors.Color(132, 140, 25, 51));
            Assert.Equal(expected, Actual);
        }

        [Fact]
        public void CIELabToColor()
        {
            Assert.Equal(new Sketching.Colors.Color(132, 140, 25, 255), (Sketching.Colors.Color)new CIELab(76.74753f, -16.1817074f, 52.0789261f));
        }

        [Fact]
        public void YCbCrToColor()
        {
            Assert.Equal(new Sketching.Colors.Color(132, 140, 25, 255), (Sketching.Colors.Color)new YCbCr(124.881149f, 71.91583f, 133.505554f));
        }
    }
}