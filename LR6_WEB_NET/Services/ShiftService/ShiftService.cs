using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.KeeperService;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LR6_WEB_NET.Services.ShiftService;

public class ShiftService : IShiftService
{
    private readonly DataContext _dataContext;

    private readonly IAnimalService _animalService;

    private readonly IKeeperService _keeperService;

    public ShiftService(IKeeperService keeperService, IAnimalService animalService, DataContext dataContext)
    {
        _keeperService = keeperService;
        _animalService = animalService;
        _dataContext = dataContext;
    }

    public async Task<Shift?> FindOne(FindShiftDto id)
    {
        return _dataContext.Shifts.FirstOrDefault(s => s.AnimalId == id.AnimalId && s.KeeperId == id.KeeperId);
    }

    public async Task<Shift> AddOne(ShiftDto shiftDto)
    {
        var doesKeeperExist = await _keeperService.DoesExist(shiftDto.KeeperId);
        if (!doesKeeperExist)
            throw new HttpResponseException(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Keeper not found")
                }
            );

        var doesAnimalExist = await _animalService.DoesExist(shiftDto.AnimalId);
        if (!doesAnimalExist)
            throw new HttpResponseException(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Animal not found")
                }
            );

        if (
            (from shift in _dataContext.Shifts
                where shift.KeeperId == shiftDto.KeeperId && shiftDto.StartDate <= shift.EndDate
                select shift).Any()
        )
            throw new HttpResponseException(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Shift with this keeper overlaps with another shift")
                }
            );
        var newShift = new Shift
        {
            KeeperId = shiftDto.KeeperId,
            AnimalId = shiftDto.AnimalId,
            StartDate = shiftDto.StartDate,
            EndDate = shiftDto.EndDate,
            Salary = shiftDto.Salary
        };

        _dataContext.Shifts.Add(newShift);
        await _dataContext.SaveChangesAsync();
        return newShift;
    }

    public async Task<Shift> UpdateOne(FindShiftDto id, ShiftUpdateDto shiftDto)
    {
        var candidate = await this.FindOne(id);
        if (candidate == null)
            throw new HttpResponseException(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Shift not found")
                }
            );
        if (
            (from shift in _dataContext.Shifts
                where shift.KeeperId == id.KeeperId && shiftDto.StartDate <= shift.EndDate
                select shift).Any()
        )
            throw new HttpResponseException(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Shift with this keeper overlaps with another shift")
                }
            );

        if (shiftDto.StartDate != null) candidate.StartDate = shiftDto.StartDate.Value;

        if (shiftDto.EndDate != null) candidate.EndDate = shiftDto.EndDate.Value;

        if (shiftDto.Salary != null) candidate.Salary = shiftDto.Salary.Value;

        await _dataContext.SaveChangesAsync();
        return candidate;
    }

    public async Task<Shift?> DeleteOne(FindShiftDto id)
    {
        var shiftToDelete = await this.FindOne(id);
        if (shiftToDelete == null) return null;
        _dataContext.Shifts.Remove(shiftToDelete);
        await _dataContext.SaveChangesAsync();
        return shiftToDelete;
    }
    
    public async Task<string?> CheckServiceConnection()
    {
        try
        {
            var shift = await _dataContext.Shifts.FirstOrDefaultAsync();
            return null;
        } catch (Exception e)
        {
Log.Error(e,"Check shift service connection failed");
            return e.Message;
        }
    }
}