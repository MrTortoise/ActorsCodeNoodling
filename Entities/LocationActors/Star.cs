using System;

namespace Entities.LocationActors
{
    /// <summary>
    /// Represents a star orbiting a COM
    /// </summary>
    public class Star : ICelestialBody, IUpdateDelta
    {
        public Star(IMaterial material, CelestialBody bodyData)
        {
            Material = material;
            BodyData = bodyData;
        }

        /// <summary>
        /// The body data for the celestial body.
        /// </summary>
        public CelestialBody BodyData { get; }

        public IMaterial Material { get; }

        /// <summary>
        /// Refuel rate from the star
        /// </summary>
        public void UpdateDelta(TimeSpan delta)
        {
            BodyData.UpdatePostion(delta);
        }
    }
}