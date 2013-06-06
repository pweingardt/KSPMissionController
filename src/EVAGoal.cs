using System;
using System.Collections.Generic;

namespace MissionController
{
    // TODO: Fix this behaviour

    /// <summary>
    /// An EVA goal. Does not work properly for now. 
    /// On vessel change the mission will be reloaded and the progress is lost for the new vessel, the kerbonaut.
    /// </summary>
    public class EVAGoal : MissionGoal
    {
        protected override List<Value> values (Vessel v)
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

