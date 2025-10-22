using System;

namespace TheTechIdea.Beep.Icons
{
 internal static class UiIconsHelpers
 {
 /// <summary>
 /// Extracts the file name (e.g., "fi-tr-user.svg") from a full manifest resource name.
 /// </summary>
 public static string GetFileName(string resourceName, string baseNamespace)
 {
 if (string.IsNullOrEmpty(resourceName))
 return resourceName ?? string.Empty;

 // resourceName format: TheTechIdea.Beep.Icons.uiicons.<filename>.svg
 if (resourceName.StartsWith(baseNamespace + ".", StringComparison.Ordinal))
 {
 return resourceName.Substring(baseNamespace.Length +1);
 }

 // Fallback: last two segments like "<name>.svg"
 var parts = resourceName.Split('.');
 if (parts.Length >=2)
 {
 return parts[^2] + "." + parts[^1];
 }

 return resourceName;
 }

 /// <summary>
 /// Extracts the file name portion from a path or name (supports both slashes).
 /// </summary>
 public static string ExtractFileName(string nameOrPath)
 {
 if (string.IsNullOrEmpty(nameOrPath))
 return nameOrPath ?? string.Empty;

 var name = nameOrPath.Replace('\\', '/');
 var slash = name.LastIndexOf('/') +1;
 return slash >0 ? name.Substring(slash) : name;
 }

 /// <summary>
 /// Ensures the file name ends with the .svg extension (case-insensitive).
 /// </summary>
 public static string EnsureExtension(string file)
 {
 if (string.IsNullOrEmpty(file)) return file ?? string.Empty;
 return file.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) ? file : file + ".svg";
 }
 }
}
