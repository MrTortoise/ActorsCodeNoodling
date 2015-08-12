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
        private ImmutableDictionary<string,IResource> _resources = ImmutableDictionary<string, IResource>.Empty;


        public class PostResourceMessage
        {
            public Resource Resource { get;  }

            public PostResourceMessage(Resource resource)
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
