using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.ShiftService;

public interface IShiftService
{
    public Task<Shift?> FindOne(FindShiftDto id);
    public Task<Shift> AddOne(ShiftDto shiftDto);
    public Task<Shift> UpdateOne(FindShiftDto id, ShiftUpdateDto shiftDto);
    public Task<Shift?> DeleteOne(FindShiftDto id);
}