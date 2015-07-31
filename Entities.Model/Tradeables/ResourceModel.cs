using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Pex.Engine.TestGeneration;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Entities.Model.Tradeables
{
    [PexClass(typeof(Resource))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class ResourceModel
    {
        [PexMethod]
        public void CreateAResource(string name)
        {
            var resource = new Resource(name);
            PexAssert.AreEqual(name, resource.Name);
        }

    }
}
