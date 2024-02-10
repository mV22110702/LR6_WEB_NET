using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.KeeperService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LR6_WEB_NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeeperController : ControllerBase
    {
        private readonly IKeeperService _keeperService;
        private readonly ILogger<KeeperController> _logger;

        public KeeperController(ILogger<KeeperController> logger, IKeeperService keeperService)
        {
            _logger = logger;
            _keeperService = keeperService;
        }

        /// <summary>
        /// Get keeper by id.
        /// </summary>
        /// <param name="id" >Keeper id</param>
        /// <returns></returns>
        [HttpGet("{id:int:min(0)}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ResponseDto<Keeper>> FindOne(int id)
        {
            var keeper = await _keeperService.FindOne(id);
            if (keeper == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Description = "Keeper not found",
                    TotalRecords = 0
                };
            }

            Response.StatusCode = StatusCodes.Status200OK;
            return new()
            {
                StatusCode = StatusCodes.Status200OK,
                Values = new() { keeper },
                Description = "Success",
                TotalRecords = 1
            };
        }

        /// <summary>
        /// Add keeper.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResponseDto<Keeper>> AddOne(KeeperDto keeperDto)
        {
            var keeper = await _keeperService.AddOne(keeperDto);
            Response.StatusCode = StatusCodes.Status201Created;
            return new()
            {
                StatusCode = StatusCodes.Status200OK,
                Values = new() { keeper },
                Description = "Success",
                TotalRecords = 1
            };
        }

        /// <summary>
        /// Update keeper.
        /// </summary>
        /// <param name="id" >Keeper id</param>
        /// <returns></returns>
        [HttpPut("{id:int:min(0)}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResponseDto<Keeper>> UpdateOne(int id, KeeperUpdateDto keeperDto)
        {
            var keeper = await _keeperService.UpdateOne(id, keeperDto);
            Response.StatusCode = StatusCodes.Status200OK;
            return new()
            {
                StatusCode = StatusCodes.Status200OK,
                Values = new() { keeper },
                Description = "Success",
                TotalRecords = 1
            };
        }

        /// <summary>
        /// Delete keeper.
        /// </summary>
        /// <param name="id" >Keeper id</param>
        /// <returns></returns>
        [HttpDelete("{id:int:min(0)}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResponseDto<Keeper>> DeleteOne(int id)
        {
            var deletedKeeper = await _keeperService.DeleteOne(id);
            Response.StatusCode = StatusCodes.Status200OK;
            return new()
            {
                StatusCode = StatusCodes.Status200OK,
                Values = deletedKeeper == null ? new() : new() { deletedKeeper },
                Description = "Success",
                TotalRecords = deletedKeeper == null ? 0 : 1
            };
        }
    }
}