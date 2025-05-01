using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Mail;

namespace SiaAdmin.Application.Interfaces.Mail
{
    public interface IMailService
    {
        Task SendMessage(MessageObject message);
    }
}
