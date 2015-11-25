using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Entities.Observation;
using NUnit.Framework.Constraints;

namespace Entities.LocationActors
{
    public class CenterOfMassManagerActor : ReceiveActor
    {
        private readonly Dictionary<string,IActorRef> _centerOfMasses = new Dictionary<string, IActorRef>();
        private readonly List<IActorRef> _contentsChangedObservers = new List<IActorRef>();

        public static string Name => "CenterOfMassManagerActor";

        public static Props CreateProps()
        {
            return Props.Create(() => new CenterOfMassManagerActor());
        }

        public CenterOfMassManagerActor()
        {
            Receive<CreateCenterOfMass>(msg =>
            {
                var com = Context.ActorOf(CenterOfMassActor.CreateProps(msg.Name, msg.Stars, msg.Planets), msg.Name);
                _centerOfMasses.Add(msg.Name, com);
                foreach (var contentsChangedObserver in _contentsChangedObservers)
                {
                    contentsChangedObserver.Tell(new EventObserved());
                }
            });

            Receive<QueryCenterOfMasses>(msg =>
            {
                if (string.IsNullOrWhiteSpace(msg.Value))
                {
                    var dic = _centerOfMasses.ToDictionary(centerOfMass => centerOfMass.Key, centerOfMass => centerOfMass.Value);
                    Sender.Tell(new CenterOfMassQueryResult(dic));
                }
                else
                {
                    if (_centerOfMasses.ContainsKey(msg.Value))
                    {
                        var dic = new Dictionary<string, IActorRef> {{msg.Value, _centerOfMasses[msg.Value]}};
                        Sender.Tell(new CenterOfMassQueryResult(dic));
                    }
                    else
                    {
                        Sender.Tell(new CenterOfMassQueryResult(new Dictionary<string, IActorRef>()));
                    }
                }
            });

            Receive<UpdateDelta>(msg =>
            {
                foreach (var actorRef in _centerOfMasses.Values)
                {
                    actorRef.Tell(msg);
                }
            });

            Receive<Observe>(msg =>
            {
                _contentsChangedObservers.Add(Sender);
            });

            Receive<UnObserve>(msg =>
            {
                _contentsChangedObservers.Remove(Sender);
            });
        }

        public class GetCenterOfMass
        {
        }

        public class CenterOfMassQueryResult
        {
            public Dictionary<string, IActorRef> CenterOfMasses { get; }

            public CenterOfMassQueryResult(Dictionary<string, IActorRef> centerOfMasses)
            {
                CenterOfMasses = centerOfMasses;
            }
        }

        public class QueryCenterOfMasses
        {
            public string Value { get; set; }
        }

        public class CreateCenterOfMass
        {
            public CreateCenterOfMass(string name, Star[] stars, Planet[] planets)
            {
                Stars = stars;
                Planets = planets;
                Name = name;
            }

            public Star[] Stars { get; private set; }
            public Planet[] Planets { get; private set; }
            public string Name { get; private set; }
        }


    }
}
