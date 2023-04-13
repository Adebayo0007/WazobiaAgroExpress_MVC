using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.Product
{
    public class CreateProductRequestModel
    {
      
      [DisplayName("First Dimention")]
      [Required]
      public byte[] FirstDimentionPicture{get; set;}
      [DisplayName("Second Dimention")]
      [Required]
      public byte[] SecondDimentionPicture{get; set;}
      [DisplayName("Third Dimention")]
      [Required]
      public byte[] ThirdDimentionPicture{get; set;}
      [DisplayName("Forth Dimention")]
      [Required]
      public byte[] ForthDimentionPicture{get; set;}
      [DisplayName("Product Name")]
      [Required]
      public string ProductName{get; set;}
      [Required]
      public int Quantity{get; set;}
       [Required]
      public double Price{get; set;}
      [Required]
      public Measurement Measurement{get; set;}
      [DisplayName("Availability Date(From)")]
      [Required]
      public DateTime AvailabilityDateFrom{get; set;}
      [DisplayName("Availability Date(To)")]
      [Required]
      public DateTime AvailabilityDateTo{get; set;}
    }
}