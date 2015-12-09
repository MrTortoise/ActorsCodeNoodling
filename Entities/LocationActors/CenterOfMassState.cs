using System;
using System.Collections.Generic;
using Akka.Actor;

namespace Entities.LocationActors
{
    public class CenterOfMassState 
    {
        public string Name { get; }
        public CelestialBody[] Stars { get; }
        public CelestialBody[] Planets { get; }
        public Dictionary<CelestialBody, IActorRef> Factories { get; set; }

        public CenterOfMassState(string name, CelestialBody[] stars, CelestialBody[] planets, Dictionary<CelestialBody, IActorRef> factories)
        {
            Name = name;
            Stars = stars;
            Planets = planets;
            Factories = factories;
        }
    }
}