using ClosedXML.Excel;

namespace LR6_WEB_NET.Services.AnimalStatisticsService;

public interface IAnimalStatisticsService
{
    public Task<double> GetAverageAge();
    public Task<string> GetAverageAgeReadable();
    public Task<XLWorkbook> GetAverageAgeExcel();
}