using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MissionController
{
    public class RoverScience: PartModule
    {      
 
        [KSPField(isPersistant = false)]
        public static bool doResearch = false;

        [KSPField(isPersistant = true)]
        private bool IsEnabled = false;

        Vessel vs = new Vessel();      

        [KSPField(isPersistant = false, guiActive = true, guiName = "Rover Landed:")]
        private bool roverlanded = false;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Starting Scan:")]
        private bool scanStart = false;

        [KSPEvent(guiActive= true,guiName= "Start MCE Rover Research",active= true)]
        public void StartResearchMCE()
        {
            checkVesselResearch();
        }
    
        public void checkVesselResearch()
        {
            if (roverlanded == true)
            {
                doResearch = true;              
                ScreenMessages.PostScreenMessage("Starting Scan of Ground Level, Please Stand By...");               
            }
            else { 
                doResearch = false;
                scanStart = false;
                ScreenMessages.PostScreenMessage("Vessel needs to be landed to start scanning at ground level");              
            }
        }
        
        public override void OnFixedUpdate()
        {
            if (FlightGlobals.fetch.activeVessel.situation == Vessel.Situations.LANDED || FlightGlobals.fetch.activeVessel.situation == Vessel.Situations.SPLASHED)
            {
                roverlanded = true;
            }
            else { roverlanded = false; }
            if (doResearch == true)
            {
                scanStart = true;
            }
            else { scanStart = false; }
        }
    }

    public class RoverResearch : MissionGoal
    {
        public bool splashedValid = true;
        public string body = "";
        public double roverSeconds = 0.0;
        private Manager manager
        {
            get { return Manager.instance; }
        }

        protected override List<Value> values(Vessel vessel, GameEvent ev)
        {          

            List<Value> values2 = new List<Value>();
            if (manager.GetRoverTime == -1.0 && roverSeconds > 0.0 && manager.GetTimeRoverName == "none" && RoverScience.doResearch == true)
            {
                manager.SetRoverTime(Planetarium.GetUniversalTime());
                manager.SetTimeRoverName(id);
            }
            if (FlightGlobals.fetch.activeVessel != null && manager.GetTimeRoverName != id && roverSeconds > 0)
            {
                RoverScience.doResearch = false;
                manager.SetRoverTime(-1.0);
                manager.SetTimeRoverName("none");
            }

            if (vessel == null)
            {
                values2.Add(new Value("Rover Research", "True"));                            
                values2.Add(new Value("Research Time", MathTools.formatTime(roverSeconds)));
                if (contractAvailable == 15)
                {
                    values2.Add(new Value("Landing Body", manager.GetRandomLanding));
                }
                else { values2.Add(new Value("Landing Body", body)); }
            }
            else
            {               
                values2.Add(new Value("Rover Research", "True", "" + RoverScience.doResearch,RoverScience.doResearch));
                if (contractAvailable == 15)
                {
                    values2.Add(new Value("Landing Body", manager.GetRandomLanding, vessel.orbit.referenceBody.bodyName,
                                                     vessel.orbit.referenceBody.bodyName.Equals(body) && (vessel.situation == Vessel.Situations.LANDED ||
                        (splashedValid ? vessel.situation == Vessel.Situations.SPLASHED : false))));
                }
                else
                {
                    values2.Add(new Value("Landing Body", body, vessel.orbit.referenceBody.bodyName,
                                                   vessel.orbit.referenceBody.bodyName.Equals(body) && (vessel.situation == Vessel.Situations.LANDED ||
                      (splashedValid ? vessel.situation == Vessel.Situations.SPLASHED : false))));
                }

                if (roverSeconds > 0.0)
                {
                    double diff2 = (manager.GetRoverTime == -1.0 ? 0 : Planetarium.GetUniversalTime() - manager.GetRoverTime);
                    values2.Add(new Value("Research Time", MathTools.formatTime(roverSeconds), MathTools.formatTime(diff2), diff2 > roverSeconds));
                }
                
            }

            return values2;
        }

        public override string getType()
        {
            return "Landing Vessel Research";
        }
    }
}
