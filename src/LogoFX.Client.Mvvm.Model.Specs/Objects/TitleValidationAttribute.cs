using System.ComponentModel.DataAnnotations;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    internal sealed class TitleValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = value is string str && str.Contains("$") == false;
            return isValid ? ValidationResult.Success : new ValidationResult("Name is invalid");
        }
    }
}