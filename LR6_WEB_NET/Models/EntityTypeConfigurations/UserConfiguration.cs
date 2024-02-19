using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        User? tempUser = null;
        for (var i = 0; i < 10; i++)
        {
            tempUser = new User
            {
                Id = i,
                Email = $"email{i}@mail.com",
                RoleId = i % 2 == 0 ? 1 : 2,
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                BirthDate = DateTime.Now.AddYears(-20).AddDays(i),
                IsLocked = false,
                LastLogin = DateTime.Now,
                InvalidLoginAttempts = 0
            };
            UserService.SetUserPasswordHash(tempUser, $"password{i}");
            builder.HasData(tempUser);
        }
    }
}