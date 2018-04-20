using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PVT.Money.Shell.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Attributes
{
    public class LoginValidateAttribute : ValidationAttribute, IClientModelValidator
    {
        private int _minLength;
        private int _maxLength;

        public LoginValidateAttribute(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Type objectType = context.ObjectType;
            ICollection<PropertyInfo> properties = objectType.GetProperties();

            PropertyInfo pInfo = null;

            foreach (PropertyInfo p in properties)
            {
                if( p.CustomAttributes.Any(ca => ca.AttributeType == typeof(LoginValidateAttribute)))
                {
                    pInfo = p;
                    break;
                }
            }

            //PropertyInfo pInfo = objectType.GetProperties().FirstOrDefault(p => p.Name == "Login");

            if (pInfo == null)
                throw new InvalidOperationException($"Wrong `LoginValidateAttribute` usage: there is no `Login` property in '{objectType.Name}' model.");
            if (pInfo.PropertyType != typeof(string))
                throw new InvalidOperationException($"Wrong `LoginValidateAttribute` usage: `Login` property type isn`t string.");

            string login = (string)pInfo.GetValue(context.ObjectInstance);

            if (login == null)
            {
                return new ValidationResult($"The login field is required");
            }
            if (login.Length > _maxLength)
            {
                return new ValidationResult($"The login length hasn`t to be greater than {_maxLength} characters");
            }
            if (login.Length < _minLength)
            {
                return new ValidationResult($"The login length has to be {_minLength} or more characters");
            }
            if (!Regex.IsMatch(login, "^[a-zA-Z0-9].*[a-zA-Z0-9]$"))
            {
                return new ValidationResult("The login has to start and end with alphanumeric characters");
            }
            if (!Regex.IsMatch(login, "^[a-zA-Z0-9_ -]+$"))
            {
                return new ValidationResult("The login has to contain alphanumeric, space, '-' or '_' characters only");
            }
            if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+(?:[_ -]?[a-zA-Z0-9])*$"))
            {
                return new ValidationResult("Special characters in login have to be followed by an alphanumeric characters");
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-LoginValidateMinlength"] = "The login length has to be 8 or more characters";
            context.Attributes["data-val-LoginValidateMaxlength"] = "The login length hasn`t to be greater than 16 characters";
            context.Attributes["data-val-LoginValidateStart"] = "The login has to start and end with alphanumeric characters";
            context.Attributes["data-val-LoginValidateAllowedCharacters"] = "The login has to contain alphanumeric, space, '-' or '_' characters only";
            context.Attributes["data-val-LoginValidateSpecialCharacters"] = "Special characters in login have to be followed by an alphanumeric characters";
        }
    }
}
