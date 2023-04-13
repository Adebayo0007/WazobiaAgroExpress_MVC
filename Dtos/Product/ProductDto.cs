using System.ComponentModel;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.Product
{
    public class ProductDto
    {
      public string Id{get; set;}
      public string FarmerId{get; set;} 
      [DisplayName("First Dimention")]
      public byte[] FirstDimentionPicture{get; set;}
      [DisplayName("Second Dimention")]
      public byte[] SecondDimentionPicture{get; set;}
      [DisplayName("Third Dimention")]
      public byte[] ThirdDimentionPicture{get; set;}
      [DisplayName("Forth Dimention")]
      public byte[] ForthDimentionPicture{get; set;}
      [DisplayName("Product Name")]
      public string ProductName{get; set;}
      public int Quantity{get; set;}
      public double Price{get; set;}
       [DisplayName("Farmer User name")]
      public string FarmerUserName{get; set;}
      [DisplayName("Farmer Email")]
      public string FarmerEmail{get; set;}
      public Measurement Measurement{get; set;}
      [DisplayName("Availability Date(From)")]
      public DateTime AvailabilityDateFrom{get; set;}
      [DisplayName("Availability Date(To)")]
      public DateTime AvailabilityDateTo{get; set;}
      [DisplayName("Post Date")]
      public DateTime DateCreated{get; set;}
     [DisplayName("Availability")]
      public bool IsAvailable{get;set;}
      [DisplayName("Product LGA")]
       public LocalGovernment ProductLocalGovernment{get; set;}
        [DisplayName("Product State")]
        public State ProductState{get; set;}
          [DisplayName("Farmer Rank")]
        public int FarmerRank{get; set;}
         public int ThumbUp {get; set;}
        public int ThumbDown {get; set;}

    }
}