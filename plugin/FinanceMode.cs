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
}
