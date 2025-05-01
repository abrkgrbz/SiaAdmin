
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using SiaAdmin.Application.DTOs.Mail;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.Mail;
using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Security;
using SiaAdmin.Application.Interfaces.SiaUser;
using SiaAdmin.Application.Interfaces.User;
using SiaAdmin.Application.Repositories;
using Microsoft.Extensions.Hosting;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MimeKit;

namespace SiaAdmin.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private IUserReadRepository _userService;
        private readonly string _smtpHost = "mail.sunucularim.net";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "sialive@sia-insight.com";
        private readonly string _smtpPass = "Ucq@875*";


        public MailService(IUserReadRepository userService)
        {
            _userService = userService;

        }


        public async Task SendMessage(MessageObject message)
        {

            try
            {
                var user = await _userService.GetSingleAsync(x => x.InternalGuid == message.UserGUID);
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("Sia Live Destek",_smtpUser)); 
                mimeMessage.Subject = "Mobil Uygulama - İletişim";
                mimeMessage.To.Add(new MailboxAddress(user.Name, "sialive@sia-insight.com"));

                var bodyBuilder = new BodyBuilder
                {
                    TextBody = $@" 
    Ad: {user.Name} {user.Surname}
    Email: {user.Email}
    Konu: {message.Subject}
    Mesaj: {message.Body}

    {user.InternalGuid}
    {user.LastIp}
    {user.LastBrowser}"
                };

                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpHost,_smtpPort,SecureSocketOptions.StartTlsWhenAvailable); 
                    client.Authenticate(_smtpUser, _smtpPass);
                    var result=await client.SendAsync(mimeMessage);
                    client.Disconnect(true);
                }

            } 
            catch (Exception ex)
            {
                // Diğer hatalar için
                throw new ApiException($"Mesajınız gönderilirken hata oluştu: {ex.Message}");
            }
        }  
    }
}
