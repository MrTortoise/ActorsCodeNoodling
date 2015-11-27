using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Util.Internal;

namespace Entities.LocationActors
{
    public class Moon : ICelestialBody, IUpdateDelta
    {
        public Moon(IMaterial material, CelestialBody bodyData)
        {
            Material = material;
            BodyData = bodyData;
        }

        public IMaterial Material { get; }

        public CelestialBody BodyData { get; }
   

        public void UpdateDelta(TimeSpan delta)
        {
            BodyData.UpdatePostion(delta);
        }
    }
}