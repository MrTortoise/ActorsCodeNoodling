using System;
using System.Collections.Immutable;

namespace Entities.LocationActors
{
    /// <summary>
    /// Represents a composite material
    /// </summary>
    public class Material : IMaterial
    {
        public Material(string name, ImmutableDictionary<IResource, int> materialComposition)
        {
            Name = name;
            MaterialComposition = materialComposition;
        }

        /// <summary>
        /// Gets the name of the material
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The composition of the material
        /// </summary>
        public ImmutableDictionary<IResource, int> MaterialComposition { get; }
    }
}