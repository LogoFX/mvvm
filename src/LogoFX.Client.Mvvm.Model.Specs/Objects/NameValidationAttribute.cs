using System.ComponentModel.DataAnnotations;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    class NameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isValid = value is string str && str.Contains("$") == false;
            return isValid ? ValidationResult.Success : new ValidationResult("Name is invalid");
        }
    }
}
