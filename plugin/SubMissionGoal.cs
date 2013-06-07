using System;
using System.Collections.Generic;
using System.Linq;

namespace MissionController
{
    /// <summary>
    /// A submission that consists of several mission goals
    /// </summary>
    public class SubMissionGoal : MissionGoal
    {
        public List<MissionGoal> subGoals = new List<MissionGoal>();

        public SubMissionGoal() {
            this.nonPermanent = true;
        }

        public void add(MissionGoal c) {
            subGoals.Add (c);
        }

        protected override List<Value> values(Vessel vessel) {
            List<Value> values = new List<Value> ();

            foreach (MissionGoal c in subGoals) {
                values = values.Union(c.getValues(vessel)).ToList();
            }

            return values;
        }

        public override String getType() {
            return "Submission";
        }
    }
}

