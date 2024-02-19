using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LR6_WEB_NET.Models.Database;

[Table("Shifts")]
[PrimaryKey(nameof(KeeperId), nameof(AnimalId))]
public class Shift
{
    [ForeignKey(nameof(Keeper))]
    public int KeeperId { get; set; }
    [ForeignKey(nameof(Animal))]
    public int AnimalId { get; set; }
    [Required]
    public Keeper Keeper { get; set; }= null!;
    [Required]
    public Animal Animal { get; set; }= null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double Salary { get; set; } 
    
}