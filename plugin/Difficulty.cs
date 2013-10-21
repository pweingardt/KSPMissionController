using System;
using UnityEngine;
using MissionController;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using KSP.IO;

namespace MissionController
{
    /// <summary>
    /// This is Used For Research Tab and mission Controller to control Fuel Research Levels
    /// Sets Up The Switches For Fuels Check MCcalc for what it effects
    /// </summary>
    public class FuelMode
    {
        static FuelMode()
        {
            fuelmode1 = new double[fuelmode0.Length];
            
            for (int i = 0; i < fuelmode0.Length; ++i) 
            {
                fuelmode1[i] = .8 * fuelmode0[i];
            }
        }

        public static readonly double[] fuelmode1;

        public static readonly double[] fuelmode0 = new double[] 
        {1};

        private static double[] fuelfactors = fuelmode0;

        public static double[] Fuelfactors
        {
            get
            {
                return fuelfactors;
            }
        }

        public static double TechFuel 
        {
            get 
            { 
                return fuelfactors [0]; 
            }
        }
        /// <summary>
        /// The Switches for Fuels
        /// </summary>
        /// <param name="fuelmode"></param>
        public static void fuelinit(int fuelmode)
        {
            switch (fuelmode)
            {
                case 0:
                    fuelfactors = fuelmode0;
                    break;

                case 1:
                    fuelfactors = fuelmode1;
                    break;
                
                default:
                    fuelfactors = fuelmode0;
                    break;       
        
            }
        }


    }

    /// <summary>
    /// Loads the Players Science Also
    /// This is used by the Research Tab and Mission Controller to control Construction cost Levels
    /// check MCCalc for what it effects
    /// </summary>
    public class ConstructionMode
    {
        /// <summary>
        /// Loads Research And Development
        /// </summary>
        public bool RDScience
        {
            get { return ResearchAndDevelopment.Instance != null; }
        }

        /// <summary>
        /// Gets And Sets The Amount Of Science In Player Saved Game
        /// </summary>
        public float Science
        {
            get
            {
                return ResearchAndDevelopment.Instance.Science;
            }
            set
            {
                float previous = ResearchAndDevelopment.Instance.Science;
                ResearchAndDevelopment.Instance.Science = value;
                Debug.LogError("Mission Controller Changed Science by " + (ResearchAndDevelopment.Instance.Science - previous) + " to " + ResearchAndDevelopment.Instance.Science + ".");
            }
        }

        /// <summary>
        /// Deducts Science from the Player Saved Game Persistent file
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public float DeductScience(float cost)
        {           
            return Science -= cost;   
        }
      
        static ConstructionMode()
        {
            construction1 = new double[construction0.Length];
            construction2 = new double[construction0.Length];
            
            for (int i = 0; i < construction0.Length; ++i) 
            {
                construction1[i] = .84 * construction0[i];
                construction2[i] = .74 * construction0[i];
            }
        }

        /// <summary>
        /// Sets up The Construction Switches For Different Research Levels
        /// </summary>
        public static readonly double[] construction1;
        public static readonly double[] construction2;

        public static readonly double[] construction0 = new double[] 
        {1.0};

        private static double[] constructionfactors = construction0;

        public static double[] ConstructionFactors
        {
            get
            {
                return constructionfactors;
            }
        }

        public static double TechConstrustion 
        {
            get 
            {
                return constructionfactors[0]; 
            }
        }
        /// <summary>
        /// The switches For Construction
        /// </summary>
        /// <param name="constructionmode"></param>
        public static void constructinit(int constructionmode)
        {
            switch (constructionmode)
            {
                case 0:
                    constructionfactors = construction0;
                    break;

                case 1:
                    constructionfactors = construction1;
                    break;

                case 2:
                    constructionfactors = construction2;
                    break;
                
                default:
                    constructionfactors = construction0;
                    break;       
        
            }
        }
    }
}
