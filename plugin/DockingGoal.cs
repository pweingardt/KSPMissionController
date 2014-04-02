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
        private MissionController ms
        {
            get { return MissionController.instance; }
        }
        private Manager manager
        {
            get { return Manager.instance; }
        }

         public DockingGoal() {
            this.vesselIndenpendent = true;
        }

        protected override List<Value> values(Vessel vessel, GameEvent events) {
            List<Value> values = new List<Value> ();

            if (vessel == null)
            {
                values.Add (new Value ("Docked", "True"));
                values.Add(new Value("Capture Asteroid", manager.GetAsteroidChoosenName));                               
            } else {
                
                bool docked = (events.docked || this.doneOnce);
                string targetAsteriod = manager.currentDockedToVessel + " (unloaded)";
                values.Add (new Value ("Docked", "True", "" + docked, docked));
                values.Add(new Value("Capture", "" + manager.GetAsteroidChoosenName, targetAsteriod, manager.GetAsteroidChoosenName.Equals(targetAsteriod)));
                                
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

