using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A mission
    /// </summary>
    public class Mission
    {
        /// <summary>
        /// Unique name
        /// </summary>
        public String name;

        /// <summary>
        /// Missions Description
        /// </summary>
        public String description;

        /// <summary>
        /// Reward for finishing the mission
        /// </summary>
        public int reward = 100;

        /// <summary>
        /// If true, this mission is repeatable. You can't finish the same mission with one vessel twice.
        /// </summary>
        public bool repeatable = false;

        /// <summary>
        /// If true, the mission goals needs to be finished in the right order
        /// </summary>
        public bool inOrder = true;

        /// <summary>
        /// All mission goals
        /// </summary>
        private List<MissionGoal> mGoals = new List<MissionGoal>();

        /// <summary>
        /// If true, the mission will be initialized with a random seed and this seed is saved within the space program
        /// object. Once loaded (until discarded), the mission will stay the same.
        /// </summary>
        public bool randomized = false;

        /// <summary>
        /// If this field is not empty, then this mission requires another mission to be finished. If it has not been finished, 
        /// then this mission can not be finished either.
        /// </summary>
        public String requiresMission = "";

        public List<MissionGoal> goals { get { return mGoals; } }

        public override string ToString ()
        {
            return name;
        }

        /// <summary>
        /// Checks if the mission is finishable with the vessel
        /// </summary>
        /// <returns><c>true</c>, if done was ised, <c>false</c> otherwise.</returns>
        /// <param name="vessel">current vessel</param>
        public bool isDone(Vessel vessel) {
            if (vessel == null) {
                return false;
            }

            foreach (MissionGoal c in goals) {
                if (!c.isDone (vessel) && !c.optional) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Needed for reflection
        /// </summary>
        /// <param name="c">C.</param>
        public void add(MissionGoal c) {
            mGoals.Add (c);
        }
    }
}

