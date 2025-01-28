using System.Web;  // For HttpUtility (in .NET Framework or .NET 6+ in the correct package)
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System;

namespace TheTechIdea.Beep.Shared
{

    public record RouteMatch
    {
        public string RouteName { get; init; }                // e.g., "CustomerOrders"
        public Dictionary<string, string> RouteParameters { get; init; } = new();
        public Dictionary<string, string> QueryParameters { get; init; } = new();
    }
    // If your C# version supports records
   
    public class RouteDefinition
    {
        public string Name { get; init; }
        public string Template { get; init; }

        public RouteDefinition(string name, string template)
        {
            Name = name;
            Template = template;
        }
    }

    public static class RouteParser
    {
        public static RouteMatch? Parse(string incomingRoute, IEnumerable<RouteDefinition> routeDefinitions)
        {
            // 1. Separate query string from path
            var (path, queryString) = SplitPathAndQuery(incomingRoute);

            // 2. Parse query parameters
            var queryParams = ParseQuery(queryString);

            // 3. Attempt to match the path to one of the known routes
            foreach (var routeDef in routeDefinitions)
            {
                var match = TryMatchRoute(path, routeDef);
                if (match != null)
                {
                    // Found a match; incorporate the query parameters
                    return match with
                    {
                        QueryParameters = queryParams
                    };
                }
            }

            // No match found
            return null;
        }

        /// <summary>
        /// Splits an incoming route string into (path, query).
        /// For example: "/customers/123/orders/999?sort=asc" -> ("/customers/123/orders/999", "sort=asc")
        /// </summary>
        private static (string path, string queryString) SplitPathAndQuery(string route)
        {
            // If the route is actually a full URL, you can do:
            // var uri = new Uri(route);
            // Then parse uri.AbsolutePath and uri.Query, etc.

            int questionMarkIndex = route.IndexOf('?');
            if (questionMarkIndex >= 0)
            {
                var pathPart = route.Substring(0, questionMarkIndex);
                var queryPart = route.Substring(questionMarkIndex + 1);
                return (pathPart, queryPart);
            }
            else
            {
                return (route, string.Empty);
            }
        }

        /// <summary>
        /// Converts "sort=asc&page=2" into a Dictionary{string,string}.
        /// </summary>
        private static Dictionary<string, string> ParseQuery(string queryString)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrEmpty(queryString))
            {
                // This uses System.Web.HttpUtility from .NET; in .NET Core / 6.0+, you might need to
                // add a reference to Microsoft.AspNetCore.WebUtilities or a relevant package.
                var parsed = HttpUtility.ParseQueryString(queryString);
                foreach (var key in parsed.AllKeys)
                {
                    if (key != null)
                    {
                        result[key] = parsed[key] ?? string.Empty;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Tries to match "/customers/123/orders/999" to a template like "/customers/{customerId}/orders/{orderId}".
        /// If it matches, returns a RouteMatch with extracted placeholders.
        /// Otherwise returns null.
        /// </summary>
        private static RouteMatch? TryMatchRoute(string path, RouteDefinition routeDef)
        {
            // 1. Split both the incoming path and the template by '/'
            var pathSegments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            var templateSegments = routeDef.Template.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Quick check: must have same segment count
            if (pathSegments.Length != templateSegments.Length)
                return null;

            var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // 2. Compare segments
            for (int i = 0; i < pathSegments.Length; i++)
            {
                var pathSegment = pathSegments[i];
                var templateSegment = templateSegments[i];

                if (IsPlaceholder(templateSegment))
                {
                    // e.g., "{customerId}" -> "customerId"
                    var key = templateSegment.Trim('{', '}');
                    parameters[key] = pathSegment;
                }
                else
                {
                    // Must match exactly (case-insensitive or not, depending on your needs)
                    if (!string.Equals(pathSegment, templateSegment, StringComparison.OrdinalIgnoreCase))
                    {
                        // Not a match, bail out
                        return null;
                    }
                }
            }

            return new RouteMatch
            {
                RouteName = routeDef.Name,
                RouteParameters = parameters
            };
        }

        public static string Build(RouteDefinition route, Dictionary<string, string> parameters)
        {
            var segments = route.Template.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            return result.ToString();
        }
        public static string Build(string route, Dictionary<string, string> parameters)
        {
            var segments = route.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            return result.ToString();
        }
        public static string Build(string route, Dictionary<string, string> parameters, Dictionary<string, string> queryParameters)
        {
            var segments = route.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            // Append query parameters
            if (queryParameters.Count > 0)
            {
                result.Append('?');
                foreach (var kvp in queryParameters)
                {
                    result.Append(kvp.Key);
                    result.Append('=');
                    result.Append(kvp.Value);
                    result.Append('&');
                }
                result.Length--; // Remove last '&'
            }
            return result.ToString();
        }
        public static string Build(RouteDefinition route, Dictionary<string, string> parameters, Dictionary<string, string> queryParameters)
        {
            var segments = route.Template.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            // Append query parameters
            if (queryParameters.Count > 0)
            {
                result.Append('?');
                foreach (var kvp in queryParameters)
                {
                    result.Append(kvp.Key);
                    result.Append('=');
                    result.Append(kvp.Value);
                    result.Append('&');
                }
                result.Length--; // Remove last '&'
            }
            return result.ToString();
        }
        public static string Build(string route, Dictionary<string, string> parameters, string queryParameters)
        {
            var segments = route.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            // Append query parameters
            if (!string.IsNullOrEmpty(queryParameters))
            {
                result.Append('?');
                result.Append(queryParameters);
            }
            return result.ToString();
        }
        public static string Build(RouteDefinition route, Dictionary<string, string> parameters, string queryParameters)
        {
            var segments = route.Template.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            // Append query parameters
            if (!string.IsNullOrEmpty(queryParameters))
            {
                result.Append('?');
                result.Append(queryParameters);
            }
            return result.ToString();
        }
        public static string Build(string route, Dictionary<string, string> parameters, Dictionary<string, string> queryParameters, string fragment)
        {
            var segments = route.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            // Append query parameters
            if (queryParameters.Count > 0)
            {
                result.Append('?');
                foreach (var kvp in queryParameters)
                {
                    result.Append(kvp.Key);
                    result.Append('=');
                    result.Append(kvp.Value);
                    result.Append('&');
                }
                result.Length--; // Remove last '&'
            }
            // Append fragment
            if (!string.IsNullOrEmpty(fragment))
            {
                result.Append('#');
                result.Append(fragment);
            }
            return result.ToString();
        }
        public static string Build(RouteDefinition route, Dictionary<string, string> parameters, Dictionary<string, string> queryParameters, string fragment)
        {
            var segments = route.Template.Split('/');
            var result = new StringBuilder();
            foreach (var segment in segments)
            {
                if (IsPlaceholder(segment))
                {
                    var key = segment.Trim('{', '}');
                    if (parameters.TryGetValue(key, out var value))
                    {
                        result.Append(value);
                    }
                    else
                    {
                        // Placeholder not found; you might want to throw an exception here
                        result.Append(segment);
                    }
                }
                else
                {
                    result.Append(segment);
                }
                result.Append('/');
            }
            // Remove trailing slash
            if (result.Length > 0)
            {
                result.Length--; // Remove last character
            }
            // Append query parameters
            if (queryParameters.Count > 0)
            {
                result.Append('?');
                foreach (var kvp in queryParameters)
                {
                    result.Append(kvp.Key);
                    result.Append('=');
                    result.Append(kvp.Value);
                    result.Append('&');
                }
                result.Length--; // Remove last '&'
            }
            // Append fragment
            if (!string.IsNullOrEmpty(fragment))
            {
                result.Append('#');
                result.Append(fragment);
            }
            return result.ToString();
        }
        private static bool IsPlaceholder(string segment)
            => segment.StartsWith("{") && segment.EndsWith("}");
    }

}
