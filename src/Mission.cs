using System;
using System.Collections.Generic;

namespace MissionController
{
    public class Mission
    {
        public String name;

        public String description;

        public int reward = 100;

        public bool repeatable = false;
        public bool inOrder = true;

        private List<MissionGoal> mGoals = new List<MissionGoal>();

        public List<MissionGoal> goals { get { return mGoals; } }

        public override string ToString ()
        {
            return name;
        }

        public bool isDone(Vessel vessel) {
            foreach (MissionGoal c in goals) {
                if (!c.isDone (vessel) && !c.optional) {
                    return false;
                }
            }
            return true;
        }

        public void add(MissionGoal c) {
            mGoals.Add (c);
        }
    }
}

