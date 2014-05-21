using System;
using UnityEngine;
// for recycling
using System.Collections.Generic;
using System.Linq;

namespace MissionController
{
    /// <summary>
    /// This part of the class handles the incoming events from with GameEvents
    /// </summary>
    public partial class MissionController
    {
        private bool canRecycle = true;
        /// <summary>
        /// This event is fired when two vessels dock.
        /// </summary>
        /// <param name="action">Action.</param>
        private void onPartCouple(GameEvents.FromToAction<Part, Part> action) {
            if (HighLogic.LoadedSceneIsFlight) {
                eventFlags = eventFlags.Add (EventFlags.DOCKED);
                Debug.LogError ("Docked FROM: " + action.from.vessel.vesselName);
                Debug.LogError ("Docked TO: " + action.to.vessel.vesselName);
                manager.currentdockedToVessel(action.from.vessel.vesselName);

                Debug.LogError("Docked TO Type Vessel: " + action.to.vessel.vesselType);

                Debug.LogError ("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError ("Docked TO ID: " + action.to.vessel.id.ToString());
            }
        }

        public static object GetMemberInfoValue(System.Reflection.MemberInfo member, object sourceObject)
        {
            object newVal;
            if (member is System.Reflection.FieldInfo)
                newVal = ((System.Reflection.FieldInfo)member).GetValue(sourceObject);
            else
                newVal = ((System.Reflection.PropertyInfo)member).GetValue(sourceObject, null);
            return newVal;
        }
        
        private void onUndock(Part action)
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                eventFlags = eventFlags.Add(EventFlags.UNDOCKED);
                print("eventFlag set: " + eventFlags);
                print("Vessel Undockd From: " + action);
                manager.currentdockedToVessel("None");
            }

        }

        // NK move recycle functionality to recovery

        private void onCreate(Vessel v)
        {
            // for now, just reenable recycling. It's so you don't recycle on return to spaceport or revert.
            //canRecycle = false;
        }
        
        /// <summary>
        /// Recycle vessel whenever recovered
        /// </summary>
        /// <param name="pv">the vessel</param>
        private void onRecovered(ProtoVessel pv)
        {
            VesselResources res = new VesselResources(pv.vesselRef);
            recycledName = pv.vesselName;
            if (!manager.isVesselFlagged(pv.vesselRef) && !HighLogic.LoadedSceneIsEditor && !HighLogic.LoadedSceneIsFlight && (manager.ResearchRecycle || HighLogic.CurrentGame.Mode != Game.Modes.CAREER) && (pv.situation.Equals(Vessel.Situations.LANDED) || pv.situation.Equals(Vessel.Situations.SPLASHED)))
            {
                recycledCost = res.recyclable(pv.situation.Equals(Vessel.Situations.LANDED) ? 1 : 0);
                print("*MC* Craft " + recycledName + " recovered for " + recycledCost);
                manager.recycleVessel(pv.vesselRef, recycledCost);
                showRecycleWindow = true;
            }
            
            pVessel = null;
        }

        /// <summary>
        /// update pvessel when target changes
        /// </summary>
        /// <param name="mo">the target</param>
        private void onTargeted(MapObject mo)
        {
            if (HighLogic.LoadedScene.Equals(GameScenes.TRACKSTATION) && mo.type.Equals(MapObject.MapObjectType.VESSEL))
            {
                //pVessel = new ProtoVessel(mo.vessel); //  HACK: should find the right one.
                pVessel = mo.vessel.protoVessel;
            }
        }




        /// <summary>
        /// We check if the vessel was crashing. If so, we set the crashed flag
        /// </summary>
        /// <param name="v">V.</param>
        private void onVesselDestroy(Vessel v) {
            // If it has been destroyed under 10 meters above surface, it probably crashed
            if (v.mainBody.GetAltitude(v.CoM) - (v.terrainAltitude < 0 ? 0 : v.terrainAltitude) < 10)
            {
                eventFlags = eventFlags.Add(EventFlags.CRASHED);
                print(" MCE Vessel Flaged as Crashed: " + v.name);
            }          
            // NK recycle
            else
            {
                try { print("*MC* Vessel " + v.name + " destroyed. Alt " + v.mainBody.GetAltitude(v.CoM) + ", body " + v.orbit.referenceBody.bodyName + ", sit = " + v.situation); }
                catch { }
                if (!manager.isVesselFlagged(activeVessel) && !HighLogic.LoadedSceneIsEditor && canRecycle && activeVessel != v && !v.isEVA // canRecycle is false iff load requested and haven't returned to flight yet.
                    && v.name.Contains("(Unloaded)") // check make sure it's because we're unloading it
                    && (v.situation == Vessel.Situations.FLYING || v.situation == Vessel.Situations.SUB_ORBITAL) && v.mainBody.GetAltitude(v.CoM) <= 25000
                    && v.orbit.referenceBody.bodyName.Equals("Kerbin") && (manager.ResearchRecycle || HighLogic.CurrentGame.Mode != Game.Modes.CAREER))
                {
                    print("*MC* Checking " + v.name);
                    double mass = 0;
                    double rmass = 0;
                    double pdrag = 0.0;
                    float mult = .60f;
                    int cost = 0;
                    float totalDrag = 0;
                    bool realchutesInstalled = false;
                    bool FXEngineInstalled = false;
                    // need 70 drag per ton of vessel for 6m/s at 500m.
                    double jetIsp = Tools.Setting("jetIsp", 600.0);
                    List<ModuleEngines> engines = new List<ModuleEngines>();
                    List<ModuleEnginesFX> enginesFX = new List<ModuleEnginesFX>();
                    List<ModuleEngines> jets = new List<ModuleEngines>();
                    List<ModuleEnginesFX> jetsFX = new List<ModuleEnginesFX>();
                    Dictionary<string, double> resources = new Dictionary<string, double>();
                    Dictionary<string, double> rmasses = new Dictionary<string, double>();
                    try
                    {
                        if (!v.packed)
                            foreach (Part p in v.Parts)
                                p.Pack();
                        
                        // Thanks to Magico13 for code to unlock Realchutes again, most realchute code based off of his code.

                        foreach (ProtoPartSnapshot p in v.protoVessel.protoPartSnapshots) 
                        {
                            print("Has part " + p.partName + ", mass " + p.mass + ", cost " + p.partRef.partInfo.cost);
                            mass += p.mass;
                            cost += p.partRef.partInfo.cost;
                            foreach (ProtoPartResourceSnapshot r in p.resources)
                            {
                                double amt = Tools.GetValueDefault(r.resourceValues, "amount", 0.0);
                                print("Printing Values Line For ProtoPartResourceSnapshot " + r.resourceValues);
                                print("Printing Direct Line for ProtPart: " + p.resources.ToString());
                                if (!(amt > 0))
                                    continue;
                                double dens = r.resourceRef.info.density;
                                if (resources.ContainsKey(r.resourceName))
                                {
                                    resources[r.resourceName] = resources[r.resourceName] + amt;
                                    rmasses[r.resourceName] = rmasses[r.resourceName] + amt * dens;
                                    print("If Resource Contains Key Test True: " + rmasses + " and " + resources);
                                }
                                else
                                {
                                    resources[r.resourceName] = amt;
                                    rmasses[r.resourceName] = amt * dens;
                                    print("If Resource Contains Key Test false: " + rmasses + " and " + resources);
                                }
                                rmass += amt * dens;
                                Debug.LogWarning("Resource Mass =  " + rmass + " Density =  " + dens + " Amount In this Part =  " + amt + " Type Of Resource is: " + r.resourceName);
                            }
                            foreach (ProtoPartModuleSnapshot m in p.modules)                                
                            {
                                
                                if (p.partRef.Modules.Contains("RealChuteModule"))
                                {
                                    PartModule realChute = p.partRef.Modules["RealChuteModule"];
                                    Type rCType = realChute.GetType();
                                    System.Reflection.MemberInfo member = rCType.GetMember("deployedDiameter")[0];
                                    float area = (float)GetMemberInfoValue(member, realChute);
                                    area = Mathf.PI * Mathf.Pow(area / 2, 2); //Determine the area manually since the "deployedArea" parameter no longer exists in RC
                                    Debug.Log("Chute area: " + area);

                                    member = rCType.GetMember("material")[0];
                                    string mat = (string)GetMemberInfoValue(member, realChute);
                                    Debug.Log("Material for RC is " + mat);

                                    Type matLibraryType = AssemblyLoader.loadedAssemblies
                                        .SelectMany(a => a.assembly.GetExportedTypes())
                                        .SingleOrDefault(t => t.FullName == "RealChute.Libraries.MaterialsLibrary");

                                    System.Reflection.MethodInfo matMethod = matLibraryType.GetMethod("GetMaterial", new Type[] { mat.GetType() });
                                    object MatLibraryInstance = matLibraryType.GetProperty("instance").GetValue(null, null);
                                    object materialObject = matMethod.Invoke(MatLibraryInstance, new object[] { mat });

                                    float dragC = (float)GetMemberInfoValue(materialObject.GetType().GetMember("dragCoefficient")[0], materialObject);
                                    Debug.Log("dragC: " + dragC);                                   
                                    
                                    totalDrag += (dragC * area * mult);
                                    
                                    pdrag = p.mass * (double)totalDrag;
                                    Debug.LogWarning("mass " + p.mass + " * " + totalDrag + "  = " + pdrag);
                                    
                                    realchutesInstalled = true;
                                    Debug.LogWarning("Realchutes Found Set to True" + totalDrag + " Is the total Drag of part: " + p.partName);
                                }
                                                         
                                if (m.moduleName.Equals("ModuleParachute") && realchutesInstalled != true)
                                {
                                    Debug.Log("[MCE] Found ModuleParachute on part");
                                    ModuleParachute mp = (ModuleParachute)m.moduleRef;
                                    mp.Load(m.moduleValues);
                                    pdrag += p.mass * mp.fullyDeployedDrag;
                                    Debug.LogWarning("mass " + p.mass + " * " + mp.fullyDeployedDrag + "  = " + pdrag);
                                }
                                if (m.moduleName.Equals("ModuleEngines"))
                                {
                                    ModuleEngines me = (ModuleEngines)m.moduleRef;
                                    me.Load(m.moduleValues);
                                    print("Found engine, SL Isp = " + me.atmosphereCurve.Evaluate(1) + " (jet cutoff: " + jetIsp + ")");
                                    if (me.atmosphereCurve.Evaluate(1) > jetIsp)
                                        jets.Add(me);
                                    else
                                    {
                                        engines.Add(me);                                     
                                    }
                                }
                                if (m.moduleName.Equals("ModuleEnginesFX"))
                                {
                                    ModuleEnginesFX mefx = (ModuleEnginesFX)m.moduleRef;
                                    mefx.Load(m.moduleValues);
                                    print("Found engine, SL Isp = " + mefx.atmosphereCurve.Evaluate(1) + " (jet cutoff: " + jetIsp + ")");
                                    if (mefx.atmosphereCurve.Evaluate(1) > jetIsp)
                                        jetsFX.Add(mefx);
                                    else
                                    {
                                        enginesFX.Add(mefx);
                                    }
                                }        
                            }
                        }

                        Debug.LogWarning("Para Landing Check, Is Total Parts Mass " + mass * Tools.Setting("parachuteDragPerTon", 70.0) + " < pdrag " + pdrag + " ?");
                        if (mass * Tools.Setting("parachuteDragPerTon", 70.0) <= pdrag)
                        {                          
                            recycledName = v.name;
                            VesselResources vr = new VesselResources(v);
                            int type = (mass + rmass) * Tools.Setting("parachuteDragPerTon", 70.0) <= pdrag ? 3 : 4;
                            recycledCost = vr.recyclable(type); // can we recycle fuel too?
                            recycledDesc = "Parachute landing (" + (type == 3 ? "with fuel" : "after fuel dumped") + ").";
                            print("*MC* Recycling vessel: enough parachutes! Val: " + recycledCost);
                            showRecycleWindow = true;
                            manager.recycleVessel(v, recycledCost);
                            realchutesInstalled = false;
                        }
                        else
                        {
                            int landing = 0;
                            List<string> props = new List<string>();
                            // try for propulsive landing
                            if (jets.Count > 0) // jets! Assume vessel is a plane. Heck, if it's a rocket, it will have >1TWR anyway.
                            {
                                Debug.LogWarning("Starting Jets Landing Algorithm");
                                // find best SL Isp
                                double isp = 0;
                                ModuleEngines e = jets[0];
                                double thrust = 0;
                                foreach (ModuleEngines ei in jets)
                                {
                                    if (ei.atmosphereCurve.Evaluate(1) > isp)
                                    {
                                        isp = ei.atmosphereCurve.Evaluate(1);
                                        e = ei;
                                        thrust += ei.maxThrust;
                                    }
                                }

                                // find propellants
                                List<double> rats = new List<double>();
                                double ratSum = 0;
                                foreach (Propellant pr in e.propellants)
                                {
                                    if (!(pr.name.ToLower().Contains("air") || pr.name.ToLower().Contains("electric") || pr.name.ToLower().Contains("coolant")))
                                    {
                                        props.Add(pr.name);
                                        rats.Add(pr.ratio);
                                        ratSum += pr.ratio;
                                    }

                                }
                                if (ratSum == 0)
                                    ratSum = 1.0;

                                // HACK TIME! :)
                                double nWgt = (rmass + mass) * e.g;
                                if (thrust > nWgt) // don't need > 1.0 TWR to land with jets. But they'll be at 0.5x maxthrust when slow, so...still need 1.0 twr max.
                                    thrust = nWgt;
                                landing = 1;
                                double jetFuelMult = Tools.Setting("jetFuelMultiplier", 0.5);
                                for (int i = 0; i < props.Count; i++)
                                {
                                    double req = thrust * jetFuelMult * (rats[i] / ratSum);
                                    double amt = resources[props[i]];
                                    print("Fuel needed: " + req + " " + props[i] + "(avail: " + amt + ")");
                                    if (amt < req)
                                        landing = 0;
                                }
                            }
                            if (landing == 0 && enginesFX.Count > 0 && (manager.GetRocketAutoLand || HighLogic.CurrentGame.Mode != Game.Modes.CAREER)) // propulsive via rockets
                            {
                                Debug.LogWarning("Starting Propulsive Landing Algorithm for ModuleEngineFX");
                                double isp = 999999;                               
                                ModuleEnginesFX efx = enginesFX[0];
                                double thrust = 0;                                
                                foreach (ModuleEnginesFX eifx in enginesFX)
                                    if (eifx.atmosphereCurve.Evaluate(1) < isp)
                                    {
                                        isp = eifx.atmosphereCurve.Evaluate(1);
                                        efx = eifx;
                                        thrust += eifx.maxThrust;
                                        print("ModuleEngineFX ISP Is: " + isp + " thrust is: " + thrust);
                                    }
                                    else print("No ModuleEngineFX found to run ISP and Thrust Values");
                                
                                double rmassdry = rmass;
                                                                
                                foreach (Propellant prfx in efx.propellants)
                                {
                                    if (rmasses.Keys.Contains(prfx.name))
                                    {
                                        rmassdry -= rmasses[prfx.name];
                                        print("ModuleEngine Using propellant " + prfx.name + "(mass: " + rmasses[prfx.name] + ")");
                                    }
                                    else print("[ModuleEngineFX] no prepellant found to run propellant and mass Values");
                                }
                                FXEngineInstalled = true;
                                double TWR = thrust / (mass + rmassdry) / efx.g;
                                double dV = isp * 9.81 * Math.Log((mass + rmass) / (mass + rmassdry));
                                print("DeltaV available: " + dV + "(Mass ratio: " + (mass + rmassdry) + " / " + (mass + rmass) + ", TWR " + TWR + ")");
                                if (dV >= Tools.Setting("deltaVRequired", 1000.0) && TWR >= Tools.Setting("minRocketTWR", 1.5))
                                {
                                    print("Rocket Assisted Landing Succesfull With ModuleEngineFX.. Kerbal X tycoon");
                                    landing = 2;
                                }
                                else print("Delta V id not pass to allow Propulsive Rocket Landing");
                            }
                            if (FXEngineInstalled == false && landing == 0 && engines.Count > 0 && (manager.GetRocketAutoLand || HighLogic.CurrentGame.Mode != Game.Modes.CAREER)) // propulsive via rockets
                            {
                                Debug.LogWarning("Starting Propulsive Landing Algorithm for ModuleEngine");
                                double isp = 999999;
                                ModuleEngines e = engines[0];                               
                                double thrust = 0;
                                foreach (ModuleEngines ei in engines)
                                    if (ei.atmosphereCurve.Evaluate(1) < isp)
                                    {
                                        isp = ei.atmosphereCurve.Evaluate(1);
                                        e = ei;
                                        thrust += ei.maxThrust;
                                        print("ModuleEngine ISP Is: " + isp + " thrust is: " + thrust);
                                    }
                                    else print("No ModuleEngine found to run ISP and Thrust Values");                             

                                double rmassdry = rmass;

                                foreach (Propellant pr in e.propellants)
                                {
                                    if (rmasses.Keys.Contains(pr.name))
                                    {
                                        rmassdry -= rmasses[pr.name];
                                        print("ModuleEngine Using propellant " + pr.name + "(mass: " + rmasses[pr.name] + ")");
                                    }
                                    else print("[ModuleEngine] no prepellant found to run propellant and mass Values");
                                }
                               
                                double TWR = thrust / (mass + rmassdry) / e.g;
                                double dV = isp * 9.81 * Math.Log((mass + rmass) / (mass + rmassdry));
                                print("DeltaV available: " + dV + "(Mass ratio: " + (mass + rmassdry) + " / " + (mass + rmass) + ", TWR " + TWR + ")");
                                if (dV >= Tools.Setting("deltaVRequired", 1000.0) && TWR >= Tools.Setting("minRocketTWR", 1.5))
                                {
                                    print("ModuleEngine Rocket Assisted Landing Succesfull.. Kerbal X tycoon");
                                    landing = 2;
                                }
                                else print("Delta V id not pass to allow Propulsive Rocket Landing");
                            }
                            if (landing > 0) // landed!
                            {
                                Debug.LogError("Starting Landing Algorithm This is last step in AutoRecyle");
                                // remove fuel used (for now, remove all fuel of used types, don't try to remove only some.
                                foreach (ProtoPartSnapshot p in v.protoVessel.protoPartSnapshots)
                                    foreach (ProtoPartResourceSnapshot r in p.resources)
                                        if (props.Contains(r.resourceName))
                                        {
                                            print("Setting " + r.resourceName + " to 0 (used in landing).");
                                            r.resourceValues.SetValue("amount", "0");
                                            if (r.resourceRef != null)
                                                r.resourceRef.amount = 0;
                                        }
                                recycledName = v.name;
                                VesselResources vr = new VesselResources(v);
                                recycledCost = vr.recyclable(3); // can we recycle fuel too?
                                string landingtype = landing == 2 ? "Rocket-powered landing." : "Jet-powered flight and landing.";
                                recycledDesc = landingtype;
                                print("*MC* Recycling vessel: " + landingtype + " Val: " + recycledCost);

                                showRecycleWindow = true;
                                manager.recycleVessel(v, recycledCost);
                                FXEngineInstalled = false;


                            }

                        }

                    }
                    catch { }
                }
            }

            // We should remove the onflybywire listener, if there is one
            try
            {
                v.OnFlyByWire -= this.onFlyByWire;
            }
            catch { }
        }

        /// <summary>
        /// If we change the vessel, we should reload the mission. 
        /// </summary>
        /// <param name="v">V.</param>
        private void onVesselChange(Vessel v) {
            // If we have a current mission selected, we need to reload it again!
            // If the new vessel is on EVA, we don't reload the mission. No need to.
            if (currentMission != null && !v.isEVA) {
                currentMission = manager.reloadMission (currentMission, activeVessel);
            }

            recycled = false;

            try
            {
                v.OnFlyByWire -= this.onFlyByWire;
            }
            catch { }
            try
            {
                v.OnFlyByWire += this.onFlyByWire;
            }
            catch { }
        }

        /// <summary>
        /// We have to account the space program for the launch
        /// </summary>
        /// <param name="r">The red component.</param>
        private void onLaunch (EventReport r) 
        {
            // Apparently this event is even fired when we stage in orbit...
            // Malkuth Edit To match the actual cost of launch with Visual Cost in Display.. (almost missed this one opps)
            if (activeVessel != null && activeVessel.situation == Vessel.Situations.PRELAUNCH)
            {               

                VesselResources res = new VesselResources(activeVessel);
                FinanceMode fn = new FinanceMode();

                if (SettingsManager.Manager.getSettings().disablePlugin)
                {
                    Debug.LogError("Vessel Being Flagged Under Disabled Plugin");
                    manager.addFlagedVessel(activeVessel);
                }


                if (settings.gameMode == 0 && !SettingsManager.Manager.getSettings().disablePlugin)
                {
                    Debug.LogError("Launching Normal Mode vessel!");
                    manager.costs(res.sum());
                    recycled = false;
                    canRecycle = true;
                    manager.recordVesselInfo(currentMission, activeVessel);
                    fn.checkloans();
                    if (currentMission != null && currentMission.repeatable != true)
                    {
                        foreach (MissionGoal mg in currentMission.goals)
                        {
                            manager.clearMissionGoalByName(mg);
                            Debug.LogWarning("Checking For NoRepeatable Mission Goals Saved.. And deleting");
                        }
                    }
                }

                if (settings.gameMode == 1 && !SettingsManager.Manager.getSettings().disablePlugin)
                {
                    Debug.LogError("Launching Hardcore Mode vessel!");
                    manager.costs(res.sum());
                    recycled = false;
                    canRecycle = true;
                    manager.recordVesselInfo(currentMission, activeVessel);
                    fn.checkloans();
                    if (currentMission != null && currentMission.repeatable != true)
                    {
                        foreach (MissionGoal mg in currentMission.goals)
                        {
                            manager.clearMissionGoalByName(mg);
                            Debug.LogWarning("Checking For NoRepeatable Mission Goals Saved.. And deleting");
                        }
                    }
                }
            }
                
            
        }

        /// <summary>
        /// Sometimes the probe core gets destroyed immediately when crashing. So we need also to check the collision
        /// event. If the collision was under 10 meters above surface, we probably crashed. 
        /// </summary>
        /// <param name="report">Report.</param>
        private void onCollision(EventReport report) {
            if (activeVessel.mainBody.GetAltitude (activeVessel.CoM) - activeVessel.terrainAltitude < 10) {
                eventFlags = eventFlags.Add (EventFlags.CRASHED);
            }
        }

        /// <summary>
        /// If a crash has been detected, we set the crash flag
        /// </summary>
        /// <param name="report">Report.</param>
        private void onCrash(EventReport report) {
            eventFlags = eventFlags.Add (EventFlags.CRASHED);
        }

        /// <summary>
        /// If MC Repair Part Is Repaired By Player This Sets Repair Flag
        /// </summary>
        /// <param name="report"></param>
        
        /// <summary>
        /// We will punish the player for killing kerbonauts...
        /// </summary>
        /// <param name="report">Report.</param>
        private void onCrewKilled(EventReport report) {
            // TODO: PUNISHING THE PLAYER FOR KILLING KERBONAUTS
            // Srsly: Manned missions should be more dangerous
            print ("You will pay for your crimes! Killing innocent kerbonauts...");
        }

        /// <summary>
        /// Currently we save the space program every time a new game scene is loaded.
        /// Maybe just save when the requested game scene is the main menu
        /// </summary>
        /// <param name="scene">Scene.</param>
        private void onGameSceneLoadRequested(GameScenes scene) {
            manager.saveProgram ();
            manager.loadProgram (HighLogic.CurrentGame.Title);
            eventFlags = EventFlags.NONE;
            pVessel = null;
            canRecycle = false; // fix for recycle on load scene/revert
        }

        private void onVesselCreate(Vessel v)
        {
            if (HighLogic.LoadedScene.Equals(GameScenes.SPACECENTER))
            {
                manager.saveProgramBackup();
            }            
        }

        /// <summary>
        /// If the mission is on a client controlled mission, we disable every incoming input
        /// </summary>
        /// <param name="s">S.</param>
        private void onFlyByWire(FlightCtrlState s) {
            Status status = calculateStatus(currentMission, false, activeVessel);

            if(status.isClientControlled) {
                s.fastThrottle = 0;
                s.gearDown = false;
                s.gearUp = false;
                s.headlight = false;
                s.killRot = false;
                s.mainThrottle = 0;
                s.pitch = 0;
                s.pitchTrim = 0;
                s.roll = 0;
                s.rollTrim = 0;
                s.X = 0;
                s.Y = 0;
                s.yaw = 0;
                s.yawTrim = 0;
                s.Z = 0;
                s.wheelSteer = 0;
                s.wheelSteerTrim = 0;
                s.wheelThrottle = 0;
                s.wheelThrottleTrim = 0;
                s.NeutralizeAll ();
            }
        }
    }
}

