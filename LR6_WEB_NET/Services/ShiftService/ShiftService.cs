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
        new Shift()
        {
            Id = 1, KeeperId = 1, AnimalId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1),
            Salary = 100
        },
        new Shift()
        {
            Id = 2, KeeperId = 2, AnimalId = 2, StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(2),
            Salary = 200
        },
        new Shift()
        {
            Id = 3, KeeperId = 3, AnimalId = 3, StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3),
            Salary = 300
        },
        new Shift()
        {
            Id = 4, KeeperId = 4, AnimalId = 4, StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(4),
            Salary = 400
        },
        new Shift()
        {
            Id = 5, KeeperId = 5, AnimalId = 5, StartDate = DateTime.Now.AddDays(4), EndDate = DateTime.Now.AddDays(5),
            Salary = 500
        },
        new Shift()
        {
            Id = 6, KeeperId = 6, AnimalId = 6, StartDate = DateTime.Now.AddDays(5), EndDate = DateTime.Now.AddDays(6),
            Salary = 600
        },
        new Shift()
        {
            Id = 7, KeeperId = 7, AnimalId = 7, StartDate = DateTime.Now.AddDays(6), EndDate = DateTime.Now.AddDays(7),
            Salary = 700
        },
        new Shift()
        {
            Id = 8, KeeperId = 8, AnimalId = 8, StartDate = DateTime.Now.AddDays(7), EndDate = DateTime.Now.AddDays(8),
            Salary = 800
        },
        new Shift()
        {
            Id = 9, KeeperId = 9, AnimalId = 9, StartDate = DateTime.Now.AddDays(8), EndDate = DateTime.Now.AddDays(9),
            Salary = 900
        },
        new Shift()
        {
            Id = 10, KeeperId = 10, AnimalId = 10, StartDate = DateTime.Now.AddDays(9),
            EndDate = DateTime.Now.AddDays(10),
            Salary = 1000
        }
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