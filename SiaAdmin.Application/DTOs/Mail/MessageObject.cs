using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.Mail
{
    public class MessageObject
    {
        public string Subject { get; set; } 
        public string Body { get; set; }
        public Guid UserGUID { get; set; } 
    }

    
}
