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
    public class PasswordValidateAttribute : ValidationAttribute, IClientModelValidator
    {
        private int _minLength;
        private int _maxLength;

        public PasswordValidateAttribute(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {

            Type objectType = context.ObjectType;

            PropertyInfo pInfo = objectType.GetProperty(context.MemberName);

            string password = (string)pInfo.GetValue(context.ObjectInstance);


            if (password.Length > _maxLength)
            {
                return new ValidationResult($"The password length hasn`t to be greater than {_maxLength} characters");
            }
            if (password.Length < _minLength)
            {
                return new ValidationResult($"The password length hasn`t to be less than {_minLength} characters");
            }
            if (!Regex.IsMatch(password, "^[a-zA-Z0-9]+$"))
            {
                return new ValidationResult("The password has to contain alphanumeric characters only");
            }
            if (!Regex.IsMatch(password, "^.*[a-z].*$"))
            {
                return new ValidationResult("The password has to contain at least one lowercase character");
            }
            if (!Regex.IsMatch(password, "^.*[A-Z].*$"))
            {
                return new ValidationResult("The password has to contain at least one uppercase character");
            }
            if (!Regex.IsMatch(password, "^.*[0-9].*$"))
            {
                return new ValidationResult("The password has to contain at least one numeric character");
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
            context.Attributes["data-val-PasswordValidateMinlength"] = "The password length has to be 8 or more characters";
            context.Attributes["data-val-PasswordValidateMaxlength"] = "The password length hasn`t to be greater than 16 characters";
            context.Attributes["data-val-PasswordAllowedCharacters"] = "The password has to contain alphanumeric characters only";
            context.Attributes["data-val-PasswordContainsLowercase"] = "The password has to contain at least one lowercase character";
            context.Attributes["data-val-PasswordContainsUppercase"] = "The password has to contain at least one uppercase character";
            context.Attributes["data-val-PasswordContainsNumeric"] = "The password has to contain at least one numeric character";
        }
    }
}
