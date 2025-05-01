using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.GetNotificationDetail
{
    public class GetNotificationDetailRequest:IRequest<GetNotificationDetailResponse>
    {
        public int Id { get; set; }
    }
}
