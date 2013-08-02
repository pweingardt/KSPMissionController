using System;

namespace MissionController
{
    
    public class Difficulty 
    {

        static Difficulty() 
        {
            easy = new double[medium.Length];
            hard = new double[medium.Length];

            for (int i = 0; i < medium.Length; ++i) 
            {
                easy[i] = 0.03 * medium[i];
                hard [i] = 1.5 * medium [i];
            }
        }

        public static readonly double[] medium = new double[] 
        { 0.7, 3, 5, 10, 3500, 6, 1.5, 10, 3 };
        

        public static readonly double[] easy;
        public static readonly double[] hard;

        private static double[] factors = medium;

        public static double[] Factors 
        {
            get 
            { 
                return factors; 
            }
        }

        public static double LiquidFuel 
        {
            get 
            { 
                return factors [0]; 
            }
        }

        public static double MonoPropellant 
        {
            get 
            { 
                return factors [1]; 
            }
        }

        public static double SolidFuel 
        {
            get 
            { 
                return factors [2]; 
            }
        }

        public static double Xenon
        {
            get
            { 
                return factors [3];
            }
        }

        public static double Mass
        {
            get
            { 
                return factors [4]; 
            }
        }

        public static double Oxidizer 
        {
            get 
            { 
                return factors [5]; 
            }
        }

        public static double LiquidEngines
        {
            get
            { 
                return factors [6];
            }
        }
        public static double LiquidOxygen
        {
            get
            {
                return factors[7];
            }
        }
        public static double LiquidH2
        {
            get
            {
                return factors[8];
            }
        }

        public static void init(double liquid, double mono, double solid, double xenon,
                                double mass, double oxidizer, double engines, double LiquidOxy, double LiquidH)
        {
            factors [0] = liquid;
            factors [1] = mono;
            factors [2] = solid;
            factors [3] = xenon;
            factors [4] = mass;
            factors [5] = oxidizer;
            factors [6] = engines;
            factors [7] = LiquidOxy;
            factors [8] = LiquidH;
        }

        public static void init(int difficulty)
        {
            switch (difficulty)
            {
                case 0:
                    factors = easy;
                    break;

                case 1:
                    factors = medium;
                    break;

                case 2:
                    factors = hard;
                    break;

                default:
                    factors = medium;
                    break;            
                
            }
        }
        
    }
}

