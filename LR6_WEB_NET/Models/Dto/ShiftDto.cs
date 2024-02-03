using System.ComponentModel.DataAnnotations;
using LR6_WEB_NET.Models.ValidationAttributes;

namespace LR6_WEB_NET.Models.Dto;

public class ShiftDto
{
    [Required(ErrorMessage = "{0} is required")]
    [Range(1,int.MaxValue,ErrorMessage = "{0} must be between {1} and {2}")]
    public int KeeperId { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [Range(1,int.MaxValue,ErrorMessage = "{0} must be between {1} and {2}")]
    public int AnimalId { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [NotLaterThan("EndDate", ErrorMessage = "{0} must be not later than {1}")]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    [NotEarlierThan("StartDate", ErrorMessage = "{0} must be not earlier than {1}")]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "{0} is required")]
    public decimal Salary { get; set; }
}