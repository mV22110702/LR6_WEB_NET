using Asp.Versioning;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers;

[ApiController]
[Route("user")]
[Authorize(Policy = "Admin")]
[ApiVersionNeutral]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    ///     Get user by id.
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns></returns>
    [HttpGet("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDto<User>> FindOne(int id)
    {
        var user = await _userService.FindOne(id);
        if (user == null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return new ResponseDto<User>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Description = "User not found",
                TotalRecords = 0
            };
        }

        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<User>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<User> { user },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Update user.
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns></returns>
    [HttpPut("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<User>> UpdateOne(int id, UserUpdateDto userUpdateDto)
    {
        var user = await _userService.UpdateOne(id, userUpdateDto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<User>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<User> { user },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Delete user.
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<User?>> DeleteOne(int id)
    {
        var deletedUser = await _userService.DeleteOne(id);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<User?>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = deletedUser == null ? new List<User?>() : new List<User?> { deletedUser },
            Description = "Success",
            TotalRecords = deletedUser == null ? 0 : 1
        };
    }
}