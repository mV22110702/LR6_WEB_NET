using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LR6_WEB_NET.Models.Database;

public class User
{
    [BindNever]
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(15, MinimumLength = 1, ErrorMessage = "{0} length must be between {2} and {1} symbols")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } 
    [Required(ErrorMessage = "{0} is required")]
    public DateTime BirthDate { get; set; }
    [BindNever]
    public byte[] PasswordHash { get; set; }
    [BindNever]
    public byte[] PasswordSalt { get; set; }
    [BindNever]
    public DateTime LastLogin { get; set; }
    [BindNever] public int InvalidLoginAttempts { get; set; } = 0;
    [BindNever] public bool IsLocked { get; set; } = false;
}