namespace LR6_WEB_NET.Models.Database;

public class Shift : ICloneable
{
    public int Id { get; set; }
    public int KeeperId { get; set; }
    public int AnimalId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public double Salary { get; set; }

    public object Clone()
    {
        return new Shift
        {
            Id = Id,
            KeeperId = KeeperId,
            AnimalId = AnimalId,
            StartDate = StartDate,
            EndDate = EndDate,
            Salary = Salary
        };
    }
}