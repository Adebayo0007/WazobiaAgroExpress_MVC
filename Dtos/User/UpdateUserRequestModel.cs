using Agro_Express.Enum;

namespace Agro_Express.Dtos.User
{
    public class UpdateUserRequestModel
    {
        public string UserName{get; set;}
        public byte[] ProfilePicture {get; set;}
        public string Name{get; set;}  
        public string PhoneNumber{get; set;}
        public string FullAddress{get; set;}
        public LocalGovernment LocalGovernment {get; set;}
        public State State {get; set;}
        public Gender Gender{get; set;}
        public string Email{get; set;}
        public string Password{get; set;}
    }
}