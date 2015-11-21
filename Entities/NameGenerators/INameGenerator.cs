namespace Entities.NameGenerators
{
    /// <summary>
    /// Returns a Generated name.
    /// </summary>
    /// <remarks>
    /// Intended to be used with some set of generation rules.
    /// </remarks>
    public interface INameGenerator
    {
        string GenerateName();
    }
}