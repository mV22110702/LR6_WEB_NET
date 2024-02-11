namespace LR6_WEB_NET.Models.Database;

public class Keeper : ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public object Clone()
    {
        return new Keeper
        {
            Id = Id,
            Name = Name,
            Age = Age
        };
    }
}