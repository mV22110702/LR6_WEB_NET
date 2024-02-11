using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AuthService;

namespace LR6_WEB_NET.Services.UserService;

public class UserService : IUserService
{
    private static readonly List<User> _users = new();

    public UserService()
    {
        User? tempUser = null;
        for (int i = 0; i < 10; i++)
        {
            tempUser = new User
            {
                Id = i,
                Email = $"email{i}@mail.com",
                Role = i % 2 == 0 ? UserRole.UserRoles[0] : UserRole.UserRoles[1],
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
                BirthDate = DateTime.Now.AddYears(-20).AddDays(i),
                IsLocked = false,
                LastLogin = DateTime.Now,
                InvalidLoginAttempts = 0
            };
            SetUserPasswordHash(tempUser, $"password{i}");
            _users.Add(tempUser);
        }
    }

    public async Task<User?> FindOne(int id)
    {
        await Task.Delay(1000);
        lock (_users)
        {
            return _users.FirstOrDefault(a => a.Id == id, null);
        }
    }

    public async Task<User?> FindOneByEmail(string email)
    {
        await Task.Delay(1000);
        lock (_users)
        {
            return _users.FirstOrDefault(a => a.Email == email, null);
        }
    }

    public Task<bool> DoesExist(int id)
    {
        lock (_users)
        {
            return Task.FromResult(_users.Any(a => a.Id == id));
        }
    }


    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    public void SetUserPasswordHash(User user, string password)
    {
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
    }

    public async Task<User> AddOne(UserRegisterDto userRegisterDto)
    {
        var nextUserId = _users.Max(u => u.Id) + 1;
        var userRole = UserRole.UserRoles.FirstOrDefault(r => r.Name == userRegisterDto.Role, null);
        if (userRole == null)
        {
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role does not exist") });
        }

        CreatePasswordHash(userRegisterDto.Password, out var passwordHash, out var passwordSalt);
        var user = new User()
        {
            Id = nextUserId,
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
        _users.Add(user);
        return user;
    }

    public async Task<User> UpdateOne(int id, UserUpdateDto userUpdateDto)
    {
        await Task.Delay(1000);
        lock (_users)
        {
            var user = _users.FirstOrDefault(a => a.Id == id, null);
            if (user == null)
                throw new HttpResponseException(new HttpResponseMessage
                    { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Animal does not exist") });

            if (userUpdateDto.FirstName != null) user.FirstName = userUpdateDto.FirstName;
            if (userUpdateDto.LastName != null) user.LastName = userUpdateDto.LastName;
            if (userUpdateDto.Email != null) user.Email = userUpdateDto.Email;
            if (userUpdateDto.Role != null)
            {
                var userRole = UserRole.UserRoles.FirstOrDefault(r => r.Name == userUpdateDto.Role, null);
                if (userRole == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage
                        { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role does not exist") });
                }

                user.Role = userRole;
            }

            if (userUpdateDto.BirthDate != null) user.BirthDate = userUpdateDto.BirthDate;
            if (userUpdateDto.Password != null)
            {
                CreatePasswordHash(userUpdateDto.Password, out var passwordHash, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }


            return user;
        }
    }

    public async Task<User?> DeleteOne(int id)
    {
        await Task.Delay(1000);
        lock (_users)
        {
            var userToDelete = _users.FirstOrDefault(a => a.Id == id, null);
            if (userToDelete == null) return null;

            var clonedUser = (User)userToDelete.Clone();
            _users.Remove(userToDelete);
            return clonedUser;
        }
    }
}