using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// Mission Goal For Repair Vessel
    /// Part is loaded in class repairSation and loaded. The Player can Right Click part and set the RepairGoal To True.. 
    /// Once True the RepairGoal Class takes over and finishes the mission
    /// </summary>

    public class repairStation : PartModule
    {

        [KSPField(isPersistant = true)]
        public static bool repair;
        

        [KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, guiActive = false, guiName = "Start Repair", active = true)]
        public void EnableRepair()
        {    
        repair = true;                   
        }      
        
    }
        
    public class RepairGoal : MissionGoal
    {
        public RepairGoal()
        {
            this.vesselIndenpendent = true;
        }

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
            return "Repair";
        }
    }
}

