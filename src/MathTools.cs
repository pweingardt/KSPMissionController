using System;

namespace MissionController
{
    public class MathTools
    {
        public static double max(double v, double precision) {
            return v * (1.0 + precision);
        }

        public static double min(double v, double precision) {
            return v * (1.0 - precision);
        }

        public static bool inRange(double r, double precision, double value) {
            return value <= max(r, precision) &&  value >= min(r, precision);
        }

        public static bool inMinMax(double min, double max, double value) {
            return value <= max &&  value >= min;
        }

        
        public const String Range = "{0:0.##} (±{1:0.##})";
        public const String SingleDoubleValue = "{0:0.##}";
        public const String MinMaxValue = "{0:0.##} - {1:0.##}";
    }
}

