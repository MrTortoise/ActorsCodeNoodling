namespace Entities.UniverseGenerator
{
    /// <summary>
    /// Produces an even PDF between min and max values
    /// </summary>
    public class EqualityPdf : ISingleVariableFunction<double, int>
    {
        public double F(int x)
        {
            return x;
        }
    }
}