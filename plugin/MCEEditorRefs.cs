using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionController
{
    class MCEEditorRefs
    {
        public GoalInfo GoalInfo1 = new GoalInfo()
        {
            ID = 1,
            Gname = "None",
            Gamount = 0,
            GSciAmount = 0
        };
        public GoalInfo GoalInfo2 = new GoalInfo()
        {
            ID = 2,
            Gname = "OrbitGoal",
            Gamount = 50000,
            GSciAmount = 0
        };
        public GoalInfo GoalInfo3 = new GoalInfo()
        {
            ID = 3,
            Gname = "LandingGoal",
            Gamount = 40000,
            GSciAmount = 0
        };
        public GoalInfo GoalInfo4 = new GoalInfo()
        {
            ID = 4,
            Gname = "DockingGoal",
            Gamount = 20000,
            GSciAmount = 0
        };
        public GoalInfo GoalInfo5 = new GoalInfo()
        {
            ID = 5,
            Gname = "CrashGoal",
            Gamount = 30000,
            GSciAmount = 30,
        };

        public PlanetInfo PlanetInfo1 = new PlanetInfo()
        {
            ID = 1,
            Planet = "None",
            Gamount = 0,
            MaxOrb = 0,
            MinOrb = 0,
        };
        public PlanetInfo PlanetInfo2 = new PlanetInfo()
        {
            ID = 2,
            Planet = "Kerbin",
            Gamount = 20000,
            MaxOrb = 84159286,
            MinOrb = 70000,
        };
        public PlanetInfo PlanetInfo3 = new PlanetInfo()
        {
            ID = 3,
            Planet = "Mun",
            Gamount = 60000,
            MaxOrb = 2247428,
            MinOrb = 8100,
        };
        public PlanetInfo PlanetInfo4 = new PlanetInfo()
        {
            ID = 4,
            Planet = "Minmus",
            Gamount = 65000,
            MaxOrb = 350000,
            MinOrb = 14100,
        };
        public PlanetInfo PlanetInfo5 = new PlanetInfo()
        {
            ID = 5,
            Planet = "Duna",
            Gamount = 450000,
            MaxOrb = 47921949,
            MinOrb = 42000,
            basePay = 275000,
            baseScience = 50,
        };
        public PlanetInfo PlanetInfo6 = new PlanetInfo()
        {
            ID = 6,
            Planet = "Ike",
            Gamount = 100000,
            MaxOrb = 1049598,
            MinOrb = 13000,
            basePay = 300000,
            baseScience = 65,
        };
        public PlanetInfo PlanetInfo7 = new PlanetInfo()
        {
            ID = 7,
            Planet = "Eve",
            Gamount = 450000,
            MaxOrb = 1049598,
            MinOrb = 97000,
            basePay = 250000,
            baseScience = 45,
        };
        public PlanetInfo PlanetInfo8 = new PlanetInfo()
        {
            ID = 8,
            Planet = "Gilly",
            Gamount = 100000,
            MaxOrb = 1049598,
            MinOrb = 13000,
            basePay = 275000,
            baseScience = 60,
        };
        public PlanetInfo PlanetInfo9 = new PlanetInfo()
        {
            ID = 9,
            Planet = "Moho",
            Gamount = 700000,
            MaxOrb = 9646663,
            MinOrb = 7000,
            basePay = 425000,
            baseScience = 200,
        };
        public PlanetInfo PlanetInfo10 = new PlanetInfo()
        {
            ID = 10,
            Planet = "Dres",
            Gamount = 600000,
            MaxOrb = 32832840,
            MinOrb = 6000,
            basePay = 325000,
            baseScience = 75,
        };
        public PlanetInfo PlanetInfo11 = new PlanetInfo()
        {
            ID = 11,
            Planet = "Jool",
            Gamount = 800000,
            MaxOrb = 212832840,
            MinOrb = 139000,
            basePay = 300000,
            baseScience = 80,
        };
        public PlanetInfo PlanetInfo12 = new PlanetInfo()
        {
            ID = 12,
            Planet = "Laythe",
            Gamount = 100000,
            MaxOrb = 3723645,
            MinOrb = 82000,
            basePay = 310000,
            baseScience = 85,
        };
        public PlanetInfo PlanetInfo13 = new PlanetInfo()
        {
            ID = 13,
            Planet = "Vall",
            Gamount = 200000,
            MaxOrb = 2406401,
            MinOrb = 8000,
            basePay = 375000,
            baseScience = 90,
        };
        public PlanetInfo PlanetInfo14 = new PlanetInfo()
        {
            ID = 14,
            Planet = "Tylo",
            Gamount = 350000,
            MaxOrb = 10856518,
            MinOrb = 12000,
            basePay = 475000,
            baseScience = 250,
        };
        public PlanetInfo PlanetInfo15 = new PlanetInfo()
        {
            ID = 15,
            Planet = "Bop",
            Gamount = 250000,
            MaxOrb = 1221060,
            MinOrb = 30,
            basePay = 320000,
            baseScience = 100,
        };
        public PlanetInfo PlanetInfo16 = new PlanetInfo()
        {
            ID = 16,
            Planet = "Pol",
            Gamount = 250000,
            MaxOrb = 1042138,
            MinOrb = 6000,
            basePay = 350000,
            baseScience = 110,
        };
        public PlanetInfo PlanetInfo17 = new PlanetInfo()
        {
            ID = 17,
            Planet = "Eeloo",
            Gamount = 900000,
            MaxOrb = 10000000,
            MinOrb = 4000,
            basePay = 450000,
            baseScience = 300,
        };
        
    }
}
