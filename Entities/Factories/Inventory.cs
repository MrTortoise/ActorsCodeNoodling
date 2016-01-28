using System;
using System.Collections.Immutable;
using Entities.Inventory;

namespace Entities.Factories
{
    /// <summary>
    /// A set of resources, size limits and quantity limits
    /// </summary>
    public class Inventory
    {
        public Inventory(InventoryType inventoryType, ImmutableDictionary<IResource,int> resources = null)
        {
            if (inventoryType == null) throw new ArgumentNullException(nameof(inventoryType));

            InventoryType = inventoryType;
            Resources = resources ?? ImmutableDictionary<IResource, int>.Empty;
        }

        public InventoryType InventoryType { get; set; }
        public ImmutableDictionary<IResource, int> Resources { get; private set; }
    }
}