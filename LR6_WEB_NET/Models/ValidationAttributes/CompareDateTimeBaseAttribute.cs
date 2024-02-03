using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.ValidationAttributes;

public abstract class CompareDateTimeBaseAttribute : ValidationAttribute
{
    public CompareDateTimeBaseAttribute(DateTime earliestDateTime) => EarliestDateTime = earliestDateTime;
    protected DateTime EarliestDateTime { get; set; }

    public CompareDateTimeBaseAttribute(string otherDateTimePropertyName) =>
        OtherDateTimePropertyName = otherDateTimePropertyName;

    protected string? OtherDateTimePropertyName { get; }

    protected abstract string GetErrorMessage(string propertyName);

    private ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult($"{validationContext.DisplayName} cannot be null");
        }

        if (value is not DateTime)
        {
            return new ValidationResult($"{OtherDateTimePropertyName} must be DateTime");
        }

        return null;
    }

    private ValidationResult? ValidateOtherProperty(ValidationContext validationContext)
    {
        if (OtherDateTimePropertyName == null)
        {
            return null;
        }

        var otherProperty = validationContext.ObjectType.GetProperty(OtherDateTimePropertyName);
        if (otherProperty == null)
        {
            return new ValidationResult($"Unknown property: {OtherDateTimePropertyName}");
        }

        var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);
        if (otherValue is DateTime)
        {
            EarliestDateTime = (DateTime)otherValue;
        }
        else
        {
            return new ValidationResult($"{OtherDateTimePropertyName} must be DateTime");
        }

        return null;
    }

    protected ValidationResult? Validate(Object? value, ValidationContext validationContext)
    {
        var valueValidationResult = ValidateValue(value, validationContext);
        if (valueValidationResult != null)
        {
            return valueValidationResult;
        }
        var otherPropertyValidationResult = ValidateOtherProperty(validationContext);
        if (otherPropertyValidationResult != null)
        {
            return otherPropertyValidationResult;
        }
        return null;
    }

    protected abstract override ValidationResult? IsValid(
        object? value, ValidationContext validationContext);
}