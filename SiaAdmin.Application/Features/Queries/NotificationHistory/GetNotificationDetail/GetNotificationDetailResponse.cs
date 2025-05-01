using SiaAdmin.Application.DTOs.NotificationDetailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.GetNotificationDetail
{
    public class GetNotificationDetailResponse
    {
        public bool success { get; set; }
        public NotificationDetailDto data { get; set; }
    }
}
