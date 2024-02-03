using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.KeeperService;

public class KeeperService : IKeeperService
{
    private static List<Keeper> _keepers = new List<Keeper>
    {
        new Keeper { Id = 1, Name = "John Doe", Age = 25 },
        new Keeper { Id = 2, Name = "Jane Doe", Age = 30 },
        new Keeper { Id = 3, Name = "John Smith", Age = 35 },
        new Keeper { Id = 4, Name = "Jane Smith", Age = 40 },
        new Keeper { Id = 5, Name = "Steve Lane", Age = 31 },
        new Keeper { Id = 6, Name = "Conor Wood", Age = 32 },
        new Keeper { Id = 7, Name = "Alfred Wolf", Age = 33 },
        new Keeper { Id = 8, Name = "Jim How", Age = 34 },
        new Keeper { Id = 9, Name = "Jane Rich", Age = 35 },
        new Keeper { Id = 10, Name = "Jack Clinton", Age = 36 },
    };

    public async Task<Keeper?> FindOne(int id)
    {
        await Task.Delay(1000);
        lock (_keepers)
        {
            return _keepers.FirstOrDefault(k => k.Id == id, null);
        }
    }

    public Task<bool> DoesExist(int id)
    {
        lock (_keepers)
        {
            return Task.FromResult(_keepers.Any(k => k.Id == id));
        }
    }

    public async Task<Keeper> AddOne(KeeperDto keeperDto)
    {
        await Task.Delay(1000);
        lock (_keepers)
        {
            if (_keepers.Find((keeper) => keeper.Name == keeperDto.Name) != null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Keeper with this name already exists")
                    }
                );
            }

            var keeper = new Keeper
            {
                Id = _keepers.Max(k => k.Id) + 1,
                Name = keeperDto.Name,
                Age = keeperDto.Age
            };
            _keepers.Add(keeper);
            return keeper;
        }
    }

    public async Task<Keeper> UpdateOne(int id, KeeperUpdateDto keeperDto)
    {
        await Task.Delay(1000);
        lock (_keepers)
        {
            var keeper = _keepers.FirstOrDefault(k => k.Id == id, null);
            if (keeper == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Keeper does not exist")
                    }
                );
            }

            if (keeperDto.Name != null)
            {
                keeper.Name = keeperDto.Name;
            }

            if (keeperDto.Age != null)
            {
                keeper.Age = keeperDto.Age.Value;
            }

            return keeper;
        }
    }

    public async Task<Keeper?> DeleteOne(int id)
    {
        await Task.Delay(1000);
        lock (_keepers)
        {
            var keeperToDelete = _keepers.FirstOrDefault(k => k.Id == id, null);
            if (keeperToDelete == null)
            {
                return null;
            }

            var clonedKeeper = (Keeper)keeperToDelete.Clone();
            _keepers.Remove(keeperToDelete);
            return clonedKeeper;
        }
    }
}