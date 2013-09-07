using System;

namespace MissionController
{
    /// <summary>
    /// A game event to check for mission goals, that are not checkable with the vessel alone.
    /// Used for CrashGoal
    /// </summary>
    public class GameEvent
    {
        public bool isCrashed = false;

        public bool docked = false;

        public bool isrepaired = false;
    }

    public enum EventFlags {NONE = 0, CRASHED = 1, DOCKED = 2, REPAIR = 3};
}

