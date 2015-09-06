using System;
using System.Collections.Immutable;
using Akka.Actor;

namespace Entities
{
    /// <summary>
    /// A market holds various contracts and their status. 
    /// It provides a workflow for both parties to post and interact with contracts. 
    /// It is like a billboard for invitations, a negotiation service and then an escrow service
    /// </summary>
    public class Market : TypedActor , IHandle<Market.QueryMarketMessage>
    {
        private IActorRef _creator;

        public string Name { get; }
        public ImmutableDictionary<string, ResourceForSale> ItemsForSale { get; private set; }

        public Market(string name, IActorRef creator)
        {
            if (creator == null) throw new ArgumentNullException(nameof(creator));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            this._creator = creator;

            ItemsForSale  = ImmutableDictionary<string, ResourceForSale>.Empty;
        }

        public void Handle(QueryMarketMessage message)
        {
            var ret = new ResultMarketResources(Name, ItemsForSale, Self);
            Sender.Tell(ret, Self);
        }

        public void AddItemForSale(IResource resource, int amount, int pricePerUnit)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (ItemsForSale.ContainsKey(resource.Name))
            {
                var existing = ItemsForSale[resource.Name];
                var newItem = new ResourceForSale(resource, existing.Amount + amount,
                    Math.Max(pricePerUnit, existing.PricePerUnit));

                ItemsForSale = ItemsForSale.SetItem(resource.Name, newItem);
            }
            else
            {
                var saleItem = new ResourceForSale(resource, amount, pricePerUnit);
                ItemsForSale = ItemsForSale.Add(saleItem.Resource.Name, saleItem);
            }
        }

        /// <summary>
        /// Represents an <see cref="IResource"/>, a quantity and a price of a resource for sale
        /// </summary>
        public class ResourceForSale
        {
            public IResource Resource { get; }
            public int Amount { get; }
            public int PricePerUnit { get; }

            /// <summary>
            /// Creates an instance of <see cref="ResourceForSale"/>
            /// </summary>
            /// <param name="resource">The <see cref="IResource"/> for sale</param>
            /// <param name="amount">the unit amount for sale</param>
            /// <param name="pricePerUnit">THe price per unit</param>
            public ResourceForSale(IResource resource, int amount, int pricePerUnit)
            {
                if (resource == null) throw new ArgumentNullException(nameof(resource));
                Resource = resource;
                Amount = amount;
                PricePerUnit = pricePerUnit;
            }

            protected bool Equals(ResourceForSale other)
            {
               if (other == null)
                  return false;

               return
                  Equals(Resource.Name, other.Resource.Name) &&
                  Amount == other.Amount &&
                  PricePerUnit == other.PricePerUnit;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ResourceForSale)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Resource?.Name.GetHashCode() ?? 0;
                    hashCode = (hashCode * 397) ^ Amount;
                    hashCode = (hashCode * 397) ^ PricePerUnit;
                    return hashCode;
                }
            }
        }

        /// <summary>
        /// The result of the <see cref="QueryMarketMessage"/>
        /// </summary>
        public class ResultMarketResources
        {
            public ResultMarketResources(string marketName, ImmutableDictionary<string, ResourceForSale> itemsForSale, IActorRef market)
            {
                if (string.IsNullOrWhiteSpace(marketName)) throw new ArgumentNullException(nameof(marketName));
                if (itemsForSale == null) throw new ArgumentNullException(nameof(itemsForSale));
                if (market == null) throw new ArgumentNullException(nameof(market));

                MarketName = marketName;
                ItemsForSale = itemsForSale;
                Market = market;
            }

            public string MarketName { get; private set; }
            public ImmutableDictionary<string, ResourceForSale> ItemsForSale { get; private set; }
            public IActorRef Market { get; private set; }
        }

        /// <summary>
        /// The message to query a market about its resources
        /// </summary>
        public class QueryMarketMessage
        {
        }


    }
}