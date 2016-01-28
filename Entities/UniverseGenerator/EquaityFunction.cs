using System;

namespace Entities.UniverseGenerator
{
    /// <summary>
    /// Produces an even CDF between min and max values
    /// </summary>
    public class EqualityCdf : ISingleVariableFunction<int,double>
    {
        public readonly int Max;
        public readonly int Min;

        public EqualityCdf(int min, int max)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(min), $"min ({min}) must be <= max ({max})");
            Min = min;
            Max = max;
        }

        public int F(double x)
        {
            var scaledInput = (Max - Min)*x;
            int retVal = (int) Math.Round(scaledInput + Min, 0);
            return retVal;
        }
    }
}