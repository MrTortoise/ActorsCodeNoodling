using System;

namespace Entities
{
    /// <summary>
    /// Represents a resource a location produces and is consumed and sold
    /// </summary>
    public class Resource : IResource
    {
        protected bool Equals(Resource other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }


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

        public override bool Equals(object obj)
        {
            var rhs = obj as Resource;
            if (rhs == null) return false;
            return rhs == this || Equals(rhs);
        }


    }
}