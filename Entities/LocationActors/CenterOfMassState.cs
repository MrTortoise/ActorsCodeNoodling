using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;

namespace Entities.LocationActors
{
    public class CenterOfMassState 
    {
        public string Name { get; }
        public CelestialBody[] Stars { get; }
        public CelestialBody[] Planets { get; }
        public ImmutableDictionary<IActorRef, CelestialBody> Factories { get; set; }

        public CenterOfMassState(string name, CelestialBody[] stars, CelestialBody[] planets, ImmutableDictionary<IActorRef, CelestialBody> factories)
        {
            Name = name;
            Stars = stars;
            Planets = planets;
            Factories = factories;
        }

        public IEnumerable<CelestialBody> UnionCelestialBodies()
        {
            return Stars.Union(Planets.SelectMany(i => i.GetSelfAndSatellites()));
        }
    }
}