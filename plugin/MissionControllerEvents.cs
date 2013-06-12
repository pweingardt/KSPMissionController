using System;
using UnityEngine;

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
            manager.costs (vesselResources.sum());
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


        private void onFlyByWire(FlightCtrlState s) {
            Status status = calculateStatus(currentMission, false);

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
            }
        }
    }
}

