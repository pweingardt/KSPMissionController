using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A single mission goal.
    /// </summary>
    public abstract class MissionGoal : InstructionSet
    {
        /// <summary>
        /// The mission goals description
        /// </summary>
        public String description = "";

        /// <summary>
        /// Used to store finished mission goals. Do not set manually!
        /// </summary>
        public String id = "";

        /// <summary>
        /// If repeatable = true, this mission goals can be accomplished more than once.
        /// </summary>
        public bool repeatable = false;

        /// <summary>
        /// optional = true makes this mission goal optional
        /// </summary>
        public bool optional = false;

        /// <summary>
        /// Optional reward for this specific mission goal
        /// </summary>
        public int reward = 0;

        /// <summary>
        /// The minimal crew count needed for this mission goal
        /// </summary>
        public int crewCount = 0;

        /// <summary>
        /// If true, the vessel needs to be throttled down in order to finish this mission goal
        /// </summary>
        public bool throttleDown = true;

        /// <summary>
        /// Minimal time for this mission goal in seconds.
        /// </summary>
        public double minSeconds = 0.0;

        /// <summary>
        /// nonPermanent = false makes the mission goal a permanent condition to finish the mission goal.
        /// Use with caution.
        /// </summary>
        public bool nonPermanent = true;

        /// <summary>
        /// Indicates that the mission goals has been finished once. Don't set this inside a mission file.
        /// </summary>
        public bool doneOnce = false;

        /// <summary>
        /// If true it does not matter with which vessel this goal has been finished.
        /// </summary>
        public bool vesselIndenpendent = false;

        /// <summary>
        /// The maximal and minimal total mass
        /// </summary>
        public double minTotalMass = 0.0;
        public double maxTotalMass = 0.0;

        private double timeStarted = -1.0;

        /// <summary>
        /// Checks, if this mission goal has been accomplished.
        /// </summary>
        /// <returns><c>true</c>, if mission goal is done, <c>false</c> otherwise.</returns>
        /// <param name="vessel">current vessel</param>
        public virtual bool isDone (Vessel vessel, GameEvent events)
        {
            if (vessel == null) {
                return false;
            }

            if (nonPermanent && doneOnce) {
                return true;
            }

            if(vessel.orbit == null) {
                return false;
            }
            
            List<Value> values = getValues (vessel, events);
            foreach (Value v in values) {
                if(!v.done) {
                    return false;
                }
            }

            // If this is a one time mission condition and everything has been done
            // we remember this for now
            if (nonPermanent) {
                doneOnce = true;
            }
            return true;
        }

        /// <summary>
        /// Returns all values related to this mission goal
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="vessel">current vessel</param>
        public List<Value> getValues(Vessel vessel, GameEvent events) {
            List<Value> vs = values (vessel, events);

            if (crewCount != 0) {
                if(vessel == null) {
                    vs.Add (new Value ("Crew count", "" + crewCount));
                } else {
                    vs.Add (new Value ("Crew count", "" + crewCount, "" + vessel.GetCrewCount (),
                                   crewCount <= vessel.GetCrewCount ()));
                }
            }

            if (minTotalMass != 0.0) {
                if (vessel == null) {
                    vs.Add(new Value("min. total mass", String.Format(MathTools.SingleDoubleValue, minTotalMass)));
                } else {
                    vs.Add(new Value("min. total mass", String.Format(MathTools.SingleDoubleValue, minTotalMass),
                                     String.Format(MathTools.SingleDoubleValue, vessel.GetTotalMass()),
                                     vessel.GetTotalMass() >= minTotalMass));
                }
            }

            if (maxTotalMass != 0.0) {
                if (vessel == null) {
                    vs.Add(new Value("max. total mass", String.Format(MathTools.SingleDoubleValue, maxTotalMass)));
                } else {
                    vs.Add(new Value("max. total mass", String.Format(MathTools.SingleDoubleValue, maxTotalMass),
                                     String.Format(MathTools.SingleDoubleValue, vessel.GetTotalMass()),
                                     vessel.GetTotalMass() <= maxTotalMass));
                }
            }

            bool done = true;
            foreach (Value v in vs) {
                done = done && v.done;
            }

            // If the mission goal is finished so far and we need to throttle down in order
            // to finish the mission goal, add another value if not throttled down
            if (vessel != null) {
                if (done && throttleDown && FlightInputHandler.state.mainThrottle != 0.0) {
                    vs.Add (new Value ("Throttle down!", "true", "false", false));
                    done = false;
                }
            }

            if (done && timeStarted == -1.0 && minSeconds > 0.0) {
                timeStarted = Planetarium.GetUniversalTime ();
            }

            if (minSeconds > 0.0 && !done) {
                timeStarted = -1.0;
            }

            if (minSeconds > 0.0) {
                if (vessel != null) {
                    double diff = (timeStarted == -1.0 ? 0 : Planetarium.GetUniversalTime () - timeStarted);
                    vs.Add (new Value("Minimal time", MathTools.formatTime(minSeconds), MathTools.formatTime(diff), diff > minSeconds));
                } else {
                    vs.Add (new Value("Minimal time", MathTools.formatTime(minSeconds)));
                }
            }

            return vs;
        }

        /// <summary>
        /// Returns an array of necessary values, like orbital parameters.
        /// </summary>
        /// <param name="v">current vessel, might be null when in editor mode or in space center mode!</param>
        virtual protected List<Value> values(Vessel v, GameEvent events) {
            return new List<Value> ();
        }

        /// <summary>
        /// Caption in the window for this mission goal
        /// </summary>
        /// <returns>The type.</returns>
        virtual public String getType() {
            return "Mission goal";
        }
    }

    /// <summary>
    /// A single value for this mission goal.
    /// </summary>
    public class Value {
        public Value(String name, String shouldBe, string currentlyIs, bool done) {
            this.currentlyIs = currentlyIs;
            this.done = done;
            this.name = name;
            this.shouldBe = shouldBe;
        }

        public Value(String name, String shouldBe, double currentlyIs, bool done) :
            this (name, shouldBe, String.Format(MathTools.SingleDoubleValue, currentlyIs), done) {
        }

        public Value(String name, double shouldBe, double currentlyIs, bool done) :
            this (name, String.Format(MathTools.SingleDoubleValue, shouldBe), String.Format(MathTools.SingleDoubleValue, currentlyIs), done) {
        }

        public Value(String name, String value) : this(name, value, "", false) {
        }

        public Value(String name, double value) : this(name, String.Format(MathTools.SingleDoubleValue, value), "", false) {
        }

        public String name;
        public String shouldBe;
        public string currentlyIs;
        public bool done;
    }
}

