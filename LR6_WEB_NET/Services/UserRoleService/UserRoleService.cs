using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Services.UserRoleService;

public class UserRoleService: IUserRoleService
{
    private readonly DataContext _dataContext;

    public UserRoleService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public Task<UserRole?> FindByName(string name)
    {
        return _dataContext.UserRoles.FirstOrDefaultAsync(ur => ur.Name == name);
    }
    
    public async Task<string?> CheckServiceConnection()
    {
        try
        {
            var userRole = await _dataContext.UserRoles.FirstOrDefaultAsync();
            return null;
        } catch (Exception e)
        {
            return e.Message;
        }
    }
}