#region Header
//+ <source name="ApplicationSettingsHelper.cs" language="C#" begin="24-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.IO.IsolatedStorage;
#endregion

namespace PlaXore.ShakeGestures.Sensors
{
    /// <summary>
    /// Provides functionality for dealing with application settings.
    /// </summary>
    public static class ApplicationSettingsHelper
    {
        /// <summary>
        /// Attempts to get the value identified by the specified key.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="key">The key name.</param>
        /// <param name="defaultValue">The default value to be returned if <paramref name="key"/> was not found.</param>
        /// <returns>The value identified by <paramref name="key"/>.</returns>
        public static TValue TryGetValueWithDefault<TValue>(string key, TValue defaultValue)
        {
            TValue value = defaultValue;

            if (IsolatedStorageSettings.ApplicationSettings.Contains(key)) {
                object obj = IsolatedStorageSettings.ApplicationSettings[key];
                if (obj is TValue) {
                    value = (TValue) obj;
                }
            }

            return value;
        }
        
        /// <summary>
        /// Adds or updates the specified key-value pair.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <param name="value">The new value.</param>
        /// <returns></returns>
        public static bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            if (IsolatedStorageSettings.ApplicationSettings.Contains(key)) {
                if (IsolatedStorageSettings.ApplicationSettings[key] != value) {
                    IsolatedStorageSettings.ApplicationSettings[key] = value;
                    valueChanged = true;
                }
            } else {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        /// <summary>
        /// Saves the application settings.
        /// </summary>
        /// <remarks>
        /// Application settings are automatically saved at program end.
        /// </remarks>
        public static void Save()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}
