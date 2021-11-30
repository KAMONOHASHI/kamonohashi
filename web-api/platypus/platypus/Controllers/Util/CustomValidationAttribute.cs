using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Nssol.Platypus.Controllers.Util
{
    public sealed class CustomValidationAttribute : RegularExpressionAttribute
    {
        public CustomValidationType CustomValidationType { get; }
        public string PropertyName { get; }

        /// <summary>
        /// 半角英数字（とハイフンなど）のみを許可する入力規則
        /// </summary>
        public CustomValidationAttribute(CustomValidationType customValidationType, [CallerMemberName] string propertyName = null) : base(GetExpression(customValidationType))
        {
            this.PropertyName = propertyName;
            this.CustomValidationType = customValidationType;
            ErrorMessage = $"'{propertyName}' {GetErrorMessage()}";
        }

        private static string GetExpression(CustomValidationType customValidationType)
        {
            switch (customValidationType)
            {
                case CustomValidationType.Alphanumeric:
                    return "^[a-z]([-a-z0-9]*[a-z0-9])?$";
                case CustomValidationType.Fqdn:
                    return "(^[a-zA-Z0-9]([-_.a-zA-Z0-9]*[a-zA-Z0-9])?$)";
                case CustomValidationType.Email:
                    return "(^[a-zA-Z0-9]([-_.@a-zA-Z0-9]*[a-zA-Z0-9])?$)";
                default:
                    throw new ArgumentException($"Unknown valication type {customValidationType}");
            }
        }

        private string GetErrorMessage()
        {
            switch (CustomValidationType)
            {
                case CustomValidationType.Alphanumeric:
                    return "must consist of lower case alphanumeric characters or dashes(-), start with an alphabetic character, and end with an alphanumeric character.";
                case CustomValidationType.Fqdn:
                    return "must begin and end with an alphanumeric character with dashes(-), underscores(_), dots(.), and alphanumerics between";
                case CustomValidationType.Email:
                    return "must begin and end with an alphanumeric character with dashes(-), underscores(_), dots(.), at(@), and alphanumerics between";
                default:
                    throw new ArgumentException($"Unknown valication type {CustomValidationType}");
            }
        }
    }
}
