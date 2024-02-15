using Asp.Versioning;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AnimalStatisticsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers;

[ApiVersion(1.0)]
[ApiVersion(2.0)]
[ApiVersion(3.0)]
[ApiController]
[Route("[controller]/v{version:apiVersion}")]
[Authorize]
public class AnimalStatisticsController : ControllerBase
{
    private readonly IAnimalStatisticsService _animalStatisticsService;
    private readonly ILogger<AnimalStatisticsController> _logger;

    public AnimalStatisticsController(ILogger<AnimalStatisticsController> logger,
        IAnimalStatisticsService animalStatisticsService
    )
    {
        _logger = logger;
        _animalStatisticsService = animalStatisticsService;
    }

    /// <summary>
    ///     Get animal average age.
    /// </summary>
    /// <returns></returns>
    [HttpGet("averageAge")]
    [MapToApiVersion(1.0)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ResponseDto<double>> GetAverageAge()
    {
        var averageAge = await _animalStatisticsService.GetAverageAge();

        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<double>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<double> { averageAge },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Get animal average age.
    /// </summary>
    /// <param name="id">Animal id</param>
    /// <returns></returns>
    [HttpGet("averageAge")]
    [MapToApiVersion(2.0)]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ResponseDto<string>> GetAverageAgeV2()
    {
        var averageAgeReadable = await _animalStatisticsService.GetAverageAgeReadable();
        // var animalCount = await _animalStatisticsService.GetAnimalCount(); 

        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<string>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<string> { averageAgeReadable },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Get animal average age.
    /// </summary>
    /// <param name="id">Animal id</param>
    /// <returns></returns>
    [HttpGet("averageAge")]
    [MapToApiVersion(3.0)]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<FileStreamResult> GetAverageAgeV3()
    {
        var averageAgeExcel = await _animalStatisticsService.GetAverageAgeExcel();

        Response.StatusCode = StatusCodes.Status200OK;
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Headers.ContentDisposition = "attachment;filename=\"AverageAge.xlsx\"";
        System.IO.Stream spreadsheetStream = new System.IO.MemoryStream();
        averageAgeExcel.SaveAs(spreadsheetStream);
        spreadsheetStream.Position = 0;

        return new FileStreamResult(spreadsheetStream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "AverageAge.xlsx" };
    }
}