using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// An EVA goal.
    /// </summary>
    public class EVAGoal : MissionGoal
    {
        public EVAGoal() {
            this.vesselIndenpendent = true;
        }

        protected override List<Value> values (Vessel v, GameEvent events)
        {
            List<Value> vs = new List<Value> ();

            if (v == null) {
                vs.Add (new Value("EVA", "true"));
            } else {
                vs.Add (new Value("EVA", "true", "" + v.isEVA, v.isEVA));
            }

            return vs;
        }

        public override string getType ()
        {
            return "EVA";
        }
    }
}

