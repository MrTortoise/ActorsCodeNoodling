using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.NameGenerators;

namespace Entities
{
    /// <summary>
    /// Represents a central listing of markets, manages the creation, querying and lifecycles of them.
    /// </summary>
    public class MarketHub : ReceiveActor
    {
        private readonly Dictionary<string, IActorRef> _markets = new Dictionary<string, IActorRef>();
        private ActorSelection _locationGenerator;

        public MarketHub()
        {
            Receive<TellCreateMarketMessage>(msg =>
            {
                Context.LogMessageDebug(msg);
                var sender = Context.Sender;
                var marketActor = Context.ActorOf(Props.Create(() => new Market(msg.Name,msg.Location)), msg.Name);

                _markets.Add(msg.Name, marketActor);
                sender.Tell(new TellMarketCreatedMessage(marketActor));
            });

            Receive<QueryMarketListingsMessage>(msg =>
            {
                Context.LogMessageDebug(msg);
                var coordinator = Context.ActorOf(MarketQueryCoordinator.CreateProps());
                coordinator.Tell(new MarketQueryCoordinator.QueryMarkets(_markets.Values.ToArray(), Sender));
            });
        }

        protected override void PreStart()
        {
            _locationGenerator = Context.ActorSelection(LocationNameGeneratorActor.Path);
        }

        private class MarketQueryCoordinator : ReceiveActor
        {
            private readonly Dictionary<IActorRef, bool> _markets = new Dictionary<IActorRef, bool>();

            private readonly Dictionary<string, Market.ResultMarketResources> _marketResources =
                new Dictionary<string, Market.ResultMarketResources>();

            private IActorRef _sender;

            public static Props CreateProps()
            {
                return Props.Create(() => new MarketQueryCoordinator());
            }

            public MarketQueryCoordinator()
            {
                Become(Waiting);
            }

            private void Waiting()
            {
                Receive<QueryMarkets>(msg =>
                {
                    Context.LogMessageDebug(msg);
                    _sender = msg.Sender;
                    foreach (var actorRef in msg.Markets)
                    {
                        actorRef.Tell(new Market.QueryMarketMessage());
                        _markets.Add(actorRef, false);
                    }

                    Become(Processing);
                });
            }

            private void Processing()
            {
                Receive<Market.ResultMarketResources>(msg =>
                {
                    Context.LogMessageDebug(msg);
                    _marketResources.Add(msg.MarketName, msg);
                    _markets[msg.Market] = true;

                    if (!_markets.Values.Contains(false))
                    {
                        _sender.Tell(new ResultsMarketListings(_marketResources.Values.ToArray()));
                        Become(Waiting);
                    }
                });
            }

            public class QueryMarkets
            {
                public IActorRef[] Markets { get; }
                public IActorRef Sender { get; }

                public QueryMarkets(IActorRef[] markets, IActorRef sender)
                {
                    Markets = markets;
                    Sender = sender;
                }
            }
        }

        /// <summary>
        /// Message used to tell a <see cref="MarketHub"/> to create a new <see cref="Market"/>
        /// </summary>
        public class TellCreateMarketMessage
        {
            /// <summary>
            /// the name of the market
            /// </summary>
            public string Name { get; private set; }

            public IActorRef Location { get; private set; }

            /// <summary>
            /// Creates an instance of <see cref="TellMarketCreatedMessage"/>
            /// </summary>
            /// <param name="name">The name of the market to create</param>
            /// <param name="location"></param>
            public TellCreateMarketMessage(string name, IActorRef location)
            {
                if (location == null) throw new ArgumentNullException(nameof(location));
                if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
              

                Name = name;
                Location = location;
            }
        }

        /// <summary>
        /// The response message of the <see cref="MarketHub"/> to whoever asked the <see cref="Market"/> to be created.
        /// </summary>
        public class TellMarketCreatedMessage
        {
            /// <summary>
            /// Creates an instance of <see cref="TellMarketCreatedMessage"/>
            /// </summary>
            /// <param name="marketActor">An <see cref="IActorRef"/> to the created <see cref="Market"/></param>
            public TellMarketCreatedMessage(IActorRef marketActor)
            {
                MarketActor = marketActor;
            }

            /// <summary>
            /// Gets a reference to the <see cref="IActorRef"/> of the created <see cref="Market"/>
            /// </summary>
            public IActorRef MarketActor { get; private set; }
        }

        /// <summary>
        /// The results of the <see cref="QueryMarketListingsMessage"/>
        /// </summary>
        public class ResultsMarketListings
        {
            public ResultsMarketListings(Market.ResultMarketResources[] marketResourcesList)
            {
                MarketListings = marketResourcesList;
            }

            public Market.ResultMarketResources[] MarketListings { get; private set; }
        }

        public class QueryMarketListingsMessage
        {
        }
    }
}

