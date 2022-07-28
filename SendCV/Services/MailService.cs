using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SendCV.Model;
using SendCV.Settings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SendCV.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            //email.To.Add(MailboxAddress.Parse("computos@toto.com.uy"));
            //email.To.Add(MailboxAddress.Parse("ivana.pontet@toto.com.uy"));
            //email.To.Add(MailboxAddress.Parse("fvalerio@toto.com.uy"));
            email.To.Add(MailboxAddress.Parse("marianela@toto.com.uy"));
            email.Bcc.Add(MailboxAddress.Parse("rdatrindade@toto.com.uy"));
            email.Bcc.Add(MailboxAddress.Parse("fvalerio@toto.com.uy"));
            if (mailRequest.Cargo == "Cadete" || mailRequest.Cargo == "Chofer profesional")
            {
                email.To.Add(MailboxAddress.Parse("ldefreitas@toto.com.uy"));
            }
            //email.Subject = mailRequest.Subject;
            //email.Subject = mailRequest.Nombre + " - " + mailRequest.Apellido;
            email.Subject = $"{mailRequest.Nombre} {mailRequest.Apellido}";
            var builder = new BodyBuilder();
            if (mailRequest.Attachment != null)
            {
                byte[] fileBytes;
                var file = mailRequest.Attachment;
                //foreach (var file in mailRequest.Attachment)
                //{
                    if ( file.Length > 0 )
                    {
                        //if ( file.FileName.Contains(".pdf") || file.FileName.Contains(".doc") || file.FileName.Contains(".jpg") || file.FileName.Contains(".jpeg") || file.FileName.Contains(".png") )
                        //{

                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        // Send file to the server  192.168.41.137
                        //}
                    }
                //}
            }
            builder.HtmlBody += "Nombre   : " + mailRequest.Nombre + "<br/> <br/>";
            builder.HtmlBody += "Apellido : " + mailRequest.Apellido + "<br/> <br/>";
            builder.HtmlBody += "Celular  : " + mailRequest.Celular + "<br/> <br/>";
            builder.HtmlBody += "Email    : " + mailRequest.Email + "<br/> <br/>";
            builder.HtmlBody += "Fecha    : " + mailRequest.Fecha + "<br/> <br/>";
            builder.HtmlBody += "Cargo    : " + mailRequest.Cargo + "<br/> <br/>";
            builder.HtmlBody += "Este correo es enviado desde la página de TOTO";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

            var connection = new SqlConnection("Data Source=192.168.41.160;Initial Catalog=Toto;User ID=sa;Password=adm1404;");
            
                connection.Open();
                var query = "INSERT INTO Postulaciones(Nombre, Apellido, Celular, Email, Nacimiento, Cargo, Fecha_postulacion) VALUES (@Nombre, @Apellido, @Celular, @Email, @Nacimiento, @Cargo, @Fecha_postulacion)";
            var cmd = new SqlCommand(query, connection);
                
                    cmd.Parameters.AddWithValue("@Nombre", mailRequest.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", mailRequest.Apellido);
                    cmd.Parameters.AddWithValue("@Celular", mailRequest.Celular);
                    cmd.Parameters.AddWithValue("@Email", mailRequest.Email);
                    cmd.Parameters.AddWithValue("@Nacimiento", Convert.ToDateTime(mailRequest.Fecha).ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@Cargo", mailRequest.Cargo);
                    cmd.Parameters.AddWithValue("@Fecha_postulacion", DateTime.Now.ToString("dd/MM/yyyy - HH:mm"));

                    cmd.ExecuteNonQuery();
                
                connection.Close();
            
        }
    }
}
