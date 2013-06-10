using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;

namespace MissionController
{
    public abstract class InstructionSet
    {
        /// <summary>
        /// Regex for random numbers: RANDOM(floating point, floating point)
        /// Works for double fields only!
        /// </summary>
        private Regex randRegex = new Regex ("^RANDOM\\(\\s*([-+]?[0-9]*\\.?[0-9]+),\\s*([-+]?[0-9]*\\.?[0-9]+)\\)$");

        /// <summary>
        /// Regex for ADD instruction: ADD(fieldName, floating point)
        /// Works for double fields only!
        /// </summary>
        private Regex addRegex = new Regex ("^ADD\\(\\s*([a-zA-Z_]+),\\s*([-+]?[0-9]*\\.?[0-9]+)\\)$");

        /// <summary>
        /// Regex for TIME value: TIME(5d).
        /// Works for double fields only!
        /// </summary>
        private Regex timeRegex = new Regex("^TIME\\(\\s*(?:(\\d+)y)?\\s*(?:(\\d+)d)?\\s*(?:(\\d+)h)?\\s*(?:(\\d+)m)?\\s*(?:(\\d+(?:\\.\\d+)?)s)?\\s*\\)$");


        private List<Instruction> instructions = new List<Instruction>();

        public bool InstructionsAvailable { get { return instructions.Count != 0; } }

        public void add(Instruction i) {
            instructions.Add (i);
        }

        /// <summary>
        /// Executes the instructions
        /// </summary>
        /// <returns>The instructions.</returns>
        /// <param name="seed">used seed</param>
        public void executeInstructions (Random random) {
            foreach (Instruction i in instructions) {
                String value = "";
                FieldInfo info = this.GetType ().GetField (i.field);

                // If the value starts with RANDOM(x, y)
                // We have to generate a new number
                if (i.value.StartsWith ("RANDOM") && info.FieldType.Equals (typeof(double))) {
                    Match m = randRegex.Match (i.value);
                    if (m.Success) {
                        double f1 = float.Parse (m.Groups[1].Value);
                        double f2 = float.Parse (m.Groups[2].Value);

                        value = "" + (random.NextDouble () * (f2 - f1) + f1);
                    }
                }

                // If the value starts with ADD(fieldName, floating point)
                // we have to get the requested value and add the second parameter
                if (i.value.StartsWith ("ADD") && info.FieldType.Equals (typeof(double))) {
                    Match m = addRegex.Match (i.value);
                    if (m.Success) {
                        String fname = m.Groups [1].Value;
                        double f2 = double.Parse (m.Groups[2].Value);

                        FieldInfo finfo = this.GetType ().GetField (fname);
                        if (finfo == null || !finfo.FieldType.Equals (typeof(double))) {
                            continue;
                        }

                        value = "" + ((double)finfo.GetValue (this) + f2);
                    }
                }

                if (i.value.StartsWith ("TIME") && info.FieldType.Equals (typeof(double))) {
                    Match m = timeRegex.Match (i.value);
                    if (m.Success) {
                        double ys = m.Groups [1].Success ? double.Parse (m.Groups[1].Value) : 0.0;
                        double ds = m.Groups [2].Success ? double.Parse (m.Groups[2].Value) : 0.0;
                        double hs = m.Groups [3].Success ? double.Parse (m.Groups[3].Value) : 0.0;
                        double ms = m.Groups [4].Success ? double.Parse (m.Groups[4].Value) : 0.0;
                        double ss = m.Groups [5].Success ? double.Parse (m.Groups[5].Value) : 0.0;

                        value = "" + (ys * (365.0 * 24.0 * 60.0 * 60.0) + ds * (24.0 * 60.0 * 60.0) + hs * (60.0 * 60.0) + ms * 60.0 + ss);
                    }
                }

                if (!value.Equals ("")) {
                    ReflectionTools.setValue (i.field, value, this);
                }
            }

            foreach (FieldInfo info in this.GetType().GetFields()) {
                object obj = info.GetValue (this);
                if (obj is InstructionSet) {
                    ((InstructionSet)obj).executeInstructions (random);
                }

                if (obj.GetType ().GetInterface ("IList") != null) {
                    IList ilist = (IList)obj;

                    foreach(object v in ilist) {
                        if(v is InstructionSet) {
                            ((InstructionSet)v).executeInstructions (random);
                        }
                    }
                }
            }
        }
    }

    public class Instruction {
        public String field;
        public String value;

        public Instruction(String field, String value) {
            this.field = field;
            this.value = value;
        }
    }
}