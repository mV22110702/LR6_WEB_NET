using ClosedXML.Excel;
using LR6_WEB_NET.Services.AnimalService;

namespace LR6_WEB_NET.Services.AnimalStatisticsService;

public class AnimalStatisticsService : IAnimalStatisticsService
{
    private readonly IAnimalService _animalService;

    public AnimalStatisticsService(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    public async Task<double> GetAverageAge()
    {
        var animals = await _animalService.FindAll();
        return animals.Average(animal => animal.Age);
    }

    public async Task<string> GetAverageAgeReadable()
    {
        var animals = await _animalService.FindAll();
        var averageAge = animals.Average(animal => animal.Age);
        return $"Average age is {averageAge} years for {animals.Count} animals";
    }

    public async Task<XLWorkbook> GetAverageAgeExcel()
    {
        var animals = await _animalService.FindAll();
        var averageAge = animals.Average(animal => animal.Age);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Average");
        worksheet.Cell("A1").Value = "Average age";
        worksheet.Cell("B1").Value = averageAge;
        worksheet.Cell("A2").Value = "Total animals";
        worksheet.Cell("B2").Value = animals.Count;
        return workbook;
    }
}