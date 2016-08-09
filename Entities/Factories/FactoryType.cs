using System.Collections.Immutable;

namespace Entities.Factories
{
    /// <summary>
    /// Defines the input:output characteristics of a factory over time
    /// </summary>
    public class FactoryType
    {
        public string Name { get;  }

        /// <summary>
        /// A dictionary of <see cref="IResource"/> and the <see cref="QuantityPeriod"/> quantity consumed per number of periods <see cref="QuantityPeriod"/>
        /// </summary>
        public ImmutableDictionary<IResource, QuantityPeriod> InputResources { get;  }

        /// <summary>
        /// A dictionary of <see cref="IResource"/>  and the quantity produced per number of period <see cref="QuantityPeriod"/>
        /// </summary>
        public ImmutableDictionary<IResource, QuantityPeriod> OutputResources { get;  }

        public FactoryType(
            string name, 
            ImmutableDictionary<IResource, QuantityPeriod> inputResources, 
            ImmutableDictionary<IResource, QuantityPeriod> outputResources) 
        {
            Name = name;
            InputResources = inputResources;
            OutputResources = outputResources;
        }

        /// <summary>
        /// Represents a quanity produced / consumed and the number of time periods this takes to occur.
        /// </summary>
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