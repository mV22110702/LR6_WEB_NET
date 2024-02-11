﻿using System.Security.Claims;
using System.Text;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;

namespace LR6_WEB_NET.Services.AuthService;

public class AuthService : IAuthService
{
    public List<User> Users { get; set; } = new();
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;

        User? tempUser = null;
        for (int i = 0; i < 10; i++)
        {
            tempUser = new User
            {
                Id = i,
                Email = $"email{i}@mail.com",
                Role = new UserRole
                {
                    Id = i,
                    Name = i % 2 == 0 ? "Admin" : "User"
                },
                FirstName = $"FirstName{i}",
                LastName = $"LastName{i}",
            };
            SetUserPasswordHash(tempUser, $"password{i}");
            Users.Add(tempUser);
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _config.GetValue<string>("Jwt:Issuer"),
            audience: _config.GetValue<string>("Jwt:Audience"),
            expires: DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:ExpirationInMinutes")),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private void SetUserPasswordHash(User user, string password)
    {
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
    }

    public Task<AuthResponseDto> Login(UserLoginDto userLoginDto)
    {
        var user = Users.FirstOrDefault(u => u.Email == userLoginDto.Email);
        if (user == null)
        {
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User does not exist") });
        }
        
        if(user.IsLocked)
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User is locked") });

        if (!VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            user.InvalidLoginAttempts++;
            if (user.InvalidLoginAttempts >= _config.GetValue<int>("Jwt:MaxInvalidLoginAttempts"))
                user.IsLocked = true;
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Passwords do not match") });
        }
        
        user.LastLogin = DateTime.Now;

        return Task.FromResult(new AuthResponseDto
        {
            Token = CreateToken(user)
        });
    }

    public Task<AuthResponseDto> Register(UserRegisterDto userRegisterDto)
    {
        var user = Users.FirstOrDefault(u => u.Email == userRegisterDto.Email);
        if (user != null)
        {
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User already exists") });
        }

        var nextUserId = Users.Max(u => u.Id) + 1;
        var userRole = UserRole.UserRoles.FirstOrDefault(r => r.Name == userRegisterDto.Role, null);
        if (userRole == null)
        {
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Role does not exist") });
        }

        CreatePasswordHash(userRegisterDto.Password, out var passwordHash, out var passwordSalt);
        user = new User()
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
        Users.Add(user);
        return Task.FromResult(new AuthResponseDto
        {
            Token = CreateToken(user)
        });
    }
}