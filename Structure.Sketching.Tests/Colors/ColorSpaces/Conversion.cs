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
            {x=>(CIELab)x,new CIELab(55.818348999038136d,-16.853418868401192d,54.700336854544005d) },
            {x=>(YCbCr)x,new YCbCr(124.49799999999999d,71.849888d,133.35088d) },
            {x=>(HSV)x,new HSV(64.17f,0.82f,0.55f) },
            {x=>(XYZ)x,new XYZ(19.069271087036391d,23.731887647203653d,4.4953578376945806d) },
            {x=>(YXY)x,new YXY(23.731887647203653d,0.40318552970034038d,0.50176819282471141d) },
            {x=>(CIELUV)x,new CIELUV(55.818348999038136d,-1.10255145019876d,59.059378241483785d) },
            {x=>(CIELCH)x,new CIELCH(55.8183479309082d,57.237789252554272d,107.12429940735514d) },
            {x=>(HunterLAB)x,new HunterLAB(48.715385297874477d,-15.379442134009619d,28.629607681009084d) }
        };

        public static readonly TheoryData<Func<IColorSpace, Sketching.Colors.Color>, IColorSpace> ConversionTestData2 = new TheoryData<Func<IColorSpace, Sketching.Colors.Color>, IColorSpace>
        {
            {x=>(Bgra)x,new Bgra(25, 140, 132, 255) },
            {x=>(CIELab)x,new CIELab(55.818348999038136d,-16.853418868401192d,54.700336854544005d) },
            {x=>(YCbCr)x,new YCbCr(124.49799999999999d,71.849888d,133.35088d) },
            {x=>(HSV)x,new HSV(64.17f,0.82f,0.55f) },
            {x=>(XYZ)x,new XYZ(19.069271087036391d,23.731887647203653d,4.4953578376945806d) },
            {x=>(YXY)x,new YXY(23.731887647203653d,0.40318552970034038d,0.50176819282471141d) },
            {x=>(CIELUV)x,new CIELUV(55.818348999038136d,-1.10255145019876d,59.059378241483785d) },
            {x=>(CIELCH)x,new CIELCH(55.8183479309082d,57.237789252554272d,107.12429940735514d) },
            {x=>(HunterLAB)x,new HunterLAB(48.715385297874477d,-15.379442134009619d,28.629607681009084d) }
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