using System;
using System.Linq;

namespace FolderComparer
{
    public class DirectoryPathValidator : IValidator<String>
    {
        public IValidationResult Validate(params string?[] items)
        {
            foreach (var item in items)
            {
                var result = Validate(item);
                if(!result.IsValid)
                    return result;
            }

            return ValidationResult.SuccessValidation();
        }

        public IValidationResult Validate(string? item)
        {
            if (item is null)
                return ValidationResult.ValidationFailed("Directori is null");

            if (!System.IO.Directory.Exists(item))
                return ValidationResult.ValidationFailed($"Directory {item} doesn't exist");

            if (System.IO.Directory.EnumerateDirectories(item).Any())
                return ValidationResult.ValidationFailed("Subdirectory feature not supported");

            return ValidationResult.SuccessValidation();
        }
    }
}
