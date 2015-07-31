using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Entities.Model.Locations
{
    [PexClass(typeof(Entities.Location))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class LocationModel
    {
        [PexMethod]
        public void CreateLocation(string name, IEnumerable<ITradeable> resources)
        {
            var loc = new Entities.Location(name, resources);
            PexAssert.AreEqual(name, loc.Name);
            foreach (var tradeable in resources)
            {
                PexAssert.AreEqual(tradeable != null, loc.Resources.Contains(tradeable));
            }
        }

    }
}
