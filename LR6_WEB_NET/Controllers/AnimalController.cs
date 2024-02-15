using Asp.Versioning;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AnimalService;
using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersionNeutral]
public class AnimalController : ControllerBase
{
    private readonly IAnimalService _animalService;
    private readonly ILogger<AnimalController> _logger;

    public AnimalController(ILogger<AnimalController> logger, IAnimalService animalService)
    {
        _logger = logger;
        _animalService = animalService;
    }

    /// <summary>
    ///     Get animal by id.
    /// </summary>
    /// <param name="id">Animal id</param>
    /// <returns></returns>
    [HttpGet("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ResponseDto<Animal>> FindOne(int id)
    {
        var animal = await _animalService.FindOne(id);
        if (animal == null)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return new ResponseDto<Animal>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Description = "Animal not found",
                TotalRecords = 0
            };
        }

        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<Animal>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<Animal> { animal },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Add animal.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<Animal>> AddOne(AnimalDto animalDto)
    {
        var animal = await _animalService.AddOne(animalDto);
        Response.StatusCode = StatusCodes.Status201Created;
        return new ResponseDto<Animal>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<Animal> { animal },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Update animal.
    /// </summary>
    /// <param name="id">Animal id</param>
    /// <returns></returns>
    [HttpPut("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<Animal>> UpdateOne(int id, AnimalUpdateDto animalDto)
    {
        var animal = await _animalService.UpdateOne(id, animalDto);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<Animal>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = new List<Animal> { animal },
            Description = "Success",
            TotalRecords = 1
        };
    }

    /// <summary>
    ///     Delete animal.
    /// </summary>
    /// <param name="id">Animal id</param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(0)}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ResponseDto<Animal?>> DeleteOne(int id)
    {
        var deletedAnimal = await _animalService.DeleteOne(id);
        Response.StatusCode = StatusCodes.Status200OK;
        return new ResponseDto<Animal?>
        {
            StatusCode = StatusCodes.Status200OK,
            Values = deletedAnimal == null ? new List<Animal?>() : new List<Animal?> { deletedAnimal },
            Description = "Success",
            TotalRecords = deletedAnimal == null ? 0 : 1
        };
    }
}