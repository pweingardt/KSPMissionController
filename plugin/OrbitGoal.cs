using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace MissionController
{
    /// <summary>
    /// A mission goal to reach a certain orbit
    /// </summary>
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

        public double minInclination = 0.0;
        public double maxInclination = 0.0;

        public double minLan = 0.0;
        public double maxLan = 0.0;

        public double minOrbitalPeriod = 0.0;
        public double maxOrbitalPeriod = 0.0;

        public double minAltitude = 0.0;
        public double maxAltitude = 0.0;

        public double minSpeedOverGround = 0.0;
        public double maxSpeedOverGround = 0.0;

        public double minVerticalSpeed = 0.0;
        public double maxVerticalSpeed = 0.0;

        public double minGForce = 0.0;
        public double maxGForce = 0.0;

        public String body = "Kerbin";

        protected override List<Value> values(Vessel vessel) {
            List<Value> values = new List<Value> ();

            if (eccentricity != 0.0) {
                if(vessel == null) {
                    values.Add (new Value ("Eccentricity", String.Format(MathTools.Range, eccentricity, eccentricityPrecision)));
                } else {
                    values.Add (new Value ("Eccentricity", String.Format(MathTools.Range, eccentricity, eccentricityPrecision), 
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
                    values.Add (new Value ("min. Periapsis", String.Format(MathTools.SingleDoubleValue, minPeA), 
                                          vessel.orbit.PeA, vessel.orbit.PeA >= minPeA));
                }
            }
            
            if (minApA != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("min. Apoapsis", String.Format(MathTools.SingleDoubleValue, minApA)));
                } else {
                    values.Add (new Value ("min. Apoapsis", String.Format(MathTools.SingleDoubleValue, minApA), 
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
            
            if (minInclination != maxInclination) {
                if (vessel == null) {
                    values.Add (new Value ("Inclination", String.Format(MathTools.MinMaxValue, minInclination, maxInclination)));
                } else {
                    values.Add (new Value ("Inclination", String.Format(MathTools.MinMaxValue, minInclination, maxInclination), 
                                           String.Format(MathTools.SingleDoubleValue, vessel.orbit.inclination), 
                                                      MathTools.inMinMax (minInclination, maxInclination, vessel.orbit.inclination)));
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

            if (minAltitude != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("min Altitude", String.Format(MathTools.SingleDoubleValue, minAltitude)));
                } else {
                    values.Add (new Value ("min Altitude", String.Format(MathTools.SingleDoubleValue, minAltitude), 
                                           vessel.orbit.altitude, vessel.orbit.altitude >= minAltitude));
                }
            }

            if (maxAltitude != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("max Altitude", String.Format(MathTools.SingleDoubleValue, maxAltitude)));
                } else {
                    values.Add (new Value ("max Altitude", String.Format(MathTools.SingleDoubleValue, maxAltitude), 
                                           vessel.orbit.altitude, vessel.orbit.altitude <= maxAltitude));
                }
            }

            if (minSpeedOverGround != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("min speed over ground", String.Format(MathTools.SingleDoubleValue, minSpeedOverGround)));
                } else {
                    values.Add (new Value ("min speed over ground", String.Format(MathTools.SingleDoubleValue, minSpeedOverGround), 
                                           vessel.horizontalSrfSpeed, vessel.horizontalSrfSpeed >= minSpeedOverGround));
                }
            }

            if (maxSpeedOverGround != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("max speed over ground", String.Format(MathTools.SingleDoubleValue, maxSpeedOverGround)));
                } else {
                    values.Add (new Value ("max speed over ground", String.Format(MathTools.SingleDoubleValue, maxSpeedOverGround), 
                                           vessel.horizontalSrfSpeed, vessel.horizontalSrfSpeed <= maxSpeedOverGround));
                }
            }

            if(maxEccentricity != minEccentricity) {
                // If both min < max, then max has not been set at all and ignore the max value
                if(minEccentricity < maxEccentricity) {
                    if (vessel == null) {
                        values.Add (new Value ("Eccentricity", String.Format(MathTools.MinMaxValue, minEccentricity, maxEccentricity)));
                    } else {
                        values.Add (new Value ("Eccentricity", String.Format(MathTools.MinMaxValue, minEccentricity, maxEccentricity), 
                                              vessel.orbit.eccentricity, MathTools.inMinMax(minEccentricity, maxEccentricity, vessel.orbit.eccentricity)));
                    }
                } else {
                    if (vessel == null) {
                        values.Add (new Value ("min. Eccentricity", String.Format(MathTools.SingleDoubleValue, minEccentricity)));
                    } else {
                        values.Add (new Value ("min. Eccentricity", String.Format(MathTools.SingleDoubleValue, minEccentricity), 
                                              vessel.orbit.eccentricity, minEccentricity < vessel.orbit.eccentricity));
                    }
                }
            }

            if (minOrbitalPeriod < maxOrbitalPeriod) {
                if (vessel == null || Planetarium.fetch == null) {
                    values.Add (new Value("Orbital Period", String.Format(MathTools.MinMaxString, 
                                                                         MathTools.formatTime(minOrbitalPeriod), MathTools.formatTime(maxOrbitalPeriod))));
                } else {
                    values.Add (new Value("Orbital Period", String.Format(MathTools.MinMaxString, 
                                                                          MathTools.formatTime(minOrbitalPeriod), MathTools.formatTime(maxOrbitalPeriod)),
                                          MathTools.formatTime(vessel.orbit.period), MathTools.inMinMax(minOrbitalPeriod, maxOrbitalPeriod, vessel.orbit.period)));
                }
            }

            if (minVerticalSpeed != 0.0) { //both min and max Vertical Speed variables are tested in game.  Good Test.
                if (vessel == null) {
                    values.Add (new Value ("Min Vertical Speed", String.Format(MathTools.SingleDoubleValue, minVerticalSpeed)));
                } else {
                    values.Add (new Value ("Min Vertical Speed", String.Format(MathTools.SingleDoubleValue, minVerticalSpeed), 
                                           vessel.verticalSpeed, vessel.verticalSpeed >= minVerticalSpeed));
                }
            }

            if (maxVerticalSpeed != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("Max Vertical Speed", String.Format(MathTools.SingleDoubleValue, maxVerticalSpeed)));
                } else {
                    values.Add (new Value ("Max Vertical Speed", String.Format(MathTools.SingleDoubleValue, maxVerticalSpeed), 
                                           vessel.verticalSpeed, vessel.verticalSpeed <= maxVerticalSpeed));
                }
            }

            if (minGForce != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("Min G Force", String.Format(MathTools.SingleDoubleValue, minGForce)));
                } else {
                    values.Add (new Value ("Min G Force", String.Format(MathTools.SingleDoubleValue, minGForce), 
                                           vessel.geeForce, vessel.geeForce >= minGForce));
                }
            }

            if (maxGForce != 0.0) {
                if (vessel == null) {
                    values.Add (new Value ("Max G Force", String.Format(MathTools.SingleDoubleValue, maxGForce)));
                } else {
                    values.Add (new Value ("Max G Force", String.Format(MathTools.SingleDoubleValue, maxGForce), 
                                           vessel.geeForce, vessel.geeForce <= maxGForce));
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

