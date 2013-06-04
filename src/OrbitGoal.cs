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
            List<Value> values = new List<Value> ();

            if (eccentricity != 0.0) {
                if(vessel == null) {
                    values.Add (new Value("Eccentricity", String.Format(MathTools.Range, eccentricity, eccentricityPrecision)));
                } else {
                    values.Add (new Value("Eccentricity", String.Format(MathTools.Range, eccentricity, eccentricityPrecision), 
                                          vessel.orbit.eccentricity, MathTools.inRange (eccentricity, eccentricityPrecision, vessel.orbit.eccentricity)));
                }
            }
            
            if (body != null) {
                if (vessel == null) {
                    values.Add (new Value ("Body", "" + body));
                } else {
                    values.Add (new Value ("Body", "" + body, vessel.orbit.referenceBody.bodyName, vessel.orbit.referenceBody.bodyName.Equals (body)));
                }
            }
            
            if (minPeA != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("min. Periapsis", String.Format(MathTools.SingleDoubleValue, minPeA)));
                } else {
                    values.Add (new Value("min. Periapsis", String.Format(MathTools.SingleDoubleValue, minPeA), 
                                          vessel.orbit.PeA, vessel.orbit.PeA >= minPeA));
                }
            }
            
            if (minApA != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("min. Apoapsis", String.Format(MathTools.SingleDoubleValue, minApA)));
                } else {
                    values.Add (new Value("min. Apoapsis", String.Format(MathTools.SingleDoubleValue, minApA), 
                                          vessel.orbit.ApA, vessel.orbit.ApA >= minApA));
                }
            }
            
            if (maxPeA != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("max. Periapsis", String.Format(MathTools.SingleDoubleValue, maxPeA)));
                } else {
                    values.Add (new Value ("max. Periapsis", String.Format(MathTools.SingleDoubleValue, maxPeA), 
                                           vessel.orbit.PeA, vessel.orbit.PeA <= maxPeA));
                }
            }
            
            if (maxApA != 0.0 ) {
                if (vessel == null) {
                    values.Add (new Value ("max. Apoapsis", String.Format(MathTools.SingleDoubleValue, maxApA)));
                } else {
                    values.Add (new Value ("max. Apoapsis", String.Format(MathTools.SingleDoubleValue, maxApA), 
                                           vessel.orbit.ApA, vessel.orbit.ApA <= maxApA));
                }
            }
            
            if (minInc != maxInc) {
                if (vessel == null) {
                    values.Add (new Value ("Inclination", String.Format(MathTools.MinMaxValue, minInc, maxInc)));
                } else {
                    values.Add (new Value ("Inclination", String.Format(MathTools.MinMaxValue, minInc, maxInc), 
                                           String.Format(MathTools.SingleDoubleValue, vessel.orbit.inclination), 
                                                      MathTools.inMinMax (minInc, maxInc, vessel.orbit.inclination)));
                }
            }
            
            if (minLan != maxLan) {
                if (vessel == null) {
                    values.Add (new Value ("LAN", String.Format(MathTools.MinMaxValue, minLan, maxLan)));
                } else {
                    values.Add (new Value ("LAN", String.Format(MathTools.MinMaxValue, minLan, maxLan), 
                                           vessel.orbit.LAN, MathTools.inMinMax (minLan, maxLan, vessel.orbit.LAN)));
                }
            }

            if(maxEccentricity != minEccentricity) {
                // If both min < max, then max has not been set at all and ignore the max value
                if(minEccentricity < maxEccentricity) {
                    if (vessel == null) {
                        values.Add (new Value ("Eccentricity", String.Format(MathTools.MinMaxValue, minEccentricity, maxEccentricity)));
                    } else {
                        values.Add (new Value("Eccentricity", String.Format(MathTools.MinMaxValue, minEccentricity, maxEccentricity), 
                                              vessel.orbit.eccentricity, MathTools.inMinMax(minEccentricity, maxEccentricity, vessel.orbit.eccentricity)));
                    }
                } else {
                    if (vessel == null) {
                        values.Add (new Value ("min. Eccentricity", String.Format(MathTools.SingleDoubleValue, minEccentricity)));
                    } else {
                        values.Add (new Value("min. Eccentricity", String.Format(MathTools.SingleDoubleValue, minEccentricity), 
                                              vessel.orbit.eccentricity, minEccentricity < vessel.orbit.eccentricity));
                    }
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

