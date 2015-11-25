using Akka.Actor;

namespace Entities.Observation
{
    /// <summary>
    /// Instructs an <see cref="IActorRef"/> to stop sending messages to the sender of <see cref="UnObserve"/>
    /// </summary>
    public class UnObserve
    {
    }
}