namespace LR6_WEB_NET.Models.Database;

public class Keeper:ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Object Clone()
    {
        return new Keeper
        {
            Id = this.Id,
            Name = this.Name,
            Age = this.Age
        };
    }
}