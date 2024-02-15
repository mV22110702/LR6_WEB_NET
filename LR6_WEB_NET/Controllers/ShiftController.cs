using Asp.Versioning;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.ShiftService;
using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersionNeutral]
public class ShiftController : ControllerBase
{
    private readonly ILogger<ShiftController> _logger;
    private readonly IShiftService _shiftService;

    public ShiftController(ILogger<ShiftController> logger, IShiftService shiftService)
    {
        _logger = logger;
        _shiftService = shiftService;
    }

    /// <summary>
    ///     Get shift by id.
    /// </summary>
    /// <param name="id">Shift id</param>
    /// <returns></returns>
    [HttpGet("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDto<Shift>> FindOne(int id)
    {
        var shift = await _shiftService.FindOne(id);
        if (shift == null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return new ResponseDto<Shift>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Description = "Shift not found",
                TotalRecords = 0
            };
        }

        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<Shift>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<Shift> { shift },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Add shift.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<Shift>> AddOne(ShiftDto shiftDto)
    {
        var shift = await _shiftService.AddOne(shiftDto);
        Response.StatusCode = StatusCodes.Status201Created;
        return new ResponseDto<Shift>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<Shift> { shift },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Update shift.
    /// </summary>
    /// <param name="id">Shift id</param>
    /// <returns></returns>
    [HttpPut("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<Shift>> UpdateOne(int id, ShiftUpdateDto shiftDto)
    {
        var shift = await _shiftService.UpdateOne(id, shiftDto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<Shift>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<Shift> { shift },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Delete shift.
    /// </summary>
    /// <param name="id">Shift id</param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<Shift>> DeleteOne(int id)
    {
        var deletedShift = await _shiftService.DeleteOne(id);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<Shift>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = deletedShift == null ? new List<Shift>() : new List<Shift> { deletedShift },
            Description = "Success",
            TotalRecords = deletedShift == null ? 0 : 1
        };
    }
}