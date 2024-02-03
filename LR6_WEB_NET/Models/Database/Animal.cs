namespace LR6_WEB_NET.Models.Database;

public class Animal:ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ScientificName { get; set; }
    public int Age { get; set; }
    
    public Object Clone()
    {
        return new Animal
        {
            Id = this.Id,
            Name = this.Name,
            ScientificName = this.ScientificName,
            Age = this.Age
        };
    }
}