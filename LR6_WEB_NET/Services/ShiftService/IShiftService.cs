using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.ShiftService;

public interface IShiftService
{
    public Task<Shift?> FindOne(int id);
    public Task<Shift> AddOne(ShiftDto shiftDto);
    public Task<Shift> UpdateOne(int id, ShiftUpdateDto shiftDto);
    public Task<Shift?> DeleteOne(int id);
}