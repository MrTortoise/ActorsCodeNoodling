using System.Collections.Generic;

namespace Entities.LocationActors
{
    public interface IResourceComposition
    {
        /// <summary>
        /// The material composition of the planet.
        /// </summary>
        /// <remarks>
        /// This is a scale starting from 0 and going up to infinity.
        /// I guess this shuld really be a sum of parts that adds up to one.
        /// </remarks>
        Dictionary<IResource, double> MaterialComposition { get; }
    }
}