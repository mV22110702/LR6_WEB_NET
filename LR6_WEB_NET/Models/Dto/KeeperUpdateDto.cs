using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.Dto;

public class KeeperUpdateDto
{
    [StringLength(255, MinimumLength = 3, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    [RegularExpression(@"[\sA-Za-z]+", ErrorMessage = "{0} must contain only letters")]
    public string? Name { get; set; }

    [Range(1, 100, ErrorMessage = "{0} must be between {1} and {2}")]
    public int? Age { get; set; }
}