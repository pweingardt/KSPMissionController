using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionController
{
    /// <summary>
    /// Manages the missions and the current space program (budget and so on)
    /// </summary>
    public class Manager : IManager
    {
        private Parser parser;

        private static Manager manager = new Manager();

        public static Manager instance {get { return manager; } }

        public static Settings settings = SettingsManager.Manager.getSettings();

        Randomizator3000.Item<int>[] contractslist;

        private SpaceProgram spaceProgram;
        private String currentTitle;

        private int latestExpenses = 0;

        public List<CurrentHires> currentHires = new List<CurrentHires>();
        public void Add(CurrentHires m)
        {
            currentHires.Add(m);
        }

        public bool showKerbalHireWindow = false;

        public void resetLatest()
        {
            latestExpenses = 0;
        }

        public Manager() {
            currentTitle = "default (Sandbox)";
            parser = new Parser();
            loadProgram (currentTitle);
        }

        /// <summary>
        /// Reset the current space program.
        /// </summary>
        public void resetSpaceProgram() {
            spaceProgram = SpaceProgram.generate ();
        }

        /// <summary>
        /// Recycles the vessel with the given costs.
        /// It is added to the recycled vessels list.
        /// </summary>
        /// <param name="vessel">Vessel.</param>
        /// <param name="costs">Costs.</param>
        public void recycleVessel(Vessel vessel, int costs) {
            if (!manager.isVesselFlagged(vessel))
            {
                recyclereward(costs);
            }    
        }

        public void loadProgram(String title) {
            currentTitle = title;
            try {
                spaceProgram = (SpaceProgram) parser.readFile (currentSpaceProgramFile);
            } catch {
                spaceProgram = SpaceProgram.generate();
            }
        }

        public void loadProgramBackup(String title)
        {
            currentTitle = title;
            try
            {
                spaceProgram = (SpaceProgram)parser.readFile(currntSpaceProgramFileBackup);
            }
            catch { spaceProgram = SpaceProgram.generate(); }
        }

        /// <summary>
        /// Discards the given random mission.
        /// Removed it from the random missions list
        /// </summary>
        /// <param name="m">M.</param>
        public void discardRandomMission(Mission m) {
            if (m.randomized) {
                RandomMission rm = currentProgram.findRandomMission (m);
                if(rm != null) {
                    currentProgram.randomMissions.Remove (rm);
                }
            }
        }

        /// <summary>
        /// Loads the given mission package
        /// </summary>
        /// <returns>The mission package.</returns>
        /// <param name="path">Path.</param>
        public MissionPackage loadMissionPackage(String path) {
            MissionPackage pkg = (MissionPackage) parser.readFile (path);
            Mission.Sort (pkg.Missions, pkg.ownOrder ? SortBy.PACKAGE_ORDER : SortBy.NAME);
            return pkg;
        }

        /// <summary>
        /// Reloads the given mission for the given vessel. Checks for already finished mission goals
        /// and reexecutes the instructions.
        /// </summary>
        /// <returns>reloaded mission</returns>
        /// <param name="m">mission</param>
        /// <param name="vessel">vessel</param>
        public Mission reloadMission(Mission m, Vessel vessel) {
            int count = 1;

            if (m.randomized) {
                RandomMission rm = currentProgram.findRandomMission (m);
                System.Random random = null;

                if (rm == null) {
                    rm = new RandomMission ();
                    rm.seed = new System.Random ().Next ();
                    rm.missionName = m.name;
                    currentProgram.add (rm);
                }

                random = new System.Random (rm.seed);
                m.executeInstructions (random);
            } else {
                // Maybe there are some instructions. We have to execute them!!!
                m.executeInstructions (new System.Random());
            }

            foreach(MissionGoal c in m.goals) {
                c.id = m.name + "__PART" + (count++);
                c.repeatable = m.repeatable;
                c.doneOnce = false;
            }

            if (vessel != null) {
                foreach (MissionGoal g in m.goals) {
                    if(isMissionGoalAlreadyFinished(g, vessel) && g.nonPermanent) {
                        g.doneOnce = true;
                    }
                }
            }

            return m;
        }

        /// <summary>
        /// Returns the current space program.
        /// </summary>
        /// <value>The current program.</value>
        private SpaceProgram currentProgram 
        {
            get { 
                if(spaceProgram == null) {
                    loadProgram(currentTitle);                   
                }
                return spaceProgram;
            }
            
        }
       

        /// <summary>
        /// Saves the current space program
        /// </summary>
        public void saveProgram() {
            if (spaceProgram != null) {
                parser.writeObject (spaceProgram, currentSpaceProgramFile);               
            }
        }

        /// <summary>
        /// saves a backup of the .sp
        /// </summary>
        public void saveProgramBackup()
        {
            if (spaceProgram != null) {
                parser.writeObject(spaceProgram, currntSpaceProgramFileBackup);
            }
        }

        /// <summary>
        /// Returns the current file, in which the current space program has been saved
        /// </summary>
        /// <value>The current space program file.</value>
        private String currentSpaceProgramFile {
            get {
                return currentTitle + ".sp";
            }
        }

        /// <summary>
        /// This is the Backup file for .sp. Used in the Revert Process
        /// </summary>
        private String currntSpaceProgramFileBackup
        {
            get
            {
                return currentTitle + ".backup";
            }
        }

        /// <summary>
        /// adds flaged vessel to the List
        /// </summary>
        /// <param name="vessel"></param>
        public void addFlagedVessel(Vessel vessel)
        {
            currentProgram.add(new FlagSystem(vessel.id.ToString()));
            saveProgram();
        }

        /// <summary>
        /// Finishes the given mission goal with the given vessel.
        /// Rewards the space program with the reward from mission goal.
        /// </summary>
        /// <param name="goal">Goal.</param>
        /// <param name="vessel">Vessel.</param>
        public void finishMissionGoal(MissionGoal goal, Vessel vessel, GameEvent events) {
            if (!isMissionGoalAlreadyFinished(goal, vessel) && goal.special)
            {
                currentProgram.add(new GoalStatus(goal.id));
                reward(goal.reward);
                totalReward(goal.reward);
            }

            if (!isMissionGoalAlreadyFinished (goal, vessel) && goal.nonPermanent && goal.isDone(vessel, events)) {
                currentProgram.add(new GoalStatus(vessel.id.ToString(), goal.id));
                reward (goal.reward);
                totalReward(goal.reward);
            }
        }

        public void StartRandomsystem()
        {
            contractslist = new Randomizator3000.Item<int>[3];
            contractslist[0] = new Randomizator3000.Item<int>();
            contractslist[0].weight = 30;
            contractslist[0].value = 0;

            contractslist[1] = new Randomizator3000.Item<int>();
            contractslist[1].weight = 20;
            contractslist[1].value = 1;

            contractslist[2] = new Randomizator3000.Item<int>();
            contractslist[2].weight = 50;
            contractslist[2].value = 2;
        }

        public void getContractType()
        {
            SetCurrentContract(Randomizator3000.PickOne<int>(contractslist));
            Debug.Log(GetCurrentContract + "This is current Contract Type Chosen by Random System");
            saveProgram();

        }

        public void DisplayContractType()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box("" + GetCurrentContract, GUILayout.Width(225), GUILayout.Height(30));
            GUILayout.EndHorizontal();
        }

        public void SetClockCountdown()
        {
            if (currentProgram.nextTimeCheck == 0)
            {
                double currentTime;
                currentTime = Planetarium.GetUniversalTime();
                currentProgram.nextTimeCheck = currentTime + 86400;
                Debug.Log("next contract check on date: " + MathTools.secondsIntoRealTime(currentProgram.nextTimeCheck));
            }
        }

        public void checkClockTiime()
        {
            double currentTime;
            currentTime = Planetarium.GetUniversalTime();
            if (currentTime >= currentProgram.nextTimeCheck)
            {
                getContractType();
                currentProgram.nextTimeCheck = 0;
                SetClockCountdown();
                Debug.Log(GetCurrentContract + " This is current Contract Type Chosen by Random System On Date: " + MathTools.secondsIntoRealTime(currentProgram.nextTimeCheck));
            }
        }
       
        /// <summary>
        /// Returns true, if the given mission goal has been finish in another game with the given vessel, false otherwise
        /// </summary>
        /// <returns><c>true</c>, if mission goal already finished, <c>false</c> otherwise.</returns>
        /// <param name="c">goal</param>
        /// <param name="v">vessel</param>
        public bool isMissionGoalAlreadyFinished(MissionGoal c, Vessel v) {
            if (v == null) 
            {
                return false;
            }

            foreach (GoalStatus con in currentProgram.completedGoals) 
            {
                // this is to help Undock Behave with Vessel Ids
                if(c.special && con.id.Equals(c.id))
                {
                    return true;
                }
                // If the mission goal is an EVAGoal, we don't care about the vessel id. Otherwise we do...
                if(con.id.Equals(c.id) && (con.vesselGuid.Equals (v.id.ToString()) || c.vesselIndenpendent)) {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// decides if vessel is flagged for it cannot do missions
        /// Either launched in Test Mode or Plugin Disabled
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool isVesselFlagged(Vessel v)
        {
            if (v == null)
            {
                return false;
            }
            foreach (FlagSystem fs in currentProgram.flagSystem)
            {
                if (fs.flagVesselGuid.Equals(v.id.ToString()))
                {
                    return true;
                }
            }
           return false;
        }

        public void displayKerbalList()
        {
            foreach (HiredKerbals hk in currentProgram.hiredkerbal)
            {
               
                GUILayout.BeginHorizontal();
                GUILayout.Box(hk.hiredKerbalName, GUILayout.Width(200));
                GUILayout.Box(MathTools.secondsIntoRealTime(hk.DateHired), GUILayout.Width(150));
                GUILayout.Box(hk.statusKerbal, GUILayout.Width(125));
                GUILayout.EndHorizontal();
            }
        }

        public void displayEndedMissionList()
        {
            foreach (MissionStatus ms in currentProgram.completedMissions)
            {
                GUILayout.BeginHorizontal();;
                GUILayout.Box(ms.missionName, GUILayout.Width(425));
                GUILayout.Box(MathTools.secondsIntoRealTime(ms.endTime), GUILayout.Width(160));
                GUILayout.Box(ms.vesselName,GUILayout.Width(275));
                GUILayout.Box("$ " + ms.payment.ToString("N2"), GUILayout.Width(140));
                GUILayout.EndHorizontal();
            }
        }

        public void displayCurrentHiredList()
        {
            foreach (CurrentHires ch in currentHires)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(ch.hiredKerbalName, GUILayout.Width(200));
                GUILayout.Box("$ " + ch.hiredCost.ToString("N2"), GUILayout.Width(200));
                GUILayout.EndHorizontal();
            }
        }

        public void displayShipList()
        {
            foreach (VesselsMade vm in currentProgram.vesselsMade)
            {
                GUILayout.BeginHorizontal(); ;
                GUILayout.Box(vm.vesselName, GUILayout.Width(250));
                GUILayout.Box(vm.MissionName, GUILayout.Width(400));
                GUILayout.Box("" + vm.crewNumber, GUILayout.Width(125));
                GUILayout.Box("$ " + vm.vesselCost.ToString("N2"), GUILayout.Width(125));
                GUILayout.EndHorizontal();
            }
        }
      
        /// <summary>
        /// Checks to see if Kerbal Was Hired.  This was inspired by Kerbal Story Missions, with permission from author to use
        /// </summary>
        public void isKerbalHired()
        {

            foreach (ProtoCrewMember CrewMember in HighLogic.CurrentGame.CrewRoster)
            {
                if (CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.AVAILABLE || CrewMember.rosterStatus == ProtoCrewMember.RosterStatus.ASSIGNED)
                {

                    if (!currentProgram.hiredkerbal.Exists(H => H.hiredKerbalName == CrewMember.name))
                    {
                        currentProgram.add(new HiredKerbals(CrewMember.name, Planetarium.GetUniversalTime(), CrewMember.rosterStatus.ToString()));
                        manager.kerbCost(FinanceMode.KerbalHiredCost);
                        showKerbalHireWindow = true;
                        currentHires.Add(new CurrentHires(CrewMember.name,FinanceMode.KerbalHiredCost));
                    }

                }
            }
        }

        public void recordVesselInfo(Mission m, Vessel vessel)
        {
            if (m == null)
            {
                string missionName = "Player Launched Mission";
                currentProgram.add(new VesselsMade(vessel.GetName(), latestExpenses, missionName, vessel.GetCrewCount()));
            }
            else
            {
                currentProgram.add(new VesselsMade(vessel.GetName(), latestExpenses, m.name, vessel.GetCrewCount()));
            }
        }

        /// <summary>
        /// Finishes the given mission with the given vessel.
        /// Rewards the space program with the missions reward.
        /// </summary>
        /// <param name="m">mission</param>
        /// <param name="vessel">vessel</param>
        public void finishMission(Mission m, Vessel vessel, GameEvent events) {
            if (!isMissionAlreadyFinished (m, vessel) && m.isDone(vessel, events)) {
                MissionStatus status = new MissionStatus (m.name, vessel.id.ToString ());
                status.repeatable = m.repeatable;
                status.repeatableSameVessel = m.repeatableSameVessel;
                status.vesselName = vessel.GetName();
                status.endTime = Planetarium.GetUniversalTime();
                status.payment = m.reward;

                if (m.passiveMission) {
                    status.endOfLife = Planetarium.GetUniversalTime () + m.lifetime;
                    status.passiveReward = m.passiveReward;
                    status.lastPassiveRewardTime = Planetarium.GetUniversalTime ();
                }

                if(m.clientControlled) {
                    status.endOfLife = Planetarium.GetUniversalTime () + m.lifetime;
                }

                status.clientControlled = m.clientControlled;

                currentProgram.add(status);
                reward (m.reward);
                totalReward(m.reward);
                sciencereward(m.scienceReward);
      
                
                    
                
                
                // finish unfinished goals
                foreach(MissionGoal goal in m.goals) {
                    finishMissionGoal(goal, vessel, events);
                }

                // If this mission is randomized, we will discard the mission
                if (m.randomized) {
                    discardRandomMission (m);
                    m = reloadMission (m, vessel);
                }
            }
        }

        /// <summary>
        /// If true, the given mission name has been finished. False otherwise.
        /// </summary>
        /// <returns><c>true</c>, if mission already finished, <c>false</c> otherwise.</returns>
        /// <param name="name">mission name</param>
        public bool isMissionAlreadyFinished(String name) {
            foreach (MissionStatus s in currentProgram.completedMissions) {
                if (s.missionName.Equals (name)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true, if the given mission has been finished with the given vessel.
        /// </summary>
        /// <returns><c>true</c>, if mission already finished, <c>false</c> otherwise.</returns>
        /// <param name="m">mission</param>
        /// <param name="v">vessel</param>
        public bool isMissionAlreadyFinished(Mission m, Vessel v) {
            if (v == null) {
                return false;
            }

            foreach (MissionStatus s in currentProgram.completedMissions) {
                if(s.missionName.Equals(m.name)) {
                    if(m.repeatable) {
                        if(s.vesselGuid.Equals(v.id.ToString())) {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all passive missions that are currently active. Removes old passive missions
        /// </summary>
        /// <returns>The passive missions.</returns>
        public List<MissionStatus> getActivePassiveMissions() {
            List<MissionStatus> status = new List<MissionStatus> ();
            List<MissionStatus> removable = new List<MissionStatus> ();

            if (!HighLogic.LoadedSceneHasPlanetarium) {
                return status;
            }

            foreach (MissionStatus s in currentProgram.completedMissions.Where(s => s.endOfLife != 0.0 && s.passiveReward > 0)) {
                if (s.endOfLife <= Planetarium.GetUniversalTime ()) {
                    if (s.repeatableSameVessel) {
                        removable.Add (s);
                    } else {
                        s.endOfLife = 0.0;
                        s.passiveReward = 0;
                    }
                } else {
                    status.Add (s);
                }
            }

            // Cleanup old missions
            // We don't clean up for now. We want to show how long the mission is still active
            // and if it not active any longer, we will show it.
            foreach (MissionStatus r in removable) {
                currentProgram.completedMissions.Remove (r);
            }
            return status;
        }

        /// <summary>
        /// Gets all client controlled missions
        /// </summary>
        /// <returns>The client controlled missions.</returns>
        public List<MissionStatus> getClientControlledMissions() {
            List<MissionStatus> status = new List<MissionStatus> ();
            List<MissionStatus> removable = new List<MissionStatus> ();

            if (!HighLogic.LoadedSceneHasPlanetarium) {
                return status;
            }

            foreach (MissionStatus s in currentProgram.completedMissions.Where(s => s.clientControlled)) {
                if (s.endOfLife <= Planetarium.GetUniversalTime ()) {
                    if (s.repeatableSameVessel) {
                        removable.Add (s);
                    } else {
                        s.clientControlled = false;
                    }
                } else {
                    status.Add (s);
                }
            }

            // Cleanup old missions
            // We don't clean up for now. We want to show how long the mission is still active
            // and if it not active any longer, we will show it.
            foreach (MissionStatus r in removable) {
                currentProgram.completedMissions.Remove (r);
            }

            return status;
        }

        /// <summary>
        /// Gets the client controlled mission that has been finished with the given vessel
        /// </summary>
        /// <returns>The client controlled mission.</returns>
        /// <param name="vessel">Vessel.</param>
        public MissionStatus getClientControlledMission(Vessel vessel) {
            foreach (MissionStatus s in currentProgram.completedMissions) {
                if(s.clientControlled && s.vesselGuid.Equals(vessel.id.ToString())) {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the currently available budget.
        /// </summary>
        /// <value>The budget.</value>
        public int budget {
            get { return currentProgram.money; }
        }

        /// <summary>
        /// Returns the Total Amount Made In Space Program Since Start Of Game.
        /// </summary>
        public int Totalbudget
        {
            get { return currentProgram.totalMoney; }
        }

        /// <summary>
        /// Returns the Total Amount Made In Recycling.
        /// </summary>
        public int TotalRecycleMoney
        {
            get { return currentProgram.totalrecycleMoney; }
        }
       
        /// <summary>
        /// Retuns Total Spent On Vehicle Launches
        /// </summary>
        public int TotalSpentVechicles
        {
            get { return currentProgram.totalSpentVessels; }
        }

        public int TotalHiredKerbCost
        {
            get { return currentProgram.TotalSpentKerbals; }
        }

       /// <summary>
       /// Research For Recycle
       /// Set Research True
       /// </summary>
        public bool ResearchRecycle
        {
            get { return currentProgram.VRecylce; }
        }
        public bool SetResearchRecycle()
        {
            return currentProgram.VRecylce = true;
        }
        // End Research Recycle

        /// <summary>
        /// Research For Fuels
        /// Set Fuels True
        /// Set Switch Fuels to 1
        /// </summary>
        public bool ResearchFuels
        {
            get { return currentProgram.VFuels; }
        }
        public bool SetResearchFuels()
        {
             return currentProgram.VFuels = true;
        }
        public int GetFuels
        {
            get { return currentProgram.fuelmode; }
        }
        public int SetFuels()
        {
            return currentProgram.fuelmode = 1;
        }
        
        /// <summary>
        /// sets the current Loan Modes for money and negative budget
        /// </summary>
        public double GetLoanMode
        {
            get { return currentProgram.loanmode; }
        }
        public double SetLoanMode(int value)
        {
            return currentProgram.loanmode = value;
        }
        // End Research Fuels

        public int GetCurrentContract
        {
            get { return currentProgram.currentcontractType; }
        }

        public int SetCurrentContract(int value)
        {
            return currentProgram.currentcontractType = value;
        }

        /// <summary>
        /// Research For Construction Levels
        /// Set Construction True
        /// Set Switch Construction to 1 or 2
        /// </summary>
        public bool ResearchConstruction1
        {
            get { return currentProgram.VConstruction1; }
        }
        public bool ResearchConstruction2
        {
            get { return currentProgram.VConstruction2; }
        }
        public bool SetResearchConstruction1()
        {
            return currentProgram.VConstruction1 = true;
        }
        public bool SetResearchConstruction2()
        {
            return currentProgram.VConstruction2 = true;
        }
        public int GetConstruction
        {
            get { return currentProgram.constructmode; }
        }
        public int SetConstruction(int value)
        {
            return currentProgram.constructmode = value ;
        }
        // End Construction Research

        public bool MissionLevel2
        {
            get { return currentProgram.missionlevel2; }
        }
        public bool MissionLevel3
        {
            get { return currentProgram.missionlevel3; }
        }
        public bool SetMissionLevel2()
        {
            return currentProgram.missionlevel2 = true;
        }
        public bool SetMissionLevel3()
        {
            return currentProgram.missionlevel3 = true;
        }
        public int GetCurrentPayoutLevel
        {
            get { return currentProgram.currentpayoutlevel; }
        }
        public int SetCurrentPayoutLevel(int num)
        {
            return currentProgram.currentpayoutlevel = num;
        }
         
        /// <summary>
        /// Checks if the given vessel is controlled by a client.
        /// </summary>
        /// <returns><c>true</c>, if vessel is controlled by a client, <c>false</c> otherwise.</returns>
        /// <param name="vessel">Vessel.</param>
        public bool isClientControlled(Vessel vessel) {
            if (!HighLogic.LoadedSceneHasPlanetarium) {
                return false;
            }

            foreach (MissionStatus status in currentProgram.completedMissions) {
                if (status.clientControlled && status.vesselGuid.Equals (vessel.id.ToString()) &&
                        status.endOfLife >= Planetarium.GetUniversalTime()) {
                    return true;
                }
            }
            return false;
        }
      
        /// <summary>
        /// Checks if the given vessel in on a passive mission
        /// </summary>
        /// <returns><c>true</c>, if on passive mission was ised, <c>false</c> otherwise.</returns>
        /// <param name="vessel">Vessel.</param>
        public bool isOnPassiveMission(Vessel vessel) {
            foreach (MissionStatus status in currentProgram.completedMissions) {
                if (status.passiveReward != 0 && status.vesselGuid.Equals (vessel.id.ToString()) &&
                        status.endOfLife >= Planetarium.GetUniversalTime()) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the passive mission for the given vessel.
        /// </summary>
        /// <returns>The passive mission.</returns>
        /// <param name="vessel">Vessel.</param>
        public MissionStatus getPassiveMission(Vessel vessel) {
            foreach (MissionStatus s in currentProgram.completedMissions) {
                if(s.passiveReward != 0 && s.vesselGuid.Equals(vessel.id.ToString())) {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Rewinds the latest expenses.
        /// </summary>
        public void rewind() {
            reward (latestExpenses);
            latestExpenses = 0;
        }

        /// <summary>
        /// Removes an mission status
        /// </summary>
        /// <param name="s">S.</param>
        public void removeMission(MissionStatus s) {
            currentProgram.completedMissions.Remove (s);
        }

        // The interface implementation for external plugins

        public int getBudget() {
            return currentProgram.money;
        }
        // Malkuth Edit This is where the program does the checks for Bank Loans and Difficulties for it chanrges gives the correct amounts for payouts
        
        /// <summary>
        /// the reward for finishing a missioin, this is the payment.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int reward(int value)
        {
            Vessel v = new Vessel();
            if (!SettingsManager.Manager.getSettings().disablePlugin)
            {
                if(value > 0) // is a reward, not a cost
                {
                    double mult = 1.0;
                    if (manager.budget < 0)
                        mult *= FinanceMode.currentloan;
                    if (settings.gameMode == 1)
                        mult *= 0.6;
                    value = (int)((double)value * mult);
                }
                latestExpenses = -value;
                currentProgram.money += (int)((double)value * PayoutLeveles.TechPayout);
                
            }
            return currentProgram.money;
        }

        /// <summary>
        /// Used Only To Keep Track Of Total Payouts to Budget
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int totalReward(int value)
        {
            Vessel v = new Vessel();
            if (!SettingsManager.Manager.getSettings().disablePlugin)
            {
                if (value > 0) // is a reward, not a cost
                {
                    double mult = 1.0;
                    if (manager.budget < 0)
                        mult *= FinanceMode.currentloan;
                    if (settings.gameMode == 1)
                        mult *= 0.6;
                    value = (int)((double)value * mult);
                }
                currentProgram.totalMoney += (int)((double)value * PayoutLeveles.TechPayout);
            }
            return currentProgram.totalMoney;
        }

        /// <summary>
        /// Used to payout a reward for recycling only
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int recyclereward(int value) {
            if (!SettingsManager.Manager.getSettings().disablePlugin)
            {
                currentProgram.money += value;
                currentProgram.totalrecycleMoney += value;

            }
            
            return currentProgram.money;}

       /// <summary>
       /// Option reward for missions for science.. Use with care... 
       /// </summary>
       /// <param name="value"></param>
       /// <returns></returns>
        public float sciencereward(float value)
        {
            ConstructionMode cn = new ConstructionMode();
            return cn.Science += value; 
            
        }

        public int costs(int value) 
        {
            currentProgram.totalSpentVessels += value;
            return reward (-value);            
        }

        public int kerbCost(int value)
        {
            currentProgram.TotalSpentKerbals += value;
            return reward(-value);
        }
    }

    public class CurrentHires
    {
        public string hiredKerbalName;
        public int hiredCost;

        public CurrentHires()
        {
        }

        public CurrentHires(string kerbalname, int value)
        {
            this.hiredKerbalName = kerbalname;
            this.hiredCost = value;
        }
    }
}

