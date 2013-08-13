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

                Debug.LogError ("Docked FROM ID: " + action.from.vessel.id.ToString());
                Debug.LogError ("Docked TO ID: " + action.to.vessel.id.ToString());
            }
        }

        // NK move recycle functionality to recovery

        private void onCreate(Vessel v)
        {
            // for now, just reenable recycling. It's so you don't recycle on return to spaceport or revert.
            canRecycle = true;
        }
        
        /// <summary>
        /// Recycle vessel whenever recovered
        /// </summary>
        /// <param name="pv">the vessel</param>
        private void onRecovered(ProtoVessel pv)
        {
            if (HighLogic.LoadedScene.Equals(GameScenes.TRACKSTATION) && settings.difficulty != 0 && (pv.situation.Equals(Vessel.Situations.LANDED) || pv.situation.Equals(Vessel.Situations.SPLASHED)))
            {
                VesselResources res = new VesselResources(pv.vesselRef);
                recycledName = pv.vesselName;
                recycledCost = res.recyclable(pv.situation.Equals(Vessel.Situations.LANDED));
                print("*MC* Craft " + recycledName + " recovered for " + recycledCost);
                showRecycleWindow = true;
                manager.recycleVessel(pv.vesselRef, recycledCost);
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
            }
            // NK recycle
            else
            {
                try { print("*MC* Vessel " + v.name + " destroyed. Alt " + v.mainBody.GetAltitude(v.CoM) + ", body " + v.orbit.referenceBody.bodyName + ", sit = " + v.situation); }
                catch { }
                if (!HighLogic.LoadedSceneIsEditor && canRecycle && activeVessel != v && !v.isEVA // canRecycle is false iff load requested and haven't returned to flight yet.
                    && v.name.Contains("(Unloaded)") // check make sure it's because we're unloading it
                    && (v.situation == Vessel.Situations.FLYING || v.situation == Vessel.Situations.SUB_ORBITAL) && v.mainBody.GetAltitude(v.CoM) <= 25000 && v.orbit.referenceBody.bodyName.Equals("Kerbin")
                    && settings.difficulty != 0)
                {
                    print("*MC* Checking " + v.name);
                    double mass = 0;
                    double pdrag = 0.0;
                    int cost = 0;
                    double AUTORECYCLE_COST_MULT = 0.6;
                    // need 70 drag per ton of vessel for 6m/s at 500m.
                    const double PARACHUTE_DRAG_PER_TON = 70.0;
                    try
                    {
                        foreach (ProtoPartSnapshot p in v.protoVessel.protoPartSnapshots)
                        {
                            print("Has part " + p.partName + ", mass " + p.mass + ", cost " + p.partRef.partInfo.cost);
                            mass += p.mass;
                            cost += p.partRef.partInfo.cost;
                            foreach (ProtoPartModuleSnapshot m in p.modules)
                            {
                                if (m.moduleName.Equals("ModuleParachute"))
                                {
                                    ModuleParachute mp = (ModuleParachute)m.moduleRef;
                                    pdrag += p.mass * mp.fullyDeployedDrag;
                                    print("Drag now " + pdrag);
                                }
                            }
                        }
                        if (mass * PARACHUTE_DRAG_PER_TON <= pdrag)
                        {
                            recycledName = v.name;
                            recycledCost = (int)((double)cost * AUTORECYCLE_COST_MULT);
                            print("*MC* Recycling vessel: enough parachutes! Val: " + cost + " * " + AUTORECYCLE_COST_MULT + " = " + recycledCost);
                            showRecycleWindow = true;
                            manager.recycleVessel(v, recycledCost);
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
        private void onLaunch (EventReport r) {
            // Apparently this event is even fired when we stage in orbit...
            // Malkuth Edit To match the actual cost of launch with Visual Cost in Display.. (almost missed this one opps)
            if (activeVessel != null && activeVessel.situation == Vessel.Situations.PRELAUNCH)
            {
                VesselResources res = new VesselResources(activeVessel);
                if (settings.difficulty != 0)
                {
                    Debug.LogError("Launching vessel!");
                    manager.costs(res.sum());
                    recycled = false; 
                }
                else
                {
                    Debug.LogError("Launching Test vessel!");
                    manager.costs((res.dry()) * 6 / 100);
                    recycled = false;
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

