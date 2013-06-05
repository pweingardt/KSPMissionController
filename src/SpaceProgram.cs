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

        public List<ReusedVessel> reusedVessels = new List<ReusedVessel> ();

        public void add(MissionStatus m) {
            completedMissions.Add (m);
        }

        public void add(GoalStatus m) {
            completedGoals.Add (m);
        }

        public void add(ReusedVessel vessel) {
            reusedVessels.Add (vessel);
        }

        public static SpaceProgram generate() {
            SpaceProgram sp = new SpaceProgram ();
            sp.money = 50000;
            return sp;
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
    }

    public class ReusedVessel {
        public String guid;
    }
}

