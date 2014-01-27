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
        /// </summary>
        /// <returns>The budget.</returns>
        int getBudget();

        /// <summary>
        /// Adds the passed value to the current budget.
        /// </summary>
        /// <returns>the new budget</returns>
        /// <param name="value">value</param>
        int reward(int value);

        /// <summary>
        /// Subtracts the passed value from the current budget.
        /// </summary>
        /// <returns>the new budget</returns>
        /// <param name="value">Value.</param>
        int costs(int value);
    }   
}

