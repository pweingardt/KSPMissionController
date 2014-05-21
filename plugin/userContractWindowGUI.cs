using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private string contractdesctext = "Welcome to the contract editor.  This editor is not meant to be a complicated editor.  It’s actually very simple in the way it works. " +
                    "You the user have to select the above values to start making a new contract.\n" + "\n" +
                    " Only 1 goal per contract can be selected. This is a limitation of the saving process from your screen to the UserContract.cfg.  At a later date this may be expanded to have the ability to " +
                    "create goals within a SubMissionGoal.  But at this time, the more complicated missions are still best done by custom editing the mission file. \n " + "\n" +
                    " OribtGoal is the only exception to this rule. You can set up to 2 OrbitGoals per Contract Mission " +
                    "This is to help make missions that might orbit Duna then orbit ike.  But do note that if you do " +
                    "Set 2 orbit goals, any DockingGoal will be applied to the 2nd OrbitGoal. \n \n " +
                    " You can by all means edit this file the old way and add new goals manually.  Just do not reset the contract or you will lose that work.\n " + "\n" +
                    "The mission payout is determined by type of goals, and the planet they are targeting.  Other factors include how many crew.\n " + "\n" +
                    "You are only allowed 1 custom mission at a time.  When you’re done, you can reset the contract and start over.\n " + "\n" +
                    "Every time you choose to send contract to bidding, this will set your mission and give you a company that is willing to pay you. Don’t do this unless you are sure the contract is ready! \n \n" +
                    "MISSION NAME: Set the Mission Name Has a character Limit of about 40. \n\n" +
                    "MISSION DESCRIPTION: Give a small description for the mission. \n\n" +
                    "VESSEL HAVE CREW: This sets if the mission will have crew or not. If yes select how many.\n\n" +
                    "VESSEL INDEPENDENT: Use this on goals that you want to use a separate vessel on.  IE if you launch a large vessel to Duna with a Interplanetary Stage and a separate lander" +
                    " its best to set the Landing goal as Vessel Independent True.  That way the lander will not cause Landing goal not to be completed because its a new vessel." +
                    " Also if the vessel you launched is all in one then this is not needed.  Only vessels that were made separate and launched separate into orbit need this. \n\n" +
                    "ORBITGOAL: Use This to set the Orbital Height Of your Vessel Only able to have 2 of these. And they are always First and second goals in line.\n\n" +
                    "WHAT BODY: This is used to decide the current goals Planet Body its attached to.\n\n" +
                    "DOCKINGGOAL: Used to dock vessel in orbit always follows last Orbit goal.\n\n" +
                    "CRASHGOAL: Used to crash a vessel into a Planet Body, used for science and research. Good idea to have orbit goal first.\n\n";

       
        private UserContracts usercontracts
        {
            get { return ManageUserContracts.UCManager.getUserContractSettings(); }
        }

        private ManageUserContracts managUserContracts
        {
            get { return ManageUserContracts.UCManager; }
        }
        MCEEditorRefs mce = new MCEEditorRefs();
       
        private int IsOrbit = 0;
        private bool islanding = false;
        private bool isdocking = false;
        private bool isCrashing = false;
        private bool iscrewselected = false;
        private bool isasteroid = false;

        private int goalpayment = 0;
        private int TotalPayout = 0;
        private int TotalCrewCount = 0;
        
        private int body = 1;
        private int goal = 1;
        private int astcount = 0; 
        private bool isVesselIndy = false;


        private double orbitApA = 0;
        private double orbitPeA = 0;

        private bool ucNoCrewGoal = false;
        private int ucHasCrew = 20000;        

        Dictionary<int, PlanetInfo> dictplanetinfo = new Dictionary<int, PlanetInfo>();
        Dictionary<int, GoalInfo> dictGoalInfo = new Dictionary<int, GoalInfo>();

        public void LoadDictionary()
        {
            dictGoalInfo.Add(mce.GoalInfo1.ID, mce.GoalInfo1);
            dictGoalInfo.Add(mce.GoalInfo2.ID, mce.GoalInfo2);
            dictGoalInfo.Add(mce.GoalInfo3.ID, mce.GoalInfo3);
            dictGoalInfo.Add(mce.GoalInfo4.ID, mce.GoalInfo4);
            dictGoalInfo.Add(mce.GoalInfo5.ID, mce.GoalInfo5);
            dictGoalInfo.Add(mce.GoalInfo6.ID, mce.GoalInfo6);

            dictplanetinfo.Add(mce.PlanetInfo1.ID, mce.PlanetInfo1);
            dictplanetinfo.Add(mce.PlanetInfo2.ID, mce.PlanetInfo2);
            dictplanetinfo.Add(mce.PlanetInfo3.ID, mce.PlanetInfo3);
            dictplanetinfo.Add(mce.PlanetInfo4.ID, mce.PlanetInfo4);
            dictplanetinfo.Add(mce.PlanetInfo5.ID, mce.PlanetInfo5);
            dictplanetinfo.Add(mce.PlanetInfo6.ID, mce.PlanetInfo6);
            dictplanetinfo.Add(mce.PlanetInfo7.ID, mce.PlanetInfo7);
            dictplanetinfo.Add(mce.PlanetInfo8.ID, mce.PlanetInfo8);
            dictplanetinfo.Add(mce.PlanetInfo9.ID, mce.PlanetInfo9);
            dictplanetinfo.Add(mce.PlanetInfo10.ID, mce.PlanetInfo10);
            dictplanetinfo.Add(mce.PlanetInfo11.ID, mce.PlanetInfo11);
            dictplanetinfo.Add(mce.PlanetInfo12.ID, mce.PlanetInfo12);
            dictplanetinfo.Add(mce.PlanetInfo13.ID, mce.PlanetInfo13);
            dictplanetinfo.Add(mce.PlanetInfo14.ID, mce.PlanetInfo14);
            dictplanetinfo.Add(mce.PlanetInfo15.ID, mce.PlanetInfo15);
            dictplanetinfo.Add(mce.PlanetInfo16.ID, mce.PlanetInfo16);
            dictplanetinfo.Add(mce.PlanetInfo17.ID, mce.PlanetInfo17);

        }

        public void LoadUserContranct()
        {
            selectUserContract(HighLogic.CurrentGame.Title + "UserContracts.cfg");
        }

        private Vector2 previewContractScrollPosition2 = new Vector2();
        private Mission currentPreviewMission3 = null;

        UserContracts UserC = new UserContracts();
        
        private void drawUserContractWindow(int id)
        {                       

            PlanetInfo splanet = dictplanetinfo[body];
            GoalInfo sgoal = dictGoalInfo[goal];
            int sgoalamount = sgoal.Gamount;
            int splanetamount = splanet.Gamount;
            int maxastcount = manager.asteroidCapture.Count;
            AsteriodCapture tempasteroid = manager.asteroidCapture[astcount];

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            GUILayout.BeginVertical(GUILayout.Height(250));                      

            GUILayout.BeginHorizontal();
            GUILayout.Box("Mission Name",GUILayout.Width(150), GUILayout.Height(30));
            usercontracts.name = GUILayout.TextField(usercontracts.name,30);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Box("Mission Description", GUILayout.Width(150), GUILayout.Height(30));
            usercontracts.description = GUILayout.TextField(usercontracts.description,52);
            GUILayout.EndHorizontal();

            if (iscrewselected != true)
            {
                GUILayout.Label("If Vessel Has Crew Or Not, this makes a difference with Payouts", styleText);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("true", styleButtonWordWrap, GUILayout.Width(50))) { ucNoCrewGoal = true; }
                if (GUILayout.Button("false", styleButtonWordWrap, GUILayout.Width(50))) { ucNoCrewGoal = false; }
                GUILayout.Box("Does Vessel Have A Crew?", GUILayout.Width(175), GUILayout.Height(30));
                GUILayout.Box("" + ucNoCrewGoal, GUILayout.Width(100), GUILayout.Height(30));
                if (GUILayout.Button("Set Goal", styleButtonWordWrap, GUILayout.Width(120)))
                {
                    if (ucNoCrewGoal != true) { usercontracts.noneCrew.Add(new NoneCrew("Vessel Has No Crew")); }
                    if (ucNoCrewGoal != false)
                    {
                        //Debug.Log("Old Current Reward = " + usercontracts.reward);
                        TotalPayout = (ucHasCrew * TotalCrewCount) + usercontracts.reward;
                        //Debug.Log("Total Crew =: " + TotalCrewCount + " X 10,000 = current reward: " + usercontracts.reward);
                        usercontracts.reward = TotalPayout;
                        TotalPayout = 0;
                    }

                    iscrewselected = true;
                    managUserContracts.saveUserContracts();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                if (ucNoCrewGoal == true)
                {                   
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { TotalCrewCount--; if (TotalCrewCount < 0) { TotalCrewCount = 0; } }
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) {TotalCrewCount++;}
                    GUILayout.Box("How many Crew?", GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.Box("" + TotalCrewCount, GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                }
            }

            else
            {
                GUILayout.Space(5);
                GUILayout.Label("Set The type of Goal, Some goals are dependent On others! For right now only 1 goal each can be set for each Contract.", styleText);
                GUILayout.BeginHorizontal();
                if (goal != 2 && goal != 3 && goal != 5) { body = 1; }
                if (goal == 1 || body == 1) { orbitApA = 0; orbitPeA = 0; }
                if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { goal--; if (goal < 1) { goal = 1; } }
                if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { goal++; if (goal > 6) { goal = 1; } }
                GUILayout.Box("Goal Type? ", GUILayout.Width(150), GUILayout.Height(30));
                GUILayout.Box("" + sgoal.Gname, GUILayout.Width(150), GUILayout.Height(30));              
                                                               
                GUILayout.Space(5);
                if (goal == 2 && body != 1 && IsOrbit != 2 && isasteroid != true)
                {
                    if (GUILayout.Button("Set OrbitGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        //Debug.Log("Old Current Reward = " + usercontracts.reward);
                        goalpayment = splanetamount + sgoalamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        //Debug.Log("planet amount = " + splanetamount + "GoalAmount = " + sgoalamount);
                        usercontracts.reward = TotalPayout;
                        //Debug.Log("Total Payout = " + TotalPayout);
                        //Debug.Log("Current Reward = " + usercontracts.reward);
                        usercontracts.ucOrbitGoal.Add(new UCOrbitGoal(TotalCrewCount,splanet.Planet.ToString(), orbitApA, orbitApA, orbitPeA, orbitPeA, isVesselIndy));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        IsOrbit++;
                        managUserContracts.saveUserContracts();
                        isVesselIndy = false;
                    }
                }
                if (goal == 3 && body != 1 && islanding != true)
                {
                    if (GUILayout.Button("Set LadingGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        //Debug.Log("Old Current Reward = " + usercontracts.reward);
                        goalpayment = sgoal.Gamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        //Debug.Log("planet amount = " + splanetamount + "GoalAmount = " + sgoalamount);
                        usercontracts.reward = TotalPayout;
                        //Debug.Log("Total Payout = " + TotalPayout);
                        
                        usercontracts.ucLandingGoal.Add(new UCLandingGoal(splanet.Planet.ToString(),isVesselIndy));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        islanding = true;
                        managUserContracts.saveUserContracts();
                        isVesselIndy = false;
                    }
                }
                if (goal == 4 && isdocking != true && IsOrbit != 0 && isasteroid != true)
                {
                    if (GUILayout.Button("Set DockingGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        //Debug.Log("Old Current Reward before Docking = " + usercontracts.reward);
                        goalpayment = sgoal.Gamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        usercontracts.reward = TotalPayout;
                        //Debug.Log("Current Reward After Docking = " + usercontracts.reward);
                        usercontracts.ucDockingGoal.Add(new UCDockingGoal("Dock At Vessel"));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        isdocking = true;
                        managUserContracts.saveUserContracts();
                    }
                }
                if (goal == 5 && isCrashing != true && ucNoCrewGoal == false && isasteroid != true)
                {
                    if (GUILayout.Button("Set CrashGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        //Debug.Log("Old Current Reward before CrashGoal = " + usercontracts.reward);
                        goalpayment = sgoal.Gamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        usercontracts.scienceReward = sgoal.GSciAmount;
                        usercontracts.reward = TotalPayout;
                        //Debug.Log("Current Reward After CrashGoal = " + usercontracts.reward);
                        usercontracts.ucCrashGoal.Add(new UCCrashGoal("This vessel is tasked with crashing into selected body" , splanet.Planet.ToString()));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        isCrashing = true;
                        managUserContracts.saveUserContracts();
                    }
                }
                if (goal == 6 && !HighLogic.LoadedSceneIsFlight && isasteroid != true)
                {
                    if (GUILayout.Button("Set ARM", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        manager.SetCurrentAsteroidCustomName(tempasteroid.asName);
                        usercontracts.ucArmGoal.Add(new UCArmGoal(true));
                        goalpayment = sgoal.Gamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        usercontracts.scienceReward = sgoal.GSciAmount;
                        usercontracts.reward = TotalPayout;
                        isasteroid = true;                       
                        goal = 1;
                        managUserContracts.saveUserContracts();
                    }                    
                }
                GUILayout.EndHorizontal();                                
                GUILayout.Space(5);
                if (goal == 2 || goal == 3 || goal == 5)
                {
                    GUILayout.Label("What body to set goal to? Please note only 1 goal type per Mission", styleText);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { body--; if (body < 1) { body = 1; } }
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { body++; if (body > 17) { body = 1; } }
                    GUILayout.Box("Body Name?", GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.Box("" + splanet.Planet, GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                    if (goal == 2 && body != 1)
                    {
                        if (orbitApA == 0) { orbitApA = splanet.MinOrb; }
                        if (orbitApA <= splanet.MinOrb) { orbitApA = splanet.MinOrb; }
                        if (orbitApA >= splanet.MaxOrb) { orbitApA = splanet.MinOrb; }
                        if (orbitPeA == 0) { orbitPeA = splanet.MinOrb; }
                        if (orbitPeA <= splanet.MinOrb) { orbitPeA = splanet.MinOrb; }
                        if (orbitPeA > orbitApA) { orbitPeA = splanet.MinOrb; }

                        GUILayout.Label("Set the High Point for Orbit, Has to be Above PeA", styleText);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA -= 1000; }
                        if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA -= 10000; }
                        if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA += 1000; }
                        if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA += 10000; }
                        GUILayout.Box("Apoapsis(MAX): ", GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.Box("" + orbitApA, GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);

                        GUILayout.Label("Set The Lowpoint for Orbit, This has to be under ApA", styleText);
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA -= 1000; }
                        if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA -= 10000; }
                        if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA += 1000; }
                        if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA += 10000; }
                        GUILayout.Box("Periapsis(MIN): ", GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.Box("" + orbitPeA, GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(5);

                    }
                }
                if (goal == 2 || goal == 3)
                {
                    GUILayout.Label("Set for Goals you might use separate vessel.IE Lander on Landing Goal!", styleText);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("True", GUILayout.Width(50))) { isVesselIndy = true; }
                    if (GUILayout.Button("False", GUILayout.Width(50))) { isVesselIndy = false; }
                    GUILayout.Box("Vessel Independent Set To", GUILayout.Width(200), GUILayout.Height(30));
                    GUILayout.Box("" + isVesselIndy, GUILayout.Width(50), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                }
                if (goal == 6)
                {
                    GUILayout.Label("Choose Available Asteroid To Capture", styleText);
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { astcount--; if (astcount < 0) { astcount = 0; } }
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { astcount++; if (astcount >= maxastcount) { astcount = 0; } }                 
                    GUILayout.Box("Name Asteroid: ", GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.Box("" + tempasteroid.asName, GUILayout.Width(250), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                }
            }
            GUILayout.Space(5);
            GUILayout.Label("You must reload contract after every change to see changes!");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load User Contract", styleButtonWordWrap))
            {
                LoadUserContranct();
                currentPreviewMission3 = currentMission;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            previewContractScrollPosition2 = GUILayout.BeginScrollView(previewContractScrollPosition2, GUILayout.MaxWidth(500));
 
            if (currentPreviewMission3 == null)
            {
                GUILayout.Label(contractdesctext, styleText);
            }
            else
            {
                drawContractsPreview(currentPreviewMission3, calculateStatus(currentPreviewMission3, false, activeVessel));
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();                               
            if (GUILayout.Button("Reset Contract", styleButtonWordWrap, GUILayout.Width(125)))
            {
                usercontracts.resetUserContracts();
                IsOrbit = 0;
                islanding = false;
                isdocking = false;
                iscrewselected = false;
                isCrashing = false;
                isasteroid = false;
                TotalCrewCount = 0;
                managUserContracts.saveUserContracts();
            }         
            if (GUILayout.Button("Send Contract For Bidding", styleButtonWordWrap, GUILayout.Width(175)))
            {
                manager.StartCompanyRandomizer();
                manager.setUserContractCompany();               
                usercontracts.IsUserContract = true;
                usercontracts.IsContract = true;
                managUserContracts.saveUserContracts();
                messageEvent = ("Contract Sent out for Bidding. Press \"Load User Contract\" to Load into Contract Screen");
                showEventWindow = true;
            }
            if (GUILayout.Button("Save Contract", styleButtonWordWrap, GUILayout.Width(125))) { managUserContracts.saveUserContracts(); }
            if (GUILayout.Button("Exit", styleButtonWordWrap, GUILayout.Width(60))) { showUserContractWindowStatus = false; }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            if (!Input.GetMouseButtonDown(1)){GUI.DragWindow();}
        }
    }

    public class UserContracts
    {       
        public string name = " Place Mission Name ";
        public string description = "A Small Description of Mission";
        public int reward = 0;
        public float scienceReward = 0;
        public bool repeatable = true;
        public bool repeatableSameVessel = true;
        public bool IsContract = false;
        public bool IsUserContract = false;
        public int CompanyOrder = 4;
        public List<NoneCrew> noneCrew = new List<NoneCrew>();
        public List<UCOrbitGoal> ucOrbitGoal = new List<UCOrbitGoal>();
        public List<UCDockingGoal> ucDockingGoal = new List<UCDockingGoal>();
        public List<UCArmGoal> ucArmGoal = new List<UCArmGoal>();
        public List<UCCrashGoal> ucCrashGoal = new List<UCCrashGoal>();
        public List<UCLandingGoal> ucLandingGoal = new List<UCLandingGoal>();

        public void add(UCOrbitGoal m)
        {
            ucOrbitGoal.Add(m);
        }
        public void add(NoneCrew m)
        {
            noneCrew.Add(m);
        }
        public void add(UCLandingGoal m)
        {
            ucLandingGoal.Add(m);
        }
        public void add(UCDockingGoal m)
        {
            ucDockingGoal.Add(m);
        }
        public void add(UCArmGoal m)
        {
            ucArmGoal.Add(m);
        }
        public void add(UCCrashGoal m)
        {
            ucCrashGoal.Add(m);
        }

        public void resetUserContracts()
        {
            name = " Place Mission Name ";
            description = "A Small Description of Mission";
            reward = 0;
            scienceReward = 0;
            IsUserContract = false;
            IsContract = false;
            noneCrew.Clear();
            ucOrbitGoal.Clear();
            ucLandingGoal.Clear();
            ucDockingGoal.Clear();
            ucCrashGoal.Clear();
            ucArmGoal.Clear();
            ManageUserContracts.UCManager.saveUserContracts();
        }
    }

    public class ManageUserContracts
    {
        private static ManageUserContracts ucmanager = new ManageUserContracts();

        public static ManageUserContracts UCManager 
        { 
            get
            { return ucmanager; } 
        }

        private UserContracts uc = new UserContracts();
        private UserContracts usercontracts;       

        public UserContracts getUserContractSettings()
        {
            return ucmanager.uc;
        }

        private Parser parser;

        private ManageUserContracts()
        {
            parser = new Parser();
            loadUserContracts();
        }

        public void loadUserContracts() 
        {
            try 
            {
                uc = (UserContracts)parser.readFile(HighLogic.CurrentGame.Title +"UserContracts.cfg");
            } 
            catch 
            {
                usercontracts = new UserContracts();
            }          
            
        }

        public void saveUserContracts()
        {
             parser.writeContract(uc, HighLogic.CurrentGame.Title + "UserContracts.cfg");              
        }

    }

    public class UCOrbitGoal
    {
        public int crewCount;
        public string body;
        public double minApA;
        public double maxApA;
        public double minPeA;
        public double maxPeA;
        public bool vesselIndenpendent = false;
        
        public UCOrbitGoal()           
        {
        }

        public UCOrbitGoal(int crew, string name, double MaxApa, double MaxPea, double MinApa, double MinPea,bool vi)
        {
            this.crewCount = crew;
            this.body = name;
            this.maxApA = MaxApa;
            this.minPeA = MinPea;
            this.minApA = MinApa;
            this.maxPeA = MaxPea;
            this.vesselIndenpendent = vi;
        }
    }
    public class UCArmGoal
    {
        public bool isAsteroidCaptureCustom;

        public UCArmGoal()
        {
        }
        public UCArmGoal(bool settrue)
        {
            this.isAsteroidCaptureCustom = settrue;
        }

    }
    public class UCLandingGoal
    {
        public string body;
        public bool vesselIndenpendent = false;

        public UCLandingGoal()
        {
        }

        public UCLandingGoal(string body, bool VI)
        {
            this.body = body;
            this.vesselIndenpendent = VI;
        }
    }
    public class UCDockingGoal
    {
        public string description; 
        public UCDockingGoal()
        {
        }
        public UCDockingGoal(string test)
        {
            this.description = test;
        }
    }

    public class PlanetInfo
    {
        public int ID { get; set; }
        public string Planet { get; set; }
        public int Gamount { get; set; }
        public double MaxOrb { get; set; }
        public double MinOrb { get; set; }
        public int basePay { get; set; }
        public float baseScience { get; set; }

    }
    public class GoalInfo
    {
        public int ID { get; set; }
        public string Gname { get; set; }
        public int Gamount { get; set; }
        public int GSciAmount { get; set; }
    }
    public class NoneCrew
    {
        public string desc = "";
        public NoneCrew()
        {
        }

        public NoneCrew(string desc)
        {
            this.desc = desc;
        }       
    }
    public class UCCrashGoal
    {
        public string desc = "";
        public string body;
        public bool CrashGoalWarning = true;
        public UCCrashGoal()
        {
        }

        public UCCrashGoal(string desc, string body)
        {
            this.desc = desc;
            this.body = body;
        }
    }
}
