using System;
using System.Collections.Generic;
using Akka.Actor;

namespace Entities.LocationActors
{
    /// <summary>
    /// A planet orbiting around the COM of a system
    /// </summary>
    public class Planet :  ICelestialBody, IUpdateDelta
    {
        public Planet(PlanetType details, CelestialBody bodyData, Moon[] moons)
        {
            Details = details;
            BodyData = bodyData;
            Moons = moons;
        }

        /// <summary>
        /// The composition details of the planet
        /// </summary>
        public PlanetType Details { get; }

        /// <summary>
        /// The data for the body component.
        /// </summary>
        public CelestialBody BodyData { get; }

        public Moon[] Moons { get; }

        public void UpdateDelta(TimeSpan delta)
        {
            BodyData.UpdatePostion(delta);
            foreach (var moon in Moons)
            {
                moon.UpdateDelta(delta);
            }
        }

        /// <summary>
        /// Represents a compositional configuration of a planet.
        /// </summary>
        public class PlanetType : IResourceComposition, IEquatable<PlanetType>
        {
            public PlanetType(string name, Dictionary<IResource, double> materialComposition)
            {
                Name = name;
                MaterialComposition = materialComposition;
            }

            public string Name { get; }

            /// <summary>
            /// The material composition of the planet.
            /// </summary>
            public Dictionary<IResource, double> MaterialComposition { get; }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((PlanetType) obj);
            }

            public bool Equals(PlanetType other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name) && MaterialComposition.Equals(other.MaterialComposition);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Name.GetHashCode()*397) ^ MaterialComposition.GetHashCode();
                }
            }

            public static bool operator ==(PlanetType left, PlanetType right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(PlanetType left, PlanetType right)
            {
                return !Equals(left, right);
            }
        }
    }
}