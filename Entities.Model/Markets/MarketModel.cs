using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace Entities.Model.Markets
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

        [PexMethod]
        public void AddResourceToSell([PexAssumeUnderTest]Entities.Market target, ITradeable resource, int amount, int pricePerUnit)
        {
            var existingItems = target.ItemsForSale;
            target.AddItemForSale(resource, amount, pricePerUnit);

            var newItems = target.ItemsForSale;

            if (existingItems.ContainsKey(resource.Name))
            {
                PexAssert.AreEqual(existingItems.Count, newItems.Count);

                var oldItem = existingItems[resource.Name];
                var newItem = newItems[resource.Name];
                PexAssert.AreEqual(oldItem.Amount + amount, newItem.Amount);

                //for now assume price per unit becomes most expensive
                PexAssert.AreEqual(Math.Max(oldItem.PricePerUnit, newItem.PricePerUnit),
                    Math.Max(pricePerUnit, oldItem.PricePerUnit));
            }
            else
            {
                PexAssert.AreEqual(existingItems.Count + 1, newItems.Count);
                var item = newItems[resource.Name];
                PexAssert.AreEqual(amount,item.Amount);
                PexAssert.AreEqual(pricePerUnit,item.PricePerUnit);
            }

            
            
        }


    }
}
