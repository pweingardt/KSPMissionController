using System;
using System.Collections.Generic;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// A mission goal that requires a certain amount of a certain resource in order to finish the mission goal.
    /// </summary>
    public class ResourceGoal : MissionGoal
    {
        public String name = "LiquidFuel";
        public double maxAmount = 0.0;
        public double minAmount = 0.0;

        protected override List<Value> values(Vessel vessel) {
            List<Value> v = new List<Value> ();

            double a = 0;

            if (vessel != null) {
                foreach (Part p in vessel.parts) {
                    if (p.Resources [name] != null) {
                        a += p.Resources [name].amount;
                    }
                }
            }

            if (maxAmount != 0) {
                if(vessel == null) {
                    v.Add(new Value("max. resource " + name, maxAmount));
                } else {
                    v.Add(new Value("max. resource " + name, maxAmount, a, a <= maxAmount));
                }
            }

            if (minAmount != 0) {
                if(vessel == null) {
                    v.Add(new Value("min. resource " + name, minAmount));
                } else {
                    v.Add(new Value("min. resource " + name, minAmount, a, a >= maxAmount));
                }
            }

            return v;
        }
    }
}

