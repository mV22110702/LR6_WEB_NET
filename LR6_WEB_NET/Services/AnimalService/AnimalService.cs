using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LR6_WEB_NET.Services.AnimalService;

public class AnimalService : IAnimalService
{
    private readonly DataContext _dataContext;

    public AnimalService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<List<Animal>> FindAll()
    {
        return _dataContext.Animals.ToList();
    }

    public async Task<Animal?> FindOne(int id)
    {
        return await _dataContext.Animals.FirstOrDefaultAsync(a => a.Id == id);
    }

    public Task<bool> DoesExist(int id)
    {
        return _dataContext.Animals.AnyAsync(a => a.Id == id);
    }

    public async Task<Animal> AddOne(AnimalDto animalDto)
    {
        var animal = new Animal
        {
            Name = animalDto.Name,
            ScientificName = animalDto.ScientificName,
            Age = animalDto.Age
        };
        _dataContext.Animals.Add(animal);
        await _dataContext.SaveChangesAsync();
        return animal;
    }

    public async Task<Animal> UpdateOne(int id, AnimalUpdateDto animalDto)
    {
        var animal = await _dataContext.Animals.FirstOrDefaultAsync(a => a.Id == id);
        if (animal == null)
            throw new HttpResponseException(new HttpResponseMessage
                { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Animal does not exist") });
        
        if (!String.IsNullOrEmpty(animalDto.ScientificName)) animal.ScientificName = animalDto.ScientificName;
        
        if (!String.IsNullOrEmpty(animalDto.Name)) animal.Name = animalDto.Name;
        
        if (animalDto.Age != null) animal.Age = animalDto.Age.Value;
        
        await _dataContext.SaveChangesAsync();
        return animal;
    }

    public async Task<Animal?> DeleteOne(int id)
    {
        var animalToDelete = await _dataContext.Animals.FirstOrDefaultAsync(a => a.Id == id);
        if (animalToDelete == null) return null;
        _dataContext.Animals.Remove(animalToDelete);
        await _dataContext.SaveChangesAsync();
        return animalToDelete;
    }

    public async Task<string?> CheckServiceConnection()
    {
        try
        {
            var animal = await _dataContext.Animals.FirstOrDefaultAsync();
            return null;
        } catch (Exception e)
        {
Log.Error(e, "Check animal service connection failed");
            return e.Message;
        }
    }
}