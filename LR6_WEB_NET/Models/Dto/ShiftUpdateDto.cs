using System.ComponentModel.DataAnnotations;
using LR6_WEB_NET.Models.ValidationAttributes;

namespace LR6_WEB_NET.Models.Dto;

public class ShiftUpdateDto
{

    [NotLaterThan("EndDate", ErrorMessage = "{0} must be not later than {1}")]
    public DateTime? StartDate { get; set; }

    [NotEarlierThan("StartDate", ErrorMessage = "{0} must be not earlier than {1}")]
    public DateTime? EndDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
    public double? Salary { get; set; }
}