using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// This area is for planned future Testing And Experiment stuff... Kinda of a stop point for extra test code too not sure if I will keep
    /// </summary>
    public class TestingAndExperiment
    {
        /// <summary>
        /// Terminate Flight Attempt To Replace The Old Method From game.. Will Save Flight and allow me to Call window without Resetting.. Won't Terminate the vessel though.. Still have to Recover
        /// </summary>
        public static void TerminateCurrentFlight()
        {
            foreach (ProtoCrewMember crewMember in FlightGlobals.ActiveVessel.GetVesselCrew())
            {
                crewMember.rosterStatus = ProtoCrewMember.RosterStatus.AVAILABLE;
            }
            FlightState state = new FlightState();
            if (state.activeVesselIdx != -1)
            {
                state.protoVessels.RemoveAt(state.activeVesselIdx);
            }
            GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
            
        }
    }
}
