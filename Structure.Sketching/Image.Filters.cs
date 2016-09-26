using Structure.Sketching.Filters.Interfaces;
using Structure.Sketching.Numerics;

namespace Structure.Sketching
{
    public partial class Image
    {
        /// <summary>
        /// Applies the filter to the specified location.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The resulting image (this)</returns>
        public Image Apply(IFilter filter, Rectangle targetLocation)
        {
            return filter.Apply(this, targetLocation);
        }

        /// <summary>
        /// Applies the filter to the specified location.
        /// </summary>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="targetLocation">The target location.</param>
        /// <returns>The resulting image (this)</returns>
        public Image Apply<TFilter>(Rectangle targetLocation)
            where TFilter : IFilter, new()
        {
            return Apply(new TFilter(), targetLocation);
        }
    }
}