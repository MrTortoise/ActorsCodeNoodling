using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;

namespace Entities
{
    /// <summary>
    /// This is an actor representing a trader. 
    /// </summary>
    public class Trader : TypedActor, ITrader,
        IHandle<Trader.PostResourceMessage>,
        IHandle<Trader.QueryResourcesMessage>,
        IHandle<ExchangeContract.OfferMadeNotification>
    {
        private readonly Dictionary<IResource, ResourceStack> _resources;

        /// <summary>
        /// Creates a Trader with a name
        /// </summary>
        /// <param name="name"></param>
        public Trader(string name)
        {
            _resources = new Dictionary<IResource, ResourceStack>();

            Name = name;
        }

        public string Name { get; }

        public void Handle(PostResourceMessage message)
        {
            if (_resources.ContainsKey(message.ResourceStack.Resource))
            {
                var resourceStack = _resources[message.ResourceStack.Resource];
                _resources[message.ResourceStack.Resource] = new ResourceStack(resourceStack.Resource,
                    resourceStack.Quantity + message.ResourceStack.Quantity);
            }
            else
            {
                _resources.Add(message.ResourceStack.Resource, message.ResourceStack);
            }
        }

        public void Handle(QueryResourcesMessage message)
        {
            Sender.Tell(new QueryResourcesResultMessage(_resources.Values.ToImmutableArray()));
        }

        public void Handle(ExchangeContract.OfferMadeNotification message)
        {
            throw new System.NotImplementedException();
        }

        public struct PostResourceMessage
        {
            public ResourceStack ResourceStack { get; }

            public PostResourceMessage(ResourceStack resourceStack)
            {
                ResourceStack = resourceStack;
            }
        }

        public struct QueryResourcesMessage
        {
        }

        public struct QueryResourcesResultMessage
        {
            public QueryResourcesResultMessage(ImmutableArray<ResourceStack> resourceStacks)
            {
                ResourceStacks = resourceStacks;
            }

            public ImmutableArray<ResourceStack> ResourceStacks { get; }
        }


    }
}