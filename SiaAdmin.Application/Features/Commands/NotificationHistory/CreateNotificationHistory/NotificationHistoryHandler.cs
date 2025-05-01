using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Application.Repositories.NotificationHistory;
using SiaAdmin.Application.Repositories.NotificationScheduledDeviceTokens;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Commands.NotificationHistory.CreateNotificationHistory
{
    public class NotificationHistoryHandler : IRequestHandler<NotificationHistoryRequest, NotificationHistoryResponse>
    {
        private readonly INotificationHistoryWriteRepository _notificationHistoryWriteRepository;
        private readonly INotificationScheduledDeviceTokensWriteRepository _notificationScheduledDeviceTokensWriteRepository;
        private readonly IDeviceRegistrationsReadRepository _deviceRegistrationsReadRepository;
         
        private readonly IMapper _mapper;

        public NotificationHistoryHandler(INotificationHistoryWriteRepository notificationHistoryWriteRepository, IMapper mapper, INotificationScheduledDeviceTokensWriteRepository notificationScheduledDeviceTokensWriteRepository, IDeviceRegistrationsReadRepository deviceRegistrationsReadRepository)
        {
            _notificationHistoryWriteRepository = notificationHistoryWriteRepository;
            _mapper = mapper;
            _notificationScheduledDeviceTokensWriteRepository = notificationScheduledDeviceTokensWriteRepository;
            _deviceRegistrationsReadRepository = deviceRegistrationsReadRepository;
        }

        public async Task<NotificationHistoryResponse> Handle(NotificationHistoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var mappingProfile = _mapper.Map<Domain.Entities.Models.NotificationHistory>(request);
                int totalCount = await _deviceRegistrationsReadRepository.GetDeviceTokenIdsCount(request.SurveyId);
                mappingProfile.RecipientCount = totalCount;
                var addedNotificationHistory = await _notificationHistoryWriteRepository
                    .AddAsyncReturnEntity(mappingProfile);
                var deviceTokens =
                    await _deviceRegistrationsReadRepository.GetDeviceTokenIdsBySurveyId(request.SurveyId);
              
                foreach (var deviceToken in deviceTokens)
                {
                    var addedNotificationScheluded = await _notificationScheduledDeviceTokensWriteRepository
                        .AddAsync(new NotificationScheduledDeviceTokens()
                        { 
                            CreatedAt = DateTime.UtcNow,
                            DeviceToken = deviceToken,
                            DeviceType = "ANDROID",
                            NotificationHistoryId = addedNotificationHistory.Id
                        });
                    
                }

                return new NotificationHistoryResponse()
                    { IsScheduled = true, Message = "Mobil Bildirim gönderimi başarıyla planlandı" };
            }
            catch (Exception e)
            {
                return new NotificationHistoryResponse()
                    { IsScheduled = false, Message = "Hata oluştu,Lütfen tekrar deneyiniz" };
            }
        }
    }
}
