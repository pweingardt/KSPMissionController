using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// Mission Goal For Repair Vessel
    /// Does NOt work Yet for some reason
    /// Done all the checks.. This should work!!
    /// I even made it more complicated and copied docking and crash, by adding a flag for isrepaired.  Everything changes over and isrepaired becomes true.. But for some reason
    /// the Else part of this code is not reading it correct?
    /// </summary>

    public class repairStation : PartModule
    {
        [KSPField(isPersistant = true)]
        public static bool repair;

        [KSPEvent(guiActive = true, guiName = "Start Repair", active = true)]
        public void EnableRepair()
        {
            repair = true;            
        }

        [KSPEvent(guiActive = true, guiName = "End Repair", active = false)]
        public void DisableRepair()
        {
            repair = false;            
        }

        [KSPAction("Toggle Repair")]
        public void ToggleRepairAction(KSPActionParam param)
        {
            repair = !repair;
        }
        public override void OnUpdate()
        {
            Events["EnableRepair"].active = !repair;
            Events["DisableRepair"].active = repair;
        }
    }
        
    public class RepairGoal : MissionGoal
    {

        protected override List<Value> values(Vessel vessel, GameEvent ev)
        {

            List<Value> values2 = new List<Value>();

            if (vessel == null)
            {
                values2.Add(new Value("Repaired", "True"));

            }
            else
            {
                
                values2.Add(new Value("Repaired", "True", "" + repairStation.repair, repairStation.repair));

            }

            return values2;
        }

        public override string getType()
        {
            return "All Repaired";
        }
    }
}

