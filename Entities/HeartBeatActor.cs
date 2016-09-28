using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Akka.Actor;

namespace Entities
{
    //
    public class HeartBeatActor : ReceiveActor
    {
        public enum UpdateType
        {
            Factory,
            Tick
        }

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
                _state = new HeartBeatState(msg.UpdatePeriod, msg.FactoryUpdatePeriod, DateTimeProvider.Now(),ImmutableDictionary<UpdateType, ImmutableHashSet<IActorRef>>.Empty);
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


        private void HandleRegister()
        {
            Receive<Register>(msg =>
            {
                Context.LogMessageDebug(msg);
                _state.RegisterActor(msg.UpdateType, msg.Actor);
                Sender.Tell(new Registered(msg));
            });

            Receive<QueryConfiguration>(msg =>
            {
                Context.LogMessageDebug(msg);
                Sender.Tell(new ConfigurationResult(_state));
            });
        }


        private void Beating()
        {
            HandleRegister();

            Receive<Tick>(msg =>
            {
                Context.LogMessageDebug(msg);
                var now = DateTimeProvider.Now();

                UpdateFactories(now);
                UpdateTicks();
            });
        }

        private void UpdateTicks()
        {
            if (_state.Registrees.ContainsKey(UpdateType.Tick))
            {
                foreach (var actorRef in _state.Registrees[UpdateType.Tick])
                {
                    actorRef.Tell(new Tick());
                }
            }
        }

        private void UpdateFactories(DateTime now)
        {
            var diff = now - _state.LastFactoryUpdate;
            if (diff > _state.FactoryUpdatePeriod)
            {
                int remainder;
                var noPeriods = Math.DivRem((int)diff.TotalMilliseconds, (int)_state.FactoryUpdatePeriod.TotalMilliseconds, out remainder);

                _state = new HeartBeatState(_state.UpdatePeriod, _state.FactoryUpdatePeriod, now - TimeSpan.FromMilliseconds(remainder), _state.Registrees);

                if (_state.Registrees.ContainsKey(UpdateType.Factory))
                {
                    foreach (var actorRef in _state.Registrees[UpdateType.Factory])
                    {
                        for (int i = 0; i < noPeriods; i++)
                        {
                            actorRef.Tell(new FactoryTick());
                        }
                    }
                }
            }
        }

        public class HeartBeatState
        {
            public TimeSpan UpdatePeriod { get; private set; }
            public TimeSpan FactoryUpdatePeriod { get; private set; }

            public ImmutableDictionary<UpdateType, ImmutableHashSet<IActorRef>> Registrees { get; private set; }
            public DateTime LastFactoryUpdate { get; private set; }

            public HeartBeatState(TimeSpan updatePeriod, TimeSpan factoryUpdatePeriod, DateTime lastFactoryUpdate, ImmutableDictionary<UpdateType, ImmutableHashSet<IActorRef>> registrees)
            {
                UpdatePeriod = updatePeriod;
                FactoryUpdatePeriod = factoryUpdatePeriod;
                LastFactoryUpdate = lastFactoryUpdate;
                Registrees = registrees;
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

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                StringBuilder registrees = new StringBuilder();
                foreach (var updateType in Registrees.Keys)
                {
                    registrees.Append($"UpdateType({updateType}:");
                    foreach (var actorRef in Registrees[updateType])
                    {
                        registrees.Append($"actorPath({actorRef.Path}),");
                    }
                    registrees.Append(")");
                }

                return $"HeartBeatState(UpdatePeriod:{UpdatePeriod.TotalMilliseconds},FactoryUpdatePeriod:{FactoryUpdatePeriod.TotalMilliseconds},Registrees({registrees}),LastFactoryUpdate:{LastFactoryUpdate})";
            }
        }



        public class Register
        {
            public UpdateType UpdateType { get; private set; }
            public IActorRef Actor { get; private set; }

            public Register(UpdateType updateType, IActorRef actor)
            {
                UpdateType = updateType;
                Actor = actor;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return $"Register(UpdateType:{UpdateType},Actor{Actor.Path.ToString()})";
            }
        }
        public class Registered
        {
            public Register Registree { get; private set; }

            public Registered(Register registree)
            {
                Registree = registree;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return $"Registree({Registree})";
            }
        }

        public class ConfigurationResult
        {
            public HeartBeatState State { get; private set; }

            public ConfigurationResult(HeartBeatState state)
            {
                State = state;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return $"ConfigurationResult(State{State})";
            }
        }

        public class QueryConfiguration{}

        public class FactoryTick{}

        public class Tick{}

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

        public class Start{}
        public class Started { }
    }
}