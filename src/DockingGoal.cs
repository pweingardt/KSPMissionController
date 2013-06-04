using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A docking goal. Does not work yet.
    /// </summary>
    public class DockingGoal : MissionGoal
    {

        protected override List<Value> values(Vessel vessel) {
            List<Value> values = new List<Value> ();

            if (vessel == null) {
                values.Add (new Value ("Docked", "true"));
            } else {
                bool docked = (vessel.situation == Vessel.Situations.DOCKED);
                values.Add (new Value ("Docked", "true", "" + docked, docked));
            }

            return values;
        }

        public override string getType ()
        {
            return "Docking";
        }
    }
}

