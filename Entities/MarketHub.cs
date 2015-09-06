using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;

namespace Entities
{
    /// <summary>
    /// Represents a central listing of markets, manages the creation, querying and lifecycles of them.
    /// </summary>
    public class MarketHub : TypedActor, IHandle<MarketHub.TellCreateMarketMessage>, IHandle<MarketHub.QueryMarketListingsMessage>
    {
        /// <summary>
        /// The message to send to a <see cref="MarketHub"/> to create a market
        /// </summary>
        public void Handle(TellCreateMarketMessage message)
        {
            var sender = Context.Sender;
            var marketActor = Context.ActorOf(Props.Create(() => new Market(message.Name, sender)), message.Name);
            
            Context.Sender.Tell(new TellMarketCreatedMessage(marketActor));
        }

        public void Handle(QueryMarketListingsMessage message)
        {
            var children = Context.GetChildren();
            var tasks = new List<Task<Market.ResultMarketResources>>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var child in children)
            {
                var t = child.Ask<Market.ResultMarketResources>(new Market.QueryMarketMessage());
                tasks.Add(t);
            }

            var conTask = Task.WhenAll(tasks);
            conTask.ContinueWith((task, state) =>
            {
                Market.ResultMarketResources[] resources = task.Result;
                var result = new ResultsMarketListings(resources);
                var s = (SenderSelf) state;

                s.Sender.Tell(result, s.Self);
            }, new SenderSelf(Sender, Self)
                );
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

            /// <summary>
            /// Creates an instance of <see cref="TellMarketCreatedMessage"/>
            /// </summary>
            /// <param name="name">The name of the market to create</param>
            public TellCreateMarketMessage(string name)
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
                Name = name;
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

    public class SenderSelf
    {
        public IActorRef Sender { get; set; }
        public IActorRef Self { get; set; }

        public SenderSelf(IActorRef sender, IActorRef self)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (self == null) throw new ArgumentNullException(nameof(self));
            Sender = sender;
            Self = self;
        }
    }
}