using System;
using System.Collections.Generic;

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
        private String currentTitle;

        public Manager() {
            currentTitle = "default (Sandbox)";
            parser = new Parser();
            loadProgram (currentTitle);
        }

        /// <summary>
        /// Recycles the vessel with the given costs.
        /// It is added to the recycled vessels list.
        /// </summary>
        /// <param name="vessel">Vessel.</param>
        /// <param name="costs">Costs.</param>
        public void recycleVessel(Vessel vessel, int costs) {
            if (!isRecycledVessel (vessel)) {
                currentProgram.money += costs;
                RecycledVessel rv = new RecycledVessel ();
                rv.guid = vessel.id.ToString ();
                currentProgram.add (rv);
            }
        }

        public void loadProgram(String title) {
            currentTitle = title;
            try {
                spaceProgram = (SpaceProgram) parser.readFile (currentSpaceProgramFile);
            } catch {
                spaceProgram = SpaceProgram.generate();
            }
        }

        /// <summary>
        /// Discards the given random mission.
        /// Removed it from the random missions list
        /// </summary>
        /// <param name="m">M.</param>
        public void discardRandomMission(Mission m) {
            if (m.randomized) {
                RandomMission rm = currentProgram.findRandomMission (m);
                if(rm != null) {
                    currentProgram.randomMissions.Remove (rm);
                }
            }
        }

        /// <summary>
        /// Loads the given mission package
        /// </summary>
        /// <returns>The mission package.</returns>
        /// <param name="path">Path.</param>
        public MissionPackage loadMissionPackage(String path) {
            MissionPackage pkg = (MissionPackage) parser.readFile (path);
            pkg.Missions.Sort (delegate(Mission m1, Mission m2) {
                return m1.name.CompareTo(m2.name);
            });
            return pkg;
        }

        /// <summary>
        /// Reloads the given mission for the given vessel. Checks for already finished mission goals
        /// and reexecutes the instructions.
        /// </summary>
        /// <returns>reloaded mission</returns>
        /// <param name="m">mission</param>
        /// <param name="vessel">vessel</param>
        public Mission reloadMission(Mission m, Vessel vessel) {
            int count = 1;

            if (m.randomized) {
                RandomMission rm = currentProgram.findRandomMission (m);
                System.Random random = null;

                if (rm == null) {
                    rm = new RandomMission ();
                    rm.seed = new System.Random ().Next ();
                    rm.missionName = m.name;
                    currentProgram.add (rm);
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

        /// <summary>
        /// Returns the current space program.
        /// </summary>
        /// <value>The current program.</value>
        private SpaceProgram currentProgram {
            get { 
                if(spaceProgram == null) {
                    loadProgram(currentTitle);
                }
                return spaceProgram;
            }
        }

        /// <summary>
        /// Saves the current space program
        /// </summary>
        public void saveProgram() {
            if (spaceProgram != null) {
                parser.writeObject (spaceProgram, currentSpaceProgramFile);
            }
        }

        /// <summary>
        /// Returns the current file, in which the current space program has been saved
        /// </summary>
        /// <value>The current space program file.</value>
        private String currentSpaceProgramFile {
            get {
                return currentTitle + ".sp";
            }
        }

        /// <summary>
        /// Finishes the given mission goal with the given vessel.
        /// Rewards the space program with the reward from mission goal.
        /// </summary>
        /// <param name="goal">Goal.</param>
        /// <param name="vessel">Vessel.</param>
        public void finishMissionGoal(MissionGoal goal, Vessel vessel, GameEvent events) {
            if (!isMissionGoalAlreadyFinished (goal, vessel) && goal.nonPermanent && goal.isDone(vessel, events) &&
                    !isRecycledVessel(vessel)) {
                currentProgram.add(new GoalStatus(vessel.id.ToString(), goal.id));
                currentProgram.money += goal.reward;
            }
        }

        /// <summary>
        /// Returns true, if the given mission goal has been finish in another game with the given vessel, false otherwise
        /// </summary>
        /// <returns><c>true</c>, if mission goal already finished, <c>false</c> otherwise.</returns>
        /// <param name="c">goal</param>
        /// <param name="v">vessel</param>
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

        /// <summary>
        /// Finishes the given mission with the given vessel.
        /// Rewards the space program with the missions reward.
        /// </summary>
        /// <param name="m">mission</param>
        /// <param name="vessel">vessel</param>
        public void finishMission(Mission m, Vessel vessel, GameEvent events) {
            if (!isMissionAlreadyFinished (m, vessel) && !isRecycledVessel(vessel) && m.isDone(vessel, events)) {
                currentProgram.add(new MissionStatus(m.name, vessel.id.ToString()));
                currentProgram.money += m.reward;
                
                // finish unfinished goals
                foreach(MissionGoal goal in m.goals) {
                    finishMissionGoal(goal, vessel, events);
                }
            }
        }

        /// <summary>
        /// If true, the given mission name has been finished. False otherwise.
        /// </summary>
        /// <returns><c>true</c>, if mission already finished, <c>false</c> otherwise.</returns>
        /// <param name="name">mission name</param>
        public bool isMissionAlreadyFinished(String name) {
            foreach (MissionStatus s in currentProgram.completedMissions) {
                if (s.missionName.Equals (name)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true, if the given mission has been finished with the given vessel.
        /// </summary>
        /// <returns><c>true</c>, if mission already finished, <c>false</c> otherwise.</returns>
        /// <param name="m">mission</param>
        /// <param name="v">vessel</param>
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

        /// <summary>
        /// Returns the currently available budget.
        /// </summary>
        /// <value>The budget.</value>
        public int budget {
            get { return currentProgram.money; }
        }

        /// <summary>
        /// Checks if the given vessel has been recycled previously. 
        /// </summary>
        /// <returns><c>true</c>, if vessel has been recycled, <c>false</c> otherwise.</returns>
        /// <param name="vessel">vessel</param>
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
            }
            return currentProgram.money;
        }

        public int costs(int value) {
            return reward (-value);
        }
    }
}

