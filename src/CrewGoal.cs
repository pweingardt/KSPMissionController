using System;
using System.Collections.Generic;

namespace MissionController
{
    public class CrewGoal : MissionGoal
    {
        public int count = 1;

        protected override List<Value> values(Vessel vessel) {
            List<Value> values = new List<Value> ();

            values.Add(new Value("Crew count", "" + count, "" + vessel.GetCrewCount(),
                                               count <= vessel.GetCrewCount()));

            return values;
        }

        public override string getType () {
            return "Crew";
        }

    }
}

