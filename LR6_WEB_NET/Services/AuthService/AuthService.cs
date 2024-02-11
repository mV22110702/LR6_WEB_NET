﻿using System.Security.Claims;
using System.Text;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography;
using System.Web.Http;
using LR6_WEB_NET.Services.UserService;
using Microsoft.IdentityModel.Tokens;

namespace LR6_WEB_NET.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly ILogger<AuthService> _logger;
    public AuthService(ILogger<AuthService> logger, IConfiguration config, IUserService userService)
    {
        _logger = logger;
        _config = config;
        _userService = userService;
    }

    private string CreateToken(User user)
    {
        try
        {
            if (_config.GetValue<string>("Jwt:Key") == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Token cannot be issued due to server error")
                });
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };
            var test = _config.GetValue<string>("Jwt:Key");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(test ?? string.Empty));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _config.GetValue<string>("Jwt:Issuer"),
                audience: _config.GetValue<string>("Jwt:Audience"),
                expires: DateTime.Now.AddMinutes(_config.GetValue<int>("Jwt:ExpirationInMinutes")),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while creating token");
            throw new HttpResponseException(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Token cannot be issued due to server error")
            });
        }
    }


    public async Task<AuthResponseDto> Login(UserLoginDto userLoginDto)
    {
        var user = await _userService.FindOneByEmail(userLoginDto.Email);
        if (user == null)
        {
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User does not exist") });
        }
        
        if(user.IsLocked)
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User is locked") });

        if (!_userService.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            user.InvalidLoginAttempts++;
            if (user.InvalidLoginAttempts >= _config.GetValue<int>("Jwt:MaxInvalidLoginAttempts"))
                user.IsLocked = true;
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Passwords do not match") });
        }
        
        user.LastLogin = DateTime.Now;
        _logger.LogInformation($"User {user.Id} logged in");
        return new AuthResponseDto
        {
            Token = CreateToken(user)
        };
    }

    public async Task<AuthResponseDto> Register(UserRegisterDto userRegisterDto)
    {
        var user =  await _userService.FindOneByEmail(userRegisterDto.Email);
        if (user != null)
        {
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("User already exists") });
        }

        user = await _userService.AddOne(userRegisterDto);
        return new AuthResponseDto
        {
            Token = CreateToken(user)
        };
    }
}