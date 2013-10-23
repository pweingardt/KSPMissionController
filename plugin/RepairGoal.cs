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

        public static float myTime = 1.0f;

        void timercount()
        {
            if (myTime > 0)
            {
                myTime -= Time.deltaTime;
                print("Repair Countdown: " + myTime);
            }
            if (myTime <= 0)
            {
                print("Time Has Ended");
                repair = true;
                CancelInvoke("timercount");
            }
        }

        [KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, guiActive = false, guiName = "Start Repair", active = true)]
        public void EnableRepair()
        {
            InvokeRepeating("timercount", 1, 1);
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
                    values2.Add(new Value("Repair Time", (double)repairStation.myTime));
                }

                return values2;
            }

            public override string getType()
            {
                return "Repair";
            }
        }

    
}

