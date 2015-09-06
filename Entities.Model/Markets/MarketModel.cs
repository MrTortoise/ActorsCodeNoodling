//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Akka.Actor;
//using Microsoft.Pex.Framework;
//using Microsoft.Pex.Framework.Validation;
//using NUnit.Framework;

//namespace Entities.Model.Markets
//{
//    [PexClass(typeof(Entities.Market))]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
//    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
//    [TestFixture]
//    public partial class MarketModel
//    {
//        /// <summary>
//        /// When a market is created at a location is sees what that location produces and adds those things to its board.
//        /// </summary>
//        /// <param name="name"></param>
//        /// <param name="location"></param>
//        [PexMethod]
//        public void CreateMarket(string name, Entities.Location location)
//        {
//            var market = new Entities.Market(name);
//            PexAssert.AreEqual(name, market.Name);
//        }

//        /// <summary>
//        /// When selling a resource it is necessary for a trader to specify the amount of what resource for sale, and what resource and quantity desired in return.
//        /// </summary>
//        /// <remarks>This resources for barter could be items essentially exchanged in currency, but half the entertainment will be differing values of currency in different areas.
//        /// IE if you are a mint, you are still in a barter system even in our economy.</remarks>
//        /// <param name="target"></param>
//        /// <param name="resourceForSale"></param>
//     [PexMethod(MaxRunsWithoutNewTests = 200)]
//        public void AddInvitationToTreatAndVerifyStatusUnaccepted([PexAssumeUnderTest]Entities.Market target, InvitationToTreat invitation)
//        {
//            PexAssume.IsNotNull(invitation);

//        //    target.AddInvitationToTreat(invitation);

//            //PexAssume.IsNotNull(resourceForSale);
//            //// add each item for sale
//            //foreach (var saleItem in resourceForSale)
//            //{
//            //    PexAssume.IsNotNull(saleItem);
//            //    PexAssume.IsTrue(resourceForSale.Count(i => Equals(i, saleItem)) == 1);

//            //    var resource = saleItem.Resource;
//            //    var amount = saleItem.Amount;
//            //    var pricePerUnit = saleItem.PricePerUnit;

//            //    var existingItems = target.ItemsForSale;
//            //    target.AddItemForSale(resource, amount, pricePerUnit);
//            //    var newItems = target.ItemsForSale;

//            //    // if item already exists them combine them oterwise set it
//            //    if (existingItems.ContainsKey(saleItem.Resource.Name))
//            //    {
//            //        var oldItem = existingItems[resource.Name];
//            //        var newItem = newItems[resource.Name];

//            //        PexAssert.AreEqual(existingItems.Count, newItems.Count);
//            //        PexAssert.AreEqual(oldItem.Amount + amount, newItem.Amount);

//            //        //for now assume price per unit becomes most expensive
//            //        PexAssert.AreEqual(
//            //            Math.Max(oldItem.PricePerUnit, newItem.PricePerUnit),
//            //            Math.Max(pricePerUnit, oldItem.PricePerUnit));
//            //    }
//            //    else
//            //    {
//            //        PexAssert.AreEqual(existingItems.Count + 1, newItems.Count);
//            //        var item = newItems[resource.Name];
//            //        PexAssert.AreEqual(amount, item.Amount);
//            //        PexAssert.AreEqual(pricePerUnit, item.PricePerUnit);
//            //    }
//            //}
//        }

//        /// <summary>
//        /// Traders can make offers on resources that are up for sale, the status before accepted and rejected is pending.
//        /// </summary>
//        /// <param name="target"></param>
//        /// <param name="trader"></param>
//        /// <param name="resource"></param>
//        /// <param name="quantity"></param>
//        /// <param name="amount"></param>
//        public void MakeOfferOnResourceAndCheckStatusPending([PexAssumeUnderTest]Market target, ITrader trader, IResource resource, int quantity, int amount)
//        {
//           // target.MakeOfferOnResource
//        }


//    }

//    public class InvitationToTreat
//    {
//    }
//}
