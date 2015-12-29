using Akka.Actor;

namespace Entities
{
    public class WaitActor : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new WaitActor());
        }

        public WaitActor()
        {
            Receive<Wait>(msg =>
            {
                Context.LogMessageDebug(msg);
                var sender = Sender;
                Context.System.Scheduler.ScheduleTellOnce(msg.TimeSpan, Self, new CompleteWait(sender), Self);
            });

            Receive<CompleteWait>(msg =>
            {
                Context.LogMessageDebug(msg);
                msg.Sender.Tell(new WaitComplete());
            });
        }

        public class CompleteWait
        {
            public IActorRef Sender { get; private set; }

            public CompleteWait(IActorRef sender)
            {
                Sender = sender;
            }
        }

        public class WaitComplete{}

        public class Wait
        {
            public Wait(int timeSpan)
            {
                TimeSpan = timeSpan;
            }

            public int TimeSpan { get; private set; }
        }
    }
}