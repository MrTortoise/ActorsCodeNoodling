using System;

namespace Entities.LocationActors
{
    /// <summary>
    /// Interface describing objects that update via a delta rather than a new <see cref="DateTimeOffset"/>
    /// </summary>
    public interface IUpdateDelta
    {
        /// <summary>
        /// Update the object and children usign given delta
        /// </summary>
        /// <param name="delta"></param>
        void UpdateDelta(TimeSpan delta);
    }
}