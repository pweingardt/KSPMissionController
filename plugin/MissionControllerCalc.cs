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
        private Status calculateStatus(Mission mission, bool fullCheck = false, Vessel vessel = null)
        {
            Status s = new Status();

            // Fill the mission status fields
            if (mission != null)
            {
                s.requiresAnotherMission = (mission.requiresMission.Length != 0
                                            && !manager.isMissionAlreadyFinished(mission.requiresMission));

                s.missionAlreadyFinished = (manager.isMissionAlreadyFinished(mission, vessel) ||
                                            (!mission.repeatable && manager.isMissionAlreadyFinished(mission.name)));
            }

            // Fill the vessel fields, that are not dependant on the current mission
            if (vessel != null)
            {
                s.onLaunchPad = (vessel.situation == Vessel.Situations.PRELAUNCH);
                s.recyclable = ((vessel.Landed || vessel.Splashed) && !s.onLaunchPad && !vessel.isEVA &&
                                vessel.orbit.referenceBody.name.Equals("Kerbin") && !recycled);
                s.vesselCanFinishMissions = true;
                s.isClientControlled = manager.isClientControlled(vessel);
                s.isOnPassiveMission = manager.isOnPassiveMission(vessel);
            }

            // for all other fields we need both: a mission and a vessel

            if (vessel == null || mission == null)
            {
                return s;
            }
            // edited malkuth to add that setting must be set to flight mode to save missons!!
            s.canFinishMission = s.vesselCanFinishMissions && !s.requiresAnotherMission && !settings.disablePlugin && settings.difficulty == 1;

            bool orderOk = true;

            // Only the selected mission is tracked and finishable. A preview Mission is *NOT* finishable.

            // We calculate the events only for the selected mission, because we might need to reset them.
            if (fullCheck)
            {
                if (eventFlags.Has(EventFlags.CRASHED))
                {
                    eventFlags = eventFlags.Remove(EventFlags.CRASHED);
                    s.events.isCrashed = true;
                }

                if (eventFlags.Has(EventFlags.DOCKED))
                {
                    eventFlags = eventFlags.Remove(EventFlags.DOCKED);
                    s.events.docked = true;
                }

                eventFlags = EventFlags.NONE;
            }

            foreach (MissionGoal g in mission.goals)
            {
                s.finishableGoals[g.id] = false;
                if (fullCheck && orderOk && g.isDone(vessel, s.events))
                {
                    if (g.nonPermanent && (s.canFinishMission || manager.isMissionGoalAlreadyFinished(g, vessel)))
                    {
                        s.finishableGoals[g.id] = true;

                        // Let the manager handle the already finished goals...
                        manager.finishMissionGoal(g, vessel, s.events);
                    }
                }
                else
                {
                    if (mission.inOrder)
                    {
                        orderOk = false;
                    }
                }
            }

            if (s.canFinishMission && fullCheck)
            {
                s.missionIsFinishable = (!manager.isMissionAlreadyFinished(mission, vessel) && mission.isDone(vessel, s.events));
            }
            else
            {
                s.missionIsFinishable = false;
            }
            return s;
        }

        private class Status
        {
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

        private VesselResources vesselResources
        {
            get
            {
                VesselResources res = new VesselResources();
                try
                {
                    res.crewCount = 0;
                    res.engineCost = 0;
                    res.podCost = 0;
                    res.tankCost = 0;
                    res.controlCost = 0;
                    res.utilCost = 0;
                    res.sciCost = 0;

                    List<Part> parts;
                    if (activeVessel == null)
                    {
                        parts = EditorLogic.SortedShipList;
                    }
                    else
                    {
                        parts = activeVessel.parts;
                        res.crewCount = activeVessel.GetCrewCount();
                    }

                    foreach (Part p in parts)
                    {
                        int cst = p.partInfo.cost;
                        if (p.partInfo.category.Equals(PartCategories.Propulsion))
                        {
                            bool isEngine = false;
                            foreach (ModuleEngines e in p.Modules.OfType<ModuleEngines>())
                            {
                                res.engineCost += cst;
                                isEngine = true;
                            }
                            if (!isEngine)
                                res.tankCost += cst;
                        }


                        // PODS
                        bool isPod = false;
                        if (p.partInfo.category.Equals(PartCategories.Pods))
                        {
                            res.podCost += cst;
                        }

                        // PROPULSION - taken care of above: engines, LF, SF, MP.

                        // CONTROL
                        if (p.partInfo.category.Equals(PartCategories.Control))
                        {
                            res.controlCost += cst;
                        }

                        // STRUCTURAL
                        if (p.partInfo.category.Equals(PartCategories.Structural))
                        {
                            res.structCost += cst;
                        }
                        // STRUCTURAL
                        if (p.partInfo.category.Equals(PartCategories.Aero))
                        {
                            res.aeroCost += cst;
                        }
                        


                        // UTILITY
                        if (p.partInfo.category.Equals(PartCategories.Utility))
                        {
                            //print("part " + p.name + " is utility");
                            res.utilCost += cst;
                        }

                        // SCIENCE
                        if (p.partInfo.category.Equals(PartCategories.Science))
                        {
                            //print("part " + p.name + " is science");
                            res.sciCost += cst;
                        }

                        // EXPENDABLE RESOURCES
                        double lf = 0.0;
                        double ox = 0.0;
                        bool doOnce = false;
                        if (p.Resources["LiquidFuel"] != null)
                        {
                            lf = p.Resources["LiquidFuel"].amount;
                            res.liquidFuel += lf;
                        }

                        if (p.Resources["SolidFuel"] != null)
                        {
                            res.solidFuel += p.Resources["SolidFuel"].amount;
                        }

                        if (p.Resources["MonoPropellant"] != null)
                        {
                            res.monoFuel += p.Resources["MonoPropellant"].amount;
                            //res.tankCost += cst;

                        }

                        if (p.Resources["Oxidizer"] != null)
                        {
                            ox = p.Resources["Oxidizer"].amount;
                            //res.oxidizerFuel += ox;
                        }

                        // edit in .12 to add support for iron cross mod -- malkuth .12 Also Added Support for Modular Fuel Mod Check Difficulty for Multipliers
                        if (p.Resources["Oxygen"] != null)
                        {
                            res.oxygen += p.Resources["Oxygen"].amount;
                            //res.tankCost += cst;
                        }
                        if (p.Resources["LiquidOxygen"] != null)
                        {
                            res.LiquidOxygen += p.Resources["LiquidOxygen"].amount;
                            //res.tankCost += cst;
                        }
                        if (p.Resources["LiquidH2"] != null)
                        {
                            res.LiquidH2 += p.Resources["LiquidH2"].amount;
                            //res.tankCost += cst;
                        }
                        /*if (lf + ox > 0)
                        {
                            res.tankCost += cst;
                        }*/

                        if (p.Resources["XenonGas"] != null)
                        {
                            res.xenonFuel += p.Resources["XenonGas"].amount;
                            //res.tankCost += cst;
                        }

                        // NK add category detection, module detection, etc
                        // also DRE support
                        if (p.Resources["AblativeShielding"] != null)
                        {
                            res.podCost += p.Resources["AblativeShielding"].amount * 2;
                            // NOTE: we are NOT including part cost here, since that was derived from amount of abl shielding!
                        }
                        //res.mass += p.mass * mult;

                    }
                }
                catch
                {
                }
                return res;
            }
        }
        // edited .11 add some new values.. .12 edit by malkuth to add support for Iron Cross Mod and Modular fuel tanks
        const double costmultiplier = 1.0;

        private class VesselResources
        {
            public double liquidFuel;
            public double oxidizerFuel;
            public double solidFuel;
            public double monoFuel;
            //public double mass;
            public double xenonFuel;
            public double oxygen;
            public double LiquidOxygen;
            public double LiquidH2;

            public int crewCount = 0;
            public double engineCost = 0;
            public double podCost = 0;
            public double tankCost = 0;
            public double controlCost = 0;
            public double utilCost = 0;
            public double sciCost = 0;
            public double aeroCost = 0;
            public double structCost = 0;

            public int pod()
            {
                return (int)(costmultiplier * podCost);
            }
            public int tank()
            {
                return (int)(costmultiplier * tankCost);
            }
            public int engine()
            {
                return (int)(costmultiplier * engineCost);
            }
            public int ctrl()
            {
                return (int)(costmultiplier * controlCost);
            }
            public int util()
            {
                return (int)(costmultiplier * utilCost);
            }
            public int sci()
            {
                return (int)(costmultiplier * sciCost);
            }
            public int stru()
            {
                return (int)(costmultiplier * structCost);
            }
            public int aero()
            {
                return (int)(costmultiplier * aeroCost);
            }


            public int oxylife()
            {
                return (int)(costmultiplier * oxygen * Difficulty.LiquidFuel);
            }
            public int LiquidOxy()
            {
                return (int)(costmultiplier * LiquidOxygen * Difficulty.LiquidOxygen);
            }
            public int LiquidH()
            {
                return (int)(costmultiplier * LiquidH2 * Difficulty.LiquidH2);
            }

            public int liquid()
            {
                return (int)(costmultiplier * liquidFuel * Difficulty.LiquidFuel);
            }

            public int mono()
            {
                return (int)(costmultiplier * monoFuel * Difficulty.MonoPropellant);
            }

            public int solid()
            {
                return (int)(costmultiplier * solidFuel * Difficulty.SolidFuel);
            }

            public int xenon()
            {
                return (int)(costmultiplier * xenonFuel * Difficulty.Xenon);
            }

            public int oxidizer()
            {
                return (int)(costmultiplier * oxidizerFuel * Difficulty.Oxidizer);
            }

            public int crew()
            {
                return SettingsManager.Manager.getSettings().kerbonautCostAsInt * crewCount;
            }

            public int dry()
            {
                return pod() + tank() + engine() + ctrl() + util() + sci() + stru() + aero();
            }
            public int wet()
            {
                return liquid() + oxidizer() + solid() + mono() + xenon() + oxylife() + LiquidOxy() + LiquidH();
            }

            public int sum()
            {
                return wet() + dry() + crew();
            }

            public int recyclable(bool landed)
            {
                if (landed)
                {
                    return (int)(0.85 * dry() + 0.95 * wet() + crew());
                }
                else
                {
                    return (int)(0.65 * dry() + 0.95 * wet() + crew());
                }
            }
        }
    }
}

