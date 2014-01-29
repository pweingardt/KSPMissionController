using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MissionController
{
    /// <summary>
    /// Parses a file into an object. Uses reflection.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Constant NamespacePrefix to avoid security issues
        /// </summary>
        private const String NamespacePrefix = "MissionController.";

        /// <summary>
        /// Writes the object into the passed file. (uses KSP.IO)
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <param name="path">Path.</param>
        public void writeObject(object obj, String path) {
            KSP.IO.TextWriter writer = KSP.IO.TextWriter.CreateForType<MissionController> (path);
            writeObject (writer, obj);
            writer.Close ();
        }

        public void writeContract(object obj, String path)
        {
            KSP.IO.TextWriter writer = KSP.IO.TextWriter.CreateForType<MissionController>(path);
            writeContract(writer, obj);
            writer.Close();
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

                if (info.FieldType.Equals (typeof(Enum))) {
                    writer.WriteLine ("    " + info.Name + " = " + info.GetValue(obj)); 
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

                if(info.FieldType.Equals(typeof(bool))) {
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

        private void writeContract(KSP.IO.TextWriter writer, object obj)
        {
            Type t = obj.GetType();
            if (t.Name == "UserContracts")
            {
                writer.WriteLine("Mission");
            }
            if (t.Name == "UCOrbitGoal")
            {
                writer.WriteLine("OrbitGoal");
            }
            if (t.Name == "UCLandingGoal") 
            {
                writer.WriteLine("LandingGoal");
            }
            if (t.Name == "UCDockingGoal") 
            {
                writer.WriteLine("DockingGoal");
            }

            writer.WriteLine("{");
            foreach (FieldInfo info in t.GetFields())
            {
                object o = info.GetValue(obj);

                if (o == null)
                {
                    continue;
                }

                if (info.FieldType.Equals(typeof(Enum)))
                {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (info.FieldType.Equals(typeof(String)))
                {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (info.FieldType.Equals(typeof(float)))
                {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (info.FieldType.Equals(typeof(double)))
                {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (info.FieldType.Equals(typeof(int)))
                {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (info.FieldType.Equals(typeof(bool)))
                {
                    writer.WriteLine("    " + info.Name + " = " + info.GetValue(obj));
                }

                if (o.GetType().GetInterface("IList") != null)
                {
                    IList ilist = (IList)o;

                    foreach (object v in ilist)
                    {
                        writeContract(writer, v);
                    }
                }
            }
            writer.WriteLine("}");
        }

        /// <summary>
        /// Parses the passed file (uses KSP.IO) and returns the parsed object
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="path">Path.</param>
        public object readFile(String path) {
            KSP.IO.TextReader reader = KSP.IO.TextReader.CreateForType<MissionController> (path);
            try {
                return readObject (reader);
            } catch (Exception e) {
                Debug.LogError (e.Message);
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
            Type t = Type.GetType (NamespacePrefix + name);
            object obj = Activator.CreateInstance (t);

            String n;
            // now parse the lines and put them into the dictionary.
            // if there is another object inside, parse it and invoke "add"
            while ((n = nextLine(reader)) != null && !n.Equals("}")) {
                if(n.IndexOf('=') != -1) {
                    string[] parts = n.Split('=');
                    string vname = parts[0].Trim();
                    string value = n.Substring(n.IndexOf('=') + 1).Trim().Replace("\\n", "\n");

                    ReflectionTools.setValue(vname, value, obj);
                } else {
                    object inner = readObject(reader, n);
                    t.GetMethod("add", new Type[] {inner.GetType()}).Invoke(obj, new object[] {inner});
                }
            }
            return obj;
        }

        /// <summary>
        /// Reads the next single line, that is not a comment and that is relevant ("not {")
        /// </summary>
        /// <returns>The line.</returns>
        /// <param name="reader">Reader.</param>
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
            return str;
        }
    }
}

