namespace Agro_Express.Email
{
    public class EmailDto
    {
        public class EmailRequestModel
        {
             public string? SenderEmail{get; set;}
            public string ReceiverName{get; set;}
            public string ReceiverEmail{get; set;}
            public string Message{get; set;}
            public string Subject{get; set;}
        } 
       
    }
}