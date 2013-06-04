using System;
using System.Collections.Generic;

namespace MissionController
{
    /// <summary>
    /// A single mission goal.
    /// </summary>
    public abstract class MissionGoal
    {
        public String description = "";
        public String id = "";
        public bool nonPermanent = true;
        public bool doneOnce = false;

        public bool repeatable = false;

        public bool optional = false;

        public int reward = 0;

        public int crewCount = 0;

        public Boolean throttleDown = true;

        /// <summary>
        /// Checks, if this mission goal has been accomplished.
        /// </summary>
        /// <returns><c>true</c>, if done was ised, <c>false</c> otherwise.</returns>
        /// <param name="vessel">current vessel</param>
        public bool isDone (Vessel vessel)
        {
            if (vessel == null) {
                return false;
            }

            if (nonPermanent && doneOnce) {
                return true;
            }

            if(vessel == null || vessel.orbit == null) {
                return false;
            }
            
            List<Value> values = getValues (vessel);
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
        public List<Value> getValues(Vessel vessel) {
            List<Value> vs = values (vessel);

            if (crewCount != 0) {
                if(vessel == null) {
                    vs.Add (new Value ("Crew count", "" + crewCount));
                } else {
                    vs.Add (new Value ("Crew count", "" + crewCount, "" + vessel.GetCrewCount (),
                                   crewCount <= vessel.GetCrewCount ()));
                }
            }

            bool done = true;
            foreach (Value v in vs) {
                done = done && v.done;
            }

            if (vessel != null) {
                if (done && throttleDown && FlightInputHandler.state.mainThrottle != 0.0) {
                    vs.Add (new Value ("Throttle down!", "true", "false", false));
                }
            }

            return vs;
        }

        /// <summary>
        /// Returns an array of necessary values, like orbital parameters.
        /// </summary>
        /// <param name="v">current vessel, might be null!</param>
        virtual protected List<Value> values(Vessel v) {
            return new List<Value> ();
        }

        /// <summary>
        /// Caption user in the window for this mission goal
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

