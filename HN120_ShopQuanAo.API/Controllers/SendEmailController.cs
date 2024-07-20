using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System;

namespace HN120_ShopQuanAo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail(string Body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("toankhac21082003@gmail.com"));
                email.To.Add(MailboxAddress.Parse("toankhac2k3@gmail.com"));

                email.Subject = "Send email test";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = Body };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("toankhac21082003@gmail.com", "glmbusaxthbmlhul");
                smtp.Send(email);
                smtp.Disconnect(true);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
