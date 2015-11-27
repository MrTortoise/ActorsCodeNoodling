using System;

namespace Entities.LocationActors
{
    /// <summary>
    /// An object that is a celestial body with the appropiate properties
    /// </summary>
    public interface ICelestialBody 
    {
        /// <summary>
        /// The celestial body data.
        /// </summary>
        CelestialBody BodyData { get; }

        /// <summary>
        /// The composition of the body
        /// </summary>
        IMaterial Material { get; }
    }
}