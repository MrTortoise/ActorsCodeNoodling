using System;
using System.Threading;
using Akka.Actor;
using TechTalk.SpecFlow;
using System.Collections.Immutable;
using System.Linq;

namespace Entities.Model.Heartbeats
{
    [Binding]
    public class HeartbeatSteps
    {
        private readonly ScenarioContextState _state;

        public HeartbeatSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I have created the HeartBeat actor")]
        public void GivenIHaveCreatedAHeartBeatActor()
        {
            var heartBeatActor = _state.TestKit.Sys.ActorOf(HeartBeatActor.CreateProps(_state.TimeProducer), HeartBeatActor.Name);
            _state.Actors.Add(HeartBeatActor.Name, heartBeatActor);
        }
        
        [Given(@"I have configured the heartBeat actor to update with the following configuration")]
        public void GivenIHaveConfiguredItToUpdateWithTheFollowingConfiguration(Table table)
        {
            //| updatePeriod | factoryUpdatePeriod |
            //| 100 | 1000 |

            var tableRow = table.Rows[0];
            TimeSpan updatePeriod = TimeSpan.Parse(tableRow["updatePeriod"]);
            TimeSpan factoryUpdatePeriod = TimeSpan.Parse(tableRow["factoryUpdatePeriod"]);

            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            heartBeatActor.Tell(new HeartBeatActor.ConfigureUpdate(updatePeriod, factoryUpdatePeriod));
            Thread.Sleep(10);
        }
        
        [When(@"I tell the heartbeat actor to start")]
        public void WhenITellTheHeartbeatActorToStart()
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            heartBeatActor.Tell(new HeartBeatActor.Start());
        }

        [Given(@"I register actor ""(.*)"" with the HeartBeat actor")]
        public void GivenIRegirsterActorWithTheHeartBeatActor(string actorName)
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            var actor = _state.Actors[actorName];
            heartBeatActor.Tell(new HeartBeatActor.Register(HeartBeatActor.UpdateType.Factory,actor),actor);
            Thread.Sleep(10);
        }

        [Then(@"I expect the actor ""(.*)"" to recieve the Start message")]
        public void ThenIExpectTheActorToRecieveTheStartMessage(string actorName)
        {
            var actor = _state.TestProbes[actorName];
            var msg = actor.ExpectMsg<HeartBeatActor.Started>();
        }
    }

    public class HeartBeatActor : ReceiveActor
    {
        private readonly IProduceDateTime _timeProducer;

        public static Props CreateProps(IProduceDateTime timeProducer)
        {
            return Props.Create(() => new HeartBeatActor(timeProducer));
        }

        public static string Name = "HeartBeatActor";
        private HeartBeatState _state;

        public HeartBeatActor(IProduceDateTime timeProvider)
        {
            _timeProducer = timeProvider;
            Become(Unconfigured);
        }

        private void Unconfigured()
        {
            Receive<ConfigureUpdate>(msg =>
            {
                _state = new HeartBeatState(msg.UpdatePeriod, msg.FactoryUpdatePeriod, _timeProducer.GetDateTime());
                Become(Configured);
            });
        }

        private void Configured()
        {
            HandleRegister();

            Receive<Start>(msg =>
            {
                foreach (var actorRef in _state.Registrees.SelectMany(i=>i.Value))
                {
                    actorRef.Tell(new Started());
                }

                Context.System.Scheduler.ScheduleTellRepeatedly(_state.UpdatePeriod, _state.UpdatePeriod, Self, new Tick(), Self);
                Become(Beating);
            });
        }

        public class Started{}

        private void HandleRegister()
        {
            Receive<Register>(msg => { _state.RegisterActor(msg.UpdateType, msg.Actor); });
        }

        private void Beating()
        {
            HandleRegister();

            Receive<Tick>(msg =>
            {
                var now = _timeProducer.GetDateTime();

                UpdateFactories(now);
            });
        }

        private void UpdateFactories(DateTime now)
        {
            var diff = now - _state.LastFactoryUpdate;
            if (diff > _state.FactoryUpdatePeriod)
            {
                int noPeriods;
                var remainder = Math.DivRem(diff.Milliseconds, _state.FactoryUpdatePeriod.Milliseconds, out noPeriods);

                _state = new HeartBeatState(_state.UpdatePeriod, _state.FactoryUpdatePeriod, now - TimeSpan.FromMilliseconds(remainder));

                foreach (var actorRef in _state.Registrees[UpdateType.Factory])
                {
                    for (int i = 0; i < noPeriods; i++)
                    {
                        actorRef.Tell(new FactoryTick());
                    }
                }
            }
        }

        public class FactoryTick{}

        private class Tick{}

        public class HeartBeatState
        {
            public TimeSpan UpdatePeriod { get; private set; }
            public TimeSpan FactoryUpdatePeriod { get; private set; }

            public ImmutableDictionary<UpdateType,ImmutableHashSet<IActorRef>> Registrees { get; private set; }
            public DateTime LastFactoryUpdate { get; private set; }

            public HeartBeatState(TimeSpan updatePeriod, TimeSpan factoryUpdatePeriod, DateTime lastFactoryUpdate)
            {
                UpdatePeriod = updatePeriod;
                FactoryUpdatePeriod = factoryUpdatePeriod;
                LastFactoryUpdate = lastFactoryUpdate;
                Registrees = ImmutableDictionary<UpdateType, ImmutableHashSet<IActorRef>>.Empty;
            }

            public void RegisterActor(UpdateType updateType, IActorRef actor)
            {
                if (Registrees.ContainsKey(updateType))
                {
                    var items = Registrees[updateType];
                    items = items.Add(actor);
                    Registrees = Registrees.SetItem(updateType, items);
                }
                else
                {
                    Registrees = Registrees.Add(updateType, ImmutableHashSet<IActorRef>.Empty.Add(actor));
                }
            }
        }

        public class ConfigureUpdate
        {
            public TimeSpan UpdatePeriod { get; private set; }
            public TimeSpan FactoryUpdatePeriod { get; private set; }

            public ConfigureUpdate(TimeSpan updatePeriod, TimeSpan factoryUpdatePeriod)
            {
                UpdatePeriod = updatePeriod;
                FactoryUpdatePeriod = factoryUpdatePeriod;
            }
        }

        public class Register
        {
            public UpdateType UpdateType { get;private set; }
            public IActorRef Actor { get; private set; }

            public Register(UpdateType updateType, IActorRef actor)
            {
                UpdateType = updateType;
                Actor = actor;
            }
        }

        public class Start{}

        public enum UpdateType
        {
            Factory
        }
    }
}
