using System;

namespace MissionController
{
    /// <summary>
    /// Extensions for non-accessible classes
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Checks if the enum has set the passed bit flag
        /// </summary>
        /// <returns><c>true</c> if enum has set the passed bit flag, <c>false</c> otherwise</returns>
        /// <param name="type">Type</param>
        /// <param name="value">Value</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool Has<T>(this System.Enum type, T value) {
            return (((int)(object)type & (int)(object)value) == (int)(object)value);
        }

        public static bool Is<T>(this System.Enum type, T value) {
            try {
                return (int)(object)type == (int)(object)value;
            }
            catch {
                return false;
            }    
        }


        public static T Add<T>(this System.Enum type, T value) {
            try {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch(Exception ex) {
                throw new ArgumentException(
                    string.Format(
                    "Could not append value from enumerated type '{0}'.",
                    typeof(T).Name
                    ), ex);
            }    
        }


        public static T Remove<T>(this System.Enum type, T value) {
            try {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex) {
                throw new ArgumentException(
                    string.Format(
                    "Could not remove value from enumerated type '{0}'.",
                    typeof(T).Name
                    ), ex);
            }  
        }
    }
}

