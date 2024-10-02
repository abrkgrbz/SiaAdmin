using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SiaAdmin.Application.Features.Queries.User.SendNotifactionUser.SendNotifactionUserHandler;

namespace SiaAdmin.Application.Features.Queries.User.SendNotifactionUser
{
    public class NotificationResponse
    {
        public List<UserTokenList> TokenList { get; set; }
        public List<UserActiveSurveys> UserSurveys { get; set; }
    }
}
