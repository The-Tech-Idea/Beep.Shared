using System;
using System.Collections.Concurrent;


namespace TheTechIdea.Beep.Shared
{
    public class InMemorySettingsProvider : ISettingsProvider
    {
        private readonly ConcurrentDictionary<string, object> _settings =
            new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public bool Contains(string key) => _settings.ContainsKey(key);

        public void SetValue(string key, object value)
        {
            _settings[key] = value;
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            if (_settings.TryGetValue(key, out object storedValue))
            {
                // Attempt a safe conversion
                return (T)Convert.ChangeType(storedValue, typeof(T));
            }
            return defaultValue;
        }

        public void Save()
        {
            // No-op for in-memory. If you want to persist to a file, do so here.
        }
    }
}
