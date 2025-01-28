using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    public class FileSettingsProvider : ISettingsProvider
    {
        private readonly string _filePath;
        private Dictionary<string, object> _settings;

        public FileSettingsProvider(string filePath)
        {
            _filePath = filePath;
            _settings = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            LoadFromFile();
        }

        public bool Contains(string key) => _settings.ContainsKey(key);

        public void SetValue(string key, object value)
        {
            _settings[key] = value;
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            if (_settings.TryGetValue(key, out object storedValue))
            {
                return (T)Convert.ChangeType(storedValue, typeof(T));
            }
            return defaultValue;
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return;

            string json = File.ReadAllText(_filePath);
            _settings = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                        ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
