using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LR6_WEB_NET.Services.KeeperService;

public class KeeperService : IKeeperService
{
    private readonly DataContext _dataContext;

    public KeeperService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Keeper?> FindOne(int id)
    {
        return _dataContext.Keepers.FirstOrDefault(k => k.Id == id);
    }

    public Task<bool> DoesExist(int id)
    {
        return _dataContext.Keepers.AnyAsync(k => k.Id == id);
    }

    public async Task<Keeper> AddOne(KeeperDto keeperDto)
    {
        var keeper = new Keeper
        {
            Name = keeperDto.Name,
            Age = keeperDto.Age
        };
        _dataContext.Keepers.Add(keeper);
        await _dataContext.SaveChangesAsync();
        return keeper;
    }

    public async Task<Keeper> UpdateOne(int id, KeeperUpdateDto keeperDto)
    {
        var keeper = await _dataContext.Keepers.FirstOrDefaultAsync(k => k.Id == id);
        if (keeper == null)
            throw new HttpResponseException(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Keeper does not exist")
                }
            );
        if (!String.IsNullOrEmpty(keeperDto.Name)) keeper.Name = keeperDto.Name;

        if (keeperDto.Age != null) keeper.Age = keeperDto.Age.Value;

        await _dataContext.SaveChangesAsync();
        return keeper;
    }

    public async Task<Keeper?> DeleteOne(int id)
    {
        var keeperToDelete = await _dataContext.Keepers.FirstOrDefaultAsync(k => k.Id == id);
        if (keeperToDelete == null) return null;
        _dataContext.Keepers.Remove(keeperToDelete);
        await _dataContext.SaveChangesAsync();
        return keeperToDelete;
    }
    
    public async Task<string?> CheckServiceConnection()
    {
        try
        {
            var keeper = await _dataContext.Keepers.FirstOrDefaultAsync();
            return null;
        } catch (Exception e)
        {
            return e.Message;
        }
    }
}