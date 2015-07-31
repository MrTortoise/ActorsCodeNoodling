using Microsoft.Pex.Framework.Exceptions;
using Microsoft.Pex.Framework.Generated;
using NUnit.Framework;
using Microsoft.Pex.Framework;
using Entities;
// <auto-generated>
// This file contains automatically generated tests.
// Do not modify this file manually.
// 
// If the contents of this file becomes outdated, you can delete it.
// For example, if it no longer compiles.
// </auto-generated>
using System;

namespace Entities.Model.Markets
{
    public partial class MarketModel
    {

[Test]
[PexGeneratedBy(typeof(MarketModel))]
public void AddResourceToSell157()
{
    Market market;
    market = new Market("");
    Market.SaleItem[] saleItems = new Market.SaleItem[0];
    this.AddResourceToSell(market, saleItems);
    PexAssert.IsNotNull((object)market);
    PexAssert.AreEqual<string>("", market.Name);
    PexAssert.IsNotNull(market.ItemsForSale);
    PexAssert.AreEqual<int>(0, market.ItemsForSale.Count);
}

[Test]
[PexGeneratedBy(typeof(MarketModel))]
public void AddResourceToSell16()
{
    Market market;
    Resource resource;
    Market.SaleItem saleItem;
    market = new Market("Ā");
    resource = new Resource("Ā");
    saleItem = new Market.SaleItem((ITradeable)resource, 0, 0);
    Market.SaleItem[] saleItems = new Market.SaleItem[1];
    saleItems[0] = saleItem;
    this.AddResourceToSell(market, saleItems);
    PexAssert.IsNotNull((object)market);
    PexAssert.AreEqual<string>("Ā", market.Name);
    PexAssert.IsNotNull(market.ItemsForSale);
    PexAssert.AreEqual<int>(1, market.ItemsForSale.Count);
}

[Test]
[PexGeneratedBy(typeof(MarketModel))]
public void AddResourceToSell971()
{
    Market market;
    Resource resource;
    Market.SaleItem saleItem;
    Resource resource1;
    Market.SaleItem saleItem1;
    market = new Market("Ā");
    resource = new Resource("耀");
    saleItem = new Market.SaleItem((ITradeable)resource, 0, 0);
    resource1 = new Resource("耀");
    saleItem1 = new Market.SaleItem((ITradeable)resource1, 0, 1);
    Market.SaleItem[] saleItems = new Market.SaleItem[2];
    saleItems[0] = saleItem;
    saleItems[1] = saleItem1;
    this.AddResourceToSell(market, saleItems);
    PexAssert.IsNotNull((object)market);
    PexAssert.AreEqual<string>("Ā", market.Name);
    PexAssert.IsNotNull(market.ItemsForSale);
    PexAssert.AreEqual<int>(1, market.ItemsForSale.Count);
}

[Test]
[PexGeneratedBy(typeof(MarketModel))]
public void AddResourceToSell97101()
{
    Market market;
    Resource resource;
    Market.SaleItem saleItem;
    Resource resource1;
    Market.SaleItem saleItem1;
    market = new Market("Ā");
    resource = new Resource("Ā");
    saleItem = new Market.SaleItem((ITradeable)resource, 0, 0);
    resource1 = new Resource("Ā");
    saleItem1 = new Market.SaleItem((ITradeable)resource1, 1, 0);
    Market.SaleItem[] saleItems = new Market.SaleItem[2];
    saleItems[0] = saleItem;
    saleItems[1] = saleItem1;
    this.AddResourceToSell(market, saleItems);
    PexAssert.IsNotNull((object)market);
    PexAssert.AreEqual<string>("Ā", market.Name);
    PexAssert.IsNotNull(market.ItemsForSale);
    PexAssert.AreEqual<int>(1, market.ItemsForSale.Count);
}

[Test]
[PexGeneratedBy(typeof(MarketModel))]
public void AddResourceToSell161()
{
    Market market;
    Resource resource;
    Market.SaleItem saleItem;
    Resource resource1;
    Market.SaleItem saleItem1;
    market = new Market("Ā");
    resource = new Resource("Ā");
    saleItem = new Market.SaleItem((ITradeable)resource, 0, 0);
    resource1 = new Resource("");
    saleItem1 = new Market.SaleItem((ITradeable)resource1, 0, 0);
    Market.SaleItem[] saleItems = new Market.SaleItem[2];
    saleItems[0] = saleItem;
    saleItems[1] = saleItem1;
    this.AddResourceToSell(market, saleItems);
    PexAssert.IsNotNull((object)market);
    PexAssert.AreEqual<string>("Ā", market.Name);
    PexAssert.IsNotNull(market.ItemsForSale);
    PexAssert.AreEqual<int>(2, market.ItemsForSale.Count);
}

[Test]
[PexGeneratedBy(typeof(MarketModel))]
public void AddResourceToSell16101()
{
    Market market;
    Resource resource;
    Market.SaleItem saleItem;
    Resource resource1;
    Market.SaleItem saleItem1;
    market = new Market("Ā");
    resource = new Resource("Ā");
    saleItem = new Market.SaleItem((ITradeable)resource, 0, 0);
    resource1 = new Resource("Ā\0");
    saleItem1 = new Market.SaleItem((ITradeable)resource1, 0, 0);
    Market.SaleItem[] saleItems = new Market.SaleItem[2];
    saleItems[0] = saleItem;
    saleItems[1] = saleItem1;
    this.AddResourceToSell(market, saleItems);
    PexAssert.IsNotNull((object)market);
    PexAssert.AreEqual<string>("Ā", market.Name);
    PexAssert.IsNotNull(market.ItemsForSale);
    PexAssert.AreEqual<int>(2, market.ItemsForSale.Count);
}
    }
}
