using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A docking goal. Does not work yet.
    /// </summary>
    public class DockingGoal : MissionGoal
    {
        protected override List<Value> values(Vessel vessel, GameEvent events) {
            List<Value> values = new List<Value> ();

            if (vessel == null) {
                values.Add (new Value ("Docked", "True"));
            } else {
                bool docked = (events.docked || this.doneOnce);
                values.Add (new Value ("Docked", "True", "" + docked, docked));
            }

            return values;
        }

        public override string getType ()
        {
            return "Docking";
        }
    }
}

