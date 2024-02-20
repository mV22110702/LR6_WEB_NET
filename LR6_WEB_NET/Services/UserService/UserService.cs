using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.UserRoleService;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _dataContext;
    private readonly IUserRoleService _userRoleService;

    public UserService(DataContext dataContext, IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
        _dataContext = dataContext;
    }

    public async Task<User?> FindOne(int id)
    {
        // await Task.Delay(1000);
        // lock (_users)
        // {
        //     return _users.FirstOrDefault(a => a.Id == id, null);
        // }
        return await _dataContext.Users.FindAsync(id);
    }

    public async Task<User?> FindOneByEmail(string email)
    {
        // await Task.Delay(1000);
        // lock (_users)
        // {
        //     return _users.FirstOrDefault(a => a.Email == email, null);
        // }
        return await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<bool> DoesExist(int id)
    {
        // lock (_users)
        // {
        //     return Task.FromResult(_users.Any(a => a.Id == id));
        // }
        return _dataContext.Users.AnyAsync(u => u.Id == id);
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    public static void SetUserPasswordHash(User user, string password)
    {
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
    }

    public async Task<User> AddOne(UserRegisterDto userRegisterDto)
    {
        var userRole = await _userRoleService.FindByName(userRegisterDto.Role);
        if (userRole == null)
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role does not exist") });
        CreatePasswordHash(userRegisterDto.Password, out var passwordHash, out var passwordSalt);
        
        var user = new User
        {
            Email = userRegisterDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            InvalidLoginAttempts = 0,
            IsLocked = false,
            LastLogin = DateTime.Now,
            Role = userRole,
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            BirthDate = userRegisterDto.BirthDate
        };
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateOne(int id, UserUpdateDto userUpdateDto)
    {
        var user = await this.FindOne(id);
        if (user == null)
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User does not exist") });
        
        if (userUpdateDto.FirstName != null) user.FirstName = userUpdateDto.FirstName;
        if (userUpdateDto.LastName != null) user.LastName = userUpdateDto.LastName;
        if (userUpdateDto.Email != null) user.Email = userUpdateDto.Email;
        if (userUpdateDto.Role != null)
        {
            var userRole = await _userRoleService.FindByName(userUpdateDto.Role);
            if (userRole == null)
                throw new HttpResponseException(new HttpResponseMessage
                    { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role does not exist") });
        
            user.Role = userRole;
        }
        
        if (userUpdateDto.BirthDate != null) user.BirthDate = (DateTime)userUpdateDto.BirthDate;
        if (userUpdateDto.Password != null)
        {
            CreatePasswordHash(userUpdateDto.Password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }
        
        await _dataContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> DeleteOne(int id)
    {
        var userToDelete = await this.FindOne(id);
        if (userToDelete == null) return null;
        _dataContext.Users.Remove(userToDelete);
        await _dataContext.SaveChangesAsync();
        return userToDelete;
    }


    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}