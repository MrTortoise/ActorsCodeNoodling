using System;

namespace Entities
{
    /// <summary>
    /// Represents a resource a location produces and is consumed and sold
    /// </summary>
    public class Resource : ITradeable
    {
        /// <summary>
        /// Creates a resource with a name
        /// </summary>
        /// <param name="name"></param>
        public Resource(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Gets the name of the resource
        /// </summary>
        public string Name { get; }
    }
}