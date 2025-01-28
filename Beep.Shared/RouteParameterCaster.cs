using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheTechIdea.Beep.Shared
{
    public class RouteParameterCaster
    {
        // Maps parameter name -> parse function
        // e.g., "customerId" -> s => int.Parse(s)
        private Dictionary<string, Func<string, object>> _parsers
            = new Dictionary<string, Func<string, object>>(StringComparer.OrdinalIgnoreCase);

        // Optionally store these if you want typed constraints
        // parameterName -> expected .NET type
        private Dictionary<string, Type> _expectedTypes
            = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void RegisterParser(string paramName, Func<string, object> parser)
        {
            _parsers[paramName] = parser;
        }

        public void RegisterExpectedType(string paramName, Type type)
        {
            _expectedTypes[paramName] = type;
        }

        /// <summary>
        /// Takes a raw dictionary of string->string route params
        /// and returns a dictionary of string->object with typed values.
        /// If a parameter has a registered parser or type, tries to parse it.
        /// Otherwise leaves it as a string.
        /// </summary>
        public Dictionary<string, object> CastParameters(Dictionary<string, string> rawParams)
        {
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var kvp in rawParams)
            {
                var key = kvp.Key;
                var value = kvp.Value;

                // Try custom parser first
                if (_parsers.TryGetValue(key, out var parser))
                {
                    try
                    {
                        result[key] = parser(value);
                    }
                    catch (Exception ex)
                    {
                        // Decide how to handle parse failure:
                        // log, throw, or store an error
                        throw new ArgumentException($"Failed to parse parameter '{key}' with value '{value}'.", ex);
                    }
                }
                else if (_expectedTypes.TryGetValue(key, out var type))
                {
                    // Use a fallback parsing approach
                    object parsed;
                    if (type == typeof(int))
                    {
                        if (!int.TryParse(value, out var intVal))
                            throw new ArgumentException($"Parameter '{key}' with value '{value}' is not a valid int.");
                        parsed = intVal;
                    }
                    else if (type == typeof(DateTime))
                    {
                        if (!DateTime.TryParse(value, out var dtVal))
                            throw new ArgumentException($"Parameter '{key}' with value '{value}' is not a valid DateTime.");
                        parsed = dtVal;
                    }
                    else if (type == typeof(Guid))
                    {
                        if (!Guid.TryParse(value, out var guidVal))
                            throw new ArgumentException($"Parameter '{key}' with value '{value}' is not a valid GUID.");
                        parsed = guidVal;
                    }
                    else
                    {
                        // fallback: keep as string or handle more types
                        parsed = value;
                    }
                    result[key] = parsed;
                }
                else
                {
                    // No parser or expected type => keep as string
                    result[key] = value;
                }
            }

            return result;
        }
    }
}
//var caster = new RouteParameterCaster();

//// Option 1: Provide a custom parser function
//caster.RegisterParser("customerId", s => int.Parse(s));
//caster.RegisterParser("orderId", s => int.Parse(s));

//// Option 2: Register an expected type for fallback parsing
//caster.RegisterExpectedType("startDate", typeof(DateTime));
//caster.RegisterExpectedType("guidId", typeof(Guid));
//var rawParams = new Dictionary<string, string>
//{
//    ["customerId"] = "123",
//    ["orderId"] = "999",
//    ["startDate"] = "2025-01-15",
//    ["otherParam"] = "abc"
//};

//var typedParams = caster.CastParameters(rawParams);

//// typedParams["customerId"] -> (object) 123 (an int)
//// typedParams["orderId"]    -> (object) 999 (int)
//// typedParams["startDate"]  -> (object) 2025-01-15 00:00:00 (DateTime)
//// typedParams["otherParam"] -> (object) "abc" (still a string)
