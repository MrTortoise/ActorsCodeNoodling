using System;
using System.Collections.Generic;
using Akka.Util.Internal;

namespace Entities.LocationActors
{
    public class Moon : ICelestialBody, IUpdateDelta, IEquatable<Moon>, ICloneable
    {
        public Moon(MoonType details, CelestialBody bodyData)
        {
            Details = details;
            BodyData = bodyData;
        }

        public CelestialBody BodyData { get; }

        public MoonType Details { get; }

        public void UpdateDelta(TimeSpan delta)
        {
            BodyData.UpdatePostion(delta);
        }

        #region Clone and equality crap

        public object Clone()
        {
            return new Moon((MoonType)Details.Clone(), (CelestialBody)BodyData.Clone());
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Moon) obj);
        }

        public bool Equals(Moon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return BodyData.Equals(other.BodyData) && Details.Equals(other.Details);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (BodyData.GetHashCode()*397) ^ Details.GetHashCode();
            }
        }

        public static bool operator ==(Moon left, Moon right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Moon left, Moon right)
        {
            return !Equals(left, right);
        }
#endregion

        public class MoonType : IResourceComposition, IEquatable<MoonType>, ICloneable
        {
            public MoonType(string name, Dictionary<IResource, double> composition)
            {
                MaterialComposition = composition;
                Name = name;
            }

            public string Name { get; }

            public Dictionary<IResource, double> MaterialComposition { get; }

            #region clone + equality crap

            public object Clone()
            {
                var comp = new Dictionary<IResource, double>(MaterialComposition.Count);
                MaterialComposition.ForEach(i => comp.Add(i.Key, i.Value));
                return new MoonType(Name,comp);
            }

            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <returns>
            /// true if the specified object  is equal to the current object; otherwise, false.
            /// </returns>
            /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MoonType) obj);
            }

            public bool Equals(MoonType other)
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

            public static bool operator ==(MoonType left, MoonType right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(MoonType left, MoonType right)
            {
                return !Equals(left, right);
            }

#endregion
        }


    }
}