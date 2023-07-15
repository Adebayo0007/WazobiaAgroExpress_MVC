using Agro_Express.Email;
using Microsoft.AspNetCore.Mvc;
using static Agro_Express.Email.EmailDto;

namespace Agro_Express.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender) => _emailSender = emailSender;
        public IActionResult CreateEmail() => View();
        [HttpPost]
         public async Task<IActionResult> CreateEmail(EmailRequestModel emailRequestModel)
        {
            var response = await _emailSender.SendEmail(emailRequestModel);
            if(response == true)
            {
                TempData["success"] = "Email sent";
                return View();
            }
            TempData["error"] = "Email not sent";
            return View();
        }
    }
}