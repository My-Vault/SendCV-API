using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendCV.Model
{
    //public class MailRequest
    //{
    //    public string ToEmail { get; set; }
    //    public string Subject { get; set; }
    //    public string Body { get; set; }
    //    public List<IFormFile> Attachment { get; set; }
    //}
    public class MailRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Fecha { get; set; }
        public string Cargo { get; set; }
        //public List<IFormFile> Attachment { get; set; }
        public IFormFile Attachment { get; set; }
    }

}
