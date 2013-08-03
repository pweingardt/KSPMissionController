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
                        double mult = 1.0;
                        // LAUNCH CLAMP HACK
                        bool isClamp = false;
                        foreach (LaunchClamp e in p.Modules.OfType<LaunchClamp>())
                        {
                            //print("part " + p.name + " is launch clamp");
                            isClamp = true;
                        }
                        if (isClamp)
                        {
                            mult = 1.0;
                            res.mass += p.mass;
                            continue;
                        }

                        foreach (ModuleEngines e in p.Modules.OfType<ModuleEngines>())
                        {
                            if (e.propellants.Where(r => r.name.Equals("SolidFuel")).Count() == 0)
                            {
                                double cst = Math.Pow((e.atmosphereCurve.Evaluate(0) * 0.8 + e.atmosphereCurve.Evaluate(1) * 0.2) - 200, 2) * e.maxThrust / 1000.0;
                                foreach (ModuleGimbal g in p.Modules.OfType<ModuleGimbal>())
                                {
                                    cst *= 1.0 + g.gimbalRange * 0.2;
                                }
                                if (e.propellants.Where(r => r.name.Equals("IntakeAir")).Count() != 0 || e.propellants.Where(r => r.name.Equals("KIntakeAir")).Count() != 0)
                                {
                                    cst *= 0.05;
                                }
                                cst = cst * Math.Pow(cst / p.mass / 4000, 0.25);
                                if (e.atmosphereCurve.Evaluate(0) > 600 && e.propellants.Where(r => r.name.Equals("XenonGas")).Count() == 0)
                                    cst *= 4; // special HACK handling for NTR
                                res.engineCost += 500 + cst;
                                mult *= 0.1;
                            }
                        }

                        //                        res.construction += p.partInfo.cost;
                        double lf = 0.0;
                        double ox = 0.0;
                        bool doOnce = false;
                        // NK add tank efficiency mult - DISABLED
                        if (p.Resources["LiquidFuel"] != null)
                        {
                            lf = p.Resources["LiquidFuel"].amount;
                            res.liquidFuel += lf;
                        }

                        if (p.Resources["SolidFuel"] != null)
                        {
                            res.solidFuel += p.Resources["SolidFuel"].amount;
                            //mult *= (p.Resources["SolidFuel"].amount / p.mass / 866); // normalized for RT-10
                        }

                        if (p.Resources["MonoPropellant"] != null)
                        {
                            res.monoFuel += p.Resources["MonoPropellant"].amount;
                            res.tankCost += p.Resources["MonoPropellant"].amount / p.mass / 666 * p.Resources["MonoPropellant"].amount * 0.1; // normalized for R25
                        }

                        if (p.Resources["Oxidizer"] != null)
                        {
                            ox = p.Resources["Oxidizer"].amount;
                            res.oxidizerFuel += ox;
                        }
                        
                        // edit in .12 to add support for iron cross mod -- malkuth .12 Also Added Support for Modular Fuel Mod Check Difficulty for Multipliers
                        if (p.Resources["Oxygen"] != null)
                        {
                            res.oxygen += p.Resources["Oxygen"].amount;
                        }
                        if (p.Resources["LiquidOxygen"] != null)
                        {
                            res.LiquidOxygen += p.Resources["LiquidOxygen"].amount;
                        }
                        if (p.Resources["LiquidH2"] != null)
                        {
                            res.LiquidH2 += p.Resources["LiquidH2"].amount;
                        }
                        if (lf + ox > 0)
                        {
                            res.tankCost += Math.Pow((lf + ox) / p.mass / 1600, 2) * (lf + ox) * 0.1; // normalized for FL-T800 
                        }

                        if (p.Resources["XenonGas"] != null)
                        {
                            res.xenonFuel += p.Resources["XenonGas"].amount;
                            res.tankCost += p.Resources["XenonGas"].amount / p.mass / 5833 * p.Resources["XenonGas"].amount * 0.25; // normalized for R25
                        }

                        // NK add category detection, module detection, etc
                        // also DRE support
                        if (p.Resources["AblativeShielding"] != null)
                        {
                            res.podCost += p.Resources["AblativeShielding"].amount * 50;
                        }

                        // PODS
                        bool isPod = false;
                        if (p.partInfo.category.Equals(PartCategories.Pods))
                        {
                            mult *= 2;
                            //print("part " + p.name + " is a pod");
                        }
                        doOnce = false;
                        foreach (ModuleCommand e in p.Modules.OfType<ModuleCommand>())
                        {
                            if (doOnce)
                                continue;
                            if (!p.partInfo.name.Contains("RemoteTech")) // HACK for RC antenna
                            {
                                res.podCost += (p.mass - p.CrewCapacity / 4.0) * 10 * 3500;
                                if (p.CrewCapacity > 0)
                                    res.podCost += p.CrewCapacity * Math.Sqrt(p.CrewCapacity / p.mass / 0.8) * 7500;
                                else
                                    res.podCost += 750 / p.mass;
                                //print("part " + p.name + " has cmd");
                                isPod = true;
                            }
                            doOnce = true;
                        }

                        // PROPULSION - taken care of above: engines, LF, SF, MP.

                        // CONTROL
                        if (p.partInfo.category.Equals(PartCategories.Control))
                        {
                            //print("part " + p.name + " is ctrl");
                            bool hasRCS = false;
                            foreach (ModuleRCS r in p.Modules.OfType<ModuleRCS>())
                            {
                                hasRCS = true;
                                //print("part " + p.name + " has rcs");
                                res.controlCost += Math.Pow(r.atmosphereCurve.Evaluate(0) - 200, 2) * r.thrusterPower;
                            }
                            if (!hasRCS)
                            {
                                res.controlCost += p.mass * 40000;
                                if (p.partInfo.moduleInfo.Equals("AdvSASModule"))
                                    res.controlCost += p.mass * 40000;
                            }
                        }

                        // STRUCTURAL
                        if (p.partInfo.category.Equals(PartCategories.Structural))
                        {
                            //print("part " + p.name + " is structural");
                            mult *= 0.1;
                        }
                        foreach (ModuleAnchoredDecoupler e in p.Modules.OfType<ModuleAnchoredDecoupler>())
                        {
                            //print("part " + p.name + " is decoupler");
                            mult *= 1.5;
                        }
                        foreach (ModuleDecouple e in p.Modules.OfType<ModuleDecouple>())
                        {
                            //print("part " + p.name + " is decoupler");
                            mult *= 1.5;
                        }
                        // better would be check force

                        // AERODYNAMIC
                        // leave as stock: 3500/ton. Yes, this is bad for nosecones vs adapters
                        if (p.partInfo.moduleInfo.Equals("ControlSurface"))
                        {
                            //print("part " + p.name + " is controlsurface");
                            mult *= 2.0; // control surfaces are a bit more expensive
                        }


                        // UTILITY
                        if (p.partInfo.category.Equals(PartCategories.Utility))
                        {
                            //print("part " + p.name + " is utility");
                            mult *= 2;
                        }
                        foreach (ModuleDockingNode e in p.Modules.OfType<ModuleDockingNode>())
                        {
                            //print("part " + p.name + " has docking port");
                            res.utilCost += Math.Sqrt(p.mass) * 5000;
                        }
                        foreach (ModuleLandingGear e in p.Modules.OfType<ModuleLandingGear>())
                        {
                            //print("part " + p.name + " has gear");
                            mult *= 0.1;
                            res.utilCost += p.mass * 3500 * 0.5;
                        }
                        /*if (p.partInfo.moduleInfo.Equals("HLandingLeg"))
                        {
                            mult *= 0.1;
                        }*/
                        // and wheels can stay as they are, too.

                        foreach (ModuleDeployableSolarPanel s in p.Modules.OfType<ModuleDeployableSolarPanel>())
                        {
                            //print("part " + p.name + " is solar panel");
                            // mult is fine for mass; change cost
                            double panelmult = 1.0;
                            if (s.sunTracking)
                                panelmult = 3.0;
                            res.utilCost += (s.chargeRate * 1200) * panelmult; // *s.chargeRate / p.mass / 150; // baseline = OX-STAT, so need high mult for tracking
                        }
                        // for generators and electric charge, check efficiency
                        if (p.Resources["ElectricCharge"] != null)
                        {
                            //print("part " + p.name + " has electric charge");
                            res.utilCost += p.Resources["ElectricCharge"].amount * 12 * (p.Resources["ElectricCharge"].amount / p.mass / 20000); //baseline = Z-100
                        }
                        foreach (ModuleGenerator g in p.Modules.OfType<ModuleGenerator>())
                        {
                            //print("part " + p.name + " is generator");
                            if (g.inputList.Count <= 0) // from nothing
                            {
                                foreach (ModuleGenerator.GeneratorResource gr in g.outputList)
                                {
                                    if (gr.name.Equals("ElectricCharge"))
                                        res.utilCost += 10000 + gr.rate * 5000 * gr.rate / p.mass / 9.375;
                                }
                            }
                        }




                        // SCIENCE
                        if (p.partInfo.category.Equals(PartCategories.Science))
                        {
                            //print("part " + p.name + " is science");
                            //mult *= 10;
                            res.sciCost += p.mass * 300000;
                            foreach (ModuleEnviroSensor e in p.Modules.OfType<ModuleEnviroSensor>())
                                res.sciCost += 1000; // fixed cost for now
                            //But for other science (Keth, RemoteTech) it's mass only alas.
                        }

                        if (p.CrewCapacity > 0)
                        {
                            if (!isPod)
                            {
                                //print("part " + p.name + " has crew (and no cmd)");
                                mult = 2.0; // reset for struct, util
                            }
                            res.podCost += (p.CrewCapacity) * 25000;
                        }

                        res.mass += p.mass * mult;

                    }
                }
                catch
                {
                }
                return res;
            }
        }
        // edited .11 add some new values.. .12 edit by malkuth to add support for Iron Cross Mod and Modular fuel tanks
        const double costmultiplier = 0.1;

        private class VesselResources
        {
            public double liquidFuel;
            public double oxidizerFuel;
            public double solidFuel;
            public double monoFuel;
            public double mass;
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

            public int pod()
            {
                return (int)(costmultiplier * podCost * Difficulty.Xenon * 0.1); // since Xenon = 10
            }
            public int tank()
            {
                return (int)(costmultiplier * tankCost * Difficulty.Xenon * 0.1); // since Xenon = 10
            }
            public int ctrl()
            {
                return (int)(costmultiplier * controlCost * Difficulty.Xenon * 0.1); // since Xenon = 10
            }
            public int util()
            {
                return (int)(costmultiplier * utilCost * Difficulty.Xenon * 0.1); // since Xenon = 10
            }
            public int sci()
            {
                return (int)(costmultiplier * sciCost * Difficulty.Xenon * 0.1); // since Xenon = 10
            }


            public int materials()
            {
                return (int)(costmultiplier * 2 * mass * Difficulty.Mass);
            }
            public int oxylife()
            {
                return (int)(costmultiplier * 10 * oxygen * Difficulty.LiquidFuel);
            }
            public int LiquidOxy()
            {
                return (int)(costmultiplier * 10 * LiquidOxygen * Difficulty.LiquidOxygen);
            }
            public int LiquidH()
            {
                return (int)(costmultiplier * 10 * LiquidH2 * Difficulty.LiquidH2);
            }

            public int liquid()
            {
                return (int)(costmultiplier * 10 * liquidFuel * Difficulty.LiquidFuel);
            }

            public int mono()
            {
                return (int)(costmultiplier * 10 * monoFuel * Difficulty.MonoPropellant);
            }

            public int solid()
            {
                return (int)(costmultiplier * 10 * solidFuel * Difficulty.SolidFuel);
            }

            public int xenon()
            {
                return (int)(costmultiplier * 10 * xenonFuel * Difficulty.Xenon);
            }

            public int oxidizer()
            {
                return (int)(costmultiplier * 10 * oxidizerFuel * Difficulty.Oxidizer);
            }

            public int engine()
            {
                return (int)(costmultiplier * 4 * engineCost * Difficulty.LiquidEngines);
            }

            public int crew()
            {
                return SettingsManager.Manager.getSettings().kerbonautCostAsInt * crewCount;
            }

            public int dry()
            {
                return pod() + tank() + engine() + ctrl() + util() + sci() + materials();
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

