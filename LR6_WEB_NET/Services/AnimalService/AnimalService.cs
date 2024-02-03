using System.Net;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Dto;
using System.Web.Http;

namespace LR6_WEB_NET.Services.AnimalService;

public class AnimalService : IAnimalService
{
    private static List<Animal> _animals = new List<Animal>
    {
        new Animal
        {
            Id = 1,
            Name = "Lion",
            ScientificName = "Panthera leo",
            Age = 5
        },
        new Animal
        {
            Id = 2,
            Name = "Tiger",
            ScientificName = "Panthera tigris",
            Age = 4
        },
        new Animal
        {
            Id = 3,
            Name = "Elephant",
            ScientificName = "Loxodonta",
            Age = 10
        },
        new Animal
        {
            Id = 4,
            Name = "Giraffe",
            ScientificName = "Giraffa camelopardalis",
            Age = 7
        },
        new Animal
        {
            Id = 5,
            Name = "Zebra",
            ScientificName = "Equus zebra",
            Age = 6
        },
        new Animal
        {
            Id = 6,
            Name = "Hippopotamus",
            ScientificName = "Hippopotamus amphibius",
            Age = 8
        },
        new Animal
        {
            Id = 7,
            Name = "Crocodile",
            ScientificName = "Crocodylus",
            Age = 9
        },
        new Animal
        {
            Id = 8,
            Name = "Penguin",
            ScientificName = "Spheniscidae",
            Age = 3
        },
        new Animal
        {
            Id = 9,
            Name = "Kangaroo",
            ScientificName = "Macropodidae",
            Age = 2
        },
        new Animal
        {
            Id = 10,
            Name = "Koala",
            ScientificName = "Phascolarctos cinereus",
            Age = 1
        }
    };

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
            {
                throw new HttpResponseException(new HttpResponseMessage()
                    { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("Animal does not exist") });
            }

            if (animalDto.ScientificName != null)
            {
                animal.ScientificName = animalDto.ScientificName;
            }

            if (animalDto.Name != null)
            {
                animal.Name = animalDto.Name;
            }

            if (animalDto.Age != null)
            {
                animal.Age = animalDto.Age.Value;
            }

            return animal;
        }
    }

    public async Task<Animal?> DeleteOne(int id)
    {
        await Task.Delay(1000);
        lock (_animals)
        {
            var animalToDelete = _animals.FirstOrDefault(a => a.Id == id, null);
            if (animalToDelete == null)
            {
                return null;
            }

            var clonedAnimal = (Animal)animalToDelete.Clone();
            _animals.Remove(animalToDelete);
            return clonedAnimal;
        }
    }
}