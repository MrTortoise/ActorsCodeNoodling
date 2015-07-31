using System;
using System.Reflection;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Entities.Model.Market
{
    [PexClass(typeof(Entities.Market))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class MarketModel
    {
        /// <summary>
        /// When a market is created at a location is sees what that location produces and adds those things to its board.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        [PexMethod]
        public void CreateMarket(string name, Entities.Location location)
        {
            var market = new Entities.Market(name);

            PexAssert.AreEqual(name, market.Name);
        }


    }
}
