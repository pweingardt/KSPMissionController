using System;
using System.Collections.Generic;

namespace MissionController
{
    public class CrashGoal : MissionGoal
    {
        public String body = "Kerbin";

        public CrashGoal() {
            this.throttleDown = false;
        }

        protected override List<Value> values (Vessel vessel, GameEvent events)
        {
            List<Value> v = new List<Value>();

            if(vessel == null) {
                v.Add(new Value("Crashing on", body));
            } else {
                v.Add (new Value("Crashing on", body, vessel.orbit.referenceBody.bodyName, 
                                 (body.Equals(vessel.orbit.referenceBody.bodyName) && events.isCrashed) || this.doneOnce));
            }

            return v;
        }

        public override string getType ()
        {
            return "Crashing";
        }
    }
}

