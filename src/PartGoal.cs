using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    public class PartGoal : MissionGoal
    {
        public String partName = "";
        public int partCount = 1;

        protected override List<Value> values(Vessel vessel) {
            List<Value> values = new List<Value> ();

            int count = 0;
            foreach (Part p in vessel.Parts) {
                if(p.partInfo.name.Equals(partName)) {
                    ++count;
                }
            }

            values.Add(new Value("Part", partCount + "x " + partName, "" + count, count >= partCount));

            return values;
        }

        public override string getType ()
        {
            return "Parts";
        }
    }
}

