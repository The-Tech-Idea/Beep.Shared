using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TheTechIdea.Beep.Icons
{
    /// <summary>
    /// Provides convenient access to embedded SVGs under the namespace:
    /// TheTechIdea.Beep.Icons.svg
    /// </summary>
    public  static partial class UISvgs
    {
        private const string BaseNamespace = "TheTechIdea.Beep.Icons.svg";

        /// <summary>
        /// Gets the assembly containing the embedded SVG resources.
        /// </summary>
        public static Assembly ResourceAssembly => typeof(UISvgs).Assembly;

        private static readonly Lazy<string[]> _resourceNames = new(() =>
            ResourceAssembly.GetManifestResourceNames()
                .Where(n => n.StartsWith(BaseNamespace, StringComparison.Ordinal))
                .Where(n => n.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                .ToArray());

        private static readonly Lazy<Dictionary<string, string>> _byFileName = new(() =>
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var fullName in _resourceNames.Value)
            {
                var fileName = UiIconsHelpers.GetFileName(fullName, BaseNamespace);
                if (!dict.ContainsKey(fileName))
                {
                    dict[fileName] = fullName;
                }
            }
            return dict;
        });

        /// <summary>
        /// Returns all embedded SVG resource names (full manifest names).
        /// </summary>
        public static string[] GetAllResourceNames() => _resourceNames.Value;

        /// <summary>
        /// Returns all SVG file names (e.g., "icon.svg").
        /// </summary>
        public static string[] GetAllFileNames() => _byFileName.Value.Keys.ToArray();

        /// <summary>
        /// Checks if an SVG resource exists by full manifest name or by file name.
        /// </summary>
        public static bool Exists(string nameOrFile)
        {
            if (string.IsNullOrWhiteSpace(nameOrFile)) return false;
            if (nameOrFile.StartsWith(BaseNamespace, StringComparison.Ordinal))
            {
                return _resourceNames.Value.Contains(nameOrFile);
            }
            var file = UiIconsHelpers.EnsureExtension(UiIconsHelpers.ExtractFileName(nameOrFile));
            return _byFileName.Value.ContainsKey(file);
        }

        /// <summary>
        /// Tries to get the full resource path from a file name or relative path.
        /// Accepted inputs: "icon.svg", "svg/icon.svg", full manifest name.
        /// </summary>
        public static bool TryGet(string nameOrFile, out string resourcePath)
        {
            resourcePath = string.Empty;
            if (string.IsNullOrWhiteSpace(nameOrFile)) return false;

            // Already a full manifest resource name
            if (nameOrFile.StartsWith(BaseNamespace, StringComparison.Ordinal))
            {
                if (_resourceNames.Value.Contains(nameOrFile))
                {
                    resourcePath = nameOrFile;
                    return true;
                }
                return false;
            }

            // Accept raw filename or folder-like path
            var file = UiIconsHelpers.EnsureExtension(UiIconsHelpers.ExtractFileName(nameOrFile));
            if (_byFileName.Value.TryGetValue(file, out var full))
            {
                resourcePath = full;
                return true;
            }

            // Try to construct a manifest path from a relative-like path
            var normalized = nameOrFile.Replace('/', '.').Replace('\\', '.');
            if (!normalized.StartsWith("svg.", StringComparison.OrdinalIgnoreCase))
            {
                normalized = $"svg.{normalized}";
            }
            if (!normalized.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            {
                normalized += ".svg";
            }
            var candidate = $"{BaseNamespace}.{normalized.Substring("svg.".Length)}";
            if (_resourceNames.Value.Contains(candidate))
            {
                resourcePath = candidate;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts a file name or relative path to a full manifest resource path.
        /// Returns null if the resource does not exist.
        /// </summary>
        public static string? ToResourcePath(string nameOrFile)
        {
            return TryGet(nameOrFile, out var path) ? path : null;
        }

        /// <summary>
        /// Opens an embedded SVG as a stream by file name or full resource name.
        /// Returns null if not found.
        /// </summary>
        public static Stream? Open(string nameOrFile)
        {
            if (!TryGet(nameOrFile, out var full)) return null;
            return ResourceAssembly.GetManifestResourceStream(full);
        }

        // Shared helper for nested icon categories
        private static string Require(string file)
            => ToResourcePath(file) ?? throw new InvalidOperationException($"SVG icon not found: {file}");

    }
}
