using System;

namespace Entities
{
    public class Market
    {
        public Market(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}