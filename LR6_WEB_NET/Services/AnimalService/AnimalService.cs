using System.Net;
using System.Web.Http;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.AnimalService;

public class AnimalService : IAnimalService
{
    public async Task<List<Animal>> FindAll()
    {
        await Task.Delay(1000);
        lock (_animals)
        {
            return _animals.ToList();
        }
    }

    public async Task<Animal?> FindOne(int id)
    {
        await Task.Delay(1000);
        lock (_animals)
        {
            return _animals.FirstOrDefault(a => a.Id == id, null);
        }
    }

    public Task<bool> DoesExist(int id)
    {
        lock (_animals)
        {
            return Task.FromResult(_animals.Any(a => a.Id == id));
        }
    }

    public async Task<Animal> AddOne(AnimalDto animalDto)
    {
        await Task.Delay(1000);
        lock (_animals)
        {
            var animal = new Animal
            {
                Id = _animals.Max(a => a.Id) + 1,
                Name = animalDto.Name,
                ScientificName = animalDto.ScientificName,
                Age = animalDto.Age
            };
            _animals.Add(animal);
            return animal;
        }
    }

    public async Task<Animal> UpdateOne(int id, AnimalUpdateDto animalDto)
    {
        await Task.Delay(1000);
        lock (_animals)
        {
            var animal = _animals.FirstOrDefault(a => a.Id == id, null);
            if (animal == null)
                throw new HttpResponseException(new HttpResponseMessage
                    { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Animal does not exist") });

            if (animalDto.ScientificName != null) animal.ScientificName = animalDto.ScientificName;

            if (animalDto.Name != null) animal.Name = animalDto.Name;

            if (animalDto.Age != null) animal.Age = animalDto.Age.Value;

            return animal;
        }
    }

    public async Task<Animal?> DeleteOne(int id)
    {
        await Task.Delay(1000);
        lock (_animals)
        {
            var animalToDelete = _animals.FirstOrDefault(a => a.Id == id, null);
            if (animalToDelete == null) return null;

            var clonedAnimal = (Animal)animalToDelete.Clone();
            _animals.Remove(animalToDelete);
            return clonedAnimal;
        }
    }
}