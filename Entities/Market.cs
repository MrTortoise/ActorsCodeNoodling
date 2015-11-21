using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Remoting.Messaging;
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
        private readonly Dictionary<string, ResourceForSale> _itemsForSale = new Dictionary<string, ResourceForSale>();

        private readonly string _name;
        private IActorRef _location;

        public static Props CreateProps(string name, IActorRef location)
        {
            return Props.Create(() => new Market(name, location));
        }

        public Market(string name, IActorRef location)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            _name = name;
            _location = location;
        }

        public void Handle(QueryMarketMessage message)
        {
            Context.LogMessageDebug(message);
            var ret = new ResultMarketResources(_name, _itemsForSale, Self);
            Sender.Tell(ret, Self);
        }

        public void AddItemForSale(IResource resource, int amount, int pricePerUnit)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (_itemsForSale.ContainsKey(resource.Name))
            {
                var existing = _itemsForSale[resource.Name];
                var newItem = new ResourceForSale(resource, existing.Amount + amount,
                    Math.Max(pricePerUnit, existing.PricePerUnit));

                _itemsForSale[resource.Name] = newItem;
            }
            else
            {
                var saleItem = new ResourceForSale(resource, amount, pricePerUnit);
                _itemsForSale.Add(saleItem.Resource.Name, saleItem);
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
            public ResultMarketResources(string marketName, Dictionary<string, ResourceForSale> itemsForSale, IActorRef market)
            {
                if (string.IsNullOrWhiteSpace(marketName)) throw new ArgumentNullException(nameof(marketName));
                if (itemsForSale == null) throw new ArgumentNullException(nameof(itemsForSale));
                if (market == null) throw new ArgumentNullException(nameof(market));

                MarketName = marketName;
                ItemsForSale = itemsForSale;
                Market = market;
            }

            public string MarketName { get; private set; }
            public Dictionary<string, ResourceForSale> ItemsForSale { get; private set; }
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