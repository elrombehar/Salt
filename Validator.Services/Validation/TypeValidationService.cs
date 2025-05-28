using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Validator.Services.Validation
{
    public class TypeValidationService : ITypeValidationService
    {
        #region RegEx Patterns
        private static readonly Regex EmailValidationRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
        private static readonly Regex UuidValidationRegex = new(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", RegexOptions.Compiled);
        private static readonly Regex AuthTokenValidationRegex = new(@"^Bearer [a-zA-Z0-9]+$", RegexOptions.Compiled);
        private static readonly Regex DateValidationRegex = new(@"^\d{2}-\d{2}-\d{4}$", RegexOptions.Compiled);
        #endregion RegEx Patterns


        #region Implementations
        public bool ValidateType(object? value, string type)
        {
            if (value == null) return false;

            try
            {
                return type.ToLower() switch
                {
                    "int" => IsInt(value),
                    "string" => IsString(value),
                    "boolean" => IsBool(value),
                    "list" => IsList(value),
                    "date" => IsDate(value),
                    "email" => IsEmail(value),
                    "uuid" => IsUuid(value),
                    "auth-token" => IsAuthToken(value),
                    _ => false
                };
            }
            catch
            {
                return false;
            }
        }
        #endregion Implementations

        #region Type Validators
        private static bool IsInt(object value)
        {
            if (value is JToken jToken)
            {
                return jToken.Type == JTokenType.Integer && jToken.Value<int?>() != null;
            }

            // Handle direct values
            return value is int || value is long ||
                   (value is string str && int.TryParse(str, out _));
        }

        private static bool IsString(object value)
        {
            if (value is JToken jToken)
            {
                return jToken.Type == JTokenType.String;
            }

            return value is string;

        }

        private static bool IsBool(object value)
        {
            if (value is JToken jToken)
            {
                return jToken.Type == JTokenType.Boolean;
            }

            return value is bool ||
                   (value is string str && bool.TryParse(str, out _));
        }

        private static bool IsList(object value)
        {
            if (value is JToken jToken)
            {
                return jToken.Type == JTokenType.Array;
            }

            return value is System.Collections.IEnumerable && !(value is string);
        }

        private static bool IsDate(object value)
        {
            var stringValue = GetStringValue(value);
            if (stringValue == null) return false;

            return DateValidationRegex.IsMatch(stringValue) &&
                   DateTime.TryParseExact(stringValue, "dd-MM-yyyy",
                       CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private static bool IsEmail(object value)
        {
            var stringValue = GetStringValue(value);
            return stringValue != null && EmailValidationRegex.IsMatch(stringValue);
        }

        private static bool IsUuid(object value)
        {
            var stringValue = GetStringValue(value);
            return stringValue != null && UuidValidationRegex.IsMatch(stringValue);
        }

        private static bool IsAuthToken(object value)
        {
            var stringValue = GetStringValue(value);
            var isValid = stringValue != null && AuthTokenValidationRegex.IsMatch(stringValue);
            return isValid;
        }
        #endregion Type Validators

        #region Helpers

        private static string? GetStringValue(object value)
        {
            if (value is JToken jToken && jToken.Type == JTokenType.String)
            {
                return jToken.Value<string>();
            }

            return value as string;
        }

        #endregion Helpers
    }
}
