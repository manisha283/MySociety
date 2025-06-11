using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MySociety.Entity.Attributes;

public class ImageTypeAttribute : ValidationAttribute
{
    private readonly string[] _allowedTypes = new[] { ".jpg", ".jpeg", ".png" };
    private readonly int _allowedFileSizeMB = 1 * 1024 * 1024; 
 
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedTypes.Contains(extension))
            {
                return new ValidationResult($"Only the following file types are allowed: {string.Join(", ", _allowedTypes)}");
            }

            if (file.Length > _allowedFileSizeMB)
            {
                return new ValidationResult($"File too Big, please select a file less than {string.Join(", ", _allowedFileSizeMB)} mb");
            }
        }
        return ValidationResult.Success!;
    }
}