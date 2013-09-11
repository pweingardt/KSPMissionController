using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A space program.
    /// </summary>
	public class SpaceProgram
	{
		public int money;

        public List<MissionStatus> completedMissions = new List<MissionStatus>();

        public List<GoalStatus> completedGoals = new List<GoalStatus> ();

        public List<RecycledVessel> recycledVessels = new List<RecycledVessel> ();

        public List<RandomMission> randomMissions = new List<RandomMission> ();

        public void add(MissionStatus m) {
            completedMissions.Add (m);
        }

        public void add(GoalStatus m) {
            completedGoals.Add (m);
        }

        public void add(RecycledVessel vessel) {
            recycledVessels.Add (vessel);
        }

        public void add(RandomMission mission) {
            randomMissions.Add (mission);
        }

        public static SpaceProgram generate() {
            SpaceProgram sp = new SpaceProgram ();
            sp.money = 50000;
            return sp;
        }

        public RandomMission findRandomMission(Mission m) {
            foreach (RandomMission rm in randomMissions) {
                if(rm.missionName.Equals(m.name)) {
                    // The random mission has been loaded already. so we need to reload it with the given seed
                    return rm;
                }
            }
            return null;
        }
	}

    public class GoalStatus {
        public String id;
        public String vesselGuid;
        public bool repeatable;

        public GoalStatus() {
        }

        public GoalStatus(String vesselGuid, String id) {
            this.vesselGuid = vesselGuid;
            this.id = id;
        }

        public GoalStatus(String id)
        {
            this.id = id;
        }
    }

    public class MissionStatus {
        public MissionStatus() {
        }

        public MissionStatus(String mission, String vesselGuid) {
            this.missionName = mission;
            this.vesselGuid = vesselGuid;
        }

        public String vesselGuid;
        public String missionName;
        public bool repeatable;
        public bool repeatableSameVessel = false;

        public double endOfLife = 0.0;
        public int passiveReward = 0;
        public double lastPassiveRewardTime = 0.0;
        public int punishment = 100000;

        public bool clientControlled = false;
    }

    public class RandomMission {
        public String missionName;
        public int seed;
    }

    public class RecycledVessel {
        public String guid;
    }
}

