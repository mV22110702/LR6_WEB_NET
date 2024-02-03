using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZooKeeperController : ControllerBase
    {
        private static List<ZooKeeper> _keepers = new List<ZooKeeper>
        {
            new ZooKeeper { Id = 1, Name = "John Doe", Age = 25 },
            new ZooKeeper { Id = 2, Name = "Jane Doe", Age = 30 },
            new ZooKeeper { Id = 3, Name = "John Smith", Age = 35 },
            new ZooKeeper { Id = 4, Name = "Jane Smith", Age = 40 },
            new ZooKeeper { Id = 5, Name = "Steve Lane", Age = 31 },
            new ZooKeeper { Id = 6, Name = "Conor Wood", Age = 32 },
            new ZooKeeper { Id = 7, Name = "Alfred Wolf", Age = 33 },
            new ZooKeeper { Id = 8, Name = "Jim How", Age = 34 },
            new ZooKeeper { Id = 9, Name = "Jane Rich", Age = 35 },
            new ZooKeeper { Id = 10, Name = "Jack Clinton", Age = 36 },
        };


        private readonly ILogger<ZooKeeperController> _logger;

        public ZooKeeperController(ILogger<ZooKeeperController> logger)
        {
            _logger = logger;
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
        public async Task<ZooKeeper> FindOne(int id)
        {
            await Task.Delay(1000);
            var keeper = _keepers.FirstOrDefault(k => k.Id == id, null);
            if(keeper == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
            }
            Response.StatusCode = StatusCodes.Status200OK;
            return keeper;
        }
        
        /// <summary>
        /// Add keeper.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ZooKeeper> AddOne(ZooKeeperDto keeperDto)
        {
            await Task.Delay(1000);
            if(_keepers.Find((keeper)=>keeper.Name == keeperDto.Name) != null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }
            var keeper = new ZooKeeper
            {
                Id = _keepers.Max(k => k.Id) + 1,
                Name = keeperDto.Name,
                Age = keeperDto.Age
            };
            _keepers.Add(keeper);
            Response.StatusCode = StatusCodes.Status201Created;
            return _keepers.FirstOrDefault(k => k.Id == keeper.Id, null);
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
        public async Task<ZooKeeper> UpdateOne(int id, ZooKeeperDto keeperDto)
        {
            await Task.Delay(1000);
            var keeper = _keepers.FirstOrDefault(k => k.Id == id, null);
            if(keeper == null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }
            keeper.Name = keeperDto.Name;
            keeper.Age = keeperDto.Age;
            Response.StatusCode = StatusCodes.Status200OK;
            return _keepers.FirstOrDefault(k => k.Id == id, null);
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
        public async Task<ZooKeeper> DeleteOne(int id)
        {
            await Task.Delay(1000);
            var keeperToDelete = _keepers.FirstOrDefault(k => k.Id == id, null);
            var clonedKeeper = (ZooKeeper)keeperToDelete?.Clone();
            _keepers.Remove(keeperToDelete);
            Response.StatusCode = StatusCodes.Status200OK;
            return clonedKeeper;
        }
    }
}