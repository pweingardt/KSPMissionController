using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A docking goal. Does not work yet.
    /// </summary>
    public class DockingGoal : MissionGoal
    {

        private Vessel activeVessel
        {
            get
            {
                // We need this try-catch-block, because FlightGlobals.ActiveVessel might throw
                // an exception
                try
                {
                    if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
                    {
                        return FlightGlobals.ActiveVessel;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        protected override List<Value> values(Vessel vessel, GameEvent events) {
            List<Value> values = new List<Value> ();

            if (activeVessel.situation == Vessel.Situations.DOCKED)
            {
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

