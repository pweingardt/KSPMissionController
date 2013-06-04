using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    public class Manager
    {
        private Parser parser;

        private static Manager manager = new Manager();

        public static Manager instance {get { return manager; } }

        private SpaceProgram spaceProgram;

        public Manager() {
            parser = new Parser();
            loadProgram ();
        }

        public void loadProgram() {
            if (spaceProgram != null) {
                saveProgram ();
            }
            try {
                spaceProgram = (SpaceProgram) parser.readFile ("spaceProgram.sp");
            } catch {
                spaceProgram = SpaceProgram.generate();
            }
        }

        public Mission loadMission(String path, Vessel vessel) {
            int count = 1;
            Mission m = (Mission) parser.readFile (path);
            foreach(MissionGoal c in m.goals) {
                c.id = m.name + "__PART" + (count++);
                c.repeatable = m.repeatable;
            }

            if (vessel != null) {
                prepareMission (m, vessel);
            }
            return m;
        }

        private void prepareMission(Mission m, Vessel vessel) {
            foreach (MissionGoal g in m.goals) {
                if(isMissionGoalAlreadyFinished(g, vessel) && g.nonPermanent) {
                    g.doneOnce = true;
                }
            }
        }

        public SpaceProgram currentProgram {
            get { 
                if(spaceProgram == null) {
                    loadProgram();
                }
                return spaceProgram;
            }
        }

        public void saveProgram() {
            if (spaceProgram != null) {
                parser.writeObject (spaceProgram, "spaceProgram.sp");
            }
        }

        public void finishMissionGoal(MissionGoal goal, Vessel vessel) {
            if (!isMissionGoalAlreadyFinished (goal, vessel) && goal.nonPermanent && goal.isDone(vessel)) {
                currentProgram.completedGoals.Add(new GoalStatus(vessel.id.ToString(), goal.id));
                currentProgram.money += goal.reward;

                saveProgram();
            }
        }
        
        public Boolean isMissionGoalAlreadyFinished(MissionGoal c, Vessel v) {
            if (v == null) {
                return false;
            }

            foreach (GoalStatus con in currentProgram.completedGoals) {
                if(con.id.Equals(c.id) && con.vesselGuid.Equals(v.id.ToString())) {
                    return true;
                }
            }
            return false;
        }
        
        public void finishMission(Mission m, Vessel vessel) {
            if (!isMissionAlreadyFinished (m, vessel)) {
                currentProgram.completedMissions.Add(new MissionStatus(m.name, vessel.id.ToString()));
                currentProgram.money += m.reward;
                
                // finish unfinished goals
                foreach(MissionGoal goal in m.goals) {
                    finishMissionGoal(goal, vessel);
                }

                saveProgram();
            }
        }
        
        public bool isMissionAlreadyFinished(Mission m, Vessel v) {
            if (v == null) {
                return false;
            }

            foreach (MissionStatus s in currentProgram.completedMissions) {
                if(s.missionName.Equals(m.name)) {
                    if(m.repeatable) {
                        if(s.vesselGuid.Equals(v.id.ToString())) {
                            return true;
                        }
                    } else {
                        return true;
                    }
                }
            }
            return false;
        }

        public int budget {
            get { return currentProgram.money; }
        }
    }
}

