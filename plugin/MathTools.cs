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

        public static double abs(double v) {
            return v > 0 ? v : -v;
        }

        public static bool inRange(double r, double precision, double value) {
            // Should this really be: return abs(value - r) < precision;
            // I don't know where this is used, so I cannot test it properly.
            return value <= max(r, precision) &&  value >= min(r, precision);
        }

        public static bool inMinMax(double min, double max, double value) {
            return value <= max &&  value >= min;
        }

        public const double minute = 60;
        public const double hour = minute * 60;
        public const double day = hour * 24;
        public const double year = day * 365;

        public static string secondsIntoRealTime(double seconds)
        {
            int Seconds = (int)(seconds % 60);
            int Minutes = (int)((seconds / minute) % 60);
            int Hours = (int)((seconds / hour) % 24);
            int Days = (int)((seconds / day) % 365);
            int Years = (int)(seconds / year);           

            if (seconds < year)
                return  "Day " + Days  + " Hour " + Hours;

            return  "Year " + Years  + " Day " + Days;
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
                parts.Add (String.Format("{0}:year", y));
            }

            if (d > 0) {
                parts.Add (String.Format("{0}:days", d));
            }

            if (h > 0) {
                parts.Add (String.Format("{0}:hours", h));
            }

            if (m > 0) {
                parts.Add (String.Format("{0}:minutes", m));
            }

            if (seconds > 0) {
                parts.Add (String.Format("{0:00}:seconds", seconds));
            }

            if (parts.Count > 0) {
                return String.Join (" ", parts.ToArray ());
            } else {
                return "0s";
            }
        }

        public static String formatDistance(double meters) {
            if (meters < 0)
                return String.Format ("{0:N1}cm", meters * 100);
             
            if (meters < 1000)
                return String.Format ("{0:N1}m", meters);

            return String.Format ("{0:N1}km", meters / 1000);
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


    public class Randomizator3000
    {
        public class Item<M>
        {
            public M value;
            public float weight;

            public static float GetTotalWeight<T>(Item<T>[] p_itens)
            {
                float __toReturn = 0;
                foreach (var item in p_itens)
                {
                    __toReturn += item.weight;
                }

                return __toReturn;
            }
        }

        private static System.Random _randHolder;
        private static System.Random _random
        {
            get
            {
                if (_randHolder == null)
                    _randHolder = new System.Random();

                return _randHolder;
            }
        }

        public static T PickOne<T>(Item<T>[] p_itens)
        {
            if (p_itens == null || p_itens.Length == 0)
            {
                return default(T);
            }

            float __randomizedValue = (float)_random.NextDouble() * (Item<T>.GetTotalWeight(p_itens));
            float __adding = 0;
            for (int i = 0; i < p_itens.Length; i++)
            {
                float __cacheValue = p_itens[i].weight + __adding;
                if (__randomizedValue <= __cacheValue)
                {
                    return p_itens[i].value;
                }

                __adding = __cacheValue;
            }

            return p_itens[p_itens.Length - 1].value;

        }
    }
}

