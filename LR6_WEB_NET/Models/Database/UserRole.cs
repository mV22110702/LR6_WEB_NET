using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using LR6_WEB_NET.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LR6_WEB_NET.Models.Database;

public class UserRole
{
    public static ReadOnlyDictionary<UserRoleName, string> UserRoleNames = new(new Dictionary<UserRoleName, string>
    {
        {UserRoleName.Admin, "Admin"},
        {UserRoleName.User, "User"}
    });
    
    public static List<UserRole> UserRoles = new()
    {
        new UserRole {Id = 1, Name = UserRoleNames[UserRoleName.Admin]},
        new UserRole {Id = 2, Name = UserRoleNames[UserRoleName.Admin]}
    };

    [BindNever] public int Id { get; set; } = 0;
    public string Name { get; set; } = String.Empty;
}



