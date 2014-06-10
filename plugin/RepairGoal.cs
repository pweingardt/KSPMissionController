using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    
    public class repairStation : PartModule
    {

        [KSPField(isPersistant = false)]
        public static bool repair = false;

        [KSPField(isPersistant = false)]
        public static bool dooropen = false;

        [KSPField(isPersistant = true)]
        public double currentRepair = 0;

        [KSPField(isPersistant = false)]
        public double repairRate = .1;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Ready To Repair")]
        public bool readyRep = false;

        [KSPEvent(externalToEVAOnly = true,unfocusedRange = 4f, guiActiveUnfocused = true, guiActive = false, guiName = "Start Repairs", active = true)]
        public void EnableRepair()
        {
            dooropen = true;
        }              

        [KSPAction("Start repair")]
        public void ToggleAction(KSPActionParam param)
        {
            EnableRepair();
        }          

        public override void OnStart(PartModule.StartState state)
        {
            this.part.force_activate();
        }

        public override void OnFixedUpdate()
        {
            if (currentRepair > 0)
            {
                readyRep = !readyRep;
            }

            if (dooropen.Equals(true))
            {
                currentRepair = this.part.Resources.Get(PartResourceLibrary.Instance.GetDefinition("repairParts").id).amount;
                this.part.RequestResource("repairParts", repairRate);
                if (currentRepair > 0)
                { repair = true; }
                if (currentRepair == 0)
                {
                    repair = false;
                    dooropen = false;
                }
            }
        }

       
    }

        public class RepairGoal : MissionGoal
        {
            
            Manager m = new Manager();
            public RepairGoal()
            {
                this.evaNotAllowed = false;
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

