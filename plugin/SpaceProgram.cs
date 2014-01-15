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
        public int totalMoney;
        public int totalSpentVessels;
        public int TotalSpentKerbals;
        public int totalrecycleMoney;
        public int fuelmode = 0;
        public int constructmode = 0;
        public bool VRecylce = false;
        public bool VFuels = false;
        public bool VConstruction1 = false;
        public bool VConstruction2 = false;
        public double loanmode = 0;
        public int currentpayoutlevel = 0;
        public bool missionlevel2 = false;
        public bool missionlevel3 = false;
        public int currentcontractType = 0;

        public List<MissionStatus> completedMissions = new List<MissionStatus>();

        public List<GoalStatus> completedGoals = new List<GoalStatus> ();

        public List<RecycledVessel> recycledVessels = new List<RecycledVessel> ();

        public List<RandomMission> randomMissions = new List<RandomMission> ();

        public List<FlagSystem> flagSystem = new List<FlagSystem>();

        public List<HiredKerbals> hiredkerbal = new List<HiredKerbals>();

        public List<VesselsMade> vesselsMade = new List<VesselsMade>();

        public void add(FlagSystem m)
        {
            flagSystem.Add(m);
        }      

        public void add(VesselsMade m)
        {
            vesselsMade.Add(m);
        }

        public void add(HiredKerbals m)
        {
            hiredkerbal.Add(m);
        }

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
            sp.totalMoney = 50000;
            foreach (ProtoCrewMember CrewMember in HighLogic.CurrentGame.CrewRoster)
            {
                if (CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.AVAILABLE || CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.ASSIGNED)
                    sp.add(new HiredKerbals(CrewMember.name, Planetarium.GetUniversalTime(), CrewMember.rosterStatus.ToString()));
            }
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
        public String vesselName;
        public int payment = 0;
        public int goalPayment = 0;
        public double endTime = 0.0;
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

    /// <summary>
    /// This Flags a vessel as not being able to do missionis.. 
    /// Either Vessel was launched during Testing Mode, or During Disabled Mode.
    /// </summary>
    public class FlagSystem
    {
        public String flagVesselGuid;

        public FlagSystem() 
        {
        }

        public FlagSystem(String idflagVesselGuid)
        {
            this.flagVesselGuid = idflagVesselGuid;
        }
    }
    /// <summary>
    /// manages the Hire Of Kerbals and keeps track of names.
    /// </summary>
    public class HiredKerbals
    {
        public string hiredKerbalName;
        public double DateHired;
        public string statusKerbal;


        public HiredKerbals()
        {
        }

        public HiredKerbals(string kerbalname, double date, string status)            
        {
            this.hiredKerbalName = kerbalname;
            this.DateHired = date;
            this.statusKerbal = status;
        }
    }

    /// <summary>
    /// Manages The List for Vessel names used in missions and there cost
    /// </summary>
    public class VesselsMade
    {
        public string vesselName;
        public int vesselCost;
        public string MissionName = "Player Mission";
        public int crewNumber;
            
        public VesselsMade()
        {
        }

        public VesselsMade(string vName, int cost, string mName, int cNum)
        {
            this.vesselName = vName;
            this.vesselCost = cost;
            this.MissionName = mName;
            this.crewNumber = cNum;
        }
    }   
}

