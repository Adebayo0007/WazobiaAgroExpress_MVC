using Agro_Express.Enum;

namespace Agro_Express.Models;

public class Address
{
    public string Id{get; set;} = Guid.NewGuid().ToString();
    public string FullAddress{get; set;}
    public LocalGovernment LocalGovernment {get; set;}
    public State State {get; set;}
}
