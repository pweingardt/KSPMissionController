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
        };
        public GoalInfo GoalInfo2 = new GoalInfo()
        {
            ID = 2,
            Gname = "OrbitGoal",
            Gamount = 40000,
        };
        public GoalInfo GoalInfo3 = new GoalInfo()
        {
            ID = 3,
            Gname = "LandingGoal",
            Gamount = 15000,
        };
        public GoalInfo GoalInfo4 = new GoalInfo()
        {
            ID = 4,
            Gname = "DockingGoal",
            Gamount = 10000,
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
            Gamount = 0,
            MaxOrb = 500000,
            MinOrb = 70100,
        };
        public PlanetInfo PlanetInfo3 = new PlanetInfo()
        {
            ID = 3,
            Planet = "Mun",
            Gamount = 40000,
            MaxOrb = 200000,
            MinOrb = 8100,
        };
        public PlanetInfo PlanetInfo4 = new PlanetInfo()
        {
            ID = 4,
            Planet = "Minmus",
            Gamount = 45000,
            MaxOrb = 350000,
            MinOrb = 14100,
        };
    }
}
