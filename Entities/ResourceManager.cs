using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Entities.Observation;

namespace Entities
{
    /// <summary>
    /// Holds the unique resource items so everythign refers to the same resource
    /// </summary>
    public class ResourceManager : ReceiveActor
    {
        private readonly HashSet<IResource> _resources = new HashSet<IResource>();
        private readonly HashSet<IActorRef> _observers = new HashSet<IActorRef>();

        public ResourceManager()
        {
            Receive<Observe>(msg =>
            {
                _observers.Add(Sender);
            });

            Receive<UnObserve>(msg =>
            {
                _observers.Remove(Sender);
            });

            Receive<PostResourceMessage>(m =>
            {
                Context.LogMessageDebug(m);
                _resources.Add(m.Resource);
                foreach (var actorRef in _observers)
                {
                    actorRef.Tell(new EventObserved());
                }
            });

            Receive<GetResource>(m =>
            {
                Context.LogMessageDebug(m);

                if (string.IsNullOrWhiteSpace(m.Name))
                {
                    Sender.Tell(new GetResourceResult(_resources.ToArray()));
                }
                else
                {
                    IResource retVal = _resources.SingleOrDefault(i => i.Name == m.Name);

                    Sender.Tell(new GetResourceResult(retVal));
                }              
            });
        }

        /// <summary>
        /// Creates a new resource
        /// </summary>
        public class PostResourceMessage
        {
            public IResource Resource { get;  }

            public PostResourceMessage(IResource resource)
            {
                Resource = resource;
            }
        }

        /// <summary>
        /// Get resource message. Gets one or more resources.
        /// </summary>
        public class GetResource
        {
            /// <summary>
            /// The name of the resource to get, empty for all.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Creates an instance of <see cref="GetResource"/>
            /// </summary>
            /// <param name="name">The name of the resource to get, null or empty for all.</param>
            public GetResource(string name)
            {
                Name = name;
            }
        }

        public class GetResourceResult
        {
            public IResource[] Values { get; set; }

            public GetResourceResult(IResource retVal)
            {
                Values = new[] {retVal};
            }

            public GetResourceResult(IResource[] values)
            {
                Values = values;
            }
        }
    }
}
