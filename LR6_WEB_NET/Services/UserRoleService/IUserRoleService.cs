using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Services.Service;

namespace LR6_WEB_NET.Services.UserRoleService;

public interface IUserRoleService: IService
{
    public Task<UserRole?> FindByName(string name);
}