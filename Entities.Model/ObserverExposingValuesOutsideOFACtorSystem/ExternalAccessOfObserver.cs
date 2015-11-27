using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.NUnit;
using Entities.Observation;
using NUnit.Framework;
using Serilog;

namespace Entities.Model.ObserverExposingValuesOutsideOFACtorSystem
{
    [TestFixture]
    public class ExternalAccessOfObserver
    {
        private TestKit _testkit;

        [SetUp]
        public void Setup()
        {
            var logger = new LoggerConfiguration()
             .WriteTo.ColoredConsole()
             .MinimumLevel.Debug()
             .CreateLogger();
            Serilog.Log.Logger = logger;

            var config = "akka { loglevel=DEBUG,  loggers=[\"Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog\"]}";

            _testkit = new TestKit(config, "testSystem");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>The goal of this is to be able to have an actor system trigger a delegate that will set a value on something (based upon a registration).
        /// </remarks>
        [TestCase()]
        public void SignUpToAnyMessageTest()
        {
            bool setToTrue = false;
            Action<EventSourceActor.Hi> action = hi => setToTrue = true;

            var eventSource = _testkit.Sys.ActorOf(Props.Create(() => new EventSourceActor()),"eventSource");
            var observer = _testkit.Sys.ActorOf(Props.Create(() => new ObserverActor<EventSourceActor.Hi>()),"HiObserver");

            Thread.Sleep(100);

            observer.Tell(new ObserverActor<EventSourceActor.Hi>.Subscribe(action, eventSource));
            Thread.Sleep(100);

            eventSource.Tell(new EventSourceActor.TellObserversHi());
            Thread.Sleep(100);
          
            Assert.IsTrue(setToTrue);
        }

        [TestCase()]
        public void SignUpToAnyMessageTestThenUnsub()
        {
            bool setToTrue = false;
            Action<EventSourceActor.Hi> action = hi => setToTrue = true;
            var eventSource = _testkit.Sys.ActorOf(Props.Create(() => new EventSourceActor()));

            var observer =
                _testkit.Sys.ActorOf(
                    Props.Create(() => new ObserverActor<EventSourceActor.Hi>()));
            Thread.Sleep(10);

            observer.Tell(new ObserverActor<EventSourceActor.Hi>.Subscribe(action, eventSource));
            Thread.Sleep(10);

            eventSource.Tell(new EventSourceActor.TellObserversHi());
            Thread.Sleep(10);
            Assert.IsTrue(setToTrue);

            setToTrue = false;
            observer.Tell(new ObserverActor<EventSourceActor.Hi>.UnSubscribe());
            Thread.Sleep(10);

            eventSource.Tell(new EventSourceActor.TellObserversHi());
            Thread.Sleep(10);

            Assert.IsFalse(setToTrue);
        }

        [TestCase()]
        public void SignUpToOneMessageCheckDontReceiveOtherTest()
        {
            bool setToTrue = false;
            Action<EventSourceActor.Hi> action = hi => setToTrue = true;
            var eventSource = _testkit.Sys.ActorOf(Props.Create(() => new EventSourceActor()));

            var observer =
                _testkit.Sys.ActorOf(
                    Props.Create(() => new ObserverActor<EventSourceActor.Hi>()));
            Thread.Sleep(1);

            observer.Tell(new ObserverActor<EventSourceActor.Hi>.Subscribe(action, eventSource));
            Thread.Sleep(1);

            eventSource.Tell(new EventSourceActor.TellObserversHi2());
            Thread.Sleep(1);

            Assert.IsFalse(setToTrue);
        }

        [TestCase()]
        public void SignUpToAMessageCountNumberOfTimesRaised()
        {
            int timesCalled = 0;
            Action<EventSourceActor.Hi> action = hi => timesCalled++;

            var eventSource = _testkit.Sys.ActorOf(Props.Create(() => new EventSourceActor()), "eventSource");
            var observer = _testkit.Sys.ActorOf(Props.Create(() => new ObserverActor<EventSourceActor.Hi>()), "HiObserver");

            Thread.Sleep(100);
            Assert.That(timesCalled==0);
            observer.Tell(new ObserverActor<EventSourceActor.Hi>.Subscribe(action, eventSource));
            Thread.Sleep(100);

            eventSource.Tell(new EventSourceActor.TellObserversHi());
            Thread.Sleep(100);
            Assert.That(timesCalled == 1);

            eventSource.Tell(new EventSourceActor.TellObserversHi());
            Thread.Sleep(100);
            Assert.That(timesCalled == 2);

            eventSource.Tell(new EventSourceActor.TellObserversHi());
            Thread.Sleep(100);
            Assert.That(timesCalled == 3);
        }
    }

    /// <summary>
    /// Observes a subject actor sending messages of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObserverActor<T> : ReceiveActor
    {
        private Subscribe _subscription;

        public ObserverActor()
        {
            Become(Unsubscribed);
        }

        private void Unsubscribed()
        {
            Receive<Subscribe>(msg =>
            {
                Context.LogMessageDebug(msg);
                _subscription = msg;
                _subscription.Subject.Tell(new Observe());
                Become(Subscribed);
            });
        }

        private void Subscribed()
        {
            Receive<UnSubscribe>(msg =>
            {
                Context.LogMessageDebug(msg);
                _subscription.Subject.Tell(new UnObserve());
                _subscription = null;
                Become(Unsubscribed);
            });

            Receive<T>(msg =>
            {
                _subscription.Callback(msg);
            });
        }

        #region Messages
        /// <summary>
        /// Unsubscribe from the observer
        /// </summary>
        public class UnSubscribe
        {
        }

        /// <summary>
        /// Subscribe for events from the subject and setup the callback to execute on receipt of the message T
        /// </summary>
        public class Subscribe
        {
            public Subscribe(Action<T> callback, IActorRef subject)
            {
                Callback = callback;
                Subject = subject;
            }

            /// <summary>
            /// The subject actor of the observer
            /// </summary>
            public IActorRef Subject { get; }

            /// <summary>
            /// The callback that will be executed upon observation of the given message
            /// </summary>
            public Action<T> Callback { get; }
        }

        #endregion
    }

    public class EventSourceActor : ReceiveActor
    {
        readonly List<IActorRef> _observers = new List<IActorRef>();

        public EventSourceActor()
        {
            Receive<Observe>(msg =>
            {
                Context.LogMessageDebug(msg);
                _observers.Add(Sender);
            });

            Receive<UnObserve>(msg =>
            {
                Context.LogMessageDebug(msg);
                _observers.Remove(Sender);
            });

            Receive<TellObserversHi>(msg =>
            {
                Context.LogMessageDebug(msg);
                foreach (var actorRef in _observers)
                {
                    actorRef.Tell(new Hi());
                }
            });

            Receive<TellObserversHi2>(msg =>
            {
                Context.LogMessageDebug(msg);
                foreach (var actorRef in _observers)
                {
                    actorRef.Tell(new Hi2());
                }
            });
        }

        public class Hi
        {
        }

        public class Hi2
        {
        }

        public class TellObserversHi
        {
        }

        public class TellObserversHi2
        {
        }
    }
}
