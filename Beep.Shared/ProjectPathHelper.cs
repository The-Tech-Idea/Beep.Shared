using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace TheTechIdea.Beep.Shared;
public static class ProjectPathHelper
{
    /// <summary>
    /// Retrieves the referencing project path during design time or runtime.
    /// </summary>
    public static string GetProjectPath(IServiceProvider serviceProvider = null)
    {
        // Attempt to get the project path during design time using IServiceProvider
      

        // Fallback to runtime methods
        try
        {
            return GetCallingProjectPath();
        }
        catch
        {
            try
            {
                return GetProjectPathFromCsproj();
            }
            catch
            {
                throw new Exception("Unable to determine the referencing project's path.");
            }
        }
    }

    /// <summary>
    /// Retrieves the calling project's path using the stack trace.
    /// </summary>
    private static string GetCallingProjectPath()
    {
        var stackTrace = new StackTrace();
        foreach (var frame in stackTrace.GetFrames())
        {
            var method = frame.GetMethod();
            var assembly = method.DeclaringType?.Assembly;

            if (assembly != null && !assembly.FullName.Contains("Design") && !assembly.FullName.Contains("System"))
            {
                return Path.GetDirectoryName(assembly.Location);
            }
        }

        throw new Exception("Unable to determine the calling project path.");
    }

    /// <summary>
    /// Retrieves the project path by locating the nearest .csproj file.
    /// </summary>
    private static string GetProjectPathFromCsproj()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);

        while (directoryInfo != null)
        {
            var csprojFiles = directoryInfo.GetFiles("*.csproj");
            if (csprojFiles.Length > 0)
            {
                return directoryInfo.FullName;
            }

            directoryInfo = directoryInfo.Parent;
        }

        throw new Exception("No .csproj file found in the directory hierarchy.");
    }
   

   
    private static string GetProjectPath(string GetMyPath)
    {

        string projectPath = null;
        string fullPath = Path.GetDirectoryName(Path.GetFullPath(GetMyPath));

        DirectoryInfo directory = new DirectoryInfo(fullPath);
        // Runtime path handling
        projectPath = directory.Parent?.FullName;
       

        return projectPath;
    }
    public static string GetMyPath([CallerFilePath] string from = null)
    {
        return GetProjectPath(from);

    }
}
