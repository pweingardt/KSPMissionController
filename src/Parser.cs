using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace MissionController
{
    /// <summary>
    /// Parses a file into an object. Uses reflection.
    /// </summary>
    public class Parser
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

        public int lastSeed = 0;

        private Random random;
        private Random seedGenerator = new Random ();
        private const String NamespacePrefix = "MissionController.";

        public void writeObject(object obj, String path) {
            KSP.IO.TextWriter writer = KSP.IO.TextWriter.CreateForType<MissionController> (path);
            writeObject (writer, obj);
            writer.Close ();
        }

        private void writeObject(KSP.IO.TextWriter writer, object obj) {
            Type t = obj.GetType ();
            writer.WriteLine (t.Name);
            writer.WriteLine ("{");
            foreach (FieldInfo info in t.GetFields()) {
                object o = info.GetValue(obj);

                if(o == null) {
                    continue;
                }

                if(info.FieldType.Equals(typeof(String))) {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }
                
                if(info.FieldType.Equals(typeof(float))) {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }
                
                if(info.FieldType.Equals(typeof(double))) {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }
                
                if(info.FieldType.Equals(typeof(int))) {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (o.GetType().GetInterface("IList") != null) {
                    IList ilist = (IList)o;
                    
                    foreach(object v in ilist) {
                        writeObject(writer, v);
                    }
                }
            }
            writer.WriteLine ("}");
        }

        public object readFile(String path, int seed = -1) {
            KSP.IO.TextReader reader = KSP.IO.TextReader.CreateForType<MissionController> (path);

            if (seed == -1) {
                seed = seedGenerator.Next ();
            }

            lastSeed = seed;
            random = new Random (seed);

            try {
                return readObject (reader);
            } catch (Exception e) {
                Console.Write(e.Message);
                return null;
            } finally {
                reader.Close ();
            }
        }

        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="reader">Reader.</param>
        private object readObject(KSP.IO.TextReader reader) {
            object obj = null;

            // Get the name of the class an create an instance
            String n = nextLine (reader);
            obj = readObject (reader, n);

            return obj;
        }

        private object readObject(KSP.IO.TextReader reader, String name) {
            Dictionary<String, String> values = new Dictionary<string, string> ();
            Type t = Type.GetType (NamespacePrefix + name);
            object obj = Activator.CreateInstance (t);

            String n;
            // now parse the lines and put them into the dictionary.
            // if there is another object inside, parse it and invoke "add"
            while ((n = nextLine(reader)) != null && !n.Equals("}")) {
                if(n.IndexOf('=') != -1) {
                    string[] parts = n.Split('=');
                    string vname = parts[0].Trim();
                    string value = n.Substring(n.IndexOf('=') + 1).Trim();
                    values.Add(vname, value);

                    setValue(vname, value, obj);
                } else {
                    object inner = readObject(reader, n);
                    t.GetMethod("add", new Type[] {inner.GetType()}).Invoke(obj, new object[] {inner});
                }
            }
            return obj;
        }

        private void setValue(String vname, String value, object o) {
            FieldInfo info = o.GetType ().GetField (vname);

            if (info == null) {
                return;
            }

            // If the value starts with RANDOM(x, y)
            // We have to generate a new number
            if (value.StartsWith ("RANDOM") && info.FieldType.Equals(typeof(double))) {
                Match m = randRegex.Match (value);
                if (m.Success) {
                    double f1 = float.Parse (m.Groups[1].Value);
                    double f2 = float.Parse (m.Groups[2].Value);

                    value = "" + (random.NextDouble () * (f2 - f1) + f1);
                }
            }

            // If the value starts with ADD(fieldName, floating point)
            // we have to get the requested value and add the second parameter
            if (value.StartsWith ("ADD") && info.FieldType.Equals(typeof(double))) {
                Match m = addRegex.Match (value);
                if (m.Success) {
                    String fname = m.Groups[1].Value;
                    double f2 = double.Parse (m.Groups[2].Value);

                    FieldInfo finfo = o.GetType ().GetField (fname);
                    if (finfo == null || !finfo.FieldType.Equals(typeof(double))) {
                        return;
                    }

                    value = "" + ((double)finfo.GetValue (o) + f2);
                }
            }

            if (info.FieldType.Equals (typeof(String))) {
                info.SetValue(o, value);
            }

            if (info.FieldType.Equals (typeof(bool))) {
                bool v;
                bool.TryParse(value, out v);
                info.SetValue(o, v);
            }

            if (info.FieldType.Equals (typeof(double))) {
                double v;
                double.TryParse(value, out v);
                info.SetValue(o, v);
            }

            if (info.FieldType.Equals (typeof(float))) {
                float v;
                float.TryParse(value, out v);
                info.SetValue(o, v);
            }

            if (info.FieldType.Equals (typeof(int))) {
                int v;
                int.TryParse(value, out v);
                info.SetValue(o, v);
            }
        }

        private String nextLine(KSP.IO.TextReader reader) {
            String str = reader.ReadLine ();
            if(str != null) {
                str = str.Trim();
            }
            while(str != null && (str.Length == 0 || str.Equals("{") || str[0] == '#')) {
                str = reader.ReadLine();
                if(str != null) {
                    str = str.Trim();
                }
            }
            if(str != null) {
                str = str.Trim();
            }
            return str;
        }
    }
}

