using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities
{
    /// <summary>
    /// Holds the unique resource items so everythign refers to the same resource
    /// </summary>
    public class ResourceManager : ReceiveActor
    {
        private readonly Dictionary<string, IResource> _resources;

        public ResourceManager()
        {
            _resources = new Dictionary<string, IResource>();
            Receive<PostResourceMessage>(m => _resources.Add(m.Resource.Name, m.Resource));
            Receive<GetResource>(m =>
            {
                IResource retVal = null;
                if (_resources.ContainsKey(m.Name))
                {
                    retVal = _resources[m.Name];
                }

                Sender.Tell(retVal);

            });
        }


        public class PostResourceMessage
        {
            public IResource Resource { get;  }

            public PostResourceMessage(IResource resource)
            {
                Resource = resource;
            }
        }

        public class GetResource
        {
            public string Name { get; }

            public GetResource(string name)
            {
                Name = name;
            }
        }
    }
}
