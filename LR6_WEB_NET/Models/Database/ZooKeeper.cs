namespace LR6_WEB_NET.Controllers;

public class ZooKeeper:ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Object Clone()
    {
        return new ZooKeeper
        {
            Id = this.Id,
            Name = this.Name,
            Age = this.Age
        };
    }
}