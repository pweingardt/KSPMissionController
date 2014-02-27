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

        Randomizator3000.Item<int>[] contractslist1;

        Randomizator3000.Item<int>[] contractslist2;

        Randomizator3000.Item<string>[] companyListRandom;

        private SpaceProgram spaceProgram;
        private String currentTitle;

        public int currentgoalPayment = 0;
        private string currentgoalName;

        private int latestExpenses = 0;

        public List<CurrentHires> currentHires = new List<CurrentHires>();
        public void Add(CurrentHires m)
        {
            currentHires.Add(m);
        }

        public List<RepairVesselsList> repairvesselList = new List<RepairVesselsList>();
        public void Add(RepairVesselsList m)
        {
            repairvesselList.Add(m);
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
                Debug.Log("MCE Space Program save File Loaded: " + HighLogic.CurrentGame.Title.ToString() + ".sp");
            } catch {
                spaceProgram = SpaceProgram.generate();
                Debug.Log("MCE New space Program Created under Name: " + HighLogic.CurrentGame.Title.ToString() + ".sp");
            }
        }

        public void loadProgramBackup(String title)
        {
            currentTitle = title;
            try
            {
                spaceProgram = (SpaceProgram)parser.readFile(currntSpaceProgramFileBackup);
            }
            catch { Debug.Log("MCE No backup Save File Found, Skipping process"); }
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
        public Mission loadContractMission(String path)
        {
            Mission ucm = (Mission)parser.readFile(path);
            return ucm;
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
                currentgoalPayment = goal.reward;
                currentgoalName = goal.getType();
                if (currentgoalPayment >= 1)
                {
                    MissionController.showBonusPaymentsWindow = true;
                }
                saveProgram();
            }

            if (!isMissionGoalAlreadyFinished (goal, vessel) && goal.nonPermanent && goal.isDone(vessel, events)) {
                currentProgram.add(new GoalStatus(vessel.id.ToString(), goal.id));
                reward (goal.reward);
                totalReward(goal.reward);
                currentgoalPayment = goal.reward;
                currentgoalName = goal.getType();
                if (currentgoalPayment >= 1)
                {
                    MissionController.showBonusPaymentsWindow = true;
                }
                saveProgram();
            }
        }
 
        public void PrintGoalReward(string mn)
        {
            GUILayout.Space(5);
            GUILayout.Box("Mission Goal Complete For Mission: " + mn, GUILayout.Width(625));
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("$" + currentgoalPayment, GUILayout.Width(175));
            GUILayout.Box("Goal Reward for Mission: " + currentgoalName + " payed.", GUILayout.Width(450));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        /// <summary>
        /// the randomizer used to pick which contract type to use
        /// in future you can doulbe this, or make another one to have
        /// more than 1 contract type available at time.  Based on Weights.
        /// The higher the weight the more chance to select that option.
        /// As long as = 100 in weights in total.
        /// </summary>
        public void StartContractTypeRandom()
        {
            contractslist = new Randomizator3000.Item<int>[6];
            contractslist[0] = new Randomizator3000.Item<int>();
            contractslist[0].weight = 60;
            contractslist[0].value = 0;

            contractslist[1] = new Randomizator3000.Item<int>();
            contractslist[1].weight = 5;
            contractslist[1].value = 1;

            contractslist[2] = new Randomizator3000.Item<int>();
            contractslist[2].weight = 10;
            contractslist[2].value = 2;

            contractslist[3] = new Randomizator3000.Item<int>();
            contractslist[3].weight = 10;
            contractslist[3].value = 3;

            contractslist[4] = new Randomizator3000.Item<int>();
            contractslist[4].weight = 7;
            contractslist[4].value = 4;

            contractslist[5] = new Randomizator3000.Item<int>();
            contractslist[5].weight = 8;
            contractslist[5].value = 5;

        }
        public void StartContractType1Random()
        {
            contractslist1 = new Randomizator3000.Item<int>[9];
            contractslist1[0] = new Randomizator3000.Item<int>();
            contractslist1[0].weight = 34;
            contractslist1[0].value = 0;

            contractslist1[1] = new Randomizator3000.Item<int>();
            contractslist1[1].weight = 10;
            contractslist1[1].value = 6;

            contractslist1[2] = new Randomizator3000.Item<int>();
            contractslist1[2].weight = 10;
            contractslist1[2].value = 7;

            contractslist1[3] = new Randomizator3000.Item<int>();
            contractslist1[3].weight = 10;
            contractslist1[3].value = 8;

            contractslist1[4] = new Randomizator3000.Item<int>();
            contractslist1[4].weight = 10;
            contractslist1[4].value = 9;

            contractslist1[5] = new Randomizator3000.Item<int>();
            contractslist1[5].weight = 8;
            contractslist1[5].value = 10;

            contractslist1[6] = new Randomizator3000.Item<int>();
            contractslist1[6].weight = 8;
            contractslist1[6].value = 11;

            contractslist1[7] = new Randomizator3000.Item<int>();
            contractslist1[7].weight = 5;
            contractslist1[7].value = 12;

            contractslist1[8] = new Randomizator3000.Item<int>();
            contractslist1[8].weight = 5;
            contractslist1[8].value = 13;
        }
        public void StartContractType2Random()
        {
            contractslist2 = new Randomizator3000.Item<int>[9];
            contractslist2[0] = new Randomizator3000.Item<int>();
            contractslist2[0].weight = 50;
            contractslist2[0].value = 0;

            contractslist2[1] = new Randomizator3000.Item<int>();
            contractslist2[1].weight = 10;
            contractslist2[1].value = 14;

            contractslist2[2] = new Randomizator3000.Item<int>();
            contractslist2[2].weight = 9;
            contractslist2[2].value = 15;

            contractslist2[3] = new Randomizator3000.Item<int>();
            contractslist2[3].weight = 6;
            contractslist2[3].value = 16;

            contractslist2[4] = new Randomizator3000.Item<int>();
            contractslist2[4].weight = 6;
            contractslist2[4].value = 17;

            contractslist2[5] = new Randomizator3000.Item<int>();
            contractslist2[5].weight = 6;
            contractslist2[5].value = 18;

            contractslist2[6] = new Randomizator3000.Item<int>();
            contractslist2[6].weight = 6;
            contractslist2[6].value = 19;

            contractslist2[7] = new Randomizator3000.Item<int>();
            contractslist2[7].weight = 4;
            contractslist2[7].value = 20;

            contractslist2[8] = new Randomizator3000.Item<int>();
            contractslist2[8].weight = 3;
            contractslist2[8].value = 21;
        }
        /// <summary>
        /// This is the randomizer for Company Info.  Company Amounts is limited by this check.  The values can be changed in MCConfig though!
        /// </summary>
        public void StartCompanyRandomizer()
        {
            companyListRandom = new Randomizator3000.Item<string>[9];
            companyListRandom[0] = new Randomizator3000.Item<string>();
            companyListRandom[0].weight = 15;
            companyListRandom[0].value = "COMA";

            companyListRandom[1] = new Randomizator3000.Item<string>();
            companyListRandom[1].weight = 10;
            companyListRandom[1].value = "COMB";

            companyListRandom[2] = new Randomizator3000.Item<string>();
            companyListRandom[2].weight = 10;
            companyListRandom[2].value = "COMC";

            companyListRandom[3] = new Randomizator3000.Item<string>();
            companyListRandom[3].weight = 5;
            companyListRandom[3].value = "COMD";

            companyListRandom[4] = new Randomizator3000.Item<string>();
            companyListRandom[4].weight = 10;
            companyListRandom[4].value = "COME";

            companyListRandom[5] = new Randomizator3000.Item<string>();
            companyListRandom[5].weight = 5;
            companyListRandom[5].value = "COMF";

            companyListRandom[6] = new Randomizator3000.Item<string>();
            companyListRandom[6].weight = 15;
            companyListRandom[6].value = "COMG";

            companyListRandom[7] = new Randomizator3000.Item<string>();
            companyListRandom[7].weight = 15;
            companyListRandom[7].value = "COMH";

            companyListRandom[8] = new Randomizator3000.Item<string>();
            companyListRandom[8].weight = 15;
            companyListRandom[8].value = "COMI";
        }

        /// <summary>
        /// sets the contract type, randomly
        /// </summary>
        public void setContractType()
        {
            SetCurrentContract(Randomizator3000.PickOne<int>(contractslist));
            Debug.Log(GetCurrentContract + "This is current Contract Type 0 Chosen by Random System");
        }
        public void setContractType1()
        {
            SetCurrentContract1(Randomizator3000.PickOne<int>(contractslist1));
            Debug.Log(GetCurrentContract1 + "This is current Contract Type 1 Chosen by Random System");
        }

        public void setContractType2()
        {
            SetCurrentContract2(Randomizator3000.PickOne<int>(contractslist2));
            Debug.Log(GetCurrentContract2 + "This is current Contract Type 2 Chosen by Random System");
        }

        /// <summary>
        /// Does the Random Check For Company Info. Then sets it to save file
        /// </summary>
        public void setCompanyName()
        {
            SetCompanyInfoString(Randomizator3000.PickOne<string>(companyListRandom));
            Debug.Log(GetCompanyInfoString + " This is Current Company Info Chosen By Random System");
            SetCompanyInfoString2(Randomizator3000.PickOne<string>(companyListRandom));
            Debug.Log(GetCompanyInfoString2 + " This is Current Company Info Chosen By Random System");
            SetCompanyInfoString3(Randomizator3000.PickOne<string>(companyListRandom));
            Debug.Log(GetCompanyInfoString3 + " This is Current Company Info Chosen By Random System");           
        }
        public void setUserContractCompany()
        {
            SetCompanyInfoString4(Randomizator3000.PickOne<string>(companyListRandom));
            Debug.Log(GetCompanyInfoString4 + " This is Current Company Info Chosen By Random System");
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
        /// <summary>
        /// checks the saved time vs. the current universal time to
        /// check if its time to Reroll Contracts
        /// </summary>
        public void checkClockTiime()
        {
            double currentTime;
            currentTime = Planetarium.GetUniversalTime();
            if (currentTime >= currentProgram.nextTimeCheck)
            {
                clearVesselRepairFromList();
                findVeselWithRepairPart();
                StartContractTypeRandom();
                StartCompanyRandomizer();
                setContractType();
                StartContractType1Random();
                setContractType1();
                StartContractType2Random();
                setContractType2();
                currentProgram.nextTimeCheck = 0;
                SetClockCountdown();
                setCompanyName();
                chooseVesselRepairFromList();               
            }           
        }

        /// <summary>
        /// Used To Find Vessels With repairStation Modules
        /// I used modules because in future repairparts might be just a module added to 
        /// existing parts.. Maybe unmanned pods?
        /// </summary>
       
        public void findVeselWithRepairPart()
        {
            foreach (Vessel vs in FlightGlobals.Vessels)
            {
                foreach (ProtoPartSnapshot p in vs.protoVessel.protoPartSnapshots)
                {
                    foreach (ProtoPartModuleSnapshot m in p.modules)
                    {
                        if (m.moduleName.Equals("repairStation"))
                        {
                            repairvesselList.Add(new RepairVesselsList(vs.name));
                            Debug.Log("MCE***" + vs.name + " Loaded Vessels With RepairStation Parts");
                        }
                    }
                }
            }
            
        }
        /// <summary>
        /// This choses the Repair Vessel target.. The random part does not really work yet have to work on it. Think it only picks the first value in list.
        /// </summary>
        public void chooseVesselRepairFromList()
        {
            System.Random rnd = new System.Random();
            RepairVesselsList random = repairvesselList[rnd.Next(repairvesselList.Count)];
            SetShowVesselRepairName(random.vesselName.ToString());
            Debug.Log("Random Repair Vessel Selected " + random.vesselName);
            Debug.Log("Random Repair Vessel Saved To .sp File " + GetShowVesselRepairName);
        }
        public void clearVesselRepairFromList()
        {
            repairvesselList.Clear();
        }

        public void clearMissionGoals(string id)
        {
            currentProgram.completedGoals.RemoveAll(s => s.vesselGuid == id);
            saveProgram();        
        }
        public void clearMissionGoalByName(MissionGoal mg)
        {
            currentProgram.completedGoals.RemoveAll(s => s.id.Contains(mg.id.ToString()));
            saveProgram();
        }
        public void wipeAllMissionGoals()
        {
            currentProgram.completedGoals.Clear();
            saveProgram();
        }
        public void wipeAllFinishedMissions()
        {
            currentProgram.completedMissions.Clear();
            saveProgram();
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
        /// launched in T Plugin Disabled
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

        /// <summary>
        /// displays kerbalList Info For Hired Kerbals.
        /// </summary>
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

        /// <summary>
        /// displays the Mission List that is stored in save file
        /// </summary>
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

        /// <summary>
        /// Displays current hired Kerbals in a list. this is the pop up that says you hired a kerbal!
        /// </summary>
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

        /// <summary>
        /// Displays Current Ships Purchased for missions and contracts! 
        /// </summary>
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

        public void displayModCost()
        {
            foreach (ModCharges MC in currentProgram.modCharges)
            {
                GUILayout.BeginHorizontal(); ;
                GUILayout.Box("" + MC.amount, GUILayout.Width(250));
                GUILayout.Box(MC.ChageDescription, GUILayout.Width(400));
                GUILayout.EndHorizontal();
            }
        }

        public void displayModPayment()
        {
            foreach (ModPayments Mp in currentProgram.modPayments)
            {
                GUILayout.BeginHorizontal(); ;
                GUILayout.Box("" + Mp.amount, GUILayout.Width(250));
                GUILayout.Box(Mp.PaymentDescription, GUILayout.Width(400));
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

                if (m.IsContract == false)
                {
                    reward(m.reward);
                    totalReward(m.reward);
                    if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                    {
                        sciencereward(m.scienceReward);
                    }
                    Debug.Log("reward Normal Mission Award");
                }
                if (m.IsContract == true)
                {                                       
                    contractReward(m.reward, m.CompanyOrder);
                    if (m.IsUserContract != true) {Setrandomcontractfreeze(false);}
                    if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                    {
                        Contractsciencereward(m.scienceReward, m.CompanyOrder);
                    }                   
                    Debug.Log("Reward Contract Mission Award");
                }
                    
                
                
                // finish unfinished goals
                foreach(MissionGoal goal in m.goals) {
                    finishMissionGoal(goal, vessel, events);
                }

                // If this mission is randomized, we will discard the mission
                if (m.randomized) {
                    discardRandomMission (m);
                    m = reloadMission (m, vessel);
                }

                
                clearMissionGoals(vessel.id.ToString());  
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
        public int TotalSpentVehicles
        {
            get { return currentProgram.totalSpentVessels; }
        }

        public int TotalHiredKerbCost
        {
            get { return currentProgram.TotalSpentKerbals; }
        }

        public int otherpaymentmoney
        {
            get { return currentProgram.otherpaymentmoney; }
        }

        public int othercostmoney
        {
            get { return currentProgram.othercostmoney; }
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

        public string GetCompanyInfoString
        {
            get { return currentProgram.showCompanyAvailable; }
        }

        public string SetCompanyInfoString(string name)
        {
            return currentProgram.showCompanyAvailable = name;
        }
        public string GetCompanyInfoString2
        {
            get { return currentProgram.showCompanyAvailable2; }
        }

        public string SetCompanyInfoString2(string name)
        {
            return currentProgram.showCompanyAvailable2 = name;
        }
        public string GetCompanyInfoString3
        {
            get { return currentProgram.showCompanyAvailable3; }
        }

        public string SetCompanyInfoString3(string name)
        {
            return currentProgram.showCompanyAvailable3 = name;
        }
        public string GetCompanyInfoString4
        {
            get { return currentProgram.showCompanyAvailable4; }
        }

        public string SetCompanyInfoString4(string name)
        {
            return currentProgram.showCompanyAvailable4 = name;
        }
        // End Research Recycle

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

        /// <summary>
        /// Get the current contract value.  this is what determines what contract is available.
        /// </summary>
        public int GetCurrentContract
        {
            get { return currentProgram.currentcontractType; }
        }
        public int GetCurrentContract1
        {
            get { return currentProgram.currentcontractytpe2; }
        }
        public int GetCurrentContract2
        {
            get { return currentProgram.currentcontracttype3; }
        }
        public bool Getrandomcontractsfreeze
        {
            get { return currentProgram.randomcontractsfreeze; }
        }
        /// <summary>
        /// sets the Random Contract number to save file.  This is what determines what contract is available. 0 sets the contracts to not be shown.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int SetCurrentContract(int value)
        {
            return currentProgram.currentcontractType = value;
        }
        public int SetCurrentContract1(int value)
        {
            return currentProgram.currentcontractytpe2 = value;
        }
        public int SetCurrentContract2(int value)
        {
            return currentProgram.currentcontracttype3 = value;
        }
        public bool Setrandomcontractfreeze(bool value)
        {
            Debug.Log("Time Check freeze For Random Contracts Set to: " + value);
            return currentProgram.randomcontractsfreeze = value;           
        }
        public double SetCurrentCheckTime(double value)
        {
            return currentProgram.nextTimeCheck = value;
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
        public string GetShowVesselRepairName
        {
            get { return currentProgram.showRepairVesselName; }
        }
        public string SetShowVesselRepairName(string name)
        {
            return currentProgram.showRepairVesselName = name;
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

        public int IgetBudget() { return currentProgram.money; }
        public int Itotalbudget() { return currentProgram.totalMoney; }
        public int ItotalSpentVehicles() { return currentProgram.totalSpentVessels; }
        public int ItotalRecycleMoney() { return currentProgram.totalrecycleMoney; }
        public int ItotalHiredKerbCost() { return currentProgram.TotalSpentKerbals; }
        public int ItotalModPayment() { return currentProgram.otherpaymentmoney; }
        public int ItotalModCost() { return currentProgram.othercostmoney; }
        public void IloadMCEbackup() { loadProgramBackup(HighLogic.CurrentGame.Title); }
        public void IloadMCESave() { loadProgram(HighLogic.CurrentGame.Title); }
        public void IsaveMCE() { saveProgram(); }
        
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

        public int CleanReward(int value)
        {
            latestExpenses = -value;
            currentProgram.money += value;
            return currentProgram.money;
        }

        public int contractReward(int value, int ms)
        {
            Vessel v = new Vessel();
            double comppayout = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString), "payout", 1.0);
            double comppayout2 = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString2), "payout", 1.0);
            double comppayout3 = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString3), "payout", 1.0);
            double comppayout4 = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString4), "payout", 1.0);

            if (!SettingsManager.Manager.getSettings().disablePlugin)
            {
                if (value > 0) // is a reward, not a cost
                {

                    double mult = 1.0;
                    if (manager.budget < 0)
                    {
                        mult *= FinanceMode.currentloan;
                    }
                    if (settings.gameMode == 1)
                    {
                        mult *= 0.6;
                        value = (int)((double)value * mult);
                    }
                }
                int Complevel = ms;

                if (ms == 1)
                {
                    currentProgram.money += (int)((double)value * comppayout * PayoutLeveles.TechPayout);
                    currentProgram.totalMoney += (int)((double)value * comppayout * PayoutLeveles.TechPayout);

                    Debug.Log("Contract Payment made comp1");
                }
                if (ms == 2)
                {
                    currentProgram.money += (int)((double)value * comppayout2 * PayoutLeveles.TechPayout);
                    currentProgram.totalMoney += (int)((double)value * comppayout2 * PayoutLeveles.TechPayout);
                    Debug.Log("Contract Payment made comp2");
                }
                if (ms == 3)
                {
                    currentProgram.money += (int)((double)value * comppayout3 * PayoutLeveles.TechPayout);
                    currentProgram.totalMoney += (int)((double)value * comppayout3 * PayoutLeveles.TechPayout);
                    Debug.Log("Contract Payment made comp3");
                }
                if (ms == 4)
                {
                    currentProgram.money += (int)((double)value * comppayout4 * PayoutLeveles.TechPayout);
                    currentProgram.totalMoney += (int)((double)value * comppayout4 * PayoutLeveles.TechPayout);
                    Debug.Log("Contract Payment made comp4");
                }
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
                    {
                        mult *= FinanceMode.currentloan;
                    }
                    if (settings.gameMode == 1)
                    {
                        mult *= 0.6;
                        value = (int)((double)value * mult);
                    }
                }
                currentProgram.totalMoney += (int)((double)value * PayoutLeveles.TechPayout);
            }
            return currentProgram.totalMoney;
        }

        public int modReward(int value, string description)
        {
            if (!SettingsManager.Manager.getSettings().disablePlugin)
            {
                currentProgram.add(new ModPayments(value, description));
                currentProgram.money += value;
                currentProgram.otherpaymentmoney += value;
            }
            return currentProgram.money;
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

        public float Contractsciencereward(float value, int ms)
        {
            double compscience = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString), "science", 1.0);
            double compscience2 = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString2), "science", 1.0);
            double compscience3 = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString3), "science", 1.0);
            double compscience4 = Tools.GetValueDefault(Tools.MCSettings.GetNode(GetCompanyInfoString4), "science", 1.0);
            ConstructionMode cn = new ConstructionMode();

            int complevel = ms;

            if (complevel == 1)
            {
                value = (float)((double)value * compscience);
                Debug.Log("Science Payment made comp1");
            }
            if (complevel == 2)
            {
                value = (float)((double)value * compscience2);
                Debug.Log("Science Payment made comp2");
            }
            if (complevel == 3)
            {
                value = (float)((double)value * compscience3);
                Debug.Log("Science Payment made comp3");
            }
            if (complevel == 4)
            {
                value = (float)((double)value * compscience4);
                Debug.Log("Science Payment made comp4");
            }
            return cn.Science += value;
            
        }

        public int costs(int value) 
        {
            currentProgram.totalSpentVessels += value;
            return CleanReward(-value);            
        }

        /// <summary>
        /// added for API and adding a cost value for Mods
        /// </summary>
        /// <param name="value"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public int ModCost(int value, string description)
        {
            currentProgram.add(new ModCharges(value, description));
            currentProgram.othercostmoney += value;
            return CleanReward(-value);
        }

        public int kerbCost(int value)
        {
            currentProgram.TotalSpentKerbals += value;
            return CleanReward(-value);
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

    public class RepairVesselsList
    {
        public string vesselName;
        public RepairVesselsList()
        {
        }
        public RepairVesselsList(string name)
        {
            this.vesselName = name;
        }
           

    }
}

