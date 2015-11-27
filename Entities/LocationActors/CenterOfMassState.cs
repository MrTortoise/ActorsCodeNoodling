using System;

namespace Entities.LocationActors
{
    public class CenterOfMassState 
    {
        public string Name { get; }
        public Star[] Stars { get; }
        public Planet[] Planets { get; }

        public CenterOfMassState(string name, Star[] stars, Planet[] planets)
        {
            Name = name;
            Stars = stars;
            Planets = planets;
        }
    }
}