using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch.SysMsg;
using Akka.Event;
using Akka.TestKit.Xunit2;
using Akka.Util.Internal;
using Xunit;

namespace Entities.Model.XUnitl
{

    public class TerminatorSpec : TestKit
    {
        private static string Config = @"akka { loglevel=INFO,  loggers=[""Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog""]}";

        public TerminatorSpec()
            :base(Config, "terminatorSpec")
        {
            
        }

        [Fact]
        public void Create_Terminator_children_when_Terminating_Should_Occur_in_correct_order()
        {
            var a = Sys.ActorOf(Props.Create(() => new Testinator(TestActor)));

            var p = CreateTestProbe();
            a.Tell(new Terminator.GetChildren(p), p);

            var msg = p.ExpectMsg<Terminator.Children>();
            Assert.True(msg.Kids.Count() == 20);
            
            Sys.Stop(p);
            for (int i = 19; i >= 0; i--)
            {
                var m = ExpectMsg<string>();
                Console.WriteLine(m);
            }
        }

        
    }

    public class Testinator : Terminator
    {
        private readonly IActorRef _testActor;

        public Testinator(IActorRef testActor)
        {
            _testActor = testActor;
        }

        protected override void PreStart()
        {
            for (int i = 0; i < 20; i++)
            {
                Context.ActorOf(Props.Create(() => new TestWorker(_testActor)));
            }

            Order = refs => refs.Reverse();
        }
    }

    public class TestWorker : ReceiveActor
    {
        private readonly IActorRef _testActor;

        public TestWorker(IActorRef testActor)
        {
            _testActor = testActor;

            Receive<object>(msg =>
            {
                Sender.Tell(Self.Path.Name);
            });
        }

        
    }

    class Master : ReceiveActor
    {
        private readonly IActorRef _terminator;
        private IEnumerable<IActorRef> _children;

        public Master(IActorRef terminator)
        {
            _terminator = terminator;

            Become(Waiting);
        }

        private void Waiting()
        {
            Receive<Terminator.Children>(msg =>
            {
                _children = msg.Kids;
                Become(Initialised);
            });
        }

        private void Initialised()
        {
          
        }

        protected override void PreStart()
        {
            _terminator.Tell(new Terminator.GetChildren(Self));
        }
    }

    public class Worker : ReceiveActor
    {
        public Worker()
        {
            Receive<object>(msg =>
            {
                Context.GetLogger().Debug("{SelfName} got a message: {message}", Self.Path.Name, msg);
            });
        }

        protected override void PreStart()
        {
            Context.GetLogger().Debug("{SelfName} ia running",Self.Path.Name);
        }

        protected override void PostStop()
        {
            Context.GetLogger().Debug("{SelfName} has stopped", Self.Path.Name);
        }
    }


    public abstract class Terminator : ReceiveActor
    {
       protected readonly IEnumerable<IActorRef> _children;
       private IActorRef _owner;
       protected TimeSpan _stopTimeout;

       protected Terminator()
       {
           Order = refs => new List<IActorRef>(refs);

           Become(Waiting);
       }

       public Func<IEnumerable<IActorRef>, IEnumerable<IActorRef>> Order { get; set; }

       private void Waiting()
        {
            Receive<GetChildren>(msg =>
            {
                Context.Watch(msg.Self);
                var children = Context.GetChildren();
                msg.Self.Tell(new Children(children));
                Become(ChildrenGiven);
                _owner = msg.Self;
            });
        }

        private void ChildrenGiven()
        {
            Receive<GetChildren>(msg =>
            {
                if (Equals(msg.Self, _owner))
                {
                    msg.Self.Tell(new Children(Context.GetChildren()));
                }
            });

            Receive<Terminated>(msg =>
            {
                KillKids(Order(_children), Sender);
            });
        }

       private void KillKids(IEnumerable<IActorRef> kids, IActorRef sender)
       {
           if (kids != null)
           {
               kids.ForEach(c => c.GracefulStop(_stopTimeout));
           }

           sender.Tell(new AllDead());
       }

       private class AllDead
       {
       }

       public class GetChildren
        {
            public IActorRef Self { get; set; }

            public GetChildren(IActorRef self)
            {
                Self = self;
            }
        }

       public class Children
       {
           public Children(IEnumerable<IActorRef> kids)
           {
               Kids = kids;
           }

           public IEnumerable<IActorRef> Kids { get; private set; }
       }
    }
}
