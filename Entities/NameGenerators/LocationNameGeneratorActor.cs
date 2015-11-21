using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Util.Internal;

namespace Entities.NameGenerators
{
    /// <summary>
    /// Manages the creation of unique names for locations and maintaing the list of used options
    /// </summary>
    public class LocationNameGeneratorActor : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef _persistence;
        private readonly HashSet<string> _locations = new HashSet<string>();
        private readonly HashSet<string> _locationsBeingAdded = new HashSet<string>();

        private readonly List<IActorRef> _observers = new List<IActorRef>();
        private INameGenerator _nameGenerator;

        public static string Path => @"user/LocationGenerator";

        public static string Name => "LocationGenerator";

        public static Props CreateProps()
        {
            return Props.Create(() => new LocationNameGeneratorActor());
        }

        public LocationNameGeneratorActor()
        {
            _nameGenerator = new LocationNameGenerator();
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
            Receive<AddLocation>(msg =>
            {
                Context.LogMessageDebug(msg);
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
            public string[] Locations { get;  }

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
    }
}