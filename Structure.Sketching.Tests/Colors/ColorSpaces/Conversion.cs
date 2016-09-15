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
            {x=>(CIELab)x,new CIELab(76.74753f,-16.1817074f,52.0789261f) },
            {x=>(YCbCr)x,new YCbCr(124.881149f,71.91583f,133.505554f) }
        };

        [Fact]
        public void BgraToColor()
        {
            Assert.Equal(new Sketching.Colors.Color(0.517f, 0.549f, 0.098f, .2f), (Sketching.Colors.Color)new Bgra(25, 140, 132, 51));
        }

        [Theory]
        [MemberData("ConversionTestData")]
        public void CheckCorrect(Func<Sketching.Colors.Color, IColorSpace> conversionFunc, IColorSpace expected)
        {
            var Actual = conversionFunc(new Sketching.Colors.Color(.52f, .55f, .1f, .2f));
            Assert.Equal(expected, Actual);
        }

        [Fact]
        public void CIELabToColor()
        {
            Assert.Equal(new Sketching.Colors.Color(.52f, .55f, .1f, 1f), (Sketching.Colors.Color)new CIELab(76.74753f, -16.1817074f, 52.0789261f));
        }

        [Fact]
        public void YCbCrToColor()
        {
            Assert.Equal(new Sketching.Colors.Color(.52f, .55f, .1f, 1f), (Sketching.Colors.Color)new YCbCr(124.881149f, 71.91583f, 133.505554f));
        }
    }
}