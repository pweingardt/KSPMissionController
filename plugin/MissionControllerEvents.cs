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

        /// <summary>
        /// We check if the vessel was crashing. If so, we set the crashed flag
        /// </summary>
        /// <param name="v">V.</param>
        private void onVesselDestroy(Vessel v) {
            // If it has been destroyed under 10 meters above surface, it probably crashed
            if (v.mainBody.GetAltitude (v.CoM) - v.terrainAltitude < 10) {
                eventFlags = eventFlags.Add (EventFlags.CRASHED);
            }

            // We should remove the onflybywire listener, if there is one
            try
            {
                v.OnFlyByWire -= this.onFlyByWire;
            }
            catch { }

            // NK recycle
            if (!HighLogic.LoadedSceneIsEditor && activeVessel != v && (v.situation == Vessel.Situations.FLYING || v.situation == Vessel.Situations.SUB_ORBITAL) && v.mainBody.GetAltitude(v.CoM) < 26000 && v.orbit.referenceBody.bodyName.Equals("Kerbin"))
            {
                print("*MC* Vessel " + v.name + " destroyed. Alt " + v.mainBody.GetAltitude(v.CoM) + ", body " + v.orbit.referenceBody.bodyName + ", sit = " + v.situation);
                double mass = 0;
                double pdrag = 0.0;
                int cost = 0;
                double AUTORECYCLE_COST_MULT = 0.6;
                // need 70 drag per ton of vessel for 6m/s at 500m.
                const double PARACHUTE_DRAG_PER_TON = 70.0;
                try
                {
                    // if loaded
                    /*foreach (Part p in v.Parts)
                    {
                        print("Has part " + p.name);
                        foreach (ModuleParachute m in p.Modules.OfType<ModuleParachute>())
                        {
                            pdrag += p.mass * m.fullyDeployedDrag;
                            print("Drag now " + pdrag);
                        }
                    }
                    mass = v.GetTotalMass();
                    if (mass * PARACHUTE_DRAG_PER_TON <= pdrag)
                    {
                        print("*MC* Recycling vessel: enough parachutes! Val: " + vesselResources.dry());
                        //manager.recycleVessel(v, res.dry());
                    }*/
                    foreach (ProtoPartSnapshot p in v.protoVessel.protoPartSnapshots)
                    {
                        print("Has part " + p.partName + ", mass " + p.mass + ", cost " + p.partRef.partInfo.cost);
                        mass += p.mass;
                        cost += p.partRef.partInfo.cost;
                        foreach(ProtoPartModuleSnapshot m in p.modules)
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
                        print("*MC* Recycling vessel: enough parachutes! Val: " + cost);
                        manager.recycleVessel(v, (int)((double)cost * AUTORECYCLE_COST_MULT));
                    }
                }
                catch { }
            }
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
                if (settings.difficulty == 0)
                {
                    Debug.LogError("Launching Test vessel!");
                    manager.costs((vesselResources.dry() + vesselResources.crew()) * 3 / 100);
                    recycled = false;
                }
                else
                {
                    Debug.LogError("Launching vessel!");
                    manager.costs(vesselResources.sum());
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

