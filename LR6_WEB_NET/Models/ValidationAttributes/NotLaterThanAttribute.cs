using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.ValidationAttributes;

public class NotLaterThanAttribute : ValidationAttribute
{
    public NotLaterThanAttribute(DateTime latestDateTime) => LatestDateTime = latestDateTime;
    DateTime LatestDateTime { get; set; }

    public NotLaterThanAttribute(string otherDateTimePropertyName) =>
        OtherDateTimePropertyName = otherDateTimePropertyName;

    string? OtherDateTimePropertyName { get; }

    public string GetErrorMessage(string propertyName) =>
        $"{propertyName} must be not later than {LatestDateTime}";

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        if (OtherDateTimePropertyName != null)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(OtherDateTimePropertyName);
            if (otherProperty == null)
            {
                return new ValidationResult($"Unknown property: {OtherDateTimePropertyName}");
            }

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);
            if (otherValue is DateTime)
            {
                LatestDateTime = (DateTime)otherValue;
            }
            else
            {
                return new ValidationResult($"{OtherDateTimePropertyName} must be DateTime");
            }
        }

        var dateTime = DateTime.MinValue;
        if (value == null)
        {
            return new ValidationResult($"{validationContext.DisplayName} cannot be null");
        }

        if (value is DateTime)
        {
            dateTime = (DateTime)value;
        }
        else
        {
            return new ValidationResult($"{OtherDateTimePropertyName} must be DateTime");
        }

        if (dateTime > LatestDateTime)
        {
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}