using System;

namespace Entities.LocationActors
{
    /// <summary>
    /// Represents a star orbiting a COM
    /// </summary>
    public class Star : ICelestialBody, IUpdateDelta
    {
        public Star(StarType details, CelestialBody bodyData)
        {
            Details = details;
            BodyData = bodyData;
        }

        /// <summary>
        /// Type data for the star
        /// </summary>
        public StarType Details { get; }

        /// <summary>
        /// The body data for the celestial body.
        /// </summary>
        public CelestialBody BodyData { get; }

        /// <summary>
        /// Refuel rate from the star
        /// </summary>
        public void UpdateDelta(TimeSpan delta)
        {
            BodyData.UpdatePostion(delta);
        }

        /// <summary>
        /// Encapsulates data to be repeated for multiple Stars of different types
        /// </summary>
        public class StarType
        {
            public string Name { get; }

            public StarType(string name, double fuelRate)
            {
                Name = name;
                FuelRate = fuelRate;
            }

            /// <summary>
            /// Refuel rate from the star
            /// </summary>
            public double FuelRate { get; }
        }
    }
}