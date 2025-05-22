using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SiaAdmin.Application.DTOs.NotificationDetailDto;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Repositories.NotificationHistory;

namespace SiaAdmin.Application.Features.Queries.NotificationHistory.GetNotificationDetail
{
    public class GetNotificationDetailHandler:IRequestHandler<GetNotificationDetailRequest, GetNotificationDetailResponse>
    {
        private readonly INotificationHistoryReadRepository _notificationHistoryReadRepository;
        private readonly ISurveyReadRepository _surveyReadRepository;
        private readonly ISiaUserReadRepository _userReadRepository;

        public GetNotificationDetailHandler(ISiaUserReadRepository userReadRepository, ISurveyReadRepository surveyReadRepository, INotificationHistoryReadRepository notificationHistoryReadRepository)
        {
            _userReadRepository = userReadRepository;
            _surveyReadRepository = surveyReadRepository;
            _notificationHistoryReadRepository = notificationHistoryReadRepository;
        }

        public async Task<GetNotificationDetailResponse> Handle(GetNotificationDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var notification = await _notificationHistoryReadRepository.GetByIdAsync(request.Id, false);

                if (notification == null)
                {
                    return new GetNotificationDetailResponse
                    {
                        success = false,
                        data = null
                    };
                }

                var survey = await _surveyReadRepository.GetAll(false)
                    .FirstOrDefaultAsync(s => s.Id == notification.Id, cancellationToken); 
                var user = await _userReadRepository.GetAll(false)
                    .FirstOrDefaultAsync(u => u.UserGUID.ToString() == notification.SentBy, cancellationToken);

                
                ErrorDetailDto errorDetails = null;
                if (notification.Status == 1 && !string.IsNullOrEmpty(notification.ErrorCode)) // 1: Başarısız durumu
                {
                    try
                    {
                        errorDetails = new ErrorDetailDto
                        {
                            code = notification.ErrorCode ?? "UNKNOWN_ERROR",
                            message = notification.ErrorMessage ?? "Bilinmeyen hata"
                        };
                    }
                    catch (Exception ex)
                    {
                       
                        errorDetails = new ErrorDetailDto
                        {
                            code = "PARSE_ERROR",
                            message = "Hata detayları ayrıştırılamadı"
                        };
                    }
                }

                // Yanıt DTO'sunu oluştur
                var detailDto = new NotificationDetailDto
                {
                    id = notification.Id,
                    title = notification.NotificationTitle,
                    content = notification.NotificationContent,
                    project = survey?.SurveyText ?? "Bilinmiyor",
                    sendTime = notification.SentAt.ToString("dd.MM.yyyy HH:mm"),
                    status = notification.Status,
                    updateTime = notification.UpdatedAt?.ToString("dd.MM.yyyy HH:mm") ?? "-",
                    successCount = notification.SuccessfulDeliveryCount,
                    failedCount = notification.FailedDeliveryCount,
                    recipientCount = notification.RecipientCount,
                    sender = user?.UserName ?? "Bilinmiyor",  
                    errorDetails = errorDetails,
                    payload = notification.ResponsePayload ?? "{}"
                };

                return new GetNotificationDetailResponse
                {
                    success = true,
                    data = detailDto
                };
            }
            catch (Exception ex)
            { 
                return new GetNotificationDetailResponse
                {
                    success = false,
                    data = null
                };
            }
        }
    }
}
