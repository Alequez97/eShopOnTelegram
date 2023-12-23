using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace eShopOnTelegram.Domain.Validation.Attributes;

// TODO: хорошая тема, из будущих улучшений - проверка по экстеншену не очень секьюрно, можно будет внедрить проверку по сигнатуре файла.
// https://stackoverflow.com/a/73068336
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return ValidationResult.Success;
        }

        var extension = Path.GetExtension(file.FileName);
        if (!_extensions.Contains(extension.ToLower()))
        {
            return new ValidationResult(GetErrorMessage(extension));
        }

        return ValidationResult.Success;
    }

    public string GetErrorMessage(string extenstion)
    {
        return $"{extenstion} is not allowed file extenstion!";
    }
}