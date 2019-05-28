using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nssol.Platypus.Controllers.Util
{
    public sealed class ValidInputAsTagAttribute : RegularExpressionAttribute
    {
        public string PropertyName { get; }
        private const string ExpStr = "^[^,!=<>][^,]*$";

        public ValidInputAsTagAttribute([CallerMemberName] string propertyName = null) : base(ExpStr)
        {
            PropertyName = propertyName;
            ErrorMessage = "'{0}' must NOT consist of comma(,) and NOT start with following 4 marks (!,=,<,>).";
        }

        public override bool IsValid(object value)
        {
            if(value is IEnumerable<string>)
            {
                foreach(string str in value as IEnumerable<string>)
                {
                    if (base.IsValid(str) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return base.IsValid(value);
            }
        }
    }
}