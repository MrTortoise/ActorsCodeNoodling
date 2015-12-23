using System;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;

namespace Entities
{
    public class HeartBeatActor : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(() => new HeartBeatActor());
        }

        public static string Name = "HeartBeatActor";
        private HeartBeatState _state;

        public HeartBeatActor()
        {
            Become(Unconfigured);
        }

        private void Unconfigured()
        {
            Receive<ConfigureUpdate>(msg =>
            {
                Context.LogMessageDebug(msg);
                _state = new HeartBeatState(msg.UpdatePeriod, msg.FactoryUpdatePeriod, DateTimeProvider.Now());
                Become(Configured);
            });
        }

        private void Configured()
        {
            HandleRegister();

            Receive<Start>(msg =>
            {
                Context.LogMessageDebug(msg);
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
            Receive<Register>(msg =>
            {
                Context.LogMessageDebug(msg);
                _state.RegisterActor(msg.UpdateType, msg.Actor);
            });

            Receive<QueryConfiguration>(msg =>
            {
                Sender.Tell(new ConfigurationResult(_state));
            });
        }

        public class ConfigurationResult
        {
            public HeartBeatState State { get;private set; }

            public ConfigurationResult(HeartBeatState state)
            {
                State = state;
            }
        }

        public class QueryConfiguration
        {
        }

        private void Beating()
        {
            HandleRegister();

            Receive<Tick>(msg =>
            {
                Context.LogMessageDebug(msg);
                var now = DateTimeProvider.Now();

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