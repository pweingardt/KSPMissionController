using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace MissionController
{
    public class OrbitGoal : MissionGoal
    {
        public double eccentricity = 0.0;
        public double eccentricityPrecision = 0.1;

        public double maxEccentricity = 0.0;
        public double minEccentricity = 0.0;

        public double minPeA = 0.0;
        public double maxPeA = 0.0;

        public double minApA = 0.0;
        public double maxApA = 0.0;

        public double minInc = 0.0;
        public double maxInc = 0.0;

        public double minLan = 0.0;
        public double maxLan = 0.0;

        public String body = "Kerbin";

        protected override List<Value> values(Vessel vessel) {
            Orbit orbit = vessel.orbit;
            List<Value> values = new List<Value> ();

            if (eccentricity != 0.0) {
                values.Add (new Value("Eccentricity", String.Format(MathTools.Range, eccentricity, eccentricityPrecision), 
                                      orbit.eccentricity, MathTools.inRange (eccentricity, eccentricityPrecision, vessel.orbit.eccentricity)));
            }
            
            if(body != null) {
                values.Add (new Value("Body", "" + body, orbit.referenceBody.bodyName, orbit.referenceBody.bodyName.Equals(body)));
            }
            
            if (minPeA != 0.0) {
                values.Add (new Value("min. Periapsis", String.Format(MathTools.SingleDoubleValue, minPeA), 
                                      orbit.PeA, vessel.orbit.PeA >= minPeA));
            }
            
            if (minApA != 0.0) {
                values.Add (new Value("min. Apoapsis", String.Format(MathTools.SingleDoubleValue, minApA), 
                                      orbit.ApA, orbit.ApA >= minApA));
            }
            
            if (maxPeA != 0.0) {
                values.Add (new Value ("max. Periapsis", String.Format(MathTools.SingleDoubleValue, maxPeA), 
                                       orbit.PeA, orbit.PeA <= maxPeA));
            }
            
            if (maxApA != 0.0 ) {
                values.Add (new Value ("max. Apoapsis", String.Format(MathTools.SingleDoubleValue, maxApA), 
                                       orbit.ApA, orbit.ApA <= maxApA));
            }
            
            if (minInc != maxInc) {
                values.Add (new Value ("Inclination", String.Format(MathTools.MinMaxValue, minInc, maxInc), 
                                       String.Format(MathTools.SingleDoubleValue, orbit.inclination), 
                                                      MathTools.inMinMax (minInc, maxInc, vessel.orbit.inclination)));
            }
            
            if (minLan != maxLan) {
                values.Add (new Value ("LAN", String.Format(MathTools.MinMaxValue, minLan, maxLan), 
                                       orbit.LAN, MathTools.inMinMax (minLan, maxLan, vessel.orbit.LAN)));
            }

            if(maxEccentricity != minEccentricity) {
                // If both min < max, then max has not been set at all and ignore the max value
                if(minEccentricity < maxEccentricity) {
                    values.Add (new Value("Eccentricity", String.Format(MathTools.MinMaxValue, minEccentricity, maxEccentricity), 
                                      orbit.eccentricity, MathTools.inMinMax(minEccentricity, maxEccentricity, orbit.eccentricity)));
                } else {
                    values.Add (new Value("min. Eccentricity", String.Format(MathTools.SingleDoubleValue, minEccentricity), 
                                         orbit.eccentricity, minEccentricity < orbit.eccentricity));
                }
            }

            return values;
        }

        public override string getType ()
        {
            return "Orbit";
        }
    }
}

