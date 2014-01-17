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

		public String targetName = "";
		public double targetLatitude = 0.0;
		public double targetLongitude = 0.0;
		public double targetMinDistance = -1.0;
		public double targetMaxDistance = -1.0;

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

			if(targetMinDistance >= 0 || targetMaxDistance >= targetMinDistance) {
				String name = null;
				String format = null;
                String minDistDisplay = MathTools.formatDistance (targetMinDistance);
                String maxDistDisplay = MathTools.formatDistance (targetMaxDistance);

				if (targetName != "")
					name = targetName;
				else
					name = String.Format ("{0:N3},{1:N3}", targetLongitude, targetLatitude);
	
				if (targetMinDistance >= 0 && targetMaxDistance < targetMinDistance) {
                    format = "at least {1} from ({0})";
				} else if (targetMinDistance < 0 && targetMaxDistance > 0) {
                    format = "less than {2} from ({0})";
				} else {
                    format = "{1} - {2} from ({0})";
				}

				if (vessel == null) {
                    values.Add (new Value ("Target", String.Format(format, name, minDistDisplay, maxDistDisplay)));
				} else {
					double longDiff = targetLongitude - vessel.longitude;
					double latDiff = targetLatitude - vessel.latitude;

					double a = Math.Pow(Math.Sin (latDiff / 2 * Math.PI / 180), 2) + Math.Cos(targetLatitude * Math.PI / 180) * Math.Cos(vessel.latitude * Math.PI / 180) * Math.Pow(Math.Sin(longDiff / 2 * Math.PI / 180), 2);
					double c = 2 * Math.Atan2 (Math.Sqrt (a), Math.Sqrt (1 - a));
					double distance = vessel.mainBody.Radius * c;

                    values.Add (new Value ("Target", String.Format (format, name, minDistDisplay, maxDistDisplay),
                        MathTools.formatDistance(distance),
						MathTools.inMinMax (targetMinDistance, targetMaxDistance < targetMinDistance ? Double.MaxValue : targetMaxDistance, distance)));
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

