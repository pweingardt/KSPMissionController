using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MissionController
{
    public class repairStation : PartModule
    {
        [KSPField(isPersistant = true)]
        public bool repair;

        [KSPEvent(guiActive = true, guiName = "Start Repair", active = true)]
        public void EnableRepair()
        {
            repair = true;
            print("repair is = " + repair);
        }

        [KSPEvent(guiActive = true, guiName = "End Repair", active = false)]
        public void DisableRepair()
        {
            repair = false;
            print("repair is = " + repair);
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
            if (repair != false) 
            {
                MissionController mc = new MissionController();
                mc.shipRepaired();               
            }
        }
    }
}
