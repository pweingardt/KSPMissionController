using System;
using System.Collections.Generic;

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

        public static String formatTime(double seconds) {
            int y = (int)(seconds / (24.0 * 60.0 * 60.0 * 365.0));
            seconds = seconds % (24.0 * 60.0 * 60.0 * 365.0);
            int d = (int)(seconds / (24.0 * 60.0 * 60.0));
            seconds = seconds % (24.0 * 60.0 * 60.0);
            int h = (int)(seconds / (60.0 * 60.0));
            seconds = seconds % (60.0 * 60.0);
            int m = (int)(seconds / (60.0));
            seconds = seconds % (60.0);

            List<String> parts = new List<String> ();

            if (y > 0) {
                parts.Add (String.Format("{0:00},year", y));
            }

            if (d > 0) {
                parts.Add (String.Format("{0:00},days", d));
            }

            if (h > 0) {
                parts.Add (String.Format("{0:00},hours", h));
            }

            if (m > 0) {
                parts.Add (String.Format("{0:00},minutes", m));
            }

            if (seconds > 0) {
                parts.Add (String.Format("{0:00},seconds", seconds));
            }

            if (parts.Count > 0) {
                return String.Join (" ", parts.ToArray ());
            } else {
                return "0s";
            }
        }

        public static double calculateLongitude(double value) {
            while (value > 180.0) {
                value -= 360.0;
            }
            while (value < -180.0) {
                value += 360.0;
            }
            return value;
        }
        
        public const String Range = "{0:N3} (±{1:N3})";
        // NK edit formatting
        public const String SingleDoubleValue = "{0:N3}";
        public const String SingleRoundedValue = "{0:N0}";
        public const String MinMaxValue = "{0:N3} - {1:N3}";
        public const String MinMaxString = "{0} - {1}";
        public const String HighPrecisionDoubleValue = "{0:N8}";
        public const String HighPrecisionMinMaxValue = "{0:N8} - {1:N8}";
    }
}

