using System;
using System.Collections.Generic;

namespace MissionController
{
    public class LandingGoal : MissionGoal
    {
        public bool splashedValid = true;
        public String body = "Kerbin";

        public double minLatitude = 0.0;
        public double maxLatitude = 0.0;

        public double minLongitude = 0.0;
        public double maxLongitude = 0.0;

        protected override List<Value> values(Vessel vessel) {
            Orbit o = vessel.orbit;
            List<Value> values = new List<Value> ();

            values.Add(new Value("Landing Body", body, o.referenceBody.bodyName, 
                                                 o.referenceBody.bodyName.Equals(body) && (vessel.situation == Vessel.Situations.LANDED ||
                                                 (splashedValid ? vessel.situation == Vessel.Situations.SPLASHED : false))));

            if(minLatitude != maxLatitude) {
                values.Add(new Value("Latitude", String.Format(MathTools.MinMaxValue, minLatitude, maxLatitude), 
                                     vessel.latitude, MathTools.inMinMax(minLatitude, maxLatitude, vessel.latitude)));
            }

            if(minLongitude != maxLongitude) {
                values.Add(new Value("Longitude", String.Format(MathTools.MinMaxValue, minLongitude, maxLongitude), 
                                     vessel.longitude, MathTools.inMinMax(minLongitude, maxLongitude, vessel.longitude)));
            }

            return values;
        }

        public override string getType ()
        {
            return "Landing";
        }
    }
}

