using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities.Observation
{
    //ToDo both these classes need to be more targeted.

    /// <summary>
    /// Instructs an <see cref="IActorRef"/> to stop sending messages to the sender of <see cref="UnObserve"/>
    /// </summary>
    public class UnObserve
    {
    }

    /// <summary>
    /// Instructs an <see cref="IActorRef"/> to send state update resports
    /// </summary>
    public class Observe
    {
    }
}
