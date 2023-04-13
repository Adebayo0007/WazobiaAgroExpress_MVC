using System.ComponentModel;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.User
{
    public class UserDto
    {
    
          [DisplayName("Identifier")]
        public string Id{get; set;}
         [DisplayName("User Name")]
        public string UserName{get; set;}
         [DisplayName("Profile Picture")]
        public byte[]? ProfilePicture {get; set;}
         [DisplayName("Name")]
        public string Name{get; set;}
         [DisplayName("Phone Number")]
        public string PhoneNumber{get; set;}
         [DisplayName("Full Address")]
        public string FullAddress{get; set;}
         [DisplayName("Local Government")]
        public LocalGovernment LocalGovernment {get; set;}
        public State State {get; set;}
        public Gender Gender{get; set;}
        public string Email{get; set;}
        public string Password{get; set;}
        public string Role{get; set;}
        public bool IsActive{get; set;}
        [DisplayName("Registration Date")]
        public DateTime DateCreated{get; set;}
          [DisplayName("Updated Date")]
        public DateTime? DateModified{get; set;}
        
    }
}