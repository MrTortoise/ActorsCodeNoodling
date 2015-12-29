using System.Collections.Immutable;

namespace Entities.Factories
{
    public class FactoryType
    {
        public string Name { get;  }
        public ImmutableDictionary<IResource, QuantityPeriod> InputResources { get;  }
        public ImmutableDictionary<IResource, QuantityPeriod> OutputResources { get;  }

        public FactoryType(string name, ImmutableDictionary<IResource, QuantityPeriod> inputResources, ImmutableDictionary<IResource, QuantityPeriod> outputResources) 
        {
            Name = name;
            InputResources = inputResources;
            OutputResources = outputResources;
        }

        public class QuantityPeriod
        {
            public int Quantity { get; }
            public int Periods { get; }

            public QuantityPeriod(int quantity, int periods)
            {
                Quantity = quantity;
                Periods = periods;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            public override string ToString()
            {
                return $"QuantityPeriod({Quantity}:{Periods})";
            }
        }
    }
}