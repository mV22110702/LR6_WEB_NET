using DocumentFormat.OpenXml.Spreadsheet;
using LR6_WEB_NET.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Data.DatabaseContext;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
    }

    public DbSet<Animal> Animals { get; set; }
    public DbSet<Keeper> Keepers { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}