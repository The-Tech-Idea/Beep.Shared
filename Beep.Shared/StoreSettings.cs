using System;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    public static class StoreSettings
    {
        public static string ProjectSettingsFile { get; set; } = "project.settings";
        /// <summary>
        /// The current settings provider used to store and retrieve key–value pairs.
        /// By default, it can be an in-memory provider, but you can assign a FileSettingsProvider or something else.
        /// </summary>
        public static ISettingsProvider Provider { get; set; } = new DynamicProjectSettingsProvider();

        /// <summary>
        /// Stores a value for a particular control GUID and property name.
        /// </summary>
        /// <param name="controlGuid">The control GUID</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="value">Value to store</param>
        public static void Set(string controlGuid, string propertyName, object value)
        {
            if (string.IsNullOrEmpty(controlGuid) || string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Control GUID and property name cannot be null or empty.");

            string settingKey = $"{controlGuid}_{propertyName}";

            // Store the value via the provider
            Provider.SetValue(settingKey, value);
            Provider.Save(); // Save if needed
        }

        /// <summary>
        /// Retrieves a value for the specified control GUID and property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controlGuid"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(string controlGuid, string propertyName, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(controlGuid) || string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Control GUID and property name cannot be null or empty.");

            string settingKey = $"{controlGuid}_{propertyName}";

            // Retrieve from the provider
            return Provider.GetValue(settingKey, defaultValue);
        }
    }
}
