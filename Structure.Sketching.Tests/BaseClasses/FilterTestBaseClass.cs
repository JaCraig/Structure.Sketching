using System.Collections.Generic;
using Xunit;

namespace Structure.Sketching.Tests.BaseClasses
{
    [Collection("FilterCollection")]
    public abstract class FilterTestBaseClass : TestBaseClass
    {
        public static readonly List<string> Files = new List<string>
        {
            "./TestImages/BitmapFilter.bmp",
            //"./TestImages/Formats/Bmp/Car.bmp",
            //"./TestImages/Formats/Png/splash.png",
            //"./TestImages/Formats/Png/indexed.png",
            //"./TestImages/Formats/Png/blur.png",
        };
    }
}