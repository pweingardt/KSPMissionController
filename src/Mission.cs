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
        /// missions Description
        /// </summary>
        public String description;

        /// <summary>
        /// Reward for finishing the mission
        /// </summary>
        public int reward = 100;

        /// <summary>
        /// If true, this mission is repeatable. Reaptable missions can be finished with one vessel only once.
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

