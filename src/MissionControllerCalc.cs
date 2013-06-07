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
            status = new Status ();

            // Fill the mission status fields
            if (currentMission != null) {

                status.requiresAnotherMission = (currentMission.requiresMission.Length != 0 
                                                 && !manager.isMissionAlreadyFinished (currentMission.requiresMission));

                status.missionAlreadyFinished = manager.isMissionAlreadyFinished (currentMission, vessel);
            }

            // Fill the vessel fields, that are not dependant on the current mission
            if (vessel != null) {
                status.onLaunchPad = (vessel.situation == Vessel.Situations.PRELAUNCH);
                status.recycledVessel = manager.isRecycledVessel (vessel);
                status.recyclable = (vessel.Landed && !status.recycledVessel && !status.onLaunchPad && !vessel.isEVA);
                status.vesselCanFinishMissions = !status.recycledVessel;
            }

            // for all other fields we need both: a mission and a vessel

            if (vessel == null || currentMission == null) {
                return;
            }

            status.canFinishMission = status.vesselCanFinishMissions && !status.requiresAnotherMission && !settings.DisablePlugin;

            bool orderOk = true;

            foreach (MissionGoal g in currentMission.goals) {
                status.finishableGoals [g.id] = false;
                if (orderOk && g.isDone (vessel)) {
                    if (g.nonPermanent && status.canFinishMission) {
                        status.finishableGoals [g.id] = true;
                        manager.finishMissionGoal (g, vessel);
                    }
                } else {
                    if (currentMission.inOrder) {
                        orderOk = false;
                    }
                }
            }

            if (status.canFinishMission) {
                status.missionIsFinishable = (!manager.isMissionAlreadyFinished(currentMission, vessel) && currentMission.isDone (vessel));
            } else {
                status.missionIsFinishable = false;
            }
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

