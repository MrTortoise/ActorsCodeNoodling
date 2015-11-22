using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Akka.Actor;
using Akka.Util.Internal;

namespace Entities.NameGenerators
{
    /// <summary>
    /// Manages the creation of unique names for locations and maintaing the list of used options
    /// </summary>
    public class LocationNameGeneratorActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IActorRef _randomActorRef;
        private readonly int _numberOfCharacters;

        private IActorRef _persistence;
        private readonly HashSet<string> _locations = new HashSet<string>();
        private readonly HashSet<string> _locationsBeingAdded = new HashSet<string>();

        private readonly List<IActorRef> _observers = new List<IActorRef>();
        private INameGenerator _nameGenerator;

        public static string Path => @"user/LocationGenerator";

        public static string Name => "LocationGenerator";

        public static Props CreateProps(IActorRef randomActorRef, int numberOfCharacters)
        {
            return Props.Create(() => new LocationNameGeneratorActor(randomActorRef, numberOfCharacters));
        }

        public LocationNameGeneratorActor(IActorRef randomActorRef, int numberOfCharacters)
        {
            _randomActorRef = randomActorRef;
            _numberOfCharacters = numberOfCharacters;

            Become(Waiting);
            BecomeStacked(Recovering);
        }

        public IStash Stash { get; set; }

        protected override void PreStart()
        {
            _persistence = Context.ActorOf(Props.Create(() => new WorldPrefixPersistanceActor()),
                "worldPersistanceActor");
        }

        private void Waiting()
        {
            Receive<GenerateLocations>(msg =>
            {
                Context.LogMessageDebug(msg);
                _locationsBeingAdded.Clear();
                TellWantRandomNumbers(msg.NumberOfLocations);
                Become(WaitingForRandomNumbers);
            });

            Receive<AddLocation>(msg =>
            {
                Context.LogMessageDebug(msg);
                _locationsBeingAdded.Clear();
                foreach (var location in msg.Locations.Where(i => !_locations.Contains(i)))
                {
                    _locationsBeingAdded.Add(location);
                    _persistence.Tell(new WorldPrefixPersistanceActor.PostNewPrefixMessage(location));
                }

                _persistence.Tell(new WorldPrefixPersistanceActor.PostStoreStateMessage());
                Become(AddingLocations);
            });

            SetupCommonReceive();
        }

        private void WaitingForRandomNumbers()
        {
            Receive<RandomActor.RandomResult>(msg =>
            {
                Context.LogMessageDebug(msg);
                int numberOfStrings = msg.Number.Length/_numberOfCharacters;

                string[] names = new string[numberOfStrings];
                for (int i = 0; i < numberOfStrings; i++)
                {
                    StringBuilder name = new StringBuilder(_numberOfCharacters);
                    for (int j = 0; j < _numberOfCharacters; j++)
                    {
                        int index = i + j;
                        name.Append(msg.Number[index]);
                    }
                    names[0] = name.ToString();
                }

                var repeated = new List<string>();
                foreach (var name in names)
                {
                    if (_locations.Contains(name))
                    {
                        repeated.Add(name);
                    }
                    else
                    {
                        _locationsBeingAdded.Add(name);
                    }
                }

                var numberNewRandomsNeeded = repeated.Count;
                if (numberNewRandomsNeeded > 0)
                {
                    TellWantRandomNumbers(numberNewRandomsNeeded);
                }
                else
                {
                    foreach (var location in _locationsBeingAdded)
                    {
                        _persistence.Tell(new WorldPrefixPersistanceActor.PostNewPrefixMessage(location));
                    }
                    _persistence.Tell(new WorldPrefixPersistanceActor.PostStoreStateMessage());
                    Become(AddingLocations);
                }
            });

            ReceiveAny(msg =>
            {
                Stash.Stash();
            });
        }

        private void TellWantRandomNumbers(int numberOfLocations)
        {
            _randomActorRef.Tell(new RandomActor.NextRandom('A', 'Z' + 1, _numberOfCharacters*numberOfLocations));
        }


        private void SetupCommonReceive()
        {
            Receive<QueryLocations>(msg =>
            {
                Context.LogMessageDebug(msg);
                Sender.Tell(new Locations(_locations.ToArray()));
            });

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

            Receive<WorldPrefixPersistanceActor.Recovering>(msg =>
            {
                BecomeStacked(Recovering);
            });
        }

        private void Recovering()
        {
            Receive<WorldPrefixPersistanceActor.WorldPrefixPersistentRecoveryComplete>(msg =>
            {
                Context.LogMessageDebug(msg);
                msg.Prefixes.ForEach(p => _locations.Add(p));
                Stash.UnstashAll();
                UnbecomeStacked();
            });

            ReceiveAny(msg =>
            {
                Stash.Stash();
            });
        }

        private void AddingLocations()
        {
            Receive<WorldPrefixPersistanceActor.StateSavedMessage>(msg =>
            {
                Context.LogMessageDebug(msg);
                var locationsAdded = new LocationsAdded(_locationsBeingAdded.ToArray());
                foreach (var observer in _observers)
                {
                    observer.Tell(locationsAdded);
                }

                Stash.UnstashAll();
                Become(Waiting);
            });

            Receive<WorldPrefixPersistanceActor.Recovering>(msg =>
            {
                BecomeStacked(Recovering);
            });

            ReceiveAny(msg=>Stash.Stash());
        }

        public class UnObserve
        {
        }

        public class Observe
        {
        }

        public class LocationsAdded
        {
            public string[] AddedLocations { get; }

            public LocationsAdded(string[] addedLocations)
            {
                AddedLocations = addedLocations;
            }
        }

        public class AddLocation
        {
            public string[] Locations { get; }

            public AddLocation(string[] locations)
            {
                Locations = locations;
            }
        }

        public class Locations
        {
            public Locations(string[] names)
            {
                Names = names;
            }

            public string[] Names { get; private set; }
        }

        public class QueryLocations
        {
        }


        public class GenerateLocations
        {
            public GenerateLocations(int numberOfLocations)
            {
                NumberOfLocations = numberOfLocations;
            }

            public int NumberOfLocations { get; private set; }
        }
    }
}