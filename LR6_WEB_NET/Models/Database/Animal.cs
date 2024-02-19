using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LR6_WEB_NET.Models.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Models.Database;

[Table("Animals")]
[EntityTypeConfiguration(typeof(AnimalConfiguration))]
public class Animal
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string ScientificName { get; set; }
    public int Age { get; set; }

    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}