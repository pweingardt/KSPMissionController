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
        public bool isAsteroidCapture = false;
        public bool isAsteroidCaptureCustom = false;
        public bool isDockingCapture = false;
        
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
                if (isAsteroidCapture != true && isDockingCapture != true && isAsteroidCaptureCustom != true)
                {
                    values.Add(new Value("Docked", "True"));
                }
                if (isAsteroidCapture == true)
                {
                    values.Add(new Value("Capture Asteroid", manager.GetAsteroidChoosenName));
                }
                if (isDockingCapture == true)
                {
                    values.Add(new Value("Dock With Satellite", manager.GetShowVesselRepairName));
                }
                if(isAsteroidCaptureCustom == true)
                {
                    values.Add(new Value("Capture Asteroid", manager.GetCurrentAsteroidCustomName));
                }
            } else {
                              
                if (isAsteroidCapture == true)
                {
                    string targetAsteriod = manager.currentDockedToVessel + " (unloaded)";
                    values.Add(new Value("Capture", "" + manager.GetAsteroidChoosenName, targetAsteriod, manager.GetAsteroidChoosenName.Equals(targetAsteriod)));
                }
                if (isAsteroidCaptureCustom == true)
                {
                    string targetAsteriod = manager.currentDockedToVessel + " (unloaded)";
                    values.Add(new Value("Capture", "" + manager.GetCurrentAsteroidCustomName, targetAsteriod, manager.GetCurrentAsteroidCustomName.Equals(targetAsteriod)));
                }
                if (isDockingCapture == true)
                {
                    string docksat = manager.currentDockedToVessel + " (unloaded)";
                    values.Add(new Value("Dock With", "" + manager.GetShowVesselRepairName, docksat, manager.GetShowVesselRepairName.Equals(docksat)));
                }
                if (isAsteroidCapture != true && isDockingCapture != true && isAsteroidCaptureCustom != true)
                {
                    bool docked = (events.docked || this.doneOnce);
                    values.Add(new Value("Docked", "True", "" + docked, docked));
                }
                                
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

