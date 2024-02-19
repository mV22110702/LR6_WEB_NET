using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Enums;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(
            new UserRole { Id = 1, Name = UserRole.UserRoleNames[UserRoleName.Admin] },
            new UserRole { Id = 2, Name = UserRole.UserRoleNames[UserRoleName.User] }
            );
    }
}