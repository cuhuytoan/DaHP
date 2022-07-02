using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace CMS.Data.ValidationCustomize
{
    internal sealed class CompareDateAttribute : ValidationAttribute
    {
        public string compareToDateTimeProperty;
        /// <summary>
        /// Greater & Less
        /// </summary>
        public string operatorValue;
        public CompareDateAttribute(string compareToPropertyName, string operatorValue)
        {
            this.compareToDateTimeProperty = compareToPropertyName;
            this.operatorValue = operatorValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationObject = validationContext.ObjectInstance;
            PropertyInfo propertyInfo = validationObject.GetType().GetProperty(compareToDateTimeProperty);

            var currentValue = (DateTime?)value;
            var compareToValue = (DateTime?)propertyInfo?.GetValue(validationObject);
            if(operatorValue == "Greater")
            {
                return currentValue < compareToValue ? ValidationResult.Success : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            }
            
            return currentValue > compareToValue ? ValidationResult.Success : new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
           

            
        }
    }
}
