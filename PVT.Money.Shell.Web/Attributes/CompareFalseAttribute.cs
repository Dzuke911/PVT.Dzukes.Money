using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Attributes
{
    public class CompareFalseAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _propName;

        public CompareFalseAttribute(string propName)
        {
            _propName = propName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Type objectType = context.ObjectType;
            ICollection<PropertyInfo> properties = objectType.GetProperties();

            PropertyInfo pInfo = context.ObjectType.GetProperty(_propName);

            string compareValue = (string)pInfo.GetValue(context.ObjectInstance);

            if (compareValue == (string)value)
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-CompareProperty"] = _propName; 
            context.Attributes["data-val-CompareFalseErrorMessage"] = ErrorMessage;
        }
    }
}
