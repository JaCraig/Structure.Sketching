using Structure.Sketching.Filters.Drawing.Interfaces;
using System;

namespace Structure.Sketching.Filters.Drawing.BaseClasses
{
    public abstract class ShapeBaseClass : IShape
    {
        public Image Apply(Image image, Numerics.Rectangle targetLocation = default(Numerics.Rectangle))
        {
            throw new NotImplementedException();
        }
    }
}