using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// A goal to launch from either the runway or the launch pad on Kerbin (or a celestial body).
    /// </summary>
    public class LaunchGoal : MissionGoal
    {
        private const double runwayMinLatitude = -0.043;
        private const double runwayMaxLatitude = -0.038;
        private const double runwayMinLongitude = -74.719;
        private const double runwayMaxLongitude = -74.700;

        private const double padMinLatitude = -0.107;
        private const double padMaxLatitude = -0.098;
        private const double padMinLongitude = -74.580;
        private const double padMaxLongitude = -74.571;

        private const string LaunchPad = "launch pad";
        private const string Runway = "runway";

        public string launchZone = LaunchPad;

        public LaunchGoal()
        {
            this.throttleDown = false;
        }

        public override bool isDone(Vessel vessel, GameEvent events)
        {
            if (vessel == null)
            {
                return false;
            }

            if (doneOnce)
            {
                return true;
            }

            List<Value> values = getValues(vessel, events);
            foreach (Value v in values)
            {
                if (!v.done)
                {
                    return false;
                }
            }

            // We remember that we have previously completed this goal.
            doneOnce = true;

            return true;
        }

        protected override List<Value> values(Vessel vessel, GameEvent events)
        {
            List<Value> values = new List<Value>();

            if (vessel == null)
            {
                values.Add(new Value("Launch from", launchZone));
            }
            else
            {
                string currentZone = vessel.orbit.referenceBody.bodyName;

                // Only check the lat/long of the vessel location to update the currentZone when the body is Kerbin
                // and the goal specifies the launch pad or runway on Kerbin.
                if ((launchZone.Equals(LaunchPad) || launchZone.Equals(Runway)) && currentZone.Equals("Kerbin"))
                {
                    double currentLongitude = MathTools.calculateLongitude(vessel.longitude);
                    double currentLatitude = vessel.latitude;

                    if (MathTools.inMinMax(runwayMinLongitude, runwayMaxLongitude, currentLongitude) &&
                        MathTools.inMinMax(runwayMinLatitude, runwayMaxLatitude, currentLatitude))
                    {
                        // Within runway parameters.
                        currentZone = Runway;
                    }
                    else if (MathTools.inMinMax(padMinLongitude, padMaxLongitude, currentLongitude) &&
                             MathTools.inMinMax(padMinLatitude, padMaxLatitude, currentLatitude))
                    {
                        // Within launch pad parameters.
                        currentZone = LaunchPad;
                    }
                }
                values.Add(new Value("Launch from", launchZone, currentZone, String.Equals(currentZone, launchZone)));

                // Check the speed to make sure that we are not moving. In most cases "<0.01" will donate a stationary object.
                // However, in some cases, that causes flickering of the text as it goes in and out of spec.
                double speed = MathTools.abs(vessel.horizontalSrfSpeed) + MathTools.abs(vessel.verticalSpeed);
                if (speed > 0.90)
                {
                    values.Add(new Value("Launch", "", "requires stationary position Test Fix, If your still getting message please report.", false));
                }
            }
            return values;
        }

        public override string getType ()
        {
            return "Launch";
        }
    }
}
