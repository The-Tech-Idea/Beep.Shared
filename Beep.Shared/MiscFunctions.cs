
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TheTechIdea.Beep.Addin;
using TheTechIdea.Beep.ConfigUtil;
using TheTechIdea.Beep.DataBase;
using TheTechIdea.Beep.Vis.Modules;

namespace TheTechIdea.Beep.Shared
{
    public static class MiscFunctions

    { 
        public static object CreateObject(string typename)
        {
            object obj = null;
            try
            {
                Type type = Type.GetType(typename);
                if (type != null)
                {
                    obj = Activator.CreateInstance(type);
                }
            }
            catch (Exception ex)
            {
                obj = null;
                ////MiscFunctions.SendLog($"Error creating object of type {typename}: {ex.Message}");
            }
            return obj;
        }
            
       /// <summary>
      /// Helper that tries to load an assembly by path (LoadFrom) or 
      /// by full name (Load) depending on whether the string is a valid file path.
      /// </summary>
        public static Assembly LoadAssembly(string assemblyFullNameOrPath)
        {
            try
            {
                if (File.Exists(assemblyFullNameOrPath))
                {
                    // It's a file path
                    return Assembly.LoadFrom(assemblyFullNameOrPath);
                }
                else
                {
                    // Assume it's an assembly full name
                    return Assembly.Load(assemblyFullNameOrPath);
                }
            }
            catch
            {
                return null;
            }
        }
        private static bool isLogOn = true;
        public static  void SendLog(string message)
        {
            if(isLogOn)
            {
                Console.WriteLine(message);
                Debug.WriteLine(message);
                //  ////MiscFunctions.SendLog(message);
            }
          
        }
        public static string GetAssemblyName(string assemblyFullNameOrPath)
        {
            try
            {
                if (File.Exists(assemblyFullNameOrPath))
                {
                    // It's a file path
                    return AssemblyName.GetAssemblyName(assemblyFullNameOrPath).Name;
                }
                else
                {
                    // Assume it's an assembly full name
                    return Assembly.Load(assemblyFullNameOrPath).GetName().Name;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string GetRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomString(int length, string chars)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomString(int length, string chars, Random random)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetRandomString(int length, string chars, Random random, int seed)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
      
       public static Dictionary<string, object> ConvertPassedArgsToParameters(IPassedArgs passedArgs)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("AddinName", passedArgs.AddinName);
            parameters.Add("AddinType", passedArgs.AddinType);
            parameters.Add("Flag", passedArgs.Flag);
            parameters.Add("Cancel", passedArgs.Cancel);
            parameters.Add("SentData", passedArgs.SentData);
            parameters.Add("SentType", passedArgs.SentType);
            parameters.Add("ReturnData", passedArgs.ReturnData);
            parameters.Add("ReturnType", passedArgs.ReturnType);
            parameters.Add("Category", passedArgs.Category);
            parameters.Add("CurrentEntity", passedArgs.CurrentEntity);
            parameters.Add("DatasourceName", passedArgs.DatasourceName);
            parameters.Add("ViewName", passedArgs.DMView.ViewName);
            parameters.Add("Entities", passedArgs.Entities);
            parameters.Add("EntitiesNames", passedArgs.EntitiesNames);
            parameters.Add("ErrorCode", passedArgs.ErrorCode);
            parameters.Add("ErrorObject", passedArgs.ErrorObject);
            parameters.Add("EventType", passedArgs.EventType);
            parameters.Add("Id", passedArgs.Id);
            parameters.Add("Messege", passedArgs.Messege);
            parameters.Add("ImagePath", passedArgs.ImagePath);
            parameters.Add("IsError", passedArgs.IsError);
            parameters.Add("ObjectName", passedArgs.ObjectName);
            parameters.Add("ObjectType", passedArgs.ObjectType);
            parameters.Add("Objects", passedArgs.Objects);
            parameters.Add("ParameterDate1", passedArgs.ParameterDate1);
            parameters.Add("ParameterDate2", passedArgs.ParameterDate2);
            parameters.Add("ParameterDate3", passedArgs.ParameterDate3);
            parameters.Add("ParameterInt1", passedArgs.ParameterInt1);
            parameters.Add("ParameterInt2", passedArgs.ParameterInt2);
            parameters.Add("ParameterInt3", passedArgs.ParameterInt3);
            parameters.Add("ParameterString1", passedArgs.ParameterString1);
            parameters.Add("ParameterString2", passedArgs.ParameterString2);
            parameters.Add("ParameterString3", passedArgs.ParameterString3);
            parameters.Add("Parameterdouble1", passedArgs.Parameterdouble1);
            parameters.Add("Parameterdouble2", passedArgs.Parameterdouble2);
            parameters.Add("Parameterdouble3", passedArgs.Parameterdouble3);
            parameters.Add("Progress", passedArgs.Progress);
            parameters.Add("Timestamp", passedArgs.Timestamp);
            parameters.Add("Title", passedArgs.Title);
            return parameters;
        }
        public static IPassedArgs ConvertParametersToPassArgs(Dictionary<string, object> parameters)
        {
            IPassedArgs passedArgs = new PassedArgs();
            passedArgs.AddinName = parameters["AddinName"].ToString();
            passedArgs.AddinType = parameters["AddinType"].ToString();
            passedArgs.Flag = Enum.Parse<Errors>(parameters["Flag"].ToString());
            passedArgs.Cancel = (bool)parameters["Cancel"];
            passedArgs.SentData = parameters["SentData"];
            passedArgs.SentType = (Type)parameters["SentType"];
            passedArgs.ReturnData = parameters["ReturnData"];
            passedArgs.ReturnType = (Type)parameters["ReturnType"];
            passedArgs.Category = parameters["Category"].ToString();
            passedArgs.CurrentEntity = parameters["CurrentEntity"].ToString();
            passedArgs.DatasourceName = parameters["DatasourceName"].ToString();
            passedArgs.DMView.ViewName = parameters["ViewName"].ToString();
            passedArgs.Entities = (List<EntityStructure>)parameters["Entities"];
            passedArgs.EntitiesNames = (List<string>)parameters["EntitiesNames"];
            passedArgs.ErrorCode = parameters["ErrorCode"].ToString();
            passedArgs.ErrorObject = (IErrorsInfo)parameters["ErrorObject"];
            passedArgs.EventType = parameters["EventType"].ToString();
            passedArgs.Id = (int)parameters["Id"];
            passedArgs.Messege = parameters["Messege"].ToString();
            passedArgs.ImagePath = parameters["ImagePath"].ToString();
            passedArgs.IsError = (bool)parameters["IsError"];
            passedArgs.ObjectName = parameters["ObjectName"].ToString();
            passedArgs.ObjectType = parameters["ObjectType"].ToString();
            passedArgs.Objects = (List<ObjectItem>)parameters["Objects"];
            passedArgs.ParameterDate1 = (DateTime)parameters["ParameterDate1"];
            passedArgs.ParameterDate2 = (DateTime)parameters["ParameterDate2"];
            passedArgs.ParameterDate3 = (DateTime)parameters["ParameterDate3"];
            passedArgs.ParameterInt1 = (int)parameters["ParameterInt1"];
            passedArgs.ParameterInt2 = (int)parameters["ParameterInt2"];
            passedArgs.ParameterInt3 = (int)parameters["ParameterInt3"];
            passedArgs.ParameterString1 = parameters["ParameterString1"].ToString();
            passedArgs.ParameterString2 = parameters["ParameterString2"].ToString();
            passedArgs.ParameterString3 = parameters["ParameterString3"].ToString();
            passedArgs.Parameterdouble1 = (double)parameters["Parameterdouble1"];
            passedArgs.Parameterdouble2 = (double)parameters["Parameterdouble2"];
            passedArgs.Parameterdouble3 = (double)parameters["Parameterdouble3"];
            passedArgs.Progress = (int)parameters["Progress"];
            passedArgs.Timestamp = (DateTime)parameters["Timestamp"];
            passedArgs.Title = parameters["Title"].ToString();
            return passedArgs;

        }

        // A set of known acronyms (uppercase).
        // If a token fully matches (case-insensitive), we'll keep it uppercase 
        // rather than splitting or spacing it out.
        private static readonly HashSet<string> knownAcronyms = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "AC",       // Air Conditioning or Alternating Current
        "AJAX",
        "AMD",
        "API",
        "ASCII",
        "ASP",
        "BIOS",
        "CSV",
        "CPU",
        "DB",
        "DHCP",
        "DOB",
        "DNS",
        "DOC",
        "EXIF",
        "FTP",
        "FHIR",
        "GPU",
        "GUID",
        "HEX",
        "HR",
        "HTML",
        "HTTP",
        "HTTPS",
        "HUD",
        "I/O",
        "ID",
        "IP",
        "ISO",
        "JSON",
        "JDBC",
        "JPEG",
        "JPG",
        "JWT",
        "KPI",
        "LAN",
        "MAC",
        "MVC",
        "MVP",
        "MVVM",
        "NET",      // Representing ".NET" (cannot include a dot in a token easily)
        "ODBC",
        "OLE",
        "OS",
        "PDF",
        "PNG",
        "RAM",
        "REST",
        "RPC",
        "SHA",
        "SMTP",
        "SNMP",
        "SOAP",
        "SQL",
        "SSAS",
        "SSIS",
        "SSN",
        "SSRS",
        "SSL",
        "SSH",
        "SSO",
        "TLS",
        "TFS",
        "UID",
        "UI",
        "URI",
        "URL",
        "USB",
        "UTF8",
        "UX",
        "VPN",
        "WCF",
        "XML"
        // Add more as needed...
    };

        /// <summary>
        /// Creates a more user-friendly caption from a raw field name.
        /// Examples:
        ///   "FirstName" -> "First Name", 
        ///   "Contact_Phone_Number" -> "Contact Phone Number",
        ///   "DOB" -> "DOB", 
        ///   "SQLVersion" -> "SQL Version"
        /// </summary>
        public static string CreateCaptionFromFieldName(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return string.Empty;

            // 1) Replace underscores with spaces
           string  FieldName = fieldName.Replace('_', ' ');

            // 2) Split on spaces so we can handle each "word" or "token" individually.
            //    E.g. "Contact Phone DOB" -> ["Contact", "Phone", "DOB"]
            string[] tokens = fieldName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                // Convert the token to uppercase for checking against knownAcronyms
                string upperToken = token.ToUpperInvariant();

                // 3) If the entire token is a known acronym, keep it uppercase
                //    (avoiding splitting it into letters).
                if (knownAcronyms.Contains(upperToken))
                {
                    tokens[i] = upperToken;
                }
                else
                {
                    // 4) Otherwise, insert spaces before uppercase letters to handle CamelCase/PascalCase.
                    //    e.g., "FirstName" -> "First Name".
                    tokens[i] = InsertSpacesBeforeUpper(token);
                }
            }

            // 5) Re-join tokens with a space and return the final caption.
            return string.Join(" ", tokens).Trim();
        }

        /// <summary>
        /// Inserts a space before uppercase letters in a token,
        /// unless the previous letter is also uppercase.
        /// e.g., "FirstName" -> "First Name", "FullAddress" -> "Full Address"
        /// </summary>
        private static string InsertSpacesBeforeUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var sb = new StringBuilder();
            sb.Append(str[0]); // Start with the first character.

            for (int i = 1; i < str.Length; i++)
            {
                char currentChar = str[i];
                char previousChar = sb[sb.Length - 1];

                // If current char is uppercase, and the previous is not uppercase/space, insert a space.
                // So "VersionNumber" -> "Version Number". 
                // "DOB" remains "D O B" unless recognized as an acronym above.
                if (char.IsUpper(currentChar)
                    && !char.IsWhiteSpace(previousChar)
                    && !char.IsUpper(previousChar))
                {
                    sb.Append(' ');
                }
                sb.Append(currentChar);
            }

            return sb.ToString();
        }
        /// <summary>
        /// Converts a value to the expected property type.
        /// Handles cases where numbers need to be converted between types safely.
        /// </summary>
        // Cache conversion functions for common types
        private static readonly Dictionary<Type, Func<string, object>> TypeConverters = new Dictionary<Type, Func<string, object>>
    {
        { typeof(bool), s => bool.TryParse(s, out bool result) ? result : false },
        { typeof(char), s => s.Length > 0 ? s[0] : '\0' },
        { typeof(string), s => s }, // Empty string is valid for string
        { typeof(int), s => int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out int result) ? result : 0 },
        { typeof(long), s => long.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out long result) ? result : 0L },
        { typeof(float), s => float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out float result) ? result : 0f },
        { typeof(double), s => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0d },
        { typeof(decimal), s => decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result) ? result : 0m },
        { typeof(DateTime), s => DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ? result : DateTime.MinValue }
    };

        // Cache default values for common value types
        private static readonly Dictionary<Type, object> DefaultValues = new Dictionary<Type, object>
    {
        { typeof(bool), false },
        { typeof(char), '\0' },
        { typeof(int), 0 },
        { typeof(long), 0L },
        { typeof(float), 0f },
        { typeof(double), 0d },
        { typeof(decimal), 0m },
        { typeof(DateTime), DateTime.MinValue }
    };

        public static object ConvertValueToPropertyType(Type targetType, object value)
        {
            // Handle null or DBNull
            if (value == null || value == DBNull.Value)
            {
                return targetType.IsValueType ? GetDefaultValue(targetType) : null;
            }

            // Get string representation once
            string stringValue = value.ToString();

            // Explicitly handle empty string
            if (stringValue == "") // Direct comparison for empty string
            {
                if (targetType == typeof(string))
                    return string.Empty; // Valid case for string
                //Debug.WriteLine($"ConvertValueToPropertyType: Empty string for type {targetType}, returning default");
                return targetType.IsValueType ? GetDefaultValue(targetType) : null;
            }

            try
            {
                // Handle nullable types
                Type underlyingType = Nullable.GetUnderlyingType(targetType);
                if (underlyingType != null)
                {
                    return ConvertValueToPropertyType(underlyingType, value); // Recurse for nullable underlying type
                }

                // Fast path for exact type match
                if (value.GetType() == targetType)
                {
                    return value;
                }

                // Use cached converter if available
                if (TypeConverters.TryGetValue(targetType, out var converter))
                {
                    return converter(stringValue);
                }

                // Enum handling
                if (targetType.IsEnum)
                {
                    if (Enum.TryParse(targetType, stringValue, true, out object enumResult))
                        return enumResult;
                   ////MiscFunctions.SendLog($"ConvertValueToPropertyType: Invalid enum value '{stringValue}' for {targetType}, returning default");
                    return GetDefaultValue(targetType);
                }

                // Fallback to Convert.ChangeType
                return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
               ////MiscFunctions.SendLog($"ConvertValueToPropertyType Format Error: TargetType={targetType}, Value='{stringValue}', Error={ex.Message}");
                return targetType.IsValueType ? GetDefaultValue(targetType) : null;
            }
            catch (ArgumentException ex)
            {
               ////MiscFunctions.SendLog($"ConvertValueToPropertyType Argument Error: TargetType={targetType}, Value='{stringValue}', Error={ex.Message}");
                return targetType.IsValueType ? GetDefaultValue(targetType) : null;
            }
            catch (Exception ex)
            {
               ////MiscFunctions.SendLog($"ConvertValueToPropertyType Unexpected Error: TargetType={targetType}, Value='{stringValue}', Error={ex.Message}");
                return targetType.IsValueType ? GetDefaultValue(targetType) : null;
            }
        }

        private static object GetDefaultValue(Type targetType)
        {
            if (DefaultValues.TryGetValue(targetType, out object defaultValue))
            {
                return defaultValue;
            }
            // Fallback for unhandled value types (rare)
            return Activator.CreateInstance(targetType);
        }
        public static void AddLogMessage(string Source, string Message, DateTime Time, int ErrorCode, string ObjectName, Errors ErrorType)
        {
          SendLog($"{Source} : {Message} : {Time} : {ErrorCode} : {ObjectName} : {ErrorType}");
        }
    }
}
