using System;
using System.Collections.Immutable;

namespace Entities
{
    public class Market
    {
        public string Name { get; }
        public ImmutableDictionary<string, SaleItem> ItemsForSale { get; private set; }

        public Market(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;

            ItemsForSale  = ImmutableDictionary<string, SaleItem>.Empty;

        }

        public void AddItemForSale(ITradeable resource, int amount, int pricePerUnit)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            if (ItemsForSale.ContainsKey(resource.Name))
            {
                var existing = ItemsForSale[resource.Name];
                var newItem = new SaleItem(resource, existing.Amount + amount,
                    Math.Max(pricePerUnit, existing.PricePerUnit));

                ItemsForSale = ItemsForSale.SetItem(resource.Name, newItem);
            }
            else
            {
                var saleItem = new SaleItem(resource, amount, pricePerUnit);
                ItemsForSale = ItemsForSale.Add(saleItem.Resource.Name, saleItem);
            }
        }

        public class SaleItem
        {
            public ITradeable Resource { get; }
            public int Amount { get; }
            public int PricePerUnit { get; }

            public SaleItem(ITradeable resource, int amount, int pricePerUnit)
            {
                if (resource == null) throw new ArgumentNullException(nameof(resource));
                Resource = resource;
                Amount = amount;
                PricePerUnit = pricePerUnit;
            }

            protected bool Equals(SaleItem other)
            {
                return Equals(Resource.Name, other.Resource.Name) && Amount == other.Amount && PricePerUnit == other.PricePerUnit;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((SaleItem)obj);
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