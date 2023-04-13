using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.User
{
    public class CreateUserRequestModel
    {
         [DisplayName("User Name")]
        [Required]
        public string UserName{get; set;}
         [DisplayName("Profile Picture")]
         [Required]
        public byte[] ProfilePicture {get; set;}
         [DisplayName("First Name")]
         [Required]
        public string FirstName{get; set;}
         [DisplayName("Last Name")]
         [Required]
        public string LastName{get; set;}
         [DisplayName("Phone Number")]
         [Required]
        public string PhoneNumber{get; set;}
         [DisplayName("Full Address")]
         [Required]
        public string FullAddress{get; set;}
         [DisplayName("Local Government")]
         [Required]
        public LocalGovernment LocalGovernment {get; set;}
        [Required]
        public State State {get; set;}
        [Required]
        public Gender Gender{get; set;}
        [Required]
        public string Email{get; set;}
        [Required]
        public string Password{get; set;}
        public string Role{get; set;}
    }
}