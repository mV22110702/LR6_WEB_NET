using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.KeeperService;
using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZooAnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly ILogger<ZooAnimalController> _logger;

        public ZooAnimalController(ILogger<ZooAnimalController> logger, IAnimalService animalService)
        {
            _logger = logger;
            _animalService = animalService;
        }

        /// <summary>
        /// Get animal by id.
        /// </summary>
        /// <param name="id" >Animal id</param>
        /// <returns></returns>
        [HttpGet("{id:int:min(0)}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Animal> FindOne(int id)
        {
            var animal = await _animalService.FindOne(id);
            if (animal == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
            }

            Response.StatusCode = StatusCodes.Status200OK;
            return animal;
        }

        /// <summary>
        /// Add animal.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Animal> AddOne(AnimalDto animalDto)
        {
            var animal = await _animalService.AddOne(animalDto);
            Response.StatusCode = StatusCodes.Status201Created;
            return animal;
        }

        /// <summary>
        /// Update animal.
        /// </summary>
        /// <param name="id" >Animal id</param>
        /// <returns></returns>
        [HttpPut("{id:int:min(0)}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Animal> UpdateOne(int id, AnimalUpdateDto animalDto)
        {
            var animal = await _animalService.UpdateOne(id, animalDto);
            Response.StatusCode = StatusCodes.Status200OK;
            return animal;
        }

        /// <summary>
        /// Delete animal.
        /// </summary>
        /// <param name="id" >Animal id</param>
        /// <returns></returns>
        [HttpDelete("{id:int:min(0)}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Animal> DeleteOne(int id)
        {
            var deletedAnimal = await _animalService.DeleteOne(id);
            Response.StatusCode = StatusCodes.Status200OK;
            return deletedAnimal;
        }
    }
}