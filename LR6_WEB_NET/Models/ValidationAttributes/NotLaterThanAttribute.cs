using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.ValidationAttributes;

public class NotLaterThanAttribute : CompareDateTimeBaseAttribute
{
    public NotLaterThanAttribute(DateTime earliestDateTime):base(earliestDateTime) { }

    public NotLaterThanAttribute(string otherDateTimePropertyName):base(otherDateTimePropertyName) { }
    
    protected override string GetErrorMessage(string propertyName) =>
        $"{propertyName} must be not later than {EarliestDateTime}";
    
    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        var validationResult = Validate(value, validationContext);
        if (validationResult != null)
        {
            return validationResult;
        }
        var dateTime = (DateTime)value;
        if (dateTime > EarliestDateTime)
        {
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}