using Entities;
using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework.Generated;
using NUnit.Framework;
// <auto-generated>
// This file contains automatically generated tests.
// Do not modify this file manually.
// 
// If the contents of this file becomes outdated, you can delete it.
// For example, if it no longer compiles.
// </auto-generated>

namespace Entities.Model.Locations
{
    public partial class LocationModel
    {

[Test]
[PexGeneratedBy(typeof(LocationModel))]
[ExpectedException(typeof(ArgumentNullException))]
public void CreateLocationThrowsArgumentNullException316()
{
    ITradeable[] iTradeables = new ITradeable[2];
    this.CreateLocation((string)null, (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
[ExpectedException(typeof(ArgumentNullException))]
public void CreateLocationThrowsArgumentNullException164()
{
    ITradeable[] iTradeables = new ITradeable[0];
    this.CreateLocation("", (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
public void CreateLocation164()
{
    ITradeable[] iTradeables = new ITradeable[0];
    this.CreateLocation("\0", (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
public void CreateLocation923()
{
    ITradeable[] iTradeables = new ITradeable[0];
    this.CreateLocation("\0\0", (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
public void CreateLocation192()
{
    ITradeable[] iTradeables = new ITradeable[0];
    this.CreateLocation("\0ĀĀ", (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
public void CreateLocation43()
{
    ITradeable[] iTradeables = new ITradeable[2];
    this.CreateLocation("", (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
[ExpectedException(typeof(ArgumentNullException))]
public void CreateLocationThrowsArgumentNullException866()
{
    this.CreateLocation("Ā", (IEnumerable<ITradeable>)null);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
public void CreateLocation154()
{
    Resource resource;
    resource = new Resource("");
    ITradeable[] iTradeables = new ITradeable[1];
    iTradeables[0] = (ITradeable)resource;
    this.CreateLocation("", (IEnumerable<ITradeable>)iTradeables);
}

[Test]
[PexGeneratedBy(typeof(LocationModel))]
public void CreateLocation647()
{
    Resource resource;
    resource = new Resource("Ā");
    ITradeable[] iTradeables = new ITradeable[5];
    iTradeables[0] = (ITradeable)resource;
    iTradeables[1] = (ITradeable)resource;
    iTradeables[2] = (ITradeable)resource;
    iTradeables[3] = (ITradeable)resource;
    iTradeables[4] = (ITradeable)resource;
    this.CreateLocation("", (IEnumerable<ITradeable>)iTradeables);
}
    }
}
