using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;

namespace Entities.LocationActors
{
    public interface IMaterial 
    {
        /// <summary>
        /// Gets the name of this material
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The composition of this material
        /// </summary>
        /// <remarks>
        /// This is a scale starting from 0 and going up to infinity.
        /// I guess this should really be a sum of parts that adds up to one - but what do you do when you want to goto 11?
        /// </remarks>
        ImmutableDictionary<IResource, int> MaterialComposition { get; }
    }
}