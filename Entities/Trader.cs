using Akka.Actor;

namespace Entities
{
    /// <summary>
    /// This is an actor representing a trader. 
    /// </summary>
    public class Trader : ReceiveActor, ITrader
    {
        /// <summary>
        /// Creates a Trader with a name
        /// </summary>
        /// <param name="name"></param>
        public Trader(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public class PostResourceMessage
        {
            public IResource Resource { get; }
            public int Quantity { get;  }

            public PostResourceMessage(IResource resource, int quantity)
            {
                Resource = resource;
                Quantity = quantity;
            }
        }

        public class QueryResourcesMessage
        {
        }

        public class QueryResourcesResultMessage
        {
        }
    }
}