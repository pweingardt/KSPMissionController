using System;
using System.Collections.Generic;

namespace MissionController
{
	public class SpaceProgram
	{
		public int money;

        public List<MissionStatus> completedMissions = new List<MissionStatus>();

        public List<GoalStatus> completedGoals = new List<GoalStatus> ();

        public void launch(int costs) {
            money -= costs;
        }

        public void reuse(int costs) {
            money += costs;
        }

        public void add(MissionStatus m) {
            completedMissions.Add (m);
        }

        public void add(GoalStatus m) {
            completedGoals.Add (m);
        }

        public static SpaceProgram generate() {
            SpaceProgram sp = new SpaceProgram ();
            sp.money = 50000;
            return sp;
        }

        public void prepareMission(Mission m) {
            foreach (MissionGoal goal in m.goals) {

            }
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
}

