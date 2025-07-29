using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace StajProject.Attributes // Eğer folder adı 'Attributes' ise
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
                return new ValidationResult("Şifre alanı zorunludur.");

            if (password.Length < 8)
                return new ValidationResult("Şifre en az 8 karakter olmalı.");

            if (!password.Any(char.IsUpper))
                return new ValidationResult("Şifre en az bir büyük harf içermeli.");

            if (!password.Any(char.IsLower))
                return new ValidationResult("Şifre en az bir küçük harf içermeli.");

            if (!password.Any(char.IsDigit))
                return new ValidationResult("Şifre en az bir rakam içermeli.");

            if (!Regex.IsMatch(password, @"[\W_]"))
                return new ValidationResult("Şifre en az bir özel karakter içermeli.");

            return ValidationResult.Success;
        }
    }
}
