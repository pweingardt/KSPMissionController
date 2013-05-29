using System;
using System.Collections.Generic;

namespace MissionController
{
    public class DockingGoal : MissionGoal
    {

        protected override List<Value> values(Vessel vessel) {
            List<Value> values = new List<Value> ();

            bool docked = (vessel.situation == Vessel.Situations.DOCKED);
            values.Add(new Value("Docked", "true", "" + docked, docked));

            return values;
        }

        public override string getType ()
        {
            return "Docking";
        }
    }
}

