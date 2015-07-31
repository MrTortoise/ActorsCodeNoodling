using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;


namespace Entities
{
    /// <summary>
    /// Represents a location that could have natural resources or trading platforms
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Creates a location with a name and resources
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resources"></param>
        public Location(string name, IEnumerable<ITradeable> resources)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (resources == null) throw new ArgumentNullException(nameof(resources));

            this.Name = name;
            this.Resources = ImmutableArray.CreateRange(resources.Where(i => i != null));
        }

        /// <summary>
        /// Gets the name of the Location
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets an immutable collection of resources available
        /// </summary>
        public ImmutableArray<ITradeable> Resources { get; private set; }
    }
}