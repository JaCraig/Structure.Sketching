using Structure.Sketching.ExtensionMethods;
using Xunit;

namespace Structure.Sketching.Tests.ExtensionMethods
{
    public class ExtensionTests
    {
        [Fact]
        public void FloatClamp()
        {
            Assert.Equal(1, 1f.Clamp(0, 1));
            Assert.Equal(1, 2f.Clamp(0, 1));
            Assert.Equal(0, 0f.Clamp(0, 1));
            Assert.Equal(0, (-1f).Clamp(0, 1));
            Assert.Equal(.5f, .5f.Clamp(0, 1));
        }

        [Fact]
        public void IntClamp()
        {
            Assert.Equal(1, 1.Clamp(0, 1));
            Assert.Equal(1, 2.Clamp(0, 1));
            Assert.Equal(0, 0.Clamp(0, 1));
            Assert.Equal(0, (-1).Clamp(0, 1));
            Assert.Equal(0, (-3).Clamp(0, 1));
        }
    }
}