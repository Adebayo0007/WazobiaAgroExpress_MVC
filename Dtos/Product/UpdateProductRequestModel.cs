using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.Product
{
   public class UpdateProductRequestModel
   {
     
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
      public int ThumbUp {get; set;}
      public int ThumbDown {get; set;}
   }
}