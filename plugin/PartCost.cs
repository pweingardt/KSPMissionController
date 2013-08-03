using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionController
{
    public class PartCost
    {
        /// <summary>
        /// Calculates the cost of a part
        /// </summary>
        /// <returns>The cost.</returns>
        /// <param name="pt">AvailablePart.</param>
        public static int cost(AvailablePart pt)
        {
            return cost(pt.partPrefab);
        }


        /// <summary>
        /// Calculates the cost of a part
        /// </summary>
        /// <returns>The cost.</returns>
        /// <param name="pt">AvailablePart.</param>
        public static int cost(Part p)
        {
            double pcst = 0;
            const double massCost = 700;
            try
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
                    return (int)(massCost * 0.05);
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
                        pcst += 250 + cst * 0.5;
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
                    lf = p.Resources["LiquidFuel"].maxAmount;
                }
                if (p.Resources["Oxidizer"] != null)
                {
                    ox = p.Resources["Oxidizer"].maxAmount;
                    //res.oxidizerFuel += ox;
                }
                if (lf + ox > 0)
                {
                    mult = 0.1;
                    pcst += Math.Pow((lf + ox) / p.mass / 1600, 2) * (lf + ox) * 0.25; // normalized for FL-T800 
                }

                if (p.Resources["SolidFuel"] != null)
                {
                    //res.solidFuel += p.Resources["SolidFuel"].maxAmount;
                    //mult *= (p.Resources["SolidFuel"].amount / p.mass / 866); // normalized for RT-10
                }

                if (p.Resources["MonoPropellant"] != null)
                {
                    mult = 0.1;
                    //res.monoFuel += p.Resources["MonoPropellant"].amount;
                    pcst += p.Resources["MonoPropellant"].maxAmount / p.mass / 666 * p.Resources["MonoPropellant"].maxAmount * 0.15; // normalized for R25
                }

                if (p.Resources["XenonGas"] != null)
                {
                    mult = 0.1;
                    //res.xenonFuel += p.Resources["XenonGas"].amount;
                    pcst += p.Resources["XenonGas"].maxAmount / p.mass / 5833 * p.Resources["XenonGas"].maxAmount * 1.0; // normalized for R25
                }

                // NK add category detection, module detection, etc
                // also DRE support
                if (p.Resources["AblativeShielding"] != null)
                {
                    pcst += p.Resources["AblativeShielding"].maxAmount * 5;
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
                        pcst += (p.mass - p.CrewCapacity / 4.0) * 10 * 350;
                        if (p.CrewCapacity > 0)
                            pcst += p.CrewCapacity * Math.Sqrt(p.CrewCapacity / p.mass / 0.8) * 750;
                        else
                            pcst += 75 / p.mass;
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
                        pcst += Math.Pow(r.atmosphereCurve.Evaluate(0) - 200, 2) * r.thrusterPower * 0.1;
                    }
                    if (!hasRCS)
                    {
                        pcst += p.mass * 4000;
                        if (p.partInfo.moduleInfo.Equals("AdvSASModule"))
                            pcst += p.mass * 4000;
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
                    pcst += Math.Sqrt(p.mass) * 500;
                }
                foreach (ModuleLandingGear e in p.Modules.OfType<ModuleLandingGear>())
                {
                    //print("part " + p.name + " has gear");
                    mult *= 0.1;
                    pcst += p.mass * 350 * 0.5;
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
                    pcst += (s.chargeRate * 120) * panelmult; // *s.chargeRate / p.mass / 150; // baseline = OX-STAT, so need high mult for tracking
                }
                // for generators and electric charge, check efficiency
                if (p.Resources["ElectricCharge"] != null)
                {
                    //print("part " + p.name + " has electric charge");
                    pcst += p.Resources["ElectricCharge"].amount * 1.2 * (p.Resources["ElectricCharge"].amount / p.mass / 20000); //baseline = Z-100
                }
                foreach (ModuleGenerator g in p.Modules.OfType<ModuleGenerator>())
                {
                    //print("part " + p.name + " is generator");
                    if (g.inputList.Count <= 0) // from nothing
                    {
                        foreach (ModuleGenerator.GeneratorResource gr in g.outputList)
                        {
                            if (gr.name.Equals("ElectricCharge"))
                                pcst += 1000 + gr.rate * 500 * gr.rate / p.mass / 9.375; // normalized for stock RTG
                        }
                    }
                }




                // SCIENCE
                if (p.partInfo.category.Equals(PartCategories.Science))
                {
                    //print("part " + p.name + " is science");
                    //mult *= 10;
                    pcst += p.mass * 30000;
                    foreach (ModuleEnviroSensor e in p.Modules.OfType<ModuleEnviroSensor>())
                        pcst += 100; // fixed cost for now
                    //But for other science (Keth, RemoteTech) it's mass only alas.
                }

                if (p.CrewCapacity > 0)
                {
                    if (!isPod)
                    {
                        //print("part " + p.name + " has crew (and no cmd)");
                        mult = 2.0; // reset for struct, util
                    }
                    pcst += (p.CrewCapacity) * 2500;
                }

                pcst += p.mass * mult * massCost;
            }
            catch
            {
            }
            return Math.Max((int)pcst, 1); // so struct parts don't cost 0!
        }
    }
}