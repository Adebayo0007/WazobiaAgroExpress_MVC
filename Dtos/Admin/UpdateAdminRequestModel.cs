using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Agro_Express.Enum;

namespace Agro_Express.Dtos.Admin
{
    public class UpdateAdminRequestModel
    {
            [DisplayName("User Name")]
            [Required]
            public string UserName{get; set;}
            [Required]
            public string Name{get; set;}          
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
            [EmailAddress]
            [Required]
            public string Email {get;set;}
            [DisplayName("Confirm Email")]
            [Compare("Email")]
            public string ConfirmEmail {get;set;}
            [Required]
           public string? Password{get; set;}

    }
}