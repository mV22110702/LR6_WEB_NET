using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class KeeperConfiguration : IEntityTypeConfiguration<Keeper>
{
    public void Configure(EntityTypeBuilder<Keeper> builder)
    {
        builder.HasData(
            new Keeper { Id = 1, Name = "John Doe", Age = 25 },
            new Keeper { Id = 2, Name = "Jane Doe", Age = 30 },
            new Keeper { Id = 3, Name = "John Smith", Age = 35 },
            new Keeper { Id = 4, Name = "Jane Smith", Age = 40 },
            new Keeper { Id = 5, Name = "Steve Lane", Age = 31 },
            new Keeper { Id = 6, Name = "Conor Wood", Age = 32 },
            new Keeper { Id = 7, Name = "Alfred Wolf", Age = 33 },
            new Keeper { Id = 8, Name = "Jim How", Age = 34 },
            new Keeper { Id = 9, Name = "Jane Rich", Age = 35 },
            new Keeper { Id = 10, Name = "Jack Clinton", Age = 36 }
        );
    }
}