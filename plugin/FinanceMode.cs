using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionController
{
    public partial class MissionController
    {
        //List<hired> kerbalshired;
        //public struct hired
        //{
        //    string name;
        //    public hired(string ename)
        //    {
        //        name = ename; 
        //    }    
        //}


        //public void getdata()
        //{
        //    ConfigNode[] data = ConfigNode.Load(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/persistent.sfs").GetNode("GAME").GetNode("ROSTER", 1).GetNodes("CREW");
        //    kerbalshired = new List<hired>();
        //    foreach (ConfigNode item in data)
        //    {
        //        string kerbname = item.GetValue("name");
        //        hired Khired = new hired(kerbname);
        //        kerbalshired.Add(Khired);
        //    }
        //}
             
        public static double loan0 = Tools.GetValueDefault(Tools.MCSettings,"loan0",0);
        public static double loan1 = Tools.GetValueDefault(Tools.MCSettings,"loan1",.10);
        public static double loan2 = Tools.GetValueDefault(Tools.MCSettings,"loan2",.25);
        public static double loan3 = Tools.GetValueDefault(Tools.MCSettings,"loan3",.50);
        public static double loan4 = Tools.GetValueDefault(Tools.MCSettings, "loan4", .75);
        
        
        public static readonly double[] modeloan1;
        public static readonly double[] modeloan2;
        public static readonly double[] modeloan3;
        public static readonly double[] modeloan4;

        public static readonly double[] modeloan0 = new double[] { loan0 };


        private static double[] modeloanfactors = modeloan0;

        public static double[] ModeLoanFactors
        {
            get
            {
                return modeloanfactors;
            }
        }

        public static double ModeLoan 
        {
            get 
            {
                return modeloanfactors[0]; 
            }
        }

        public static void LoanModeinit(int loanmode)
        {
            switch (loanmode)
            {
                case 0:
                    modeloanfactors = modeloan0;
                    break;

                case 1:
                    modeloanfactors = modeloan1;
                    break;

                case 2:
                    modeloanfactors = modeloan2;
                    break;

                case 3:
                    modeloanfactors = modeloan3;
                    break;   
            
                case 4:
                    modeloanfactors = modeloan4;
                    break;

                default:
                    modeloanfactors = modeloan0;
                    break;
            }
        }
    
    }

  

}
