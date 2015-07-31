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

            var saleItem = new SaleItem(resource, amount, pricePerUnit);
            ItemsForSale = ItemsForSale.Add(saleItem.Resource.Name, saleItem);
        }

        public class SaleItem
        {
            public ITradeable Resource { get; }
            public int Amount { get; private set; }
            public int PricePerUnit { get; private set; }

            public SaleItem(ITradeable resource, int amount, int pricePerUnit)
            {
                Resource = resource;
                Amount = amount;
                PricePerUnit = pricePerUnit;
            }
        }
    }
}