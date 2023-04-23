using Agro_Express.Enum;

namespace Agro_Express.Models;

public class RequestedProduct
{
     public string Id{get; set;} = Guid.NewGuid().ToString();
     public string FarmerId{get; set;}
     public Farmer Farmer{get; set;}
     public string BuyerId{get; set;}
     public Buyer Buyer{get; set;}
     public string BuyerEmail{get; set;}
     public string BuyerPhoneNumber{get; set;}
     public LocalGovernment BuyerLocalGovernment{get; set;}
     public string ProductName{get; set;}
      public string FarmerName{get; set;}
       public string FarmerNumber{get; set;}
     public bool OrderStatus{get; set;}
      public bool IsAccepted{get; set;}
     public bool IsDelivered{get; set;}
      public bool NotDelivered{get; set;}
      public bool Haspaid{get; set;}
       public string FarmerEmail{get; set;}
    
}
