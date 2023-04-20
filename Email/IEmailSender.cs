using static Agro_Express.Email.EmailDto;

namespace Agro_Express.Email
{
    public interface IEmailSender
    {
          Task<bool> SendEmail(EmailRequestModel email);
    }
}