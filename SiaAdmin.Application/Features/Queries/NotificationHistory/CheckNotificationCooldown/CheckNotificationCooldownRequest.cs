using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.CheckNotificationCooldown
{
    public class CheckNotificationCooldownRequest:IRequest<CheckNotificationCooldownResponse>
    {
        public int SurveyId { get; set; }
    }
}
