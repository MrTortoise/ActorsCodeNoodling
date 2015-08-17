using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TechTalk.SpecFlow;

namespace Entities.Model
{
   public static class SpecflowHelpers
   {
      public static Resource GetResourceFromName(string resourceName)
      {
         var resourceManager = ScenarioContext.Current.GetResourceManagerActorRef();
         var resourceQuery = resourceManager.Ask<Resource>(new ResourceManager.GetResource(resourceName));
         resourceQuery.Wait(TimeSpan.FromMilliseconds(50));
         var resource = resourceQuery.Result;
         return resource;
      }


      /// <summary>
      /// Returns a trader actor assuming that the name is in the cotext in the format of "trader_[name]"
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
      public static IActorRef GetTraderActorFromName(string name)
      {
         return (IActorRef)ScenarioContext.Current["trader_" + name];
      }

      public static IActorRef GetExchangeContractActor(string exchangeContractName)
      {
         return (IActorRef) ScenarioContext.Current["ExchangeContract_" + exchangeContractName];
      }
   }
}
