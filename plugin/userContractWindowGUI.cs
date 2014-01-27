using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public partial class MissionController
    {

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

        public int body = 1;
        public int goal = 1;
        public double orbitApA = 0;
        public double orbitPeA = 0;
        
        private void drawUserContractWindow(int id)
        {
            Dictionary<int, PlanetInfo> dictplanetinfo = new Dictionary<int, PlanetInfo>();
            Dictionary<int, GoalInfo> dictGoalInfo = new Dictionary<int, GoalInfo>(); 

            dictGoalInfo.Add(GoalInfo1.ID, GoalInfo1);
            dictGoalInfo.Add(GoalInfo2.ID, GoalInfo2);
            dictGoalInfo.Add(GoalInfo3.ID, GoalInfo3);
            dictGoalInfo.Add(GoalInfo4.ID, GoalInfo4);

            dictplanetinfo.Add(PlanetInfo1.ID, PlanetInfo1);
            dictplanetinfo.Add(PlanetInfo2.ID, PlanetInfo2);
            dictplanetinfo.Add(PlanetInfo3.ID, PlanetInfo3);
            dictplanetinfo.Add(PlanetInfo4.ID, PlanetInfo4);

            PlanetInfo splanet = dictplanetinfo[body];

            GoalInfo sgoal = dictGoalInfo[goal];

            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();

            if (goal != 2 && goal != 3){body = 1;}
            if (goal <= 1){goal = 1;}
            if (goal >= 5){goal = 1;}
            if (goal == 1 || body == 1){ orbitApA = 0; orbitPeA = 0;}
            if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))){goal--;}
            if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))){goal++;}
            GUILayout.Box("Goal Type?: " + sgoal.Gname, GUILayout.Width(250), GUILayout.Height(30));
            GUILayout.Box("" + sgoal.Gname, GUILayout.Width(250), GUILayout.Height(30));
            GUILayout.EndHorizontal();
            
            if (goal == 2 || goal == 3)
            {
                if (body <= 1){body = 1;}
                if (body >= 5){body = 1;}
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))){body--;}
                if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))){body++;}   
                GUILayout.Box("Body Name?: " + splanet.Planet, GUILayout.Width(250), GUILayout.Height(30));
                GUILayout.Box("" + splanet.Planet, GUILayout.Width(250), GUILayout.Height(30));
                GUILayout.EndHorizontal();

                if (goal == 2 && body != 0)
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
                    GUILayout.Box("Apoapsis(MAX): ", GUILayout.Width(250), GUILayout.Height(30));
                    GUILayout.Box("" + orbitApA, GUILayout.Width(250), GUILayout.Height(30));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("-", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA -= 1000;}
                    if (GUILayout.Button("--", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA -= 10000;}
                    if (GUILayout.Button("+", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA += 1000;}
                    if (GUILayout.Button("++", styleButtonWordWrap, GUILayout.Width(25))){orbitPeA += 10000;}
                    GUILayout.Box("Periapsis(MIN): ", GUILayout.Width(250), GUILayout.Height(30));
                    GUILayout.Box("" + orbitPeA, GUILayout.Width(250), GUILayout.Height(30));
                    GUILayout.EndHorizontal();
                }
            }

                if (GUILayout.Button("Exit", styleButtonWordWrap, GUILayout.Width(100))){showUserContractWindowStatus = false;}
                GUILayout.EndVertical();
            
            if (!Input.GetMouseButtonDown(1)){GUI.DragWindow();}
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
