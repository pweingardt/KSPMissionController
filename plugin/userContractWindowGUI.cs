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

        GoalInfo GoalInfo1 = new GoalInfo()
        {
            ID = 1,
            Gname = "None",
            Gamount = 0,
        };
        GoalInfo GoalInfo2 = new GoalInfo()
        {
            ID = 2,
            Gname = "OrbitGoal",
            Gamount = 40000,
        };
        GoalInfo GoalInfo3 = new GoalInfo()
        {
            ID = 3,
            Gname = "LandingGoal",
            Gamount = 15000,
        };
        GoalInfo GoalInfo4 = new GoalInfo()
        {
            ID = 4,
            Gname = "DockingGoal",
            Gamount = 10000,
        };

        PlanetInfo PlanetInfo1 = new PlanetInfo()
        {
            ID = 1,
            Planet = "None",
            Gamount = 0,
            MaxOrb = 0,
            MinOrb = 0,
        };
        PlanetInfo PlanetInfo2 = new PlanetInfo()
        {
            ID = 2,
            Planet = "Kerbin",
            Gamount = 0,
            MaxOrb = 500000,
            MinOrb = 70000,
        };
        PlanetInfo PlanetInfo3 = new PlanetInfo()
        {
            ID = 3,
            Planet = "Mun",
            Gamount = 40000,
            MaxOrb = 200000,
            MinOrb = 8000,
        };
        PlanetInfo PlanetInfo4 = new PlanetInfo()
        {
            ID = 4,
            Planet = "Minmus",
            Gamount = 45000,
            MaxOrb = 350000,
            MinOrb = 14000,
        };
        private bool IsOrbit = false;
        private bool islanding = false;
        private bool isdocking = false;

        private int goalpayment = 0;
        private int TotalPayout = 0;
        
        private int body = 1;
        private int goal = 1;


        private double orbitApA = 0;
        private double orbitPeA = 0;

        Dictionary<int, PlanetInfo> dictplanetinfo = new Dictionary<int, PlanetInfo>();
        Dictionary<int, GoalInfo> dictGoalInfo = new Dictionary<int, GoalInfo>();

        public void LoadDictionary()
        {
            dictGoalInfo.Add(GoalInfo1.ID, GoalInfo1);
            dictGoalInfo.Add(GoalInfo2.ID, GoalInfo2);
            dictGoalInfo.Add(GoalInfo3.ID, GoalInfo3);
            dictGoalInfo.Add(GoalInfo4.ID, GoalInfo4);

            dictplanetinfo.Add(PlanetInfo1.ID, PlanetInfo1);
            dictplanetinfo.Add(PlanetInfo2.ID, PlanetInfo2);
            dictplanetinfo.Add(PlanetInfo3.ID, PlanetInfo3);
            dictplanetinfo.Add(PlanetInfo4.ID, PlanetInfo4);
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

            GUILayout.BeginHorizontal();

            if (goal != 2 && goal != 3){body = 1;}
                       
            if (goal == 1 || body == 1){ orbitApA = 0; orbitPeA = 0;}
            if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { goal--; if (goal < 1) { goal = 1; }}
            if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { goal++; if (goal > 4) { goal = 1; }}
            GUILayout.Box("Goal Type? ", GUILayout.Width(150), GUILayout.Height(30));
            GUILayout.Box("" + sgoal.Gname, GUILayout.Width(150), GUILayout.Height(30));
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
                    LoadUserContranct();
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
                    LoadUserContranct();
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
                    LoadUserContranct();
                }
            }
            GUILayout.EndHorizontal();
            
            if (goal == 2 || goal == 3)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))) { body--; if (body < 1) { body = 1; }}
                if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))) { body++; if (body > 4) { body = 1; }}   
                GUILayout.Box("Body Name?", GUILayout.Width(150), GUILayout.Height(30));
                GUILayout.Box("" + splanet.Planet, GUILayout.Width(150), GUILayout.Height(30));
                GUILayout.EndHorizontal();

                if (goal == 2 && body != 1)
                {
                    if (orbitApA == 0){orbitApA = splanet.MinOrb;}
                    if (orbitApA <= splanet.MinOrb){orbitApA = splanet.MinOrb;}
                    if (orbitApA >= splanet.MaxOrb){orbitApA = splanet.MinOrb;}
                    if (orbitPeA == 0){orbitPeA = splanet.MinOrb;}
                    if (orbitPeA <= splanet.MinOrb){orbitPeA = splanet.MinOrb;}
                    if (orbitPeA > orbitApA){orbitPeA = splanet.MinOrb;}

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))){orbitApA -= 1000;}
                    if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))){orbitApA -= 10000;}
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))){orbitApA += 1000;}
                    if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))){orbitApA += 10000;}
                    GUILayout.Box("Apoapsis(MAX): ", GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.Box("" + orbitApA, GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA -= 1000;}
                    if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA -= 10000;}
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA += 1000;}
                    if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA += 10000;}
                    GUILayout.Box("Periapsis(MIN): ", GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.Box("" + orbitPeA, GUILayout.Width(150), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                }
            }

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
            if (GUILayout.Button("Load User Contract", styleButtonWordWrap, GUILayout.Width(150)))
            {
                LoadUserContranct();
                currentPreviewMission3 = currentMission;
            }
            if (GUILayout.Button("Save Contract", styleButtonWordWrap, GUILayout.Width(140)))
            {
                managUserContracts.saveUserContracts();
            }           
            if (GUILayout.Button("Reset Contracts", styleButtonWordWrap, GUILayout.Width(140)))
            {
                usercontracts.resetUserContracts();
                IsOrbit = false;
                islanding = false;
                isdocking = false;
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
        public List<UCOrbitGoal> ucOrbitGoal = new List<UCOrbitGoal>();
        public List<UCDockingGoal> ucDockingGoal = new List<UCDockingGoal>();
        public List<UCLandingGoal> ucLandingGoal = new List<UCLandingGoal>();       
        public void add(UCOrbitGoal m)
        {
            ucOrbitGoal.Add(m);
        }

        public void resetUserContracts()
        {
            name = "";
            description = "";
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
}
