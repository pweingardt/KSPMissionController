using System;
using System.Reflection;

namespace MissionController
{
    public class ReflectionTools
    {
        public static void setValue(String field, String value, object o) {
            FieldInfo info = o.GetType ().GetField (field);

            if (info == null) {
                return;
            }

            // If it is an instruction, we assume there is an add(string) method and invoke it
            if (value.StartsWith ("RANDOM") || value.StartsWith ("ADD") || value.StartsWith ("TIME")) {
                o.GetType ().GetMethod ("add", new Type[] { typeof(Instruction) })
                    .Invoke (o, new object[] { new Instruction(field, value) });                                                                              
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
    }
}

