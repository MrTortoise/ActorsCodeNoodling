using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Entities.Factories;

namespace Entities
{
    /// <summary>
    /// This is an actor representing a trader. 
    /// </summary>
    public class Trader : ReceiveActor, ITrader
    {
        private readonly Dictionary<IResource, ResourceStack> _resources;
        private ImmutableHashSet<IActorRef> _factories = ImmutableHashSet<IActorRef>.Empty;

        /// <summary>
        /// Creates a Trader with a name
        /// </summary>
        /// <param name="name"></param>
        public Trader(string name)
        {
            _resources = new Dictionary<IResource, ResourceStack>();

            Name = name;

            Receive<PostResourceMessage>(message =>
            {
                Context.LogMessageDebug(message);
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
            });

            Receive<QueryResourcesMessage>(message =>
            {
                Context.LogMessageDebug(message);
                Sender.Tell(new QueryResourcesResultMessage(_resources.Values.ToImmutableArray()));
            });

            Receive<FactoryCoordinatorActor.FactoryCreated>(msg =>
            {
                Context.LogMessageDebug(msg);
                _factories = _factories.Add(msg.Factory);
            });

            Receive<QueryFactories>(msg =>
            {
                Sender.Tell(new FactoryQueryResult(_factories));
            });
        }

        public string Name { get; }

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

        public class FactoryQueryResult
        {
            public ImmutableHashSet<IActorRef> Factories { get; private set; }


            public FactoryQueryResult(ImmutableHashSet<IActorRef> factories)
            {
                Factories = factories;
            }
        }

        public class QueryFactories
        {
        }
    }
}