using System;
using UnityEngine;
// for recycling
using System.Collections.Generic;
using System.Linq;

namespace MissionController
{
    public class repairStation : PartModule
    {
        MissionController control = new MissionController();
        
        public override void OnStart(StartState state)
        {
            print("MC Part Loaded");
        }
       
        [KSPEvent(unfocusedRange = 2f, guiActiveUnfocused = true, name = "Repair Menu", active = true, guiActive = true, guiName = "Repair Vessel")]
        public void Repair()
        {
            print("You pushed The Repair button on the Vessels Part");
            control.shipRepaired();   
        } 
           
    }
}
