using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;

namespace Entities.Model
{
   public class ScenarioContextState
   {
      public ScenarioContextState()
      {
         Traders = new Dictionary<string, TestActorRef<Trader>>();
         ExchangeContractActors = new Dictionary<string, TestActorRef<ExchangeContract>>();
      }

      public TestKit TestKit { get; set; }
      public TestActorRef<ResourceManager> ResourceManager { get; set; }
      public Dictionary<string,TestActorRef<Trader>> Traders { get; private set; }
      public Dictionary<string,TestActorRef<ExchangeContract>> ExchangeContractActors { get; private set; }

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
