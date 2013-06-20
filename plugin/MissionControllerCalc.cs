using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionController
{
    /// <summary>
    /// This part of the class calculates the vesselResources and the current Status
    /// </summary>
    public partial class MissionController
    {
        /// <summary>
        /// Calculates the mission status. Some fields are not set by this method unless the passed mission
        /// is the currently selected mission1!!
        /// </summary>
        /// <returns>The status.</returns>
        /// <param name="mission">Mission.</param>
        private Status calculateStatus(Mission mission, bool fullCheck = false) {
            Status s = new Status ();

            // Fill the mission status fields
            if (mission != null) {
                s.requiresAnotherMission = (mission.requiresMission.Length != 0 
                                            && !manager.isMissionAlreadyFinished (mission.requiresMission));

                s.missionAlreadyFinished = (manager.isMissionAlreadyFinished (mission, activeVessel) ||
                                            (!mission.repeatable && manager.isMissionAlreadyFinished(mission.name)));
            }

            // Fill the vessel fields, that are not dependant on the current mission
            if (activeVessel != null) {
                s.onLaunchPad = (activeVessel.situation == Vessel.Situations.PRELAUNCH);
                s.recyclable = ((activeVessel.Landed || activeVessel.Splashed) && !s.onLaunchPad && !activeVessel.isEVA && 
                                activeVessel.orbit.referenceBody.name.Equals("Kerbin") && !recycled);
                s.vesselCanFinishMissions = true;
                s.isClientControlled = manager.isClientControlled (activeVessel);
                s.isOnPassiveMission = manager.isOnPassiveMission (activeVessel);
            }

            // for all other fields we need both: a mission and a vessel

            if (activeVessel == null || mission == null) {
                return s;
            }

            s.canFinishMission = s.vesselCanFinishMissions && !s.requiresAnotherMission && !settings.disablePlugin;

            bool orderOk = true;

            // Only the selected mission is tracked and finishable. A preview Mission is *NOT* finishable.

            // We calculate the events only for the selected mission, because we might need to reset them.
            if (fullCheck) {
                if(eventFlags.Has(EventFlags.CRASHED)) {
                    eventFlags = eventFlags.Remove (EventFlags.CRASHED);
                    s.events.isCrashed = true;
                }

                if(eventFlags.Has (EventFlags.DOCKED)) {
                    eventFlags = eventFlags.Remove (EventFlags.DOCKED);
                    s.events.docked = true;
                }

                eventFlags = EventFlags.NONE;
            }

            foreach (MissionGoal g in mission.goals) {
                s.finishableGoals [g.id] = false;
                if (fullCheck && orderOk && g.isDone (activeVessel, s.events)) {
                    if (g.nonPermanent && (s.canFinishMission || manager.isMissionGoalAlreadyFinished (g, activeVessel))) {
                        s.finishableGoals [g.id] = true;

                        // Let the manager handle the already finished goals...
                        manager.finishMissionGoal (g, activeVessel, s.events);
                    }
                } else {
                    if (mission.inOrder) {
                        orderOk = false;
                    }
                }
            }

            if (s.canFinishMission && fullCheck) {
                s.missionIsFinishable = (!manager.isMissionAlreadyFinished(mission, activeVessel) && mission.isDone (activeVessel, s.events));
            } else {
                s.missionIsFinishable = false;
            }
            return s;
        }

        private class Status {
            public bool onLaunchPad = false;

            public bool missionAlreadyFinished = false;

            public bool missionIsFinishable = false;

            public bool recyclable = false;

            public bool requiresAnotherMission = false;

            public Dictionary<String, bool> finishableGoals = new Dictionary<string, bool>();

            public bool vesselCanFinishMissions = false;

            public bool canFinishMission = false;

            public bool isClientControlled = false;

            public bool isOnPassiveMission = false;

            public GameEvent events = new GameEvent();
        }

        private VesselResources vesselResources {
            get {
                VesselResources res = new VesselResources ();
                try {
                    List<Part> parts;
                    if(activeVessel == null) {
                        parts = EditorLogic.SortedShipList;
                    } else {
                        parts = activeVessel.parts;
                        res.crewCount = activeVessel.GetCrewCount();
                    }

                    foreach (Part p in parts) {
                        foreach(ModuleEngines e in p.Modules.OfType<ModuleEngines>()) {
                            if(e.propellants.Where(r => r.name.Equals("SolidFuel")).Count() == 0 &&
                                    e.propellants.Where(r => r.name.Equals("IntakeAir")).Count() == 0) {
                                res.engineCost += Math.Pow(e.atmosphereCurve.Evaluate(0) - 200, 2) * e.maxThrust / 1000.0;
                            }
                        }

                        res.construction += p.partInfo.cost;

                        if (p.Resources ["LiquidFuel"] != null) {
                            res.liquidFuel += p.Resources ["LiquidFuel"].amount;
                        }

                        if (p.Resources ["SolidFuel"] != null) {
                            res.solidFuel += p.Resources ["SolidFuel"].amount;
                        }

                        if (p.Resources ["MonoPropellant"] != null) {
                            res.monoFuel += p.Resources ["MonoPropellant"].amount;
                        }

                        if (p.Resources ["Oxidizer"] != null) {
                            res.oxidizerFuel += p.Resources ["Oxidizer"].amount;
                        }

                        if (p.Resources ["XenonGas"] != null) {
                            res.xenonFuel += p.Resources ["XenonGas"].amount;
                        }

                        res.mass += p.mass;
                    }
                } catch {
                }
                return res;
            }
        }

        private class VesselResources
        {
            public double liquidFuel;
            public double oxidizerFuel;
            public double solidFuel;
            public double monoFuel;
            public double mass;
            public double xenonFuel;
            public double construction;

            public int crewCount = 0;
            public double engineCost = 0;

            public int liquid() {
                return (int)(liquidFuel * Difficulty.LiquidFuel);
            }

            public int mono() {
                return (int)(monoFuel * Difficulty.MonoPropellant);
            }

            public int solid() {
                return (int)(solidFuel * Difficulty.SolidFuel);
            }

            public int xenon() {
                return (int)(xenonFuel * Difficulty.Xenon);
            }

            public int materials() {
                return (int)(mass * Difficulty.Mass);
            }

            public int oxidizer() {
                return (int)(oxidizerFuel * Difficulty.Oxidizer);
            }

            public int engine() {
                return (int)(engineCost * Difficulty.LiquidEngines);
            }

            public int crew() {
                return SettingsManager.Manager.getSettings ().kerbonautCostAsInt * crewCount;
            }

            public int sum() {
                return (int)(construction + liquid () + solid () + mono () + xenon () + materials () + oxidizer() + crew ()
                             + engine());
            }

            public int recyclable(bool landed) {
                if (landed) {
                    return (int)(0.85 * (construction + materials () + engine()) + 0.95 * (liquid () + solid () + mono () + xenon () + + oxidizer ()) + crew ());
                } else {
                    return (int)(0.65 * (construction + materials () + engine()) + 0.95 * (liquid () + solid () + mono () + xenon () + + oxidizer ()) + crew ());
                }
            }
        }
    }
}

