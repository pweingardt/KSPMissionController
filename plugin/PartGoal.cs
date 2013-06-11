using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// A mission goal that requires a certain amount of a certain part in order to finish the mission.
    /// </summary>
    public class PartGoal : MissionGoal
    {
        public String partName = "";
        public int partCount = 1;
        public int maxPartCount = -1;

        public PartGoal() {
            this.throttleDown = false;
        }

        protected override List<Value> values(Vessel vessel, GameEvent events) {
            List<Value> values = new List<Value> ();

            int count = 0;
            if (vessel != null) {
                foreach (Part p in vessel.Parts) {
                    if (p.partInfo.name.Equals (partName)) {
                        ++count;
                    }
                }
            }
            if(maxPartCount == -1) {
                if (vessel == null) {
                    values.Add (new Value ("Part", partCount + "x " + partName));
                } else {
                    values.Add (new Value ("Part", partCount + "x " + partName, "" + count, count >= partCount));
                }
            } else {
                if (vessel == null) {
                    values.Add (new Value ("max part", maxPartCount + "x " + partName));
                } else {
                    values.Add (new Value ("max part", maxPartCount + "x " + partName, "" + count, count <= maxPartCount));
                }
            }


            return values;
        }

        public override string getType ()
        {
            return "Parts";
        }
    }
}

