using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionController
{
    public class PartCost //DBG : MonoBehaviour
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
        /// <param name="pt">Part.</param>
        public static int cost(Part p)
        {
            double pcst = 0;
            try
            {

                if(Tools.MCSettings == null)
                    return 0;

                double massCost = Tools.tryDouble(Tools.MCSettings, "massCost", 700);
                double totalCostMult = Tools.tryDouble(Tools.MCSettings, "totalCostScalar", 1.0);
                double massCostMult = 1.0;

                //get base multiplier
                switch (p.partInfo.category)
                {
                    case PartCategories.Pods:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Pods", 2);
                        break;
                    case PartCategories.Propulsion:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Propulsion", 0.1);
                        break;
                    case PartCategories.Control:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Control", 0.1);
                        break;
                    case PartCategories.Structural:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Structural", 0.1);
                        break;
                    case PartCategories.Aero:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Aero", 2.0);
                        break;
                    case PartCategories.Utility:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Utility", 2.0);
                        break;
                    case PartCategories.Science:
                        massCostMult = Tools.tryDouble(Tools.MCSettings.GetNode("CATEGORYMASSCOSTMULT"), "Science", 7.0);
                        break;
                }

                // get crew capacity
                pcst += p.CrewCapacity * Tools.tryDouble(Tools.MCSettings, "costPerCrew", 6000);
                //DBG print"*MCEPC* " + p.name + ", m" + massCostMult + ", c" + pcst);
                foreach(ConfigNode mNode in Tools.MCSettings.GetNode("MODULECOST").nodes)
                {
                    double cst = 0;
                    if (p.Modules.Contains(mNode.name)) // part has this node's module
                    {
                        //DBG print"found module " + mNode.name);
                        //set up base part cost
                        // check for Nodes, and apply values in the nodes as multipliers to the partmodule's variable named as the keys.
                        // like, if FLOATS has chargeRate, apply chargeRate's value as a multiplier to partmodule.chargerate, and add
                        // that to the cost.
                        if (mNode.HasNode("FLOATS"))
                        {
                            foreach (string valName in mNode.GetNode("FLOATS").values.DistinctNames())
                            {
                                double valCost = Tools.atod(mNode.GetNode("FLOATS").GetValue(valName));
                                cst += ((float)p.Modules[mNode.name].Fields.GetValue(valName)) * valCost;
                            }
                        }

                        // special handling for engines/RCS
                        bool doEngine = false;

                        // this stuff is so we can combine engines and RCS since they use the same code
                        // just engines have some special factors
                        double gimbalFactor = 1.0;
                        double propMult = 1.0;
                        double nukeMult = 1.0;
                        double ispSL = 0;
                        double ispV = 0;
                        double thrust = 0;
                        if (mNode.name.Equals("ModuleEngines"))
                        {
                            doEngine = true;
                            ModuleEngines e = (ModuleEngines)p.Modules["ModuleEngines"];

                            ispSL = e.atmosphereCurve.Evaluate(1);
                            ispV = e.atmosphereCurve.Evaluate(0);
                            thrust = e.maxThrust;

                            bool foundPropMod = false;
                            foreach(ModuleEngines.Propellant r in e.propellants)
                            {
                                if(mNode.HasValue(r.name))
                                {
                                    propMult *= Tools.atod(mNode.GetValue(r.name));
                                    foundPropMod = true;
                                }
                            }

                                
                            if (!foundPropMod && ispV > 600)
                                nukeMult = Tools.tryDouble(mNode, "nukeMult", nukeMult);

                            foreach (ModuleGimbal g in p.Modules.OfType<ModuleGimbal>())
                                gimbalFactor = 1.0 + g.gimbalRange * Tools.tryDouble(mNode, "gimbalFactor", 0.2);
                        }
                        if (mNode.name.Equals("ModuleRCS"))
                        {
                            doEngine = true;
                            ModuleRCS e = (ModuleRCS)p.Modules["ModuleRCS"];
                            ispSL = e.atmosphereCurve.Evaluate(1);
                            ispV = e.atmosphereCurve.Evaluate(0);
                            thrust = e.thrusterPower;
                        }
                        if(doEngine)
                        {
                            //DBG print"Found engine/rcs: " + ispSL + "-" + ispV);
                            double atmoRatio = Tools.tryDouble(mNode, "atmoRatio", 0.2);
                            double ispOffset = Tools.tryDouble(mNode, "ispOffset", 200);
                            double power = Tools.tryDouble(mNode, "power", 2.0);
                            double scalar = Tools.tryDouble(mNode, "scalar", 0.001);

                            double ecst = Math.Pow((ispSL * atmoRatio + ispV * (1-atmoRatio)) - ispOffset, power) * thrust * scalar;
                            ecst *= gimbalFactor;
                            ecst *= propMult;
                            cst = ecst;
                        }

                        // another special handling function: generators
                        if (mNode.name.Equals("ModuleGenerator"))
                        {
                            if (!p.Modules.Contains("LaunchClamp"))
                            {
                                foreach (ModuleGenerator g in p.Modules.OfType<ModuleGenerator>())
                                {
                                    //DBG print"part " + p.name + " is generator");
                                    if (g.inputList.Count <= 0) // from nothing
                                    {
                                        foreach (ModuleGenerator.GeneratorResource gr in g.outputList)
                                        {
                                            if (gr.name.Equals("ElectricCharge"))
                                                cst = gr.rate;
                                        }
                                    }
                                }
                            }
                        }

                        // apply modifiers
                        if (mNode.HasValue("effScalar"))
                        {
                            double effS, effP;
                            effS = Tools.atod(mNode.GetValue("effScalar"));
                            effP = Tools.tryDouble(mNode, "effPower", 1.0);
                            if (cst < 1)
                                cst = 1;
                            ////DBG print" cost " + cst + ", es " + effS + "," + effP + "\n");
                            cst *= Math.Pow(cst / p.mass * effS, effP);
                        }
                        cst *= Tools.tryDouble(mNode, "costMult", 1.0);
                        cst += Tools.tryDouble(mNode, "costAdd", 0);
                        ////DBG print"module cost = " + cst);
                        pcst += cst; // add this module's cost to the part

                        // apply for other modules
                        massCostMult *= Tools.tryDouble(mNode, "massCostMult", 1.0);
                        totalCostMult *= Tools.tryDouble(mNode, "totalCostMult", 1.0);
                    }
                }
                //DBG print"Part cost now " + pcst);
                // now add partcost based on tankage
                foreach (ConfigNode rNode in Tools.MCSettings.GetNode("RESOURCECOST").nodes)
                    if (p.Resources[rNode.name] != null)
                        pcst += Tools.tryDouble(rNode, "tank", 0.0) * ((PartResource)p.Resources[rNode.name]).maxAmount;
                
                //DBG print"After resources, part cost now " + pcst);

                pcst += p.mass * massCostMult * massCost * FuelMode.TechFuel;
                pcst *= totalCostMult;
            }
            catch
            {
            }
            return Math.Max((int)pcst, 1); // so struct parts don't cost 0!
        }
    }
}