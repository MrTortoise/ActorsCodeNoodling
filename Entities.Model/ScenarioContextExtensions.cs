using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    public static class ScenarioContextExtensions
    {
        public static IActorRef GetResourceManagerActorRef(this ScenarioContext context)
        {
            return (IActorRef)context[Constants.ResourceManager];
        }

        public static ActorSystem GetActorSystem(this ScenarioContext context)
        {
            return (ActorSystem)context[Constants.TestActorSystemName];
        }

       public static string GetField(this Table table, string fieldName)
       {
          return table.Rows.Single(r => r["Field"] == fieldName)["Value"];
       }

    }
}
