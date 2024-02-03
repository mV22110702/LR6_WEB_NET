using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.KeeperService;

public interface IKeeperService
{
    public Task<Keeper> FindOne(int id);
    public Task<Keeper> AddOne(KeeperDto keeperDto);
    public Task<Keeper> UpdateOne(int id, KeeperUpdateDto keeperDto);
    public Task<Keeper> DeleteOne(int id);
}