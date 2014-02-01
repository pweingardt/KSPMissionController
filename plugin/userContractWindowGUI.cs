using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {
        private UserContracts usercontracts
        {
            get { return ManageUserContracts.UCManager.getUserContractSettings(); }
        }

        private ManageUserContracts managUserContracts
        {
            get { return ManageUserContracts.UCManager; }
        }
        MCEEditorRefs mce = new MCEEditorRefs();
       
        private bool IsOrbit = false;
        private bool islanding = false;
        private bool isdocking = false;
        private bool iscrewselected = false;

        private int goalpayment = 0;
        private int TotalPayout = 0;
        
        private int body = 1;
        private int goal = 1;


        private double orbitApA = 0;
        private double orbitPeA = 0;

        private bool ucNoCrewGoal = false;

        Dictionary<int, PlanetInfo> dictplanetinfo = new Dictionary<int, PlanetInfo>();
        Dictionary<int, GoalInfo> dictGoalInfo = new Dictionary<int, GoalInfo>();

        public void LoadDictionary()
        {
            dictGoalInfo.Add(mce.GoalInfo1.ID, mce.GoalInfo1);
            dictGoalInfo.Add(mce.GoalInfo2.ID, mce.GoalInfo2);
            dictGoalInfo.Add(mce.GoalInfo3.ID, mce.GoalInfo3);
            dictGoalInfo.Add(mce.GoalInfo4.ID, mce.GoalInfo4);

            dictplanetinfo.Add(mce.PlanetInfo1.ID, mce.PlanetInfo1);
            dictplanetinfo.Add(mce.PlanetInfo2.ID, mce.PlanetInfo2);
            dictplanetinfo.Add(mce.PlanetInfo3.ID, mce.PlanetInfo3);
            dictplanetinfo.Add(mce.PlanetInfo4.ID, mce.PlanetInfo4);
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

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            GUILayout.BeginVertical(GUILayout.Height(250));
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load User Contract", styleButtonWordWrap))
            {
                LoadUserContranct();
                currentPreviewMission3 = currentMission;
            }
            GUILayout.EndHorizontal();

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
                GUILayout.Label("If Vessel Has Crew Or Not, this makes a difference with Payouts, and Fines if Crew Killed");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { ucNoCrewGoal = true; }
                if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { ucNoCrewGoal = false; }
                GUILayout.Box("Vessel Have Crew?", GUILayout.Width(150), GUILayout.Height(30));
                GUILayout.Box("" + ucNoCrewGoal, GUILayout.Width(150), GUILayout.Height(30));
                if (GUILayout.Button("Set Goal", styleButtonWordWrap, GUILayout.Width(120)))
                {
                    if (ucNoCrewGoal != false){ usercontracts.noneCrew.Add(new NoneCrew("Vessel Needs A Crew"));}                    
                    iscrewselected = true;
                    managUserContracts.saveUserContracts();
                }
                GUILayout.EndHorizontal();

            }

            else 
            {
                GUILayout.Space(10);
                GUILayout.Label("Set The type of Goal, Some goals are dependent On others!");
                GUILayout.BeginHorizontal();
                if (goal != 2 && goal != 3) { body = 1; }
                if (goal == 1 || body == 1) { orbitApA = 0; orbitPeA = 0; }
                if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { goal--; if (goal < 1) { goal = 1; } }
                if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { goal++; if (goal > 4) { goal = 1; } }
                GUILayout.Box("Goal Type? ", GUILayout.Width(150), GUILayout.Height(30));
                GUILayout.Box("" + sgoal.Gname, GUILayout.Width(150), GUILayout.Height(30));

                GUILayout.Space(10);
                if (goal == 2 && body != 1 && IsOrbit != true)
                {
                    if (GUILayout.Button("Set OrbitGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        goalpayment = splanetamount + sgoalamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        Debug.Log("planet amount" + splanetamount + "GoalAmount" + sgoalamount);
                        usercontracts.reward = TotalPayout;
                        Debug.Log(TotalPayout);
                        Debug.Log(usercontracts.reward);
                        usercontracts.ucOrbitGoal.Add(new UCOrbitGoal(splanet.Planet.ToString(), orbitApA, orbitPeA));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        IsOrbit = true;
                        managUserContracts.saveUserContracts();
                    }
                }
                if (goal == 3 && body != 1 && islanding != true)
                {
                    if (GUILayout.Button("Set LadingGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        goalpayment = splanet.Gamount + sgoal.Gamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        usercontracts.reward = TotalPayout;
                        usercontracts.ucLandingGoal.Add(new UCLandingGoal(splanet.Planet.ToString()));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        islanding = true;
                        managUserContracts.saveUserContracts();
                    }
                }
                if (goal == 4 && isdocking != true && IsOrbit != false)
                {
                    if (GUILayout.Button("Set DockingGoal", styleButtonWordWrap, GUILayout.Width(120)))
                    {
                        goalpayment = sgoal.Gamount;
                        TotalPayout = goalpayment + usercontracts.reward;
                        usercontracts.reward = TotalPayout;
                        usercontracts.ucDockingGoal.Add(new UCDockingGoal("Dock At Vessel"));
                        goalpayment = 0;
                        TotalPayout = 0;
                        goal = 1;
                        isdocking = true;
                        managUserContracts.saveUserContracts();
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                if (goal == 2 || goal == 3)
                {
                    GUILayout.Label("What body to set goal to? Please note only 1 goal type per Mission");
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { body--; if (body < 1) { body = 1; } }
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { body++; if (body > 4) { body = 1; } }
                    GUILayout.Box("Body Name?", GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.Box("" + splanet.Planet, GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                    if (goal == 2 && body != 1)
                    {
                        if (orbitApA == 0) { orbitApA = splanet.MinOrb; }
                        if (orbitApA <= splanet.MinOrb) { orbitApA = splanet.MinOrb; }
                        if (orbitApA >= splanet.MaxOrb) { orbitApA = splanet.MinOrb; }
                        if (orbitPeA == 0) { orbitPeA = splanet.MinOrb; }
                        if (orbitPeA <= splanet.MinOrb) { orbitPeA = splanet.MinOrb; }
                        if (orbitPeA > orbitApA) { orbitPeA = splanet.MinOrb; }

                        GUILayout.Label("Set the High Point for Orbit, Has to be Above PeA");
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA -= 1000; }
                        if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA -= 10000; }
                        if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA += 1000; }
                        if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))) { orbitApA += 10000; }
                        GUILayout.Box("Apoapsis(MAX): ", GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.Box("" + orbitApA, GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(10);

                        GUILayout.Label("Set The Lowpoint for Orbit, This has to be under ApA");
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA -= 1000; }
                        if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA -= 10000; }
                        if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA += 1000; }
                        if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))) { orbitPeA += 10000; }
                        GUILayout.Box("Periapsis(MIN): ", GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.Box("" + orbitPeA, GUILayout.Width(150), GUILayout.Height(30));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);

                    }
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            previewContractScrollPosition2 = GUILayout.BeginScrollView(previewContractScrollPosition2, GUILayout.Width(500));

            // Show the description text if no mission is currently selected
            if (currentPreviewMission3 == null)
            {
               
            }
            else
            {
                // otherwise draw the mission parameters
                drawContracts(currentPreviewMission3, calculateStatus(currentPreviewMission3, false, activeVessel));
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Save Contract", styleButtonWordWrap, GUILayout.Width(125)))
            {
                managUserContracts.saveUserContracts();
            }           
            if (GUILayout.Button("Reset Contracts", styleButtonWordWrap, GUILayout.Width(125)))
            {
                usercontracts.resetUserContracts();
                IsOrbit = false;
                islanding = false;
                isdocking = false;
                iscrewselected = false;
                usercontracts.IsContract = false;
                managUserContracts.saveUserContracts();
            }
            if (GUILayout.Button("Send Contract For Bidding", styleButtonWordWrap, GUILayout.Width(175)))
            {
                usercontracts.IsContract = true;
                managUserContracts.saveUserContracts();
                LoadUserContranct();
                
            }
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
        public bool IsContract = false;
        public List<NoneCrew> noneCrew = new List<NoneCrew>();
        public List<UCOrbitGoal> ucOrbitGoal = new List<UCOrbitGoal>();
        public List<UCDockingGoal> ucDockingGoal = new List<UCDockingGoal>();
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

        public void resetUserContracts()
        {
            name = " Place Mission Name ";
            description = "A Small Description of Mission";
            reward = 0;
            scienceReward = 0;
            ucOrbitGoal.Clear();
            ucLandingGoal.Clear();
            ucDockingGoal.Clear();
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
        public string body;
        public double maxApA;
        public double minPeA;
        public UCOrbitGoal()           
        {
        }

        public UCOrbitGoal(string name, double MaxApa, double MinPea)
        {
            this.body = name;
            this.maxApA = MaxApa;
            this.minPeA = MinPea;
        }
    }
    public class UCLandingGoal
    {
        public string body;

        public UCLandingGoal()
        {
        }

        public UCLandingGoal(string body)
        {
            this.body = body;
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

    }
    public class GoalInfo
    {
        public int ID { get; set; }
        public string Gname { get; set; }
        public int Gamount { get; set; }
    }
    public class NoneCrew
    {
        public string desc = "";
        NoneCrew()
        {
        }

        public NoneCrew(string desc)
        {
            this.desc = desc;
        }       
    }
}
