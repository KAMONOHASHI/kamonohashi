using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.Util
{
    public sealed class CustomValidationAttribute : RegularExpressionAttribute
    {
        public bool IsAlphanumeric { get; }
        public string PropertyName { get; }

        /// <summary>
        /// 半角英数字（とハイフンなど）のみを許可する入力規則
        /// </summary>
        public CustomValidationAttribute(bool isAlphanumeric, [CallerMemberName] string propertyName = null) : base(GetExpression(isAlphanumeric))
        {
            this.IsAlphanumeric = isAlphanumeric;
            this.PropertyName = propertyName;
            ErrorMessage = GetErrorMessage();
        }

        private static string GetExpression(bool asName)
        {
            if (asName) {
                return "^[a-z]([-a-z0-9]*[a-z0-9])?$";
            }
            else {
                return "(^$)|(^[a-zA-Z0-9]([-_.a-zA-Z0-9]*[a-zA-Z0-9])?$)";
            }
        }

        private string GetErrorMessage()
        {
            if (IsAlphanumeric)
            {
                return $"'{PropertyName}' must consist of lower case alphanumeric characters or dashes(-), start with an alphabetic character, and end with an alphanumeric character.";
            }
            else
            {
                return $"'{PropertyName}' must be empty or begin and end with an alphanumeric character with dashes(-), underscores(_), dots(.), and alphanumerics between";
            }
        }
    }
}
