using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.NotificationHistory;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.GetAllNotificationHistory
{
    public class GetAllNotificationHistoryHandler:IRequestHandler<GetAllNotificationHistoryRequest,GetAllNotificationHistoryResponse>
    {
        private readonly INotificationHistoryReadRepository _notificationHistoryReadRepository;
       private readonly ISiaUserReadRepository _userReadRepository;
        private readonly ISurveyReadRepository _surveyReadRepository;

        public GetAllNotificationHistoryHandler(INotificationHistoryReadRepository notificationHistoryReadRepository, ISurveyReadRepository surveyReadRepository, ISiaUserReadRepository userReadRepository)
        {
            _notificationHistoryReadRepository = notificationHistoryReadRepository;
             
            _surveyReadRepository = surveyReadRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<GetAllNotificationHistoryResponse> Handle(GetAllNotificationHistoryRequest request, CancellationToken cancellationToken)
        { 
            var notificationsQuery = _notificationHistoryReadRepository.GetAll(false); 
            var surveysQuery = _surveyReadRepository.GetAll(false);
            var usersQuery = _userReadRepository.GetAll(false);

            // Join ile birleştirme yapın
            var query = from notification in notificationsQuery
                join survey in surveysQuery on notification.SurveyId equals survey.Id
                join user in usersQuery on notification.SentBy equals user.UserGUID.ToString()
                select new
                {
                    Notification = notification,
                    Survey = survey,
                    User = user
                };
            if (!string.IsNullOrEmpty(request.status))
            {
                int status = Convert.ToInt32(request.status);
                query = query.Where(n => n.Notification.Status == status);
            }
            if (!string.IsNullOrEmpty(request.projectId))
            {
                int projectId = Convert.ToInt32(request.projectId);
                query = query.Where(n => n.Notification.SurveyId == projectId);
            }

            
            if (!string.IsNullOrEmpty(request.userId))
            {
                query = query.Where(n => n.Notification.SentBy == request.userId);
            }
            if (!string.IsNullOrEmpty(request.dateRange))
            {
                var dates = request.dateRange.Split(" - ");
                if (dates.Length == 2)
                {
                    DateTime startDate = DateTime.ParseExact(dates[0], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime endDate = DateTime.ParseExact(dates[1], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                    query = query.Where(n => n.Notification.SentAt >= startDate && n.Notification.SentAt < endDate);
                }
            }
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                query = query.Where(n =>
                    n.Notification.NotificationTitle.Contains(request.searchValue) ||
                    n.Notification.NotificationContent.Contains(request.searchValue) ||
                    n.Survey.SurveyText.Contains(request.searchValue) ||
                    n.User.UserName.Contains(request.searchValue)
                );
            }
            var recordsTotal = await query.CountAsync(cancellationToken);
            // Sıralama
            if (!string.IsNullOrEmpty(request.orderColumnIndex) && !string.IsNullOrEmpty(request.orderDir))
            {
                bool isAscending = request.orderDir.ToLower() == "asc";

                // DataTables'tan gelen sütun indeksine göre sıralama yapın
                switch (request.orderColumnIndex)
                {
                    case "0": // Proje/Anket
                        query = isAscending
                            ? query.OrderBy(n => n.Notification.SurveyId)
                            : query.OrderByDescending(n => n.Notification.SurveyId);
                        break;
                    case "1": // Başlık
                        query = isAscending
                            ? query.OrderBy(n => n.Notification.NotificationTitle)
                            : query.OrderByDescending(n => n.Notification.NotificationTitle);
                        break;
                    case "2": // Gönderim Zamanı
                        query = isAscending
                            ? query.OrderBy(n => n.Notification.SentAt)
                            : query.OrderByDescending(n => n.Notification.SentAt);
                        break;
                    case "3": // Durum
                        query = isAscending
                            ? query.OrderBy(n => n.Notification.Status)
                            : query.OrderByDescending(n => n.Notification.Status);
                        break;
                    case "4": // Alıcı Sayısı
                        query = isAscending
                            ? query.OrderBy(n => n.Notification.RecipientCount)
                            : query.OrderByDescending(n => n.Notification.RecipientCount);
                        break;
                    
                    default:
                        // Varsayılan sıralama - gönderim zamanına göre
                        query = query.OrderByDescending(n => n.Notification.SentAt);
                        break;
                }
            }
            else
            {
                // Varsayılan sıralama - gönderim zamanına göre
                query = query.OrderByDescending(n => n.Notification.SentAt);
            }

            var pagedData = await query
                .Skip(request.Start)
                .Take(request.Length)
                .Select(n => new
                {
                    id = n.Notification.Id,
                    project = n.Notification.SurveyId,
                    title = n.Notification.NotificationTitle,
                    sender=n.User.Name+" "+n.User.Surname,
                    sendTime = n.Notification.SentAt.ToString("dd.MM.yyyy HH:mm"),
                    status = n.Notification.Status,
                    recipientCount = n.Notification.RecipientCount
                })
                .ToListAsync(cancellationToken);

             
            return new GetAllNotificationHistoryResponse
            {
                draw = request.draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal,
                data = pagedData.ToArray()
            };
        }
    }
}
