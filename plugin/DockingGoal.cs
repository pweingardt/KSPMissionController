using System;
using System.Collections.Generic;
using UnityEngine;

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

            if (vessel == null)
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

