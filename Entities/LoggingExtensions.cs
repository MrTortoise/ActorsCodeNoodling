using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace Entities
{
    public static class LoggingExtensions
    {
        public static void LogMessageDebug(this IActorContext context, object msg)
        {
            context.GetLogger()
                .Debug("{Actor}:{MessageType}:{@Message}", context.Self.Path, msg.GetType().FullName, msg);
        }
    }
}
