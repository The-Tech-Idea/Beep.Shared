using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTechIdea.Beep.Shared
{
    public interface ISettingsProvider
    {
        bool Contains(string key);

        /// <summary>
        /// Sets a value for the specified key.
        /// </summary>
        void SetValue(string key, object value);

        /// <summary>
        /// Retrieves a value for the specified key, or returns defaultValue if not found.
        /// </summary>
        T GetValue<T>(string key, T defaultValue);

        /// <summary>
        /// Saves any pending changes to the underlying store (optional).
        /// </summary>
        void Save();
    }
}

