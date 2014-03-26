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
         public DockingGoal() {
            this.vesselIndenpendent = true;
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

    public class UnDockingGoal : MissionGoal
    {
        public UnDockingGoal()
        {
            this.special = true;
        }
        
        protected override List<Value> values(Vessel vessel, GameEvent events)
        {
            List<Value> values = new List<Value>();

            if (vessel == null)
            {
                values.Add(new Value("UnDock Vessel", "True"));

            }
            else
            {
                bool undocked = (events.undocked || this.doneOnce);
                values.Add(new Value("UnDock Vessel", "True", "" + undocked, undocked));

            }

            return values;
        }

        public override string getType()
        {
            return "UnDock";
        }

    }
}

