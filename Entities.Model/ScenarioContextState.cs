﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;

namespace Entities.Model
{
   /// <summary>
   /// Holds state for the test scenario, this is shared between steps classes within a test scenario execution through constructor injection
   /// </summary>
   public class ScenarioContextState
   {
      /// <summary>
      /// Creates an instance of <see cref="ScenarioContextState"/> class.
      /// </summary>
      public ScenarioContextState()
      {
         Traders = new Dictionary<string, TestActorRef<Trader>>();
         ExchangeContractActors = new Dictionary<string, TestActorRef<ExchangeContract>>();
      }

      /// <summary>
      /// A reference to the <see cref="TestKit"/> in order to get at all the Akka.Test goodness
      /// </summary>
      public TestKit TestKit { get; set; }
      public TestActorRef<ResourceManager> ResourceManager { get; set; }
      /// <summary>
      /// Dictionary of <see cref="Trader"/> by name 
      /// </summary>
      public Dictionary<string,TestActorRef<Trader>> Traders { get; private set; }
      /// <summary>
      /// Dictionary of <see cref="ExchangeContract"/> by name
      /// </summary>
      public Dictionary<string,TestActorRef<ExchangeContract>> ExchangeContractActors { get; private set; }

      /// <summary>
      /// Given a resource name will return a <see cref="Resource"/> from <see cref="ResourceManager"/>
      /// </summary>
      /// <param name="resourceName">The value of a <see cref="Resource.Name"/></param>
      /// <exception cref="InvalidOperationException">When <see cref="ResourceManager"/> ha snot been initialised in a previous step.</exception>
      /// <returns>The <see cref="Resource"/></returns>
      public Resource GetResourceFromName(string resourceName)
      {
         if (ReferenceEquals(ResourceManager, null))
         {
            throw new InvalidOperationException("Resource Manager not initialised");
         }

         var resourceQuery = ResourceManager.Ask<Resource>(new ResourceManager.GetResource(resourceName));
         resourceQuery.Wait(TimeSpan.FromMilliseconds(50));
         var resource = resourceQuery.Result;
         return resource;   
      }
   }
}
