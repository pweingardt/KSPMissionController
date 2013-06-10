using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// Manages the missions and the current space program (budget and so on)
    /// </summary>
    public class Manager : IManager
    {
        private Parser parser;

        private static Manager manager = new Manager();

        public static Manager instance {get { return manager; } }

        private SpaceProgram spaceProgram;

        public Manager() {
            parser = new Parser();
            loadProgram ();
        }

        public void recycleVessel(Vessel vessel, int costs) {
            if (!isRecycledVessel (vessel)) {
                currentProgram.money += costs;
                RecycledVessel rv = new RecycledVessel ();
                rv.guid = vessel.id.ToString ();
                currentProgram.add (rv);
                saveProgram ();
            }
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

        public void discardRandomMission(Mission m) {
            if (m.randomized) {
                RandomMission rm = currentProgram.findRandomMission (m);
                if(rm != null) {
                    currentProgram.randomMissions.Remove (rm);
                }
            }
        }

        public MissionPackage loadMissionPackage(String path) {
            MissionPackage pkg = (MissionPackage) parser.readFile (path);
            pkg.Missions.Sort (delegate(Mission m1, Mission m2) {
                return m1.name.CompareTo(m2.name);
            });
            return pkg;
        }

        public Mission reloadMission(Mission m, Vessel vessel) {
            int count = 1;

            if (m.randomized) {
                RandomMission rm = currentProgram.findRandomMission (m);
                System.Random random = null;

                if (rm == null) {
                    rm = new RandomMission ();
                    rm.seed = new System.Random ().Next ();
                    rm.missionName = m.name;
            
                    // TODO: Do we need to save the current program after this operation?
                    // currently: yes
                    currentProgram.add (rm);
                    saveProgram ();
                }

                random = new System.Random (rm.seed);
                m.executeInstructions (random);
            }

            foreach(MissionGoal c in m.goals) {
                c.id = m.name + "__PART" + (count++);
                c.repeatable = m.repeatable;
                c.doneOnce = false;
            }

            if (vessel != null) {
                foreach (MissionGoal g in m.goals) {
                    if(isMissionGoalAlreadyFinished(g, vessel) && g.nonPermanent) {
                        g.doneOnce = true;
                    }
                }
            }

            return m;
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
            if (!isMissionGoalAlreadyFinished (goal, vessel) && goal.nonPermanent && goal.isDone(vessel) &&
                    !isRecycledVessel(vessel)) {
                currentProgram.add(new GoalStatus(vessel.id.ToString(), goal.id));
                currentProgram.money += goal.reward;

                saveProgram();
            }
        }
        
        public bool isMissionGoalAlreadyFinished(MissionGoal c, Vessel v) {
            if (v == null) {
                return false;
            }

            foreach (GoalStatus con in currentProgram.completedGoals) {
                // If the mission goal is an EVAGoal, we don't care about the vessel id. Otherwise we do...
                if(con.id.Equals(c.id) && (con.vesselGuid.Equals (v.id.ToString()) || c is EVAGoal)) {
                    return true;
                }
            }
            return false;
        }
        
        public void finishMission(Mission m, Vessel vessel) {
            if (!isMissionAlreadyFinished (m, vessel) && !isRecycledVessel(vessel) && m.isDone(vessel)) {
                currentProgram.add(new MissionStatus(m.name, vessel.id.ToString()));
                currentProgram.money += m.reward;
                
                // finish unfinished goals
                foreach(MissionGoal goal in m.goals) {
                    finishMissionGoal(goal, vessel);
                }

                saveProgram();
            }
        }

        public bool isMissionAlreadyFinished(String name) {
            foreach (MissionStatus s in currentProgram.completedMissions) {
                if (s.missionName.Equals (name)) {
                    return true;
                }
            }
            return false;
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

        public bool isRecycledVessel(Vessel vessel) {
            if (vessel == null) {
                return false;
            }

            foreach (RecycledVessel rv in currentProgram.recycledVessels) {
                if (rv.guid.Equals (vessel.id.ToString())) {
                    return true;
                }
            }
            return false;
        }

        // The interface implementation for external plugins

        public int getBudget() {
            return currentProgram.money;
        }

        public int reward(int value) {
            if (!SettingsManager.Manager.getSettings ().DisablePlugin) {
                currentProgram.money += value;
                saveProgram ();
            }
            return currentProgram.money;
        }

        public int costs(int value) {
            return reward (-value);
        }
    }
}

