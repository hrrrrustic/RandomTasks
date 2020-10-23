using System;

namespace FolderComparer
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        String? Message { get; }
    }

    public class ValidationResult : IValidationResult
    {
        public bool IsValid { get; }
        public string? Message { get; }

        private ValidationResult(bool success, string? message)
        {
            IsValid = success;
            Message = message;
        }

        public static IValidationResult SuccessValidation()
        {
            return new ValidationResult(true, null);
        }

        public static IValidationResult ValidationFailed(string message)
        {
            return new ValidationResult(false, message);
        }

        public static IValidationResult ValidationFailed(Exception exception)
        {
            return new ValidationResult(false, exception.ToString());
        }
    }
}