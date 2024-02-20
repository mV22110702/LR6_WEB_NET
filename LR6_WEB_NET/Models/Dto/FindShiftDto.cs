using System.ComponentModel.DataAnnotations;
using LR6_WEB_NET.Models.ValidationAttributes;

namespace LR6_WEB_NET.Models.Dto;

public class FindShiftDto
{
    [Required(ErrorMessage = "{0} is required")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
    public int KeeperId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} must be between {1} and {2}")]
    public int AnimalId { get; set; }
}