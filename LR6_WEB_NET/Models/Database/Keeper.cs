using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LR6_WEB_NET.Models.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Models.Database;

[Table("Keepers")]
[EntityTypeConfiguration(typeof(KeeperConfiguration))]
public class Keeper
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int Age { get; set; }

    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}