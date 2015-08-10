using System;
using System.Collections.Generic;
using System.Linq;
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

     [PexMethod(MaxRunsWithoutNewTests = 200)]
        public void AddResourceToSell([PexAssumeUnderTest]Entities.Market target, Market.ResourceForSale[] resourceForSale)
        {
            PexAssume.IsNotNull(resourceForSale);
            // add each item for sale
            foreach (var saleItem in resourceForSale)
            {
                PexAssume.IsNotNull(saleItem);
                PexAssume.IsTrue(resourceForSale.Count(i => Equals(i, saleItem)) == 1);

                var resource = saleItem.Resource;
                var amount = saleItem.Amount;
                var pricePerUnit = saleItem.PricePerUnit;

                var existingItems = target.ItemsForSale;
                target.AddItemForSale(resource, amount, pricePerUnit);
                var newItems = target.ItemsForSale;

                // if item already exists them combine them oterwise set it
                if (existingItems.ContainsKey(saleItem.Resource.Name))
                {
                    var oldItem = existingItems[resource.Name];
                    var newItem = newItems[resource.Name];

                    PexAssert.AreEqual(existingItems.Count, newItems.Count);
                    PexAssert.AreEqual(oldItem.Amount + amount, newItem.Amount);

                    //for now assume price per unit becomes most expensive
                    PexAssert.AreEqual(
                        Math.Max(oldItem.PricePerUnit, newItem.PricePerUnit),
                        Math.Max(pricePerUnit, oldItem.PricePerUnit));
                }
                else
                {
                    PexAssert.AreEqual(existingItems.Count + 1, newItems.Count);
                    var item = newItems[resource.Name];
                    PexAssert.AreEqual(amount, item.Amount);
                    PexAssert.AreEqual(pricePerUnit, item.PricePerUnit);
                }
            }
        }

        public void MakeOfferOnResource(Market target, ITrader trader, ITradeable resource, int quantity, int amount)
        {
            
        }


    }
}
