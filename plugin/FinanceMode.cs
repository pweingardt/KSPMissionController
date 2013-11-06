using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionController
{
    public class FinanceMode
    {
        public static double lowloan = (Tools.Setting("lowloan", 0.75));
        public static double medloan = (Tools.Setting("medloan", 0.50));
        public static double highloan = (Tools.Setting("highloan", 0.25));
        public static double currentloan = lowloan;

        private Manager manager
        {
            get
            {
                return Manager.instance;
            }
        }

        public void checkloans()
        {
            if (manager.budget < 0 && manager.budget >= -25000)
            {
                currentloan = lowloan;
            }
            if (manager.budget < -25000 && manager.budget >= -60000)
            {
                currentloan = medloan;
            }
            if (manager.budget < -60000)
            {
                currentloan = highloan;
            }

        }        
    
    }
    
    /// <summary>
    /// sets the payout levels with research.  So payout * payoutlevels = payment.
    /// </summary>
    public class PayoutLeveles
    {  
      static PayoutLeveles()
        {
            payoutmode1 = new double[payoutmode0.Length];
            payoutmode2 = new double[payoutmode0.Length];

            for (int i = 0; i < payoutmode0.Length; ++i) 
            {
                payoutmode1[i] = 1.2 * payoutmode0[i];
                payoutmode2[i] = 1.3 * payoutmode0[i];
            }
        }

        public static readonly double[] payoutmode1;
        public static readonly double[] payoutmode2;

        public static readonly double[] payoutmode0 = new double[] 
        {1};

        private static double[] payoutfactors = payoutmode0;

        public static double[] Payoutfactors
        {
            get
            {
                return payoutfactors;
            }
        }

        public static double TechPayout 
        {
            get 
            { 
                return payoutfactors [0]; 
            }
        }
        
        public static void payoutlevels(int payout)
        {
            switch (payout)
            {
                case 0:
                    payoutfactors = payoutmode0;
                    break;                  
                case 1: 
                    payoutfactors = payoutmode1;
                    break;
                case 2:
                    payoutfactors = payoutmode2;
                    break;
                default:
                    payoutfactors = payoutmode0;
                    break;                    
            }
        }        
    }
}
