using System;
using System.Collections.Immutable;

namespace Entities
{
    /// <summary>
    /// A market holds various contracts and their status. 
    /// It provides a workflow for both parties to post and interact with contracts. 
    /// It is like a billboard for invitations, a negotiation service and then an escrow service
    /// </summary>
    public class Market
    {
        public string Name { get; }
        public ImmutableDictionary<string, ResourceForSale> ItemsForSale { get; private set; }

        public Market(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;

            ItemsForSale  = ImmutableDictionary<string, ResourceForSale>.Empty;

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

        public class ResourceForSale
        {
            public IResource Resource { get; }
            public int Amount { get; }
            public int PricePerUnit { get; }

            public ResourceForSale(IResource resource, int amount, int pricePerUnit)
            {
                if (resource == null) throw new ArgumentNullException(nameof(resource));
                Resource = resource;
                Amount = amount;
                PricePerUnit = pricePerUnit;
            }

            protected bool Equals(ResourceForSale other)
            {
                return Equals(Resource.Name, other.Resource.Name) && Amount == other.Amount && PricePerUnit == other.PricePerUnit;
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

 
    }
}