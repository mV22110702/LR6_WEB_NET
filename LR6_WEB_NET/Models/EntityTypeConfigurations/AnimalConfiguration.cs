using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasData(
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
        );
    }
}