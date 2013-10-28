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
            s.canFinishMission = s.vesselCanFinishMissions && !s.requiresAnotherMission && !settings.disablePlugin && settings.gameMode != 0;

            bool orderOk = true;

            // Only the selected mission is tracked and finishable. A preview Mission is *NOT* finishable.

            // We calculate the events only for the selected mission, because we might need to reset them.
            if (fullCheck)
            {
                if (eventFlags.Has(EventFlags.UNDOCKED))
                {
                    eventFlags = eventFlags.Remove(EventFlags.UNDOCKED);
                    s.events.undocked = true;
                }

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
                try
                {
                    if (activeVessel == null)
                    {
                        if (pVessel == null)
                            return new VesselResources(null);
                        else
                        {
                            if (pVessel.vesselRef != null)
                            {
                                return new VesselResources(pVessel.vesselRef);
                            }
                            else
                            {
                                print("*MC* Protovessel has no vessel ref!");
                                return new VesselResources(null);
                            }
                        }
                    }
                    else // Editor
                    {
                        return new VesselResources(null);
                    }

                }
                catch
                {
                }
                return new VesselResources(null);
            }
        }

        // edited .11 add some new values.. .12 edit by malkuth to add support for Iron Cross Mod and Modular fuel tanks
        const double costmultiplier = 1.0;
        
        private class VesselResources
        {

            public int crewCount = 0;
            public double engineCost = 0;
            public double podCost = 0;
            public double tankCost = 0;
            public double controlCost = 0;
            public double utilCost = 0;
            public double sciCost = 0;
            public double aeroCost = 0;
            public double structCost = 0;

            public Dictionary<string, double> resources = new Dictionary<string, double>();

            public VesselResources(Vessel v = null)
            {
                try
                {
                    crewCount = 0;
                    engineCost = 0;
                    podCost = 0;
                    tankCost = 0;
                    controlCost = 0;
                    utilCost = 0;
                    sciCost = 0;

                    List<Part> parts;
                    bool usesnapshots = false;
                    if (v == null)
                        parts = EditorLogic.SortedShipList;
                    else
                    {
                        parts = v.parts;
                        //print("*MC* Costing vessel " + v.vesselName);
                        if (v.loaded)
                            crewCount = v.GetCrewCount();
                        else
                        {
                            //crewCount = v.protoVessel.protoPartSnapshots.Sum(pps => pps.protoModuleCrew.Count);
                            // (done per part now, might as well not trawl the list twice.)
                            usesnapshots = true;
                        }
                        //print("Has " + crewCount + " crew");
                    }

                    if (!usesnapshots)
                    {
                        foreach (Part p in parts)
                        {
                            //int cst = p.partInfo.cost;
                            int cst = PartCost.cost(p); // for procedural parts, have to redo each time.
                            p.partInfo.cost = cst; // so it's stored in snapshot, I hope.

                            //print("part " + p.name + " has cost " + cst);
                            if (p.partInfo.category.Equals(PartCategories.Propulsion))
                            {
                                bool isEngine = false;
                                foreach (ModuleEngines e in p.Modules.OfType<ModuleEngines>())
                                {
                                    //if (e.propellants.Where(r => r.name.Equals("SolidFuel")).Count() == 0)
                                    //{
                                        engineCost += cst;
                                        isEngine = true;
                                    //}
                                }
                                if (!isEngine)
                                    tankCost += cst;
                            }


                            // PODS
                            if (p.partInfo.category.Equals(PartCategories.Pods))
                            {
                                podCost += cst;
                            }

                            // PROPULSION - taken care of above: engines, LF, SF, MP.

                            // CONTROL
                            if (p.partInfo.category.Equals(PartCategories.Control))
                            {
                                controlCost += cst;
                            }

                            // STRUCTURAL
                            if (p.partInfo.category.Equals(PartCategories.Structural))
                            {
                                structCost += cst;
                            }
                            // STRUCTURAL
                            if (p.partInfo.category.Equals(PartCategories.Aero))
                            {
                                aeroCost += cst;
                            }



                            // UTILITY
                            if (p.partInfo.category.Equals(PartCategories.Utility))
                            {
                                //print("part " + p.name + " is utility");
                                utilCost += cst;
                            }

                            // SCIENCE
                            if (p.partInfo.category.Equals(PartCategories.Science))
                            {
                                //print("part " + p.name + " is science");
                                sciCost += cst;
                            }

                            // new resource calc
                            if (Tools.MCSettings != null)
                            {

                                foreach (PartResource r in p.Resources)
                                {
                                    if (!(r.amount > 0))
                                        continue;

                                    // get multiplier
                                    double rCost = 0;
                                    foreach (ConfigNode rNode in Tools.MCSettings.GetNode("RESOURCECOST").nodes)
                                    {
                                        if (rNode.name.Equals(r.resourceName))
                                        {
                                            rCost = Tools.GetValueDefault(rNode, "cost", 0.0);
                                            //DBG print("Found resource " + r.resourceName + ", amount " + r.amount + ", cost = " + rCost);
                                            if (resources.ContainsKey(rNode.name))
                                                resources[rNode.name] = resources[rNode.name] + r.amount * rCost;
                                            else
                                                resources[rNode.name] = r.amount * rCost;

                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        foreach (ProtoPartSnapshot p in v.protoVessel.protoPartSnapshots)
                        {
                            int cst = p.partInfo.cost;
                            crewCount += p.protoModuleCrew.Count;
                            //print("part " + p.partName + " has cost " + cst);
                            podCost += cst;
                            // breakdown unnneccessary for recovery purposes.
                            // Now get resources:
                            // new resource calc
                            if (Tools.MCSettings != null)
                            {
                                foreach (ProtoPartResourceSnapshot r in p.resources)
                                {
                                    double rCost = 0;
                                    double amt = Tools.GetValueDefault(r.resourceValues, "amount", 0.0);
                                    //print(Tools.NodeToString(r.resourceValues, 0));
                                    if (!(amt > 0))
                                        continue;

                                    foreach (ConfigNode rNode in Tools.MCSettings.GetNode("RESOURCECOST").nodes)
                                    {
                                        if (rNode.name.Equals(r.resourceName))
                                        {
                                            rCost = Tools.GetValueDefault(rNode, "cost", 0.0);
                                            //DBG print("Found resource " + r.resourceName + ", amount " + r.amount + ", cost = " + rCost);
                                            if (resources.ContainsKey(rNode.name))
                                                resources[rNode.name] = resources[rNode.name] + amt * rCost;
                                            else
                                                resources[rNode.name] = amt * rCost;

                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            public int pod()
            {
                return (int)(podCost * ConstructionMode.TechConstrustion);
            }
            public int tank()
            {
                return (int)(tankCost * ConstructionMode.TechConstrustion);
            }
            public int engine()
            {
                return (int)(engineCost * ConstructionMode.TechConstrustion);
            }
            public int ctrl()
            {
                return (int)(controlCost * ConstructionMode.TechConstrustion);
            }
            public int util()
            {
                return (int)(utilCost * ConstructionMode.TechConstrustion);
            }
            public int sci()
            {
                return (int)(sciCost * ConstructionMode.TechConstrustion);
            }
            public int stru()
            {
                return (int)(structCost * ConstructionMode.TechConstrustion);
            }
            public int aero()
            {
                return (int)(aeroCost * ConstructionMode.TechConstrustion);
            }

            public int crew()
            {
                return (Tools.Setting("insurance", 5000) * crewCount);
            }
            
            public int dry()
            {
                return  pod() + tank() + engine() + ctrl() + util() + sci() + stru() + aero();               
            }
            public int wet()
            {
                double val = 0;
                foreach (double v in resources.Values)
                    val += v;

                return (int)Math.Round(val,0);
            }

            public int sum()
            {
                return wet() + dry() + crew();
            }

            public int recyclable(int sit)
            {
                switch(sit)
                {
                    case 1: // landed
                        return (int)(Tools.Setting("landedRecycle", 0.85) * dry() + Tools.Setting("fuelRecycle", 0.95) * wet()); 
                    // case 2: // landed on runway
                    case 3: // autorecycle with fuel
                        return (int)(Tools.Setting("autoRecycle", 0.63) * (Tools.Setting("runwayRecycle", 0.95) * dry() + Tools.Setting("fuelRecycle", 0.95) * wet())); 
                    case 4: // autorecycle without fuel
                        return (int)(Tools.Setting("autoRecycle", 0.63) * (Tools.Setting("runwayRecycle", 0.95) * dry())); 
                    default: // case 0, or any other.
                        return (int)(Tools.Setting("splashedRecycle", 0.65) * dry() + Tools.Setting("landedRecycle", 0.95) * wet());
                }
            }
            public int crewreturn(int sit)
            {
                return (int)crew();
            }
        }
    }
}

