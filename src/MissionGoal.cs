using System;
using System.Collections.Generic;

namespace MissionController
{
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

        public bool isDone (Vessel vessel)
        {
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

        public List<Value> getValues(Vessel vessel) {
            List<Value> vs = values (vessel);

            if (crewCount != 0) {
                vs.Add (new Value ("Crew count", "" + crewCount, "" + vessel.GetCrewCount (),
                                   crewCount <= vessel.GetCrewCount ()));
            }

            bool done = true;
            foreach (Value v in vs) {
                done = done && v.done;
            }

            if (done && throttleDown && FlightInputHandler.state.mainThrottle != 0.0) {
                vs.Add(new Value("Throttle down!", "true", "false", false));
            }

            return vs;
        }

        virtual protected List<Value> values(Vessel v) {
            return new List<Value> ();
        }

        virtual public String getType() {
            return "Mission goal";
        }
    }

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

        public String name;
        public String shouldBe;
        public string currentlyIs;
        public bool done;
    }
}

