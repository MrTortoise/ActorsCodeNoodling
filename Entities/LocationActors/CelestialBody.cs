using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities.LocationActors
{
    /// <summary>
    /// THe core data for any celestial body
    /// </summary>
    /// <remarks>Nothing clever assume all on one axis - and everything on same azis</remarks>
    public class CelestialBody :  IUpdateDelta
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="radius"></param>
        /// <param name="orbitDistance"></param>
        /// <param name="orbitalAngularVelocity"></param>
        /// <param name="rotatationalAngularVelocity"></param>
        /// <param name="initialOrbitalAngularPositionOffset"></param>
        /// <param name="currentAngularPosition">set to <paramref name="initialOrbitalAngularPositionOffset"/>if creating, or not if reloading</param>
        /// <param name="material">The material the celestial body is made from</param>
        /// <param name="bodyType">The celestial body type</param>
        /// <param name="satellites">The satellites of this body.</param>
        public CelestialBody(string name, double radius, double orbitDistance, double orbitalAngularVelocity, double rotatationalAngularVelocity, double initialOrbitalAngularPositionOffset, double currentAngularPosition, IMaterial material, CelestialBodyType bodyType, CelestialBody[] satellites = null)
        {
            Name = name;
            Radius = radius;
            OrbitDistance = orbitDistance;
            OrbitalAngularVelocity = orbitalAngularVelocity;
            RotatationalAngularVelocity = rotatationalAngularVelocity;
            InitialOrbitalAngularPositionOffset = initialOrbitalAngularPositionOffset;
            CurrentAngularPosition = currentAngularPosition;
            Material = material;
            BodyType = bodyType;
            Satellites = satellites ?? new CelestialBody[0];
        }

        /// <summary>
        /// The name of the body
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The radius of the body itself
        /// </summary>
        public double Radius { get; }

        /// <summary>
        /// THe distance the body orbits fromt he COM
        /// </summary>
        public double OrbitDistance { get; }

        /// <summary>
        /// The angular velocity of the orbit, in radians per second
        /// </summary>
        public double OrbitalAngularVelocity { get; }

        /// <summary>
        /// The rotational angular velocity of the object, in radians per second
        /// </summary>
        public double RotatationalAngularVelocity { get; }

        /// <summary>
        /// The initial angular offset
        /// </summary>
        public double InitialOrbitalAngularPositionOffset { get; }

        /// <summary>
        /// Gets the current angular position
        /// </summary>
        public double CurrentAngularPosition { get; private set; }
        
        /// <summary>
        /// The current orientation of the body
        /// </summary>
        public double Angle { get; private set; }

        /// <summary>
        /// The composition of the body
        /// </summary>
        public IMaterial Material { get; }

        /// <summary>
        /// The type of the celestial body
        /// </summary>
        public CelestialBodyType BodyType { get; }

        /// <summary>
        /// The satellites of this celestial body.
        /// </summary>
        /// <remarks>
        /// Remember that stars do not have satelites - the COM does
        /// </remarks>
        public CelestialBody[] Satellites { get; }

        /// <summary>
        /// Update the position of the body
        /// </summary>
        /// <param name="dt"></param>
        public void UpdateDelta(TimeSpan delta)
        {
            double dp = OrbitalAngularVelocity * (double)delta.Milliseconds / 1000d;
            dp = dp % Constants.TwoPi;

            double a = RotatationalAngularVelocity * (double)delta.Milliseconds / 1000d;
            a = a % Constants.TwoPi;

            CurrentAngularPosition += dp;
            Angle += a;
        }

        public IEnumerable<CelestialBody> GetSelfAndSatellites()
        {
            return (new[] {this}).Union(Satellites);
        }
    }
}