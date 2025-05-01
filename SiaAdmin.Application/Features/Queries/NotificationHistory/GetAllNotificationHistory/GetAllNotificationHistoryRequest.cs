using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Features.Queries.SurveyAssigned.GetAllSurveyAssigned;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.GetAllNotificationHistory
{
    public class GetAllNotificationHistoryRequest:IRequest<GetAllNotificationHistoryResponse>
    {
        public int draw { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }
        public string? orderColumnIndex { get; set; }
        public string? orderDir { get; set; }
        public string? orderColumnName { get; set; }
        public string? searchValue { get; set; }

        // Ek filtreleme alanları
        public string? status { get; set; }
        public string? projectId { get; set; }
        public string? dateRange { get; set; }
        public string? userId { get; set; }
    }
}
