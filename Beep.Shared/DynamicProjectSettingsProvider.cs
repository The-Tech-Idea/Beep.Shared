using System;
using System.Configuration;
using System.Reflection;
using TheTechIdea.Beep.Shared;



namespace TheTechIdea.Beep.Shared
{
    /// <summary>
    /// A provider that attempts to dynamically discover the calling project’s
    /// typed Properties.Settings (by reflection) and store key-value pairs there.
    /// </summary>
    public class DynamicProjectSettingsProvider : ISettingsProvider
    {
        private readonly object _typedSettingsInstance;      // e.g. MyProject.Properties.Settings.Default
        private readonly Type _typedSettingsType;            // e.g. typeof(MyProject.Properties.Settings)
        private readonly SettingsProvider _localProvider;    // e.g. "LocalFileSettingsProvider"

        public DynamicProjectSettingsProvider(
            string settingsClassName = "Properties.Settings")
        {
            // Attempt to find the calling assembly and locate "Properties.Settings" by default
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            // E.g., "MyProject.Properties.Settings" or just "Properties.Settings"
            _typedSettingsType = callingAssembly.GetType(settingsClassName);
            if (_typedSettingsType == null)
            {
                throw new InvalidOperationException(
                    $"Could not find a typed Settings class named '{settingsClassName}' in assembly '{callingAssembly.FullName}'.");
            }

            // We assume a static property "Default" for typed settings
            PropertyInfo defaultProp = _typedSettingsType.GetProperty("Default",
                BindingFlags.Public | BindingFlags.Static);
            if (defaultProp == null)
            {
                throw new InvalidOperationException(
                    $"Type '{_typedSettingsType.FullName}' does not have a static 'Default' property.");
            }

            // Get the typed settings instance
            _typedSettingsInstance = defaultProp.GetValue(null, null);
            if (_typedSettingsInstance == null)
            {
                throw new InvalidOperationException(
                    $"The 'Default' property on '{_typedSettingsType.FullName}' returned null.");
            }

            // Attempt to find the .Properties, .Providers
            var propertiesProp = _typedSettingsType.GetProperty("Properties", BindingFlags.Public | BindingFlags.Instance);
            var providersProp = _typedSettingsType.GetProperty("Providers", BindingFlags.Public | BindingFlags.Instance);

            if (propertiesProp == null || providersProp == null)
            {
                throw new InvalidOperationException(
                    $"Could not reflect 'Properties' or 'Providers' on '{_typedSettingsType.FullName}'.");
            }

            // Typically "LocalFileSettingsProvider"
            var providers = providersProp.GetValue(_typedSettingsInstance, null) as SettingsProviderCollection;
            if (providers == null || providers["LocalFileSettingsProvider"] == null)
            {
                throw new InvalidOperationException("LocalFileSettingsProvider not found in typed settings providers.");
            }
            _localProvider = providers["LocalFileSettingsProvider"];
        }

        public bool Contains(string key)
        {
            var propertyCollection = GetSettingsPropertyCollection();
            return propertyCollection[key] != null;
        }

        public void SetValue(string key, object value)
        {
            var propertyCollection = GetSettingsPropertyCollection();

            // If it doesn't exist, create it
            if (propertyCollection[key] == null)
            {
                AddDynamicSetting(key, value);
            }

            // Once guaranteed to exist, update
            SetValueCore(key, value);
            Save();
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            var propertyCollection = GetSettingsPropertyCollection();

            // If property doesn't exist, create it now
            if (propertyCollection[key] == null)
            {
                AddDynamicSetting(key, defaultValue);
                return defaultValue;
            }

            // read the value from typed settings
            object storedValue = GetValueCore(key);
            if (storedValue == null)
            {
                return defaultValue;
            }
            return (T)Convert.ChangeType(storedValue, typeof(T));
        }

        public void Save()
        {
            // reflect "Save()" method
            var saveMethod = _typedSettingsType.GetMethod("Save", BindingFlags.Public | BindingFlags.Instance);
            saveMethod?.Invoke(_typedSettingsInstance, null);
        }

        /// <summary>
        /// Dynamically adds a user-scoped property to the typed settings if it doesn’t exist.
        /// </summary>
        private void AddDynamicSetting(string key, object defaultValue)
        {
            var propertyCollection = GetSettingsPropertyCollection();

            var settingType = defaultValue?.GetType() ?? typeof(string);
            var newSettingProperty = new SettingsProperty(key)
            {
                PropertyType = settingType,
                IsReadOnly = false,
                DefaultValue = defaultValue,
                Provider = _localProvider,   // Usually "LocalFileSettingsProvider"
                SerializeAs = SettingsSerializeAs.String
            };
            newSettingProperty.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());

            propertyCollection.Add(newSettingProperty);

            // reflect "Reload()" so the new property is recognized
            var reloadMethod = _typedSettingsType.GetMethod("Reload", BindingFlags.Public | BindingFlags.Instance);
            reloadMethod?.Invoke(_typedSettingsInstance, null);
        }

        /// <summary>
        /// Helper to retrieve the typed settings property collection via reflection.
        /// </summary>
        private SettingsPropertyCollection GetSettingsPropertyCollection()
        {
            // typedSettingsInstance.Properties => SettingsPropertyCollection
            var propertiesProp = _typedSettingsType.GetProperty("Properties", BindingFlags.Public | BindingFlags.Instance);
            return (SettingsPropertyCollection)propertiesProp.GetValue(_typedSettingsInstance, null);
        }

        private object GetValueCore(string key)
        {
            // typedSettingsInstance[key] => retrieve
            var indexer = _typedSettingsType.GetProperty("Item", new[] { typeof(string) });
            return indexer?.GetValue(_typedSettingsInstance, new object[] { key });
        }

        private void SetValueCore(string key, object value)
        {
            // typedSettingsInstance[key] = value
            var indexer = _typedSettingsType.GetProperty("Item", new[] { typeof(string) });
            indexer?.SetValue(_typedSettingsInstance, value, new object[] { key });
        }
    }
}
