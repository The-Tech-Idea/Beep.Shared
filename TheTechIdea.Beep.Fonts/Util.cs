using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TheTechIdea.Beep.Fonts
{
    /// <summary>
    /// Provides convenient access to embedded font resources (.ttf/.otf) under the namespace:
    /// TheTechIdea.Beep.Fonts
    /// </summary>
    public static class Util
    {
        private const string BaseNamespace = "TheTechIdea.Beep.Fonts";

        /// <summary>
        /// Gets the assembly containing the embedded font resources.
        /// </summary>
        public static Assembly ResourceAssembly => typeof(Util).Assembly;

        private static readonly Lazy<string[]> _resourceNames = new(() =>
            ResourceAssembly.GetManifestResourceNames()
                .Where(n => n.StartsWith(BaseNamespace, StringComparison.Ordinal))
                .Where(n => n.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) || n.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
                .ToArray());

        private static readonly Lazy<Dictionary<string, string>> _byKey = new(() =>
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var full in _resourceNames.Value)
            {
                // Strip base namespace prefix
                var sub = full.Substring(BaseNamespace.Length + 1); // e.g. "Cairo.Cairo-Black.ttf"

                // Add by just file name (e.g. Cairo-Black.ttf)
                var fileOnly = GetFileOnly(sub);
                AddKey(dict, fileOnly, full);

                // Add by dot-qualified (folder.file)
                AddKey(dict, sub, full);

                // Add by folder/path style (e.g. Cairo/Cairo-Black.ttf)
                var slashKey = ToSlashKey(sub);
                AddKey(dict, slashKey, full);

                // Also allow backslash variant
                AddKey(dict, slashKey.Replace('/', '\\'), full);
            }
            return dict;
        });

        private static void AddKey(Dictionary<string, string> dict, string key, string full)
        {
            if (!string.IsNullOrWhiteSpace(key) && !dict.ContainsKey(key))
            {
                dict[key] = full;
            }
        }

        private static string GetFileOnly(string sub)
        {
            var idx = sub.LastIndexOf('.') + 1; // before extension dot
            // We want the file name with extension
            int sep = sub.LastIndexOf('.', sub.Length - 5); // find the dot before extension roughly
            int dirDot = sub.IndexOf('.');
            // Safer approach: split on '.' and rebuild last two parts
            var parts = sub.Split('.');
            if (parts.Length >= 2)
            {
                return parts[^2] + "." + parts[^1];
            }
            return sub;
        }

        private static string ToSlashKey(string sub)
        {
            // Convert first '.' to '/' but keep the filename dots intact
            int firstDot = sub.IndexOf('.');
            if (firstDot > 0)
            {
                return sub.Substring(0, firstDot) + "/" + sub.Substring(firstDot + 1);
            }
            return sub;
        }

        /// <summary>
        /// Returns all embedded font resource names (full manifest names).
        /// </summary>
        public static string[] GetAllResourceNames() => _resourceNames.Value;

        /// <summary>
        /// Returns all font file names (e.g., "Cairo-Black.ttf").
        /// </summary>
        public static string[] GetAllFileNames()
            => _resourceNames.Value
                .Select(r => r.Substring(BaseNamespace.Length + 1)) // e.g., Cairo.Cairo-Black.ttf
                .Select(GetFileOnly)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

        /// <summary>
        /// Checks if a font resource exists by full manifest name or by file/path key.
        /// </summary>
        public static bool Exists(string nameOrFile)
        {
            if (string.IsNullOrWhiteSpace(nameOrFile)) return false;
            if (nameOrFile.StartsWith(BaseNamespace, StringComparison.Ordinal))
            {
                return _resourceNames.Value.Contains(nameOrFile);
            }

            // Try direct dictionary keys
            if (_byKey.Value.ContainsKey(EnsureExtension(nameOrFile)))
                return true;

            // Try constructed manifest name
            return TryBuildCandidate(nameOrFile, out _);
        }

        /// <summary>
        /// Tries to get the full resource path from a file name or relative path.
        /// Accepted inputs: "Roboto/Roboto-Regular.ttf", "Roboto-Regular.ttf", dot qualified, or full manifest name.
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

            // Direct lookup by keys (filename, folder/path, dot-qualified)
            var key = EnsureExtension(nameOrFile);
            if (_byKey.Value.TryGetValue(key, out var full))
            {
                resourcePath = full;
                return true;
            }

            // Try .otf alternative if .ttf wasn't present
            if (!HasFontExtension(key))
            {
                var ttf = key + ".ttf";
                if (_byKey.Value.TryGetValue(ttf, out full))
                {
                    resourcePath = full;
                    return true;
                }
                var otf = key + ".otf";
                if (_byKey.Value.TryGetValue(otf, out full))
                {
                    resourcePath = full;
                    return true;
                }
            }

            // Try to construct a manifest path from a relative-like path
            if (TryBuildCandidate(key, out var candidate) && _resourceNames.Value.Contains(candidate))
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
            => TryGet(nameOrFile, out var path) ? path : null;

        /// <summary>
        /// Opens an embedded font as a stream by file name or full resource name.
        /// Returns null if not found.
        /// </summary>
        public static Stream? Open(string nameOrFile)
        {
            if (!TryGet(nameOrFile, out var full)) return null;
            return ResourceAssembly.GetManifestResourceStream(full);
        }

        private static bool TryBuildCandidate(string nameOrFile, out string candidate)
        {
            candidate = string.Empty;
            var normalized = nameOrFile.Replace('\\', '/');
            var ensured = EnsureExtension(normalized);

            // If it's like "Cairo/Cairo-Black.ttf" -> TheTechIdea.Beep.Fonts.Cairo.Cairo-Black.ttf
            if (ensured.Contains('/'))
            {
                var parts = ensured.Split('/');
                if (parts.Length == 2)
                {
                    candidate = $"{BaseNamespace}.{parts[0]}.{parts[1]}";
                    return true;
                }
            }

            // If it's dot-qualified already (e.g., Cairo.Cairo-Black.ttf)
            if (ensured.Contains('.') && !ensured.Contains('/'))
            {
                candidate = $"{BaseNamespace}.{ensured}";
                return true;
            }

            // If it's just filename (e.g., Cairo-Black.ttf), try to locate by scanning
            var file = ensured;
            var match = _resourceNames.Value.FirstOrDefault(r => r.EndsWith($".{file}", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(match))
            {
                candidate = match;
                return true;
            }

            return false;
        }

        private static bool HasFontExtension(string file)
            => file.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".otf", StringComparison.OrdinalIgnoreCase);

        private static string EnsureExtension(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            var trimmed = input.Trim();
            if (HasFontExtension(trimmed)) return trimmed;
            return trimmed + ".ttf"; // default to .ttf
        }

        // Shared helper for nested font categories
        private static string Require(string nameOrFile)
            => ToResourcePath(nameOrFile) ?? throw new InvalidOperationException($"Font resource not found: {nameOrFile}");

        // Strongly-typed curated families
        public static class Cairo
        {
            public static string Black => Require("Cairo/Cairo-Black.ttf");
            public static string Bold => Require("Cairo/Cairo-Bold.ttf");
            public static string ExtraBold => Require("Cairo/Cairo-ExtraBold.ttf");
            public static string ExtraLight => Require("Cairo/Cairo-ExtraLight.ttf");
            public static string Light => Require("Cairo/Cairo-Light.ttf");
            public static string Medium => Require("Cairo/Cairo-Medium.ttf");
            public static string Regular => Require("Cairo/Cairo-Regular.ttf");
            public static string SemiBold => Require("Cairo/Cairo-SemiBold.ttf");
        }

        public static class ComicNeue
        {
            public static string Bold => Require("Comic_Neue/ComicNeue-Bold.ttf");
            public static string BoldItalic => Require("Comic_Neue/ComicNeue-BoldItalic.ttf");
            public static string Italic => Require("Comic_Neue/ComicNeue-Italic.ttf");
            public static string Light => Require("Comic_Neue/ComicNeue-Light.ttf");
            public static string LightItalic => Require("Comic_Neue/ComicNeue-LightItalic.ttf");
            public static string Regular => Require("Comic_Neue/ComicNeue-Regular.ttf");
        }

        public static class Roboto
        {
            public static string Black => Require("Roboto/Roboto-Black.ttf");
            public static string BlackItalic => Require("Roboto/Roboto-BlackItalic.ttf");
            public static string Bold => Require("Roboto/Roboto-Bold.ttf");
            public static string BoldItalic => Require("Roboto/Roboto-BoldItalic.ttf");
            public static string ExtraBold => Require("Roboto/Roboto-ExtraBold.ttf");
            public static string ExtraBoldItalic => Require("Roboto/Roboto-ExtraBoldItalic.ttf");
            public static string ExtraLight => Require("Roboto/Roboto-ExtraLight.ttf");
            public static string ExtraLightItalic => Require("Roboto/Roboto-ExtraLightItalic.ttf");
            public static string Italic => Require("Roboto/Roboto-Italic.ttf");
            public static string Light => Require("Roboto/Roboto-Light.ttf");
            public static string LightItalic => Require("Roboto/Roboto-LightItalic.ttf");
            public static string Medium => Require("Roboto/Roboto-Medium.ttf");
            public static string MediumItalic => Require("Roboto/Roboto-MediumItalic.ttf");
            public static string Regular => Require("Roboto/Roboto-Regular.ttf");
            public static string SemiBold => Require("Roboto/Roboto-SemiBold.ttf");
            public static string SemiBoldItalic => Require("Roboto/Roboto-SemiBoldItalic.ttf");
            public static string Thin => Require("Roboto/Roboto-Thin.ttf");
            public static string ThinItalic => Require("Roboto/Roboto-ThinItalic.ttf");
        }

        public static class RobotoCondensed
        {
            public static string Black => Require("Roboto/Roboto_Condensed-Black.ttf");
            public static string BlackItalic => Require("Roboto/Roboto_Condensed-BlackItalic.ttf");
            public static string Bold => Require("Roboto/Roboto_Condensed-Bold.ttf");
            public static string BoldItalic => Require("Roboto/Roboto_Condensed-BoldItalic.ttf");
            public static string ExtraBold => Require("Roboto/Roboto_Condensed-ExtraBold.ttf");
            public static string ExtraBoldItalic => Require("Roboto/Roboto_Condensed-ExtraBoldItalic.ttf");
            public static string ExtraLight => Require("Roboto/Roboto_Condensed-ExtraLight.ttf");
            public static string ExtraLightItalic => Require("Roboto/Roboto_Condensed-ExtraLightItalic.ttf");
            public static string Italic => Require("Roboto/Roboto_Condensed-Italic.ttf");
            public static string Light => Require("Roboto/Roboto_Condensed-Light.ttf");
            public static string LightItalic => Require("Roboto/Roboto_Condensed-LightItalic.ttf");
            public static string Medium => Require("Roboto/Roboto_Condensed-Medium.ttf");
            public static string MediumItalic => Require("Roboto/Roboto_Condensed-MediumItalic.ttf");
            public static string Regular => Require("Roboto/Roboto_Condensed-Regular.ttf");
            public static string SemiBold => Require("Roboto/Roboto_Condensed-SemiBold.ttf");
            public static string SemiBoldItalic => Require("Roboto/Roboto_Condensed-SemiBoldItalic.ttf");
            public static string Thin => Require("Roboto/Roboto_Condensed-Thin.ttf");
            public static string ThinItalic => Require("Roboto/Roboto_Condensed-ThinItalic.ttf");
        }

        public static class RobotoSemiCondensed
        {
            public static string Black => Require("Roboto/Roboto_SemiCondensed-Black.ttf");
            public static string BlackItalic => Require("Roboto/Roboto_SemiCondensed-BlackItalic.ttf");
            public static string Bold => Require("Roboto/Roboto_SemiCondensed-Bold.ttf");
            public static string BoldItalic => Require("Roboto/Roboto_SemiCondensed-BoldItalic.ttf");
            public static string ExtraBold => Require("Roboto/Roboto_SemiCondensed-ExtraBold.ttf");
            public static string ExtraBoldItalic => Require("Roboto/Roboto_SemiCondensed-ExtraBoldItalic.ttf");
            public static string ExtraLight => Require("Roboto/Roboto_SemiCondensed-ExtraLight.ttf");
            public static string ExtraLightItalic => Require("Roboto/Roboto_SemiCondensed-ExtraLightItalic.ttf");
            public static string Italic => Require("Roboto/Roboto_SemiCondensed-Italic.ttf");
            public static string Light => Require("Roboto/Roboto_SemiCondensed-Light.ttf");
            public static string LightItalic => Require("Roboto/Roboto_SemiCondensed-LightItalic.ttf");
            public static string Medium => Require("Roboto/Roboto_SemiCondensed-Medium.ttf");
            public static string MediumItalic => Require("Roboto/Roboto_SemiCondensed-MediumItalic.ttf");
            public static string Regular => Require("Roboto/Roboto_SemiCondensed-Regular.ttf");
            public static string SemiBold => Require("Roboto/Roboto_SemiCondensed-SemiBold.ttf");
            public static string SemiBoldItalic => Require("Roboto/Roboto_SemiCondensed-SemiBoldItalic.ttf");
            public static string Thin => Require("Roboto/Roboto_SemiCondensed-Thin.ttf");
            public static string ThinItalic => Require("Roboto/Roboto_SemiCondensed-ThinItalic.ttf");
        }

        public static class Tajawal
        {
            public static string Black => Require("Tajawal/Tajawal-Black.ttf");
            public static string Bold => Require("Tajawal/Tajawal-Bold.ttf");
            public static string ExtraBold => Require("Tajawal/Tajawal-ExtraBold.ttf");
            public static string ExtraLight => Require("Tajawal/Tajawal-ExtraLight.ttf");
            public static string Light => Require("Tajawal/Tajawal-Light.ttf");
            public static string Medium => Require("Tajawal/Tajawal-Medium.ttf");
            public static string Regular => Require("Tajawal/Tajawal-Regular.ttf");
        }

        public static class JetBrainsMono
        {
            // Static styles only (variable removed)
            public static string Thin => Require("JetBrains_Mono/static/JetBrainsMono-Thin.ttf");
            public static string ThinItalic => Require("JetBrains_Mono/static/JetBrainsMono-ThinItalic.ttf");
            public static string ExtraLight => Require("JetBrains_Mono/static/JetBrainsMono-ExtraLight.ttf");
            public static string ExtraLightItalic => Require("JetBrains_Mono/static/JetBrainsMono-ExtraLightItalic.ttf");
            public static string Light => Require("JetBrains_Mono/static/JetBrainsMono-Light.ttf");
            public static string LightItalic => Require("JetBrains_Mono/static/JetBrainsMono-LightItalic.ttf");
            public static string Regular => Require("JetBrains_Mono/static/JetBrainsMono-Regular.ttf");
            public static string Italic => Require("JetBrains_Mono/static/JetBrainsMono-Italic.ttf");
            public static string Medium => Require("JetBrains_Mono/static/JetBrainsMono-Medium.ttf");
            public static string MediumItalic => Require("JetBrains_Mono/static/JetBrainsMono-MediumItalic.ttf");
            public static string SemiBold => Require("JetBrains_Mono/static/JetBrainsMono-SemiBold.ttf");
            public static string SemiBoldItalic => Require("JetBrains_Mono/static/JetBrainsMono-SemiBoldItalic.ttf");
            public static string Bold => Require("JetBrains_Mono/static/JetBrainsMono-Bold.ttf");
            public static string BoldItalic => Require("JetBrains_Mono/static/JetBrainsMono-BoldItalic.ttf");
            public static string ExtraBold => Require("JetBrains_Mono/static/JetBrainsMono-ExtraBold.ttf");
            public static string ExtraBoldItalic => Require("JetBrains_Mono/static/JetBrainsMono-ExtraBoldItalic.ttf");
        }

        public static class FiraCode
        {
            // Static styles only (variable removed)
            public static string Light => Require("Fira_Code/static/FiraCode-Light.ttf");
            public static string Regular => Require("Fira_Code/static/FiraCode-Regular.ttf");
            public static string Medium => Require("Fira_Code/static/FiraCode-Medium.ttf");
            public static string SemiBold => Require("Fira_Code/static/FiraCode-SemiBold.ttf");
            public static string Bold => Require("Fira_Code/static/FiraCode-Bold.ttf");
        }

        public static class NotoColorEmoji
        {
            public static string Regular => Require("Noto_Color_Emoji/NotoColorEmoji-Regular.ttf");
        }

        public static class Individual
        {
            public static string CaprasimoRegular => Require("Caprasimo-Regular.ttf");
            public static string Consolas => Require("consolas.ttf");
        }
    }
}
