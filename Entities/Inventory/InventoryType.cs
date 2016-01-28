using System;
using Entities.Model;

namespace Entities.Inventory
{
    public class InventoryType
    {
        public string Name { get; private set; }
        public int Capacity { get; private set; }
        public CargoSize Size { get; private set; }

        public InventoryType(string name, int capacity, CargoSize size)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument is null or whitespace", nameof(name));
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (!Enum.IsDefined(typeof (CargoSize), size)) throw new ArgumentOutOfRangeException(nameof(size));

            Name = name;
            Capacity = capacity;
            Size = size;
        }
    }
}