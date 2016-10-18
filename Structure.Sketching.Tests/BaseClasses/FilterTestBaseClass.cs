using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Structure.Sketching.Tests.BaseClasses
{
    //[Collection("FilterCollection")]
    public abstract class FilterTestBaseClass : TestBaseClass
    {
        public static readonly List<string> Files = new List<string>
        {
            "./TestImages/BitmapFilter.bmp"
            //"./TestImages/Formats/Bmp/Car.bmp",
            //"./TestImages/Formats/Png/splash.png",
            //"./TestImages/Formats/Png/indexed.png",
            //"./TestImages/Formats/Png/blur.png",
        };

        protected void CheckCorrect(string name, Structure.Sketching.Filters.Interfaces.IFilter filter, Sketching.Numerics.Rectangle target)
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
    }
}