using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// This part of the class calculates the vesselResources and the current Status
    /// </summary>
    public partial class MissionController
    {
        private void calculateStatus() {
            this.status = calculateStatus (currentMission);
        }

        private Status calculateStatus(Mission mission) {
            Status s = new Status ();
            bool selectedMission = (mission == currentMission);

            // Fill the mission status fields
            if (mission != null) {

                s.requiresAnotherMission = (mission.requiresMission.Length != 0 
                                            && !manager.isMissionAlreadyFinished (mission.requiresMission));

                s.missionAlreadyFinished = manager.isMissionAlreadyFinished (mission, vessel);
            }

            // Fill the vessel fields, that are not dependant on the current mission
            if (vessel != null) {
                s.onLaunchPad = (vessel.situation == Vessel.Situations.PRELAUNCH);
                s.recycledVessel = manager.isRecycledVessel (vessel);
                s.recyclable = (vessel.Landed && !s.recycledVessel && !s.onLaunchPad && !vessel.isEVA);
                s.vesselCanFinishMissions = !s.recycledVessel;
            }

            // for all other fields we need both: a mission and a vessel

            if (vessel == null || mission == null) {
                return s;
            }

            s.canFinishMission = s.vesselCanFinishMissions && !s.requiresAnotherMission && !settings.DisablePlugin;

            bool orderOk = true;

            // Only the selected mission is tracked and finishable. A preview Mission is *NOT* finishable.

            foreach (MissionGoal g in mission.goals) {
                s.finishableGoals [g.id] = false;
                if (orderOk && g.isDone (vessel)) {
                    if (g.nonPermanent && s.canFinishMission && !manager.isMissionGoalAlreadyFinished (g, vessel) && selectedMission) {
                        s.finishableGoals [g.id] = true;
                        manager.finishMissionGoal (g, vessel);
                    }
                } else {
                    if (mission.inOrder) {
                        orderOk = false;
                    }
                }
            }

            if (s.canFinishMission && selectedMission) {
                s.missionIsFinishable = (!manager.isMissionAlreadyFinished(mission, vessel) && mission.isDone (vessel));
            } else {
                s.missionIsFinishable = false;
            }

            return s;
        }

        private class Status {
            public bool onLaunchPad = false;

            public bool missionAlreadyFinished = false;

            public bool missionIsFinishable = false;

            public bool recycledVessel = false;

            public bool recyclable = false;

            public bool requiresAnotherMission = false;

            public Dictionary<String, bool> finishableGoals = new Dictionary<string, bool>();

            public bool vesselCanFinishMissions = false;

            public bool canFinishMission = false;
        }

        private VesselResources vesselResources {
            get {
                VesselResources res = new VesselResources ();
                try {
                    List<Part> parts;
                    if(vessel == null) {
                        parts = EditorLogic.SortedShipList;
                    } else {
                        parts = vessel.parts;
                    }

                    foreach (Part p in parts) {
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


            public int liquid() {
                return (int)liquidFuel * 2;
            }

            public int mono() {
                return (int)monoFuel * 15;
            }

            public int solid() {
                return (int)solidFuel * 5;
            }

            public int xenon() {
                return (int)xenonFuel * 20;
            }

            public int other() {
                return (int)mass * 1000;
            }

            public int oxidizer() {
                return (int)(oxidizerFuel * 8.54);
            }

            public int sum() {
                return (int)(construction + liquid () + solid () + mono () + xenon () + other () + oxidizer());
            }

            public int recyclable() {
                return (int)(0.75 * (construction + other ()) + 0.95 * (liquid () + solid () + mono () + xenon () +  + oxidizer()));
            }
        }
    }
}

