using System.ComponentModel;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.RequestedProduct
{
    public class RequestedProductDto
    {
        public string Id{get; set;}
        public string FarmerId{get; set;}
        public string FarmerName{get; set;}
       public string FarmerNumber{get; set;}
        public string BuyerId{get; set;}
        [DisplayName("Buyer Email")]
        public string BuyerEmail{get; set;}
        [DisplayName("Buyer Phone number")]
        public string BuyerPhoneNumber{get; set;}
        [DisplayName("Buyer LGA")]
        public LocalGovernment BuyerLocalGovernment{get; set;}
        [DisplayName("Product Name")]
        public string ProductName{get; set;}
        public bool OrderStatus{get; set;}
         public bool IsAccepted{get; set;}
        public bool IsDelivered{get; set;}
         public bool NotDelivered{get; set;}
        public bool Haspaid{get; set;}
    }
}