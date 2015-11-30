using System;

namespace Entities.LocationActors
{
    public class CenterOfMassState 
    {
        public string Name { get; }
        public CelestialBody[] Stars { get; }
        public CelestialBody[] Planets { get; }

        public CenterOfMassState(string name, CelestialBody[] stars, CelestialBody[] planets)
        {
            Name = name;
            Stars = stars;
            Planets = planets;
        }
    }
}