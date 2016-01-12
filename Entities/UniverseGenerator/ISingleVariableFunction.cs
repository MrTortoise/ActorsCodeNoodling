namespace Entities.UniverseGenerator
{
    /// <summary>
    /// Generates a value from a single variable (ie something of the form y = f(x)). Note produced distribution are on integer axis so ensure sufficient states.
    /// </summary>
    /// <remarks>Also expect double to be simply truncated into int so fence post errors are exceptionally likley</remarks>
    public interface ISingleVariableFunction<out TOut, in TIn>
    {
        TOut F(TIn x);
    }
}