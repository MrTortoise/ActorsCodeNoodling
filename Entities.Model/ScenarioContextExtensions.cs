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
       public static string GetField(this Table table, string fieldName)
       {
          return table.Rows.Single(r => r["Field"] == fieldName)["Value"];
       }

    }
}
