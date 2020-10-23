using System;
using System.Collections.Generic;
using System.IO;

namespace FolderComparer
{
    public class DirectoryPathValidator : IValidator<String>
    {
        public IEnumerator<IFile> GetEnumerator()
        {
            throw new NotImplementedException();
        }

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

            if (!Directory.Exists(item))
                return ValidationResult.ValidationFailed($"Directory {item} doesn't exist");

            return ValidationResult.SuccessValidation();
        }
    }
}
