using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SendCV.Model;
using SendCV.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SendCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IMailService _mailService;
        public EmailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

