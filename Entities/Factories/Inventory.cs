using System;
using System.Collections.Immutable;

namespace Entities.Factories
{
    /// <summary>
    /// A set of resources, size limits and quantity limits
    /// </summary>
    public class Inventory
    {
        public Inventory(InventoryType inventoryType)
        {
            if (inventoryType == null) throw new ArgumentNullException(nameof(inventoryType));

            InventoryType = inventoryType;
            Resources = ImmutableDictionary<IResource, double>.Empty;
        }

        public InventoryType InventoryType { get; set; }
        public ImmutableDictionary<IResource,double> Resources { get; private set; }
    }
}