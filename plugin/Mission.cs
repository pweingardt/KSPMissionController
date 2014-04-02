using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A mission
    /// </summary>
    public class Mission : InstructionSet
    {
        /// The enum for the mission category
        /// The Flags annotation declares a bitfield
        [Flags]
        public enum Category {DEFAULT = 1, ORBIT = 2, LANDING = 4, DOCKING = 8, EVA = 16, MINING = 32,
            SATELLITE = 64, PROBE = 128, IMPACT = 256, TIME = 512, MANNED = 1024, AVIATION = 2048, SCIENCE = 4096,
            COMMUNICATION = 8192, ROVER = 16384, REPAIR = 32768
        };

        /// <summary>
        /// Mission category
        /// </summary>
        public Category category = Category.DEFAULT;

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
        /// Displays the vessel name for Repair Missions See IsRepair For other checks!
        /// </summary>
        public bool vesselName = false;

        /// <summary>
        /// This helps Hide contracts from the Regular Missions
        /// </summary>
        public bool IsContract = false;
        public bool IsUserContract = false;
        public bool isRoverMission = false;
        public bool isRepairMission = false;

        public int CompanyOrder;

        /// <summary>
        /// checks if repair mission if so, another check is made to see if Vessel Has RepairParts.  If both are true Repair Contract Will Show.
        /// </summary>
        public bool IsRepair = false;

        public bool isAsteroidCapture = false;

        /// <summary>
        /// science reward for finishing the mission
        /// </summary>
        public float scienceReward = 0;

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
        /// The order in the mission package.
        /// Set to a very high number so that it is the last mission
        /// </summary>
        public int packageOrder = 1000;

        /// <summary>
        /// Sets the contract Number that the randomizer checks for.  Each contract can only be 1 number. If repeat both contracts will show at same time, no matter what.  Limited by Randomizer weight System.
        /// </summary>
        public int contractAvailable = 0;

        /// <summary>
        /// If true, then this mission is a passive mission with a lifetime and income gerneration
        /// </summary>
        public bool passiveMission = false;

        /// <summary>
        /// The passive reward per day
        /// </summary>
        public int passiveReward = 0;

        /// <summary>
        /// The punishment if the user destroyed the vessel
        /// </summary>
        public int destroyPunishment = 100000;

        /// <summary>
        /// if true then the vessel that finished this mission is controlled by the client, not by the user
        /// </summary>
        public bool clientControlled = false;

        /// <summary>
        /// Lifetime of this mission in secondes. use TIME.
        /// </summary>
        public double lifetime = 0;

        /// <summary>
        /// If this field is not empty, then this mission requires another mission to be finished. If it has not been finished, 
        /// then this mission can not be finished either.
        /// </summary>
        public String requiresMission = "";

        /// <summary>
        /// If true, then the repeatable mission (repeatable must be true), then this mission is repeatable with the same vessel.
        /// This field is ignored, when the mission is not client controlled of passive!
        /// </summary>
        public bool repeatableSameVessel = false;

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
        public bool isDone(Vessel vessel, GameEvent events) {
            if (vessel == null) {
                return false;
            }

            foreach (MissionGoal c in goals) {
                if (!c.isDone (vessel, events) && !c.optional) {
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


        public static int SortByReward(Mission x, Mission y) {
            return x.reward.CompareTo(y.reward);
        }

        public static int SortByName(Mission x, Mission y) {
            return x.name.CompareTo(y.name);
        }

        public static int SortByPackageOrder(Mission x, Mission y) {
            return x.packageOrder.CompareTo(y.packageOrder);
        }

        /// <summary>
        /// Sorts the given missions with the given method
        /// </summary>
        /// <param name="missions">Missions.</param>
        /// <param name="sortBy">Sort by.</param>
        public static void Sort(List<Mission> missions, SortBy sortBy) {
            if (sortBy == SortBy.NAME) {
                missions.Sort (Mission.SortByName);
            } else if (sortBy == SortBy.REWARD) {
                missions.Sort (Mission.SortByReward);
            } else if (sortBy == SortBy.PACKAGE_ORDER) {
                missions.Sort (Mission.SortByPackageOrder);
            }
        }
    }
}

