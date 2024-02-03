using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.ValidationAttributes;

public class NotEarlierThanAttribute : CompareDateTimeBaseAttribute
{
    public NotEarlierThanAttribute(DateTime earliestDateTime):base(earliestDateTime) { }

    public NotEarlierThanAttribute(string otherDateTimePropertyName):base(otherDateTimePropertyName) { }
    
    protected override string GetErrorMessage(string propertyName) =>
        $"{propertyName} must be not earlier than {EarliestDateTime}";
    
    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        
        
        var dateTime = (DateTime)value;
        if (dateTime < EarliestDateTime)
        {
            return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}