using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CMS.Data.ValidationCustomize
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal sealed class PhoneNumberAttribute : ValidationAttribute
    {      

        public override bool IsValid(object value)
        {
            var inputText = (String)value;
            bool result = true;
            if (!CMS.Common.Utils.IsPhoneNumber(inputText))
            {
                result = false;
            }
            return result;
        }

        

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
             ErrorMessageString);
        }
    }
}