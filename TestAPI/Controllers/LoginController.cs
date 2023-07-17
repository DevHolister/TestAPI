using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TestAPI.Data;
using TestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<LoginController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listUsers = await _context.userModel.ToListAsync();
                return Ok(listUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UserRegister")]
        public async Task<IActionResult> UserRegister(Users loginUser)
        {
            try
            {
                _context.userModel.Add(loginUser);
                await _context.SaveChangesAsync();
                return Ok("Se completó el registro correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> Login([Bind("Username, Password")] string Username, string Password)
        {
            var currentUser = await _context.userModel.FirstOrDefaultAsync(user => user.Username.ToLower() == Username.ToLower() && user.Password.ToLower() == Password.ToLower());
            if (currentUser != null)
            {
                _ = RedirectToPage("https://www.google.com/");
                return Ok("Usuario Loggeado");
            }
            return NotFound(new { message = "Usuario no encontrado" });
        }

        [HttpPost]
        [Route("UserResetPassword/{EmailAddress}")]
        public async Task<IActionResult> ResetPassword([Bind("EmailAddress")] string EmailAddress, bool isGmail)
        {
            var currentUser = await _context.userModel.FirstOrDefaultAsync(user => user.EmailAddress.ToLower() == EmailAddress.ToLower());
            if (currentUser != null && isGmail)
            {
                #region GMAIL - Terminado
                MailMessage correo = new MailMessage();
                correo.To.Add(EmailAddress);
                correo.From = new MailAddress("asanchezzapata93@gmail.com");
                correo.Subject = "Restablecimiento de contraseña";
                correo.Body = "<html><body><p>Hola, usted solicitó restablecer la contraseña</p><a href='https://localhost:44322/swagger/index.html'>Haga click aquí</a></body></html>";
                correo.Priority = MailPriority.High;
                correo.IsBodyHtml = true;

                SmtpClient serverEmail = new SmtpClient();
                serverEmail.Credentials = new NetworkCredential("asanchezzapata93@gmail.com", "goinypvwegjmgghv");//DevHollister11$
                serverEmail.Host = "smtp.gmail.com";//"smtp-relay.brevo.com";
                serverEmail.Port = 587;
                serverEmail.EnableSsl = true;
                serverEmail.DeliveryMethod = SmtpDeliveryMethod.Network;
                serverEmail.UseDefaultCredentials = false;

                try
                {
                    serverEmail.Send(correo);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                #endregion
            }
            else if (currentUser != null && !isGmail)
            {
                #region SendingBlue - Terminado
                MailMessage correo = new MailMessage();
                correo.To.Add(EmailAddress);
                correo.From = new MailAddress("asanchezzapata93@gmail.com");
                correo.Subject = "Restablecimiento de contraseña";
                correo.Body = "<html><body><p>Hola, usted solicitó restablecer la contraseña</p><a href='https://localhost:44322/swagger/index.html'>Haga click aquí</a></body></html>";
                correo.Priority = MailPriority.High;
                correo.IsBodyHtml = true;

                SmtpClient serverEmail = new SmtpClient();
                serverEmail.Credentials = new NetworkCredential("asanchezzapata93@gmail.com", "xnyVSh4LfAHbkz5j");//DevHollister11$
                serverEmail.Host = "smtp-relay.brevo.com";
                serverEmail.Port = 587;
                serverEmail.EnableSsl = true;
                serverEmail.DeliveryMethod = SmtpDeliveryMethod.Network;
                serverEmail.UseDefaultCredentials = false;

                try
                {
                    serverEmail.Send(correo);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                #endregion
            }
            else
            {
                return NotFound("No existen registros del correo proporcionado");
            }
        }
    }
}
