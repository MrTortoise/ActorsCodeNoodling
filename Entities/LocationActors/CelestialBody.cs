using System;

namespace Entities.LocationActors
{
    /// <summary>
    /// THe core data for any celestial body
    /// </summary>
    /// <remarks>Nothing clever assume all on one axis - and everything on same azis</remarks>
    public class CelestialBody 
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
        public CelestialBody(string name, double radius, double orbitDistance, double orbitalAngularVelocity, double rotatationalAngularVelocity, double initialOrbitalAngularPositionOffset, double currentAngularPosition)
        {
            Name = name;
            Radius = radius;
            OrbitDistance = orbitDistance;
            OrbitalAngularVelocity = orbitalAngularVelocity;
            RotatationalAngularVelocity = rotatationalAngularVelocity;
            InitialOrbitalAngularPositionOffset = initialOrbitalAngularPositionOffset;
            CurrentAngularPosition = currentAngularPosition;
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
        /// Update the position of the body
        /// </summary>
        /// <param name="dt"></param>
        public void UpdatePostion(TimeSpan dt)
        {
            double dp =  OrbitalAngularVelocity*(double) dt.Milliseconds/1000d;
            dp = dp%Constants.TwoPi;

            double a = RotatationalAngularVelocity*(double) dt.Milliseconds/1000d;
            a = a%Constants.TwoPi;

            CurrentAngularPosition += dp;
            Angle += a;
        }
    }
}