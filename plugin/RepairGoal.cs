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
                bool repair = (ev.isrepaired || this.doneOnce);
                values2.Add(new Value("Repaired", "True", "" + repair, repair));

            }

            return values2;
        }

        public override string getType()
        {
            return "All Repaired";
        }
    }
}

