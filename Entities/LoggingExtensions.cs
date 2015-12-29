using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.Serialization;

namespace Entities
{
    public static class LoggingExtensions
    {
        public static void LogMessageDebug(this IActorContext context, object msg, string extra = "")
        {
            context.GetLogger()
                .Debug("{0}:{1}:{2}:{3}", context.Self.Path, msg.GetType().FullName, msg.ToString(),
                    extra);
        }
    }
}
