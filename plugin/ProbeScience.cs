using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionController
{
    public class RoverScience: PartModule
    {
        private MissionController missioncontroller
        {
            get { return MissionController.instance; }
        }
        private Manager manager
        {
            get { return Manager.instance; }
        }
        [KSPField(isPersistant = false)]
        private bool missionIsResearch = false;
        
        [KSPField(isPersistant = false)]
        public static bool doResearch = false;

        Vessel vs = new Vessel();
        
        [KSPField(isPersistant = false, guiActive = true, guiName = "Rover Landed")]
        private bool StartingResearch = false;

        [KSPEvent(guiActive= true,guiName= "Start MCE Rover Research",active= true)]
        public void StartResearchMCE()
        {
            checkVesselResearch();
        }
        public void checkVesselResearch()
        {
            if (StartingResearch != false)
            {
                doResearch = true;
            }
            else { doResearch = false; }
        }
        public override void OnFixedUpdate()
        {
            if (FlightGlobals.fetch.activeVessel.situation == Vessel.Situations.LANDED && missionIsResearch != false || FlightGlobals.fetch.activeVessel.situation == Vessel.Situations.SPLASHED && missionIsResearch != false)
            {
                StartingResearch = true;
            }
            else { StartingResearch = false; }
            if (missioncontroller.getCurrentMission.isRoverMission != false)
            {
                missionIsResearch = true;
            }
        }      
    }

    public class RoverResearch : MissionGoal
    {
        public bool splashedValid = true;
        public string body = "";
        public double roverseconds = 0.0;
        private Manager manager
        {
            get { return Manager.instance; }
        }
        public double RoverTimeDiff;

        protected override List<Value> values(Vessel vessel, GameEvent ev)
        {
            RoverTimeDiff = Planetarium.GetUniversalTime() - manager.GetRoverTime;

            List<Value> values2 = new List<Value>();
            if (manager.GetRoverTime == -1.0 && roverseconds > 0.0 && manager.GetTimeRoverName == "none" && RoverScience.doResearch == true)
            {
                manager.SetRoverTime(Planetarium.GetUniversalTime());
                manager.SetTimeRoverName(id);
            }
            if (FlightGlobals.fetch.activeVessel != null && manager.GetTimeRoverName != id && roverseconds > 0)
            {
                RoverScience.doResearch = false;
                manager.SetRoverTime(-1.0);
                manager.SetTimeRoverName("none");
            }

            //if (RoverTimeDiff > roverseconds)
            //{
            //    manager.SetRoverTime(-1.0);
            //    manager.SetTimeRoverName("none");
            //}

            if (vessel == null)
            {
                values2.Add(new Value("Rover Research", "True"));
                values2.Add(new Value("Rover Landing Body", body));              
                values2.Add(new Value("Research Time", MathTools.formatTime(roverseconds)));

            }
            else
            {               
                values2.Add(new Value("Rover Research", "True", "" + RoverScience.doResearch,RoverScience.doResearch));
                values2.Add(new Value("Rover Landing Body", body, vessel.orbit.referenceBody.bodyName,
                                                 vessel.orbit.referenceBody.bodyName.Equals(body) && (vessel.situation == Vessel.Situations.LANDED ||
                    (splashedValid ? vessel.situation == Vessel.Situations.SPLASHED : false))));
                if (roverseconds > 0.0)
                {
                    double diff2 = (manager.GetRoverTime == -1.0 ? 0 : Planetarium.GetUniversalTime() - manager.GetRoverTime);
                    values2.Add(new Value("Research Time", MathTools.formatTime(roverseconds), MathTools.formatTime(diff2), diff2 > roverseconds));    
                }
            }

            return values2;
        }

        public override string getType()
        {
            return "Rover Research";
        }
    }
}
