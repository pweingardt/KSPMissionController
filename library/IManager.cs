using System;
using System.Reflection;

namespace MissionController
{
    /// <summary>
    /// Use this class to access the manager singleton MissionController.Manager, to get access to the current space program.
    /// </summary>
    public class ManagerAccessor {
        private static IManager instance = null;

        public static IManager get {
            get {
                // If it has not been loaded yet, try to load it
                if (instance == null) {
                    Type t = Type.GetType ("MissionController.Manager,MissionController");

                    if (t != null) {
                        PropertyInfo prop = t.GetProperty ("instance", BindingFlags.Public | BindingFlags.Static);
                        if (prop != null) {
                            instance = (IManager) prop.GetValue (null, null);
                        }
                    }
                }

                return instance;
            }
        }
    }

    /// <summary>
    /// The Interface to communicate with the current space program.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Returns the current available budget.
        /// See Docs for Info on other values Listed, all are Infomation only
        /// </summary>
        /// <returns>The budget.</returns>
        int IgetBudget(); // Current Budget
        int Itotalbudget(); // Total Payments of Missions and Contracts
        int ItotalSpentVehicles(); // Total spent vehicle launches
        int ItotalRecycleMoney(); // Total Payments of Recycling
        int ItotalHiredKerbCost(); // Total Cost of Kerbals hire
        int ItotalModPayment(); // Total Payments made through ModPayments
        int ItotalModCost(); // Total Cost made through ModCost

        /// <summary>
        /// Use ModReward to add a Reward to the current MCE Budget, this will add to the budget
        /// </summary>
        /// <param name="value">amount of reward</param>
        /// <param name="Description">This description will show up in the Other Cost Manifest, give description of what reward was for</param>
        /// <returns></returns>
        int modReward(int value, string Description);

        /// <summary>
        /// Use ModCost to add a Cost to the current MCE budget, this will subtract from budget
        /// </summary>
        /// <param name="value">how much you want to charge the player</param>
        /// <param name="Description">This description will show up in the other Payments Manifest, give description of what this cost was for</param>
        /// <returns></returns>
        int ModCost(int value, string Description);
        
        //The Values Listed below are not really needed, but I have included them incase you want to use these instead of the mod values.  If you use the below values then you will make charges to the Budget System that
        //will not show up in any Manuscript.  If they do show up they won't have a description. The above values are better to use because you can give a description of what the cost or payments was.     

        /// <summary>
        /// the clean reward that has no modifications from research or any other values.  In other words if you add 10 credits, it will always be 10 credits.
        /// </summary>
        int CleanReward(int value);              

        /// <summary>
        /// this is for recycling and to add the reward for all types of recycling cost.  And adds a value to Total Recycling in the Budget manifest.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int recyclereward(int value);

        /// <summary>
        /// This is a normal reward for science points, thats used in regualr missions that don't include Companies random % additions.  So straight up science points.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float sciencereward(float value);      

        /// <summary>
        /// this is used to charge for Kerbals and shows up on the Kerbal Hire Cost Manifest.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int kerbCost(int value);
    }   
}

