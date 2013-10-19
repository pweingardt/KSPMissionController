using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MissionController
{
    class Tools
    {
        public static double atod(string a)
        {
            double o;
            double.TryParse(a, out o);
            return o;
        }

        public static double tryDouble(ConfigNode node, string name, double val)
        {
            if (node.HasValue(name))
                val = atod(node.GetValue(name));
            // DBG else
            //DBG print"*MCEPC key not found: " + name);
            return val;
        }

        public static string spaces(int num)
        {
            string str = "";
            for (int i = 0; i < num; i++)
                str += " ";
            return str;
        }

        public static string NodeToString(ConfigNode node, int depth)
        {
            string str = spaces(depth) + node.name + "\n" + spaces(depth) + "{\n";

            foreach (ConfigNode.Value val in node.values)
                str += spaces(depth + 1) + val.name + " = " + val.value + "\n";

            foreach (ConfigNode n in node.nodes)
                str += NodeToString(n, depth + 1);

            return str + spaces(depth) + "}\n";
        }
    }
}
