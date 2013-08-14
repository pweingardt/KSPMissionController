using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A landing goal an a certain celestial body. Latitude and longitude range are optional
    /// </summary>
    public class LandingGoal : MissionGoal
    {
        public bool splashedValid = true;
        public String body = "Kerbin";

        public double minLatitude = 0.0;
        public double maxLatitude = 0.0;

        public double minLongitude = 0.0;
        public double maxLongitude = 0.0;

        protected override List<Value> values(Vessel vessel, GameEvent events) {

            List<Value> values = new List<Value> ();

            if (vessel == null) {
                values.Add (new Value ("Landing Body", body));
            } else {
                values.Add (new Value ("Landing Body", body, vessel.orbit.referenceBody.bodyName, 
                                                 vessel.orbit.referenceBody.bodyName.Equals (body) && (vessel.situation == Vessel.Situations.LANDED ||
                    (splashedValid ? vessel.situation == Vessel.Situations.SPLASHED : false))));
            }

            if(minLatitude != maxLatitude) {
                if(vessel == null) {
                    values.Add(new Value("Latitude", String.Format(MathTools.MinMaxValue, minLatitude, maxLatitude)));
                } else {
                    values.Add(new Value("Latitude", String.Format(MathTools.MinMaxValue, minLatitude, maxLatitude), 
                                     String.Format(MathTools.SingleDoubleValue, vessel.latitude), MathTools.inMinMax(minLatitude, maxLatitude, vessel.latitude)));
                }
            }

            if(minLongitude != maxLongitude) {
                if(vessel == null) {
                    values.Add(new Value("Longitude", String.Format(MathTools.MinMaxValue, minLongitude, maxLongitude)));
                } else {
                    values.Add(new Value("Longitude", String.Format(MathTools.MinMaxValue, minLongitude, maxLongitude), 
                                     String.Format(MathTools.SingleDoubleValue, MathTools.calculateLongitude(vessel.longitude)), 
                                     MathTools.inMinMax(minLongitude, maxLongitude, MathTools.calculateLongitude(vessel.longitude))));
                }
            }

            return values;
        }

        public override string getType ()
        {
            return "Landing";
        }
    }
}

