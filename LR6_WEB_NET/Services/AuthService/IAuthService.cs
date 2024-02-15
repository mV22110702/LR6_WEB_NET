using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.AuthService;

public interface IAuthService
{
    public Task<AuthResponseDto> Login(UserLoginDto userLoginDto);
    public Task<AuthResponseDto> Register(UserRegisterDto userRegisterDto);
}