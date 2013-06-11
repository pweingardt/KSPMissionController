using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A bundle of missions.
    /// </summary>
    public class MissionPackage
    {
        /// <summary>
        /// The name of this bundle, should be unique
        /// </summary>
        public String name = "";

        /// <summary>
        /// A small description of your mission package.
        /// </summary>
        public String description = "";

        /// <summary>
        /// If true the mission package uses its own order mechanism. Use order for *all* missions
        /// </summary>
        public bool ownOrder = false;

        /// <summary>
        /// The missions in this package
        /// </summary>
        private List<Mission> missions = new List<Mission>();

        public List<Mission> Missions { get { return missions; } }

        public void add(Mission m) {
            missions.Add (m);
        }
    }
}

