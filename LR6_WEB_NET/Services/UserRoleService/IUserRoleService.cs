using LR6_WEB_NET.Models.Database;

namespace LR6_WEB_NET.Services.UserRoleService;

public interface IUserRoleService
{
    public Task<UserRole?> FindByName(string name);
}