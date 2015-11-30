using System;

namespace Entities.LocationActors
{
    public interface ICelestialBody 
    {
        /// <summary>
        /// The name of the body
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The radius of the body itself
        /// </summary>
        double Radius { get; }

        /// <summary>
        /// THe distance the body orbits fromt he COM
        /// </summary>
        double OrbitDistance { get; }

        /// <summary>
        /// The angular velocity of the orbit, in radians per second
        /// </summary>
        double OrbitalAngularVelocity { get; }

        /// <summary>
        /// The rotational angular velocity of the object, in radians per second
        /// </summary>
        double RotatationalAngularVelocity { get; }

        /// <summary>
        /// The initial angular offset
        /// </summary>
        double InitialOrbitalAngularPositionOffset { get; }

        /// <summary>
        /// Gets the current angular position
        /// </summary>
        double CurrentAngularPosition { get; }

        /// <summary>
        /// The current orientation of the body
        /// </summary>
        double Angle { get; }

        /// <summary>
        /// The composition of the body
        /// </summary>
        IMaterial Material { get; }

        /// <summary>
        /// The type of the celestial body
        /// </summary>
        CelestialBodyType BodyType { get; }

        /// <summary>
        /// The satellites of this celestial body.
        /// </summary>
        /// <remarks>
        /// Remember that stars do not have satelites - the COM does
        /// </remarks>
        ICelestialBody[] Satellites { get; }
    }
}