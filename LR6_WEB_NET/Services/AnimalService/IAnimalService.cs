
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.AnimalService;

public interface IAnimalService
{
    public Task<Animal?> FindOne(int id);
    public Task<bool> DoesExist(int id);
    public Task<Animal> AddOne(AnimalDto animalDto);
    public Task<Animal> UpdateOne(int id, AnimalUpdateDto animalDto);
    public Task<Animal?> DeleteOne(int id);
}