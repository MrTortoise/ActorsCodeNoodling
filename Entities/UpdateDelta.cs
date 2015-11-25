using System;

namespace Entities
{
    /// <summary>
    /// The update delta message for objects that are part of a regular every frame update loop
    /// </summary>
    /// <remarks>graphics vs physics vs sound are probably on different cadence</remarks>
    public class UpdateDelta
    {
        public UpdateDelta(TimeSpan delta)
        {
            Delta = delta;
        }

        /// <summary>
        /// The <see cref="TimeSpan"/> since last update
        /// </summary>
        public TimeSpan Delta { get; private set; }
    }
}