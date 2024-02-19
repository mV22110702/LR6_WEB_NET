using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.KeeperService;

namespace LR6_WEB_NET.Services.ShiftService;

public class ShiftService : IShiftService
{
    private static readonly List<Shift> _shifts = new()
    {
        
    };

    private readonly IAnimalService _animalService;

    private readonly IKeeperService _keeperService;

    public ShiftService(IKeeperService keeperService, IAnimalService animalService)
    {
        _keeperService = keeperService;
        _animalService = animalService;
    }

    public async Task<Shift?> FindOne(int id)
    {
        await Task.Delay(1000);
        lock (_shifts)
        {
            return _shifts.FirstOrDefault(k => k.Id == id, null);
        }
    }

    public async Task<Shift> AddOne(ShiftDto shiftDto)
    {
        await Task.Delay(1000);
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

        lock (_shifts)
        {
            if (
                _shifts.Find(
                    shift => shift.KeeperId == shiftDto.KeeperId && shiftDto.StartDate <= shift.EndDate
                ) != null
            )
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Shift with this keeper overlaps with another shift")
                    }
                );

            var shift = new Shift
            {
                Id = _shifts.Max(k => k.Id) + 1,
                KeeperId = shiftDto.KeeperId,
                AnimalId = shiftDto.AnimalId,
                StartDate = shiftDto.StartDate,
                EndDate = shiftDto.EndDate,
                Salary = shiftDto.Salary
            };
            _shifts.Add(shift);
            return shift;
        }
    }

    public async Task<Shift> UpdateOne(int id, ShiftUpdateDto shiftDto)
    {
        await Task.Delay(1000);
        if (shiftDto.KeeperId != null)
        {
            var doesKeeperExist = await _keeperService.DoesExist(shiftDto.KeeperId.Value);
            if (!doesKeeperExist)
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Keeper not found")
                    }
                );
        }

        if (shiftDto.AnimalId != null)
        {
            var doesAnimalExist = await _animalService.DoesExist(shiftDto.AnimalId.Value);
            if (!doesAnimalExist)
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Animal not found")
                    }
                );
        }

        lock (_shifts)
        {
            var shift = _shifts.FirstOrDefault(k => k.Id == id, null);
            if (shift == null)
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Shift not found")
                    }
                );

            if (
                _shifts.Find(
                    shift => shift.Id != id && shift.KeeperId == shiftDto.KeeperId &&
                             shiftDto.StartDate <= shift.EndDate
                ) != null
            )
                throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent("Shift with this keeper overlaps with another shift")
                    }
                );

            if (shiftDto.KeeperId != null) shift.KeeperId = shiftDto.KeeperId.Value;

            if (shiftDto.AnimalId != null) shift.AnimalId = shiftDto.AnimalId.Value;

            if (shiftDto.StartDate != null) shift.StartDate = shiftDto.StartDate.Value;

            if (shiftDto.EndDate != null) shift.EndDate = shiftDto.EndDate.Value;

            if (shiftDto.Salary != null) shift.Salary = shiftDto.Salary.Value;

            return shift;
        }
    }

    public async Task<Shift?> DeleteOne(int id)
    {
        await Task.Delay(1000);
        lock (_shifts)
        {
            var shiftToDelete = _shifts.FirstOrDefault(k => k.Id == id, null);
            if (shiftToDelete == null) return null;

            var clonedKeeper = (Shift)shiftToDelete.Clone();
            _shifts.Remove(shiftToDelete);
            return clonedKeeper;
        }
    }
}