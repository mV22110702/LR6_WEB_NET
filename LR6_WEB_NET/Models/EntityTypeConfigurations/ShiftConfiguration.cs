using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

namespace LR6_WEB_NET.Models.EntityTypeConfigurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.HasData(
            new Shift
            {
                KeeperId = 1, AnimalId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1),
                Salary = 100
            },
            new Shift
            {
                KeeperId = 2, AnimalId = 2, StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(2),
                Salary = 200
            },
            new Shift
            {
                KeeperId = 3, AnimalId = 3, StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(3),
                Salary = 300
            },
            new Shift
            {
                KeeperId = 4, AnimalId = 4, StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(4),
                Salary = 400
            },
            new Shift
            {
                KeeperId = 5, AnimalId = 5, StartDate = DateTime.Now.AddDays(4), EndDate = DateTime.Now.AddDays(5),
                Salary = 500
            },
            new Shift
            {
                KeeperId = 6, AnimalId = 6, StartDate = DateTime.Now.AddDays(5), EndDate = DateTime.Now.AddDays(6),
                Salary = 600
            },
            new Shift
            {
                KeeperId = 7, AnimalId = 7, StartDate = DateTime.Now.AddDays(6), EndDate = DateTime.Now.AddDays(7),
                Salary = 700
            },
            new Shift
            {
                KeeperId = 8, AnimalId = 8, StartDate = DateTime.Now.AddDays(7), EndDate = DateTime.Now.AddDays(8),
                Salary = 800
            },
            new Shift
            {
                KeeperId = 9, AnimalId = 9, StartDate = DateTime.Now.AddDays(8), EndDate = DateTime.Now.AddDays(9),
                Salary = 900
            },
            new Shift
            {
                KeeperId = 10, AnimalId = 10, StartDate = DateTime.Now.AddDays(9),
                EndDate = DateTime.Now.AddDays(10),
                Salary = 1000
            }
        );
        Log.Information("Shifts have been seeded with {Count} entities", 10);
    }
}