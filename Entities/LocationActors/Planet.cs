using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;

namespace Entities.LocationActors
{
    /// <summary>
    /// A planet orbiting around the COM of a system
    /// </summary>
    public class Planet :  ICelestialBody, IUpdateDelta
    {
        public Planet(IMaterial material, CelestialBody bodyData, Moon[] moons)
        {
            Material = material;
            BodyData = bodyData;
            Moons = moons;
        }

        /// <summary>
        /// The data for the body component.
        /// </summary>
        public CelestialBody BodyData { get; }

        public IMaterial Material { get; }

        // ReSharper disable once CoVariantArrayConversion - it is read only
        public Moon[] Moons { get; }

        public void UpdateDelta(TimeSpan delta)
        {
            BodyData.UpdatePostion(delta);
            foreach (var moon in Moons)
            {
                moon.UpdateDelta(delta);
            }
        }
    }
}