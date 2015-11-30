namespace Entities.LocationActors
{
    /// <summary>
    /// the type of the celestial body
    /// </summary>
    /// <remarks>
    /// Whilst very crude now expect these 3 values to be broekn down a lot in the future.
    /// The worry is that material and this value will start to mirror each other.
    /// </remarks>
    public enum CelestialBodyType
    {
        Star,
        Planet,
        Moon
    }
}